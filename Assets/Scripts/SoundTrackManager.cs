using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
public class SoundTrackManager : MonoBehaviour
{
    
    public AudioSource powerchords;
    public AudioSource bass;
    public AudioSource doublebass;
    public AudioSource guitar;
    public AudioSource guitar2;
    public AudioSource violin;
    public AudioSource violin2;
    public AudioSource drum;
    public AudioSource tambourine;
    public AudioSource trumpet;
    public AudioSource saxophone;
    public AudioSource flute;
    public AudioSource piano;
    public AudioSource clarinet;

    

    //MAIN CANVAS
    public GameObject CARDUI;

    //CARDS 
    public GameObject cardLParent;
    public GameObject cardRParent;

    // IMAGE
    public GameObject bass_image;
    public GameObject doublebass_image;
    public GameObject guitar_image;
    public GameObject violin_image;
    public GameObject drum_image;
    public GameObject tambourine_image;
    public GameObject trumpet_image;
    public GameObject flute_image;
    public GameObject saxophone_image;
    public GameObject piano_image;
    public GameObject clarinet_image;
   

    public GraphicRaycaster raycaster;
    public EventSystem eventSystem;

    // BONUS/MALUS
    public TMP_Text[] feedbackLine;
    private int feedbackIndex = 0; 

    private bool isRotating = false;
    private float RotationDuration = 1f;

    [SerializeField]
    public string genereCorrente = "Metal";

    //strumenti bonus per ogni genere
    private Dictionary<string, HashSet<string>> strumentiPerGenere = new Dictionary<string, HashSet<string>>
    {
        { "Metal", new HashSet<string> { "ElectricBass", "Drum", "Guitar" } },
        { "Classico", new HashSet<string> { "Violin", "DoubleBass", "Clarinet" } },
        { "Jazz", new HashSet<string> { "Saxophone", "Trumpet", "DoubleBass" } }
    };

    // Descrizioni bonus/malus per ogni combinazione
    private Dictionary<string, Dictionary<string, string>> descrizioniBonusMalus = new Dictionary<string, Dictionary<string, string>>
    {
        {
            "Metal", new Dictionary<string, string>
            {
                {"ElectricBass", "Dash cooldown diminuito"},
                {"Drum", "Danno aumentato"},
                {"Guitar", "Aumentata velocità di movimento"},
                {"Violin", "Dash cooldown aumentato"},
                {"DoubleBass",  "Danno diminuito"},
                {"Tambourine", "Diminuita velocità di movimento"}
            }
        },
        {
            "Classico", new Dictionary<string, string>
            {
                {"Violin", "Dash cooldown diminuito"},
                {"Clarinet", "Danno aumentato"},
                {"DoubleBass", "Aumentata velocità di movimento"},
                {"ElectricBass", "Dash cooldown aumentato"},
                {"Drum", "Danno diminuito"},
                {"Guitar","Diminuita velocità di movimento"}
            }
        },
        {
            "Jazz", new Dictionary<string, string>
            {
                {"Saxophone", "Dash cooldown diminuito"},
                {"Trumpet", "Danno aumentato"},
                {"DoubleBass", "Aumentata velocità di movimento"},
                {"Guitar", "Dash cooldown aumentato"},
                {"ElectricBass", "Danno diminuito"},
                {"Flute", "Diminuita velocità di movimento"}
            }
        }
    };


    [System.Serializable] public struct StatEffect
    {
        public string statName;
        public float value;
    }

        private Dictionary<string, Dictionary<string, StatEffect>> effettiNumerici =
        new Dictionary<string, Dictionary<string, StatEffect>>
    {
        {
            "Metal", new Dictionary<string, StatEffect>
            {
                { "ElectricBass", new StatEffect { statName = "dashcd", value = -2f } },
                { "Violin", new StatEffect { statName = "movementspeed", value = -5f } },
                { "Drum", new StatEffect { statName = "damage", value = +10f } },
                { "DoubleBass", new StatEffect { statName = "dashcd", value = +2f } },
                { "Guitar",  new StatEffect { statName = "movementspeed", value = +10f } },
                { "Tambourine", new StatEffect { statName = "damage",  value = -5f } }
            }
        },
        {
            "Classico", new Dictionary<string, StatEffect>
            {
                { "Violin",new StatEffect { statName = "movementspeed", value = +5f } },
                { "Clarinet", new StatEffect { statName = "damage", value = +5f } },
                { "DoubleBass", new StatEffect { statName = "dashcd", value = -1f } },
                { "ElectricBass", new StatEffect { statName = "dashcd", value = +1f } },
                { "Drum", new StatEffect { statName = "damage", value = -5f } },
                { "Guitar", new StatEffect { statName = "movementspeed", value = -3f } }
            }
        },
        {
            "Jazz", new Dictionary<string, StatEffect>
            {
                { "Trumpet", new StatEffect { statName = "damage", value = +5f } },
                { "Saxophone", new StatEffect { statName = "dashcd", value = -1f } },
                { "DoubleBass", new StatEffect { statName = "movementspeed", value = +3f}},
                { "ElectricBass", new StatEffect { statName = "damage", value = -5f }},
                { "Guitar",new StatEffect { statName = "dashcd", value = +1f } },
                { "Flute", new StatEffect { statName = "movementspeed", value = +2f } }
            }
        }
    };

    void Start()
    {
        AudioSource[] allSources = {
            powerchords, bass, doublebass, guitar, violin, guitar2, violin2, drum, tambourine, trumpet, piano,flute, clarinet
        };

        double startTime = AudioSettings.dspTime + 1.0;
        foreach (AudioSource source in allSources)
        {
            if (source != null)
            {
                source.ignoreListenerPause = true;
                source.volume = 0f;
                source.PlayScheduled(startTime);
            }
        }

        powerchords.volume = 1f;

         /*bass_image.SetActive(false);
         doublebass_image.SetActive(false);
        /*if (guitar_image != null)*/ //guitar_image.SetActive(false);
        /*if (violin_image != null)*/ //violin_image.SetActive(false);
        /*if (drum_image != null)*/ //drum_image.SetActive(false);
        /*if (tambourine_image != null) */ //tambourine_image.SetActive(false);
        /*if (trumpet_image != null) */ // trumpet_image.SetActive(false);
        /*if (flute_image != null)*/ //flute_image.SetActive(false);
        /*if (saxophone_image != null)*/ //saxophone_image.SetActive(false);
        /*if (piano_image != null)*/ // piano_image.SetActive(false);


        if(feedbackLine != null)
        {
            foreach(var line in feedbackLine)
            {
                if(line != null)
                {
                    line.text = "";
                    line.color = Color.white;
                }
            }
        }
    }

    public void CheckPoint1()
    {
        resetImages();
        if (genereCorrente == "Metal")
    {
        cardLParent.transform.Find("ElectricBass").gameObject.SetActive(true);
        cardRParent.transform.Find("DoubleBass").gameObject.SetActive(true);
    }
    else if (genereCorrente == "Jazz")
    {
        cardLParent.transform.Find("Trumpet").gameObject.SetActive(true);
        cardRParent.transform.Find("Flute").gameObject.SetActive(true);
    }
    else if (genereCorrente == "Classico")
    {
        cardLParent.transform.Find("Violin").gameObject.SetActive(true);
        cardRParent.transform.Find("ElectricBass").gameObject.SetActive(true);
    }

        CARDUI.SetActive(true);
    }

    public void CheckPoint2()
    {
        resetImages();
         if (genereCorrente == "Metal")
    {
        cardLParent.transform.Find("Violin").gameObject.SetActive(true);
        cardRParent.transform.Find("Guitar").gameObject.SetActive(true);
    }
    else if (genereCorrente == "Jazz")
    {
        cardLParent.transform.Find("Guitar").gameObject.SetActive(true);
        cardRParent.transform.Find("Saxophone").gameObject.SetActive(true);
    }
    else if (genereCorrente == "Classico")
    {
        cardLParent.transform.Find("Drum").gameObject.SetActive(true);
        cardRParent.transform.Find("DoubleBass").gameObject.SetActive(true);
    }

        CARDUI.SetActive(true);
    }

    public void CheckPoint3()
    {
        resetImages();
        if (genereCorrente == "Metal")
    {
        cardLParent.transform.Find("Drum").gameObject.SetActive(true);
        cardRParent.transform.Find("Tambourine").gameObject.SetActive(true);
    }
    else if (genereCorrente == "Jazz")
    {
        cardLParent.transform.Find("DoubleBass").gameObject.SetActive(true);
        cardRParent.transform.Find("ElectricBass").gameObject.SetActive(true);
    }
    else if (genereCorrente == "Classico")
    {
        cardLParent.transform.Find("Guitar").gameObject.SetActive(true);
        cardRParent.transform.Find("Clarinet").gameObject.SetActive(true);
    }
        CARDUI.SetActive(true);
    }

 

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            PointerEventData pointerData = new PointerEventData(eventSystem);
            pointerData.position = Input.mousePosition;

            List<RaycastResult> results = new List<RaycastResult>();
            raycaster.Raycast(pointerData, results);

            foreach (RaycastResult result in results)
            {
                if (result.gameObject.CompareTag("CLICKABLE"))
                {
                    string side = result.gameObject.name.Equals("Butt1") ? "L" : "R";
                    string chosen = CheckActive(side);
                    bool isCorrect = ValutaScelta(chosen);
                    Debug.Log($"CLICK: genere={genereCorrente}, chosen={chosen}, isCorrect={isCorrect}");
                    Debug.Log("activateTrack: " + genereCorrente + " / " + chosen);


                    ShowChoiceFeedback(isCorrect, chosen);

                    activateTrack(chosen);

                    Time.timeScale = 1f;
                    CARDUI.SetActive(false);
                    FightManager.Instance.cardChosing = false;
                    Cursor.visible = false;
                    Cursor.lockState = CursorLockMode.Locked;
                }
            }
        }
    }

    public void ShowChoiceFeedback(bool isCorrect, string strumento)
    {
        if(feedbackLine == null || feedbackLine.Length == 0)
        {
            return;
        }


        if(feedbackIndex < 0 || feedbackIndex >= feedbackLine.Length)
        {
            Debug.LogWarning($"feedbackIndex fuori range: {feedbackIndex}, resetto a 0");
            feedbackIndex = 0;
        }
        string descrizione = "";
        if (descrizioniBonusMalus.ContainsKey(genereCorrente) 
            && descrizioniBonusMalus[genereCorrente].ContainsKey(strumento))
        {
            descrizione = descrizioniBonusMalus[genereCorrente][strumento];
            Debug.Log($"ShowChoiceFeedback: genere={genereCorrente}, strumento={strumento}");

        }
        else
        {
            descrizione = $"{strumento}: nessuna descrizione definita";
        }

        TMP_Text line = feedbackLine[feedbackIndex];
        if(line != null)
        {
            line.text = descrizione;
            line.color = isCorrect ? Color.green : Color.red;    
        }

        feedbackIndex = (feedbackIndex + 1) % feedbackLine.Length;
        
        
    }

   

    // True se lo strumento è tra quelli bonus del genere corrente
    private bool ValutaScelta(string sceltaStrumento)
    {
        if (strumentiPerGenere.ContainsKey(genereCorrente))
            return strumentiPerGenere[genereCorrente].Contains(sceltaStrumento);
        return false;
    }

   public void resetImages()
{
    if (cardLParent != null)
    {
        foreach (Transform g in cardLParent.transform)
            g.gameObject.SetActive(false);
    }

    if (cardRParent != null)
    {
        foreach (Transform g in cardRParent.transform)
            g.gameObject.SetActive(false);
    }
}

    public string CheckActive(string side)
    {
        string chosen_instr = "";
        GameObject parent = side.Equals("L") ? cardLParent : cardRParent;

        if (parent != null)
        {
            foreach (Transform g in parent.transform)
            {
                if (g.gameObject.activeInHierarchy)
                {
                    chosen_instr = g.name;
                    break;     // trovata la carta attiva
                }
            }
        }


        /*if (side.Equals("L"))
        {
            GameObject parent1 = drum_image.gameObject.transform.parent?.gameObject;
            foreach (Transform g in parent1.transform)
            {
                if (g.gameObject.activeInHierarchy) chosen_instr = g.name;
            }
        }
        else if (side.Equals("R"))
        {
            GameObject parent2 = tambourine_image.gameObject.transform.parent?.gameObject;
            foreach (Transform g in parent2.transform)
            {
                if (g.gameObject.activeInHierarchy) chosen_instr = g.name;
            }
        }*/
        Debug.Log($"Genere={genereCorrente}, chosen={chosen_instr}");

        return chosen_instr;
    }

    public void RotateZ(GameObject target, float angle)
    {
        if (target != null && !isRotating)
        {
            isRotating = true;
            StartCoroutine(RotateOverTime(target.transform, angle));
        }
    }

    private IEnumerator RotateOverTime(Transform targetTransform, float angle)
    {
        Quaternion startRotation = targetTransform.rotation;
        Quaternion endRotation = startRotation * Quaternion.Euler(0f, 0f, angle);

        float elapsed = 0f;

        while (elapsed < RotationDuration)
        {
            targetTransform.rotation = Quaternion.Slerp(startRotation, endRotation, elapsed / RotationDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        targetTransform.rotation = endRotation;
        isRotating = false;
    }

    public void activateTrack(string instrument)
    {
        if(effettiNumerici.ContainsKey(genereCorrente) && effettiNumerici[genereCorrente].ContainsKey(instrument))
        {
            StatEffect eff = effettiNumerici[genereCorrente][instrument];
            FightManager.Instance.UpdateGameStat(eff.statName, eff.value);
        }
        switch (instrument)
        {
            case ("ElectricBass"):
                //FightManager.Instance.UpdateGameStat("dashcd", -2f);
                if (bass != null)bass.volume = 1f;
                break;
            case ("Violin"):
                //FightManager.Instance.UpdateGameStat("movementspeed", -5f);
                if (violin != null) violin.volume = 1f;
                if (violin2 != null) violin2.volume = 1f;
                break;
            case ("Drum"):
                //FightManager.Instance.UpdateGameStat("damage", 10f);
                if(drum != null) drum.volume = 1f;
                break;
            case ("DoubleBass"):
                //FightManager.Instance.UpdateGameStat("dashcd", +2f);
               if(doublebass != null) doublebass.volume = 1f;
                break;
            case ("Guitar"):
                //FightManager.Instance.UpdateGameStat("movementspeed", +10f);
                if (guitar != null) guitar.volume = 1f; 

                if(guitar2 != null) guitar2.volume = 1f;
                
                break;
            case ("Tambourine"):
                //FightManager.Instance.UpdateGameStat("damage", -5f);
                if(tambourine !=null) tambourine.volume = 1f;
                break;

            case("Trumpet"):
                if(trumpet != null) trumpet.volume = 0.6f;  
                break;

            case("Flute"):
                if(flute != null) flute.volume = 0.6f;       
                break;

            case("Piano"):
                if(piano != null) piano.volume = 1f;
                break;

             case ("Saxophone"):
                if (saxophone != null) saxophone.volume = 1f;
                break;

            case("Clarinet"):
                if(clarinet != null) clarinet.volume = 1f;
                break;
        }
         Debug.Log("activateTrack: " + genereCorrente + " / " + instrument);
    }


    public void setAllVolumeToZero()
    {
        AudioSource[] allSources = {
            powerchords, bass, doublebass, guitar, violin, guitar2, violin2, drum, tambourine, trumpet, saxophone, piano, flute
        };
        foreach (AudioSource source in allSources)
        {
            source.volume = 0f;
        }
        Debug.Log("SET ALL VOLUME TO ZERO");
    }
}
