using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // �� �������� �������� ��� ������
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class DialogueManager : MonoBehaviour
{
    public Text dialogueText; // ������ �� ��������� ���� ��� ����������� ��������
    public float typingSpeed = 0.05f; // �������� ��������� ������
    public CharactersData charactersData; // ������ �� ������ CharactersData
    public Dropdown languageDropdown; // ���������� ������ ��� ������ �����
    public string endSceneName; // �������� ����� ��� �������� ����� ���������� ��������
    public string indexText;

    private List<Dialogue> dialogues; // ������ ���� ��������
    private int currentDialogueIndex = 0; // ������ �������� �������
    private string selectedLanguage = "en"; // ��������� ���� ("en" ��� �����������, "ru" ��� ��������)
    private bool isTyping = false; // ���� ��� ��������, ���������� �� �����
    private string endText; // �����, ������� ����� ������� ����� ��������� ��������

    void Start()
    {
        // �������� ������������� �����
        if (dialogueText == null)
        {
            Debug.LogError("DialogueText is not assigned in the inspector.");
        }
        if (charactersData == null)
        {
            Debug.LogError("CharactersData is not assigned in the inspector.");
        }
        if (languageDropdown == null)
        {
            Debug.LogError("LanguageDropdown is not assigned in the inspector.");
        }

        // ������������� ����������� ������
        if (languageDropdown != null)
        {
            languageDropdown.onValueChanged.AddListener(delegate {
                OnLanguageChanged(languageDropdown);
            });
        }

        LoadDialogues(selectedLanguage,indexText);
        DisplayDialogue(currentDialogueIndex);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isTyping) // ���������, ������ �� ����� ������ ���� � �� ���������� �� �����
        {
            NextDialogue();
        }
    }

    // �������� �������� �� JSON ����� � ����������� �� ���������� �����
    void LoadDialogues(string language, string _indexText)
    {
        _indexText = indexText;
        string filePath = Path.Combine(Application.streamingAssetsPath, $"dialogues_{language}{_indexText}.json");
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            DialogueList dialogueList = JsonUtility.FromJson<DialogueList>(json);
            dialogues = dialogueList.texts;
            endText = dialogueList.endText; // ��������� �������� �����
        }
        else
        {
            Debug.LogError($"JSON file for language '{language}' not found!");
        }
    }

    // ���������� ������ �� ��� ������� � ��������� ������
    void DisplayDialogue(int index)
    {
        if (index >= 0 && index < dialogues.Count)
        {
            Dialogue dialogue = dialogues[index];
            StartCoroutine(TypeText(dialogue.text)); // ��������� �������� ��� �������� ������
            // ��������� ������ ��������� ��� �������� �������
            if (charactersData != null)
            {
                charactersData.UpdateCharacterSize(dialogue.id, true);
            }
        }
        else
        {
            StartCoroutine(EndDialogueAndSwitchScene()); // �������� �������� ����� � ������� �� ������ �����
        }
    }

    // ��������� � ���������� �������
    void NextDialogue()
    {
        if (currentDialogueIndex < dialogues.Count - 1)
        {
            // �������� ���������� �������� �������
            LogDialogueCompletion(currentDialogueIndex);

            // ��������� ������ ��������� ��� �������� �������
            if (charactersData != null)
            {
                charactersData.UpdateCharacterSize(dialogues[currentDialogueIndex].id, false);
            }

            currentDialogueIndex++;
            DisplayDialogue(currentDialogueIndex);
        }
        else
        {
            // �������� ���������� ���������� �������
            LogDialogueCompletion(currentDialogueIndex);

            // ��������, ��� ������� �����������
            Debug.Log("All dialogues have ended.");

            // ���������� ��������� � ���������� �������� � ��������� �� ������ �����
            StartCoroutine(EndDialogueAndSwitchScene());
        }
    }

    // �������� ��� �������� ������
    IEnumerator TypeText(string text)
    {
        isTyping = true;
        dialogueText.text = ""; // ������� ������� ��������� ����
        foreach (char letter in text.ToCharArray())
        {
            dialogueText.text += letter; // ��������� ����� �� �����
            yield return new WaitForSeconds(typingSpeed); // ���� ��������� ����� ����� ����������� ��������� �����
        }
        isTyping = false;
    }

    // ���������� ��������� �����
    void OnLanguageChanged(Dropdown dropdown)
    {
        selectedLanguage = dropdown.options[dropdown.value].text;
        LoadDialogues(selectedLanguage, indexText);
        currentDialogueIndex = 0; // ����� ������� ������� ��� ����� �����
        DisplayDialogue(currentDialogueIndex);
    }

    // ����� ��� ������� ��������
    public void StartDialogue()
    {
        LoadDialogues(selectedLanguage, indexText); // �������� �������� ��� �������� �����
        currentDialogueIndex = 0; // ����� ������� �������
        DisplayDialogue(currentDialogueIndex); // ������ �������
    }

    // �������� ��� ������ ��������� ������ � �������� �� ������ �����
    IEnumerator EndDialogueAndSwitchScene()
    {
        // ����� ��������� ������
        yield return TypeText(endText);

        // ������� �� ������ ����� ����� ���������� ������
        if (!string.IsNullOrEmpty(endSceneName))
        {
            SceneManager.LoadScene(endSceneName); // ������� �� ��������� �����
        }
        else
        {
            Debug.LogError("End scene name is not set!");
        }
    }

    // �������� ���������� �������
    void LogDialogueCompletion(int index)
    {
        if (index >= 0 && index < dialogues.Count)
        {
            Dialogue dialogue = dialogues[index];
            Debug.Log($"Dialogue with ID {dialogue.id} and text '{dialogue.text}' has ended.");
        }
    }
}

// ����� ��� �������� ���������� � �������
[System.Serializable]
public class Dialogue
{
    public int id;
    public string text;
}

// ����� ��� �������� ������ �������� � ��������� ������
[System.Serializable]
public class DialogueList
{
    public List<Dialogue> texts;
    public string endText;
}
