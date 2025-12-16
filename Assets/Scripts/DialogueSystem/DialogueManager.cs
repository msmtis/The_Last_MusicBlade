using System.Collections;

using System.Collections.Generic;

using TMPro;

using Unity.VisualScripting.Antlr3.Runtime;

using UnityEngine;

using UnityEngine.UI;
 
public class DialogueManager : MonoBehaviour

{

    [SerializeField]

    private DialogueSequence Dialogue;
 
    public GameObject DialogueCanvas;

    public GameObject image;

    public TextMeshProUGUI name;

    public TextMeshProUGUI dialogueText;

    public GameObject dialogueBox;
 
    private int lineIndex;

    private float textspeed;

    private Coroutine textAnimationCoroutine;

    public GeneralPlayerMovement playerMovement;
 
 
    public Sprite characterSprite;

    public Sprite guideSprite;

    public Sprite enemySprite;
 
    public string characterName;

    public string guideName;

    public string enemyName;
 
 
    private Dictionary<string, Sprite> nameToSprite = new Dictionary<string, Sprite>();
 
 
   /*private void Awake()

    {

        nameToSprite.Add(characterName, characterSprite);

        nameToSprite.Add(enemyName, enemySprite);

        nameToSprite.Add(guideName, guideSprite);
 
    }*/

   private void Awake()
{
    nameToSprite.Clear();

    TryAddSprite(characterName, characterSprite);
    TryAddSprite(enemyName,    enemySprite);
    TryAddSprite(guideName,    guideSprite);
}

private void TryAddSprite(string key, Sprite sprite)
{
    if (string.IsNullOrEmpty(key) || sprite == null)
        return;

    if (!nameToSprite.ContainsKey(key))
    {
        nameToSprite.Add(key, sprite);
    }
    else
    {
        Debug.LogWarning($"DialogueManager: chiave duplicata '{key}' ignorata");
    }
}
 
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()

    {

        dialogueBox.SetActive(false);

        textspeed = 0.1f;

        StartDialogue();

        Debug.Log("STARTING DIALOGUE");
 
 
        

    }
 
    private void Update()

    {
 
       

        if (Input.GetKeyDown(KeyCode.Space))

        {

            NextLine();

        }

    }
 
    public void StartDialogue()

    {

        Cursor.visible = true;                // Rende visibile il cursore

        Cursor.lockState = CursorLockMode.None;

        lineIndex = 0;

        DisplayLine();

        dialogueBox.SetActive(true);

    }
 
    public void DisplayLine()

    {

        if (lineIndex >= Dialogue.lines.Length)

        {

            EndDialogue();

            return;

        }

        //retrieve Dialogue Line

        DialogueLine line = Dialogue.lines[lineIndex];

        name.text = line.characterName;

        image.GetComponent<Image>().sprite= nameToSprite[line.characterName];
 
        //check if the coroutine is in progress

        if (textAnimationCoroutine != null)

            StopCoroutine(textAnimationCoroutine);
 
 
        

        textAnimationCoroutine = StartCoroutine(TypeSentence(line.sentence));

    }
 
    IEnumerator TypeSentence(string sentence)

    {

        dialogueText.text = "";

        Debug.Log(sentence);

        foreach (char letter in sentence)

        {

            dialogueText.text += letter;

            yield return new WaitForSeconds(textspeed);

        }

        textAnimationCoroutine = null;

    }
 
    public void NextLine()

    {

        if (textAnimationCoroutine != null)

        {

            StopCoroutine(textAnimationCoroutine);

            dialogueText.text = Dialogue.lines[lineIndex].sentence;

            textAnimationCoroutine = null;

            return;

        }
 
        lineIndex++;

        DisplayLine();

    }
 
    public void EndDialogue()

    {

        lineIndex = Dialogue.lines.Length + 1;

        Debug.Log("Dialogo terminato.");

        FightManager.Instance.cutSceneEnded = true;
        Time.timeScale = 1f;

        DialogueCanvas.SetActive(false);
        UIManager.Instance.StartFight();
        Debug.Log("Fight!");

    }
 
 
 
 
}

 