using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
[RequireComponent(typeof(AudioSource))]

public class DialogueController : MonoBehaviour
{
    [Tooltip("Dialogue for the entire scene.")]
    public Dialogue dialogue;
    public TextMeshProUGUI displayText;
    public Image portraitToDisplay;
    public GameObject decisionHolder;
    private static int conversationIndex = 0;
    private static int decisionIndex = 0;
    private bool decisionIsBeingMade = false;
    private List<IConsequence> consequences = new List<IConsequence>();

    AudioSource Dialogue_SFX;
    public AudioClip[] dialogues_SFX;
    public AudioClip dialogue_SFX;

    private void Start()
    {
        if (dialogue.hasBeenFormatted == false)
        {
            FormatScript();
        }
    }

    private void FormatScript()
    {
        //"^[a-zA-Z]+: " is searching for the beginning of a string (^) with a word ([a-zA-Z])
        //  of one or more characters (+), followed by a colon (:) and a space ( ).

        var myRegExpression = new Regex("^[a-zA-Z]+: ");
        CharacterName lastCharacter = CharacterName.Nobody;

        for (int i = 0; i < dialogue.text.Count; i++)
        {
            if (myRegExpression.IsMatch(dialogue.text[i]))
            {
                string[] parsedLine = dialogue.text[i].Split(':');
                Debug.Log(parsedLine.Length);
                Debug.Log(parsedLine[0]);
                Debug.Log(parsedLine[1]);
                lastCharacter = CharacterNames.NameToEnum(parsedLine[0]);
                dialogue.characterOnScreen.Add(lastCharacter);
                parsedLine = myRegExpression.Split(dialogue.text[i]);
                Debug.Log(parsedLine.Length);
                Debug.Log(parsedLine[0]);
                Debug.Log(parsedLine[1]);
                dialogue.text[i] = parsedLine[1];
            }
            else
            {
                dialogue.characterOnScreen.Add(lastCharacter);
            }
        }

        dialogue.hasBeenFormatted = true;
    }

    private void Update()
    {
        DisplayDialogueBox();
        CheckForDecision();

        if (decisionIsBeingMade == false)
        {
            ContinueConversation();
        }
        else
        {
            FinishDecisionMaking();
        }
    }

    private void DisplayDialogueBox()
    {
        SetText();
        SetPortrait();
        SetFontStyle();
    }

    private void SetText()
    {
        displayText.text = dialogue.text[conversationIndex];
    }

    private void SetPortrait()
    {
        if(dialogue.characterOnScreen[conversationIndex] != CharacterName.Nobody)
        {
            int index = 0;
            foreach(Portrait portrait in dialogue.characters)
            {
                if (dialogue.characterOnScreen[conversationIndex] == dialogue.characters[index].characterName)
                {
                    portraitToDisplay.sprite = dialogue.characters[index].characterSprite;
                    break;
                }
                else
                {
                    index++;
                }
            }
        }
        else
        {
            portraitToDisplay = null;
        }
    }

    private void SetFontStyle()
    {
        if (dialogue.characterOnScreen[conversationIndex] == CharacterName.Nobody)
        {
            displayText.fontStyle = FontStyles.Bold;
            displayText.fontStyle = FontStyles.Italic;
        }
        else
        {
            displayText.fontStyle = FontStyles.Bold;
        }
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
        if(dialogue.decisions[decisionIndex].choices[0].GetComponent<IConsequence>() != null)
        {
            for (int i = 0; i < dialogue.decisions[decisionIndex].choices.Length; i++)
            {
                consequences.Add(dialogue.decisions[decisionIndex].choices[i].GetComponent<IConsequence>());
            }
            decisionIsBeingMade = true;
            decisionHolder.SetActive(true);
        } 
    }

    private void FinishDecisionMaking()
    {
        //Only room for 4 decisions using InputManager's Action Number
        int selectionIndex = InputManager.ActionNumber();
        if (selectionIndex != -1)
        {
            consequences[selectionIndex].Consequence();
            SafelyIncrementDecisionIndex();
            decisionIsBeingMade = false;
            decisionHolder.SetActive(true);
        }
    }

    private void SafelyIncrementConversationIndex()
    {
        if (conversationIndex < dialogue.text.Count - 1)
        {
            AudioSource Dialogue_SFX = GetComponent<AudioSource>();
            Dialogue_SFX.clip = dialogue_SFX;
            Dialogue_SFX.Play();

            int index = Random.Range(0, dialogues_SFX.Length);
            dialogue_SFX = dialogues_SFX[index];
            Dialogue_SFX.clip = dialogue_SFX;
            Dialogue_SFX.Play();

            conversationIndex++;
        }
    }

    private void SafelyIncrementDecisionIndex()
    {
        if(decisionIndex < dialogue.decisions.Count - 1)
        {
            decisionIndex++;
        }
    }

    private void ContinueConversation()
    {
        if (InputManager.ActionNumber() != -1)
        {
            SafelyIncrementConversationIndex();
        }
    }
}