using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    public string DEBUGETXT = "UIMANAGER ISTANCE IS WORKING";
    public Image CadenceIcon;
    public Image CadenceLife;
    public Image InfernoRiffLife;
    public Image Victory;
    public Image GameOver;
    public Image Roll;
    public RawImage FinishBackGround;


    public TextMeshProUGUI cd_text;
    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }


    void Start()
    {
        CadenceIcon.gameObject.SetActive(false);
        InfernoRiffLife.gameObject.SetActive(false);
        CadenceLife.gameObject.SetActive(false);
        Roll.gameObject.SetActive(false);
    }

    public void StartFight()
    {
        CadenceIcon.gameObject.SetActive(true);
        InfernoRiffLife.gameObject.SetActive(true);
        CadenceLife.gameObject.SetActive(true);
        Roll.gameObject.SetActive(true);
    }

    public void SetTextFight(bool value,string final)
    {
        if (final.Equals("Victory"))
        {
            Victory.gameObject.SetActive(value);
        }
        if (final.Equals("Lose"))
        {
            GameOver.gameObject.SetActive(value);
        }

    }

    public void UpdateLifeBar(float value,string character)
    {
        if (character.Equals("Enemy"))
        {
            Debug.Log("ENEMY PERCENTAGE " + value);
            InfernoRiffLife.fillAmount = value;
        }
        if(character.Equals("Cadence"))
        {
            Debug.Log("CADENCE PERCENTAGE " + value);
            CadenceLife.fillAmount = value;
        }
    }


    public void GameOverUI()
    {
        CadenceIcon.gameObject.SetActive(false);
        InfernoRiffLife.gameObject.SetActive(false);
        CadenceLife.gameObject.SetActive(false);
        Roll.gameObject.SetActive(false);
        GameOver.gameObject.SetActive( true);
        FinishBackGround.gameObject.SetActive( true);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        FightManager.Instance.cardChosing = true;
        //StartCoroutine(FadeInRoutine(FinishBackGround));
    }


    public void WonUI()
    {
        CadenceIcon.gameObject.SetActive(false);
        InfernoRiffLife.gameObject.SetActive(false);
        CadenceLife.gameObject.SetActive(false);
        Roll.gameObject.SetActive(false);
        Victory.gameObject.SetActive(true);
        FinishBackGround.gameObject.SetActive(true);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        FightManager.Instance.cardChosing = true;
        //StartCoroutine(FadeInRoutine(FinishBackGround));
    }


    IEnumerator FadeInRoutine(RawImage sprite)
    {
        sprite.gameObject.SetActive(true);
        float elapsedTime = 0f;
        Color originalColor = sprite.color;
        float fadeDuration = 5f;
        while (elapsedTime < fadeDuration) 
        {
            float alpha = Mathf.Lerp(0f, 255f, elapsedTime / fadeDuration);
            sprite.color = new Color(0f, 0f, 0f, alpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Assicura che l'opacita' sia esattamente 0 alla fine
        sprite.color = new Color (0f, 0f, 0f, 1f);
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
