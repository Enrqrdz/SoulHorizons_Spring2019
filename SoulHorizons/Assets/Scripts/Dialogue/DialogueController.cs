using UnityEngine;
using UnityEngine.UI;

public class DialogueController : MonoBehaviour
{
    public static DialogueController Instance { get; private set; }

    [Tooltip("Dialogue for the entire scene.")]
    public Dialogue dialogue;
    public Text displayText;
    public Sprite portraitToDisplay;
    private static int conversationIndex = 0;
    private static int decisionIndex = 0;
    private bool decisionIsBeingMade = false;
    private CharacterName characterOnScreen;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        //Read text and see when characters start talking
    }

    private void Update()
    {
        if (decisionIsBeingMade == false)
        {
            SetFontStyle();
            DisplayDialogueBox();
            CheckForDecision();
            ContinueConversation();
        }
        else
        {
            FinishDecisionMaking();
        }
    }

    private void SetFontStyle()
    {
        if (dialogue.dialogueBox[conversationIndex].characterTalking.characterName == CharacterName.Nobody)
        {
            displayText.fontStyle = FontStyle.BoldAndItalic;
        }
        else
        {
            displayText.fontStyle = FontStyle.Normal;
        }
    }

    private void DisplayDialogueBox()
    {
        displayText.text = dialogue.text[conversationIndex];
        //Display portrait
    }
    
    private void CheckForDecision()
    {
        if (dialogue.decisionIndexes.Contains(conversationIndex))
        {
            StartDecisionMaking();
        }
    }

    private void StartDecisionMaking()
    {
        decisionIsBeingMade = true;
        for (int i = 0; i < dialogue.decisions[decisionIndex].choices.Length; i++)
        {
            dialogue.decisions[decisionIndex].choices[i].gameObject.SetActive(true);
        }
    }

    private void FinishDecisionMaking()
    {
        //Only room for 4 decisions using InputManager's Action Number
        int selectionIndex = InputManager.ActionNumber();
        if(dialogue.decisions[decisionIndex].consequences[selectionIndex].ConsequenceHasFinished())
        {
            for (int i = 0; i < dialogue.decisions[decisionIndex].choices.Length; i++)
            {
                dialogue.decisions[decisionIndex].choices[i].gameObject.SetActive(false);
            }
            SafelyIncrementDecisionIndex();
            decisionIsBeingMade = false;
        }
    }

    private void SafelyIncrementConversationIndex()
    {
        if (conversationIndex < dialogue.dialogueBox.Count - 2)
        {
            conversationIndex++;
        }
    }

    private void SafelyIncrementDecisionIndex()
    {
        if(decisionIndex < dialogue.decisions.Count - 2)
        {
            decisionIndex++;
        }
    }

    private void ContinueConversation()
    {
        if (Input.GetKey("PlayCard3_Button"))
        {
            SafelyIncrementConversationIndex();
        }
    }
}
