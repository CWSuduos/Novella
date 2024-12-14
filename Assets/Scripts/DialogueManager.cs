using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // Не забудьте добавить эту строку
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class DialogueManager : MonoBehaviour
{
    public Text dialogueText; // Ссылка на текстовое поле для отображения диалогов
    public float typingSpeed = 0.05f; // Скорость появления текста
    public CharactersData charactersData; // Ссылка на скрипт CharactersData
    public Dropdown languageDropdown; // Выпадающий список для выбора языка
    public string endSceneName; // Название сцены для перехода после завершения диалогов
    public string indexText;

    private List<Dialogue> dialogues; // Список всех диалогов
    private int currentDialogueIndex = 0; // Индекс текущего диалога
    private string selectedLanguage = "en"; // Выбранный язык ("en" для английского, "ru" для русского)
    private bool isTyping = false; // Флаг для проверки, печатается ли текст
    private string endText; // Текст, который будет показан после окончания диалогов

    void Start()
    {
        // Проверка инициализации полей
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

        // Инициализация выпадающего списка
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
        if (Input.GetMouseButtonDown(0) && !isTyping) // Проверяем, нажата ли левая кнопка мыши и не печатается ли текст
        {
            NextDialogue();
        }
    }

    // Загрузка диалогов из JSON файла в зависимости от выбранного языка
    void LoadDialogues(string language, string _indexText)
    {
        _indexText = indexText;
        string filePath = Path.Combine(Application.streamingAssetsPath, $"dialogues_{language}{_indexText}.json");
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            DialogueList dialogueList = JsonUtility.FromJson<DialogueList>(json);
            dialogues = dialogueList.texts;
            endText = dialogueList.endText; // Загружаем конечный текст
        }
        else
        {
            Debug.LogError($"JSON file for language '{language}' not found!");
        }
    }

    // Отображаем диалог по его индексу с анимацией текста
    void DisplayDialogue(int index)
    {
        if (index >= 0 && index < dialogues.Count)
        {
            Dialogue dialogue = dialogues[index];
            StartCoroutine(TypeText(dialogue.text)); // Запускаем корутину для анимации текста
            // Обновляем размер персонажа для текущего диалога
            if (charactersData != null)
            {
                charactersData.UpdateCharacterSize(dialogue.id, true);
            }
        }
        else
        {
            StartCoroutine(EndDialogueAndSwitchScene()); // Показать конечный текст и перейти на другую сцену
        }
    }

    // Переходим к следующему диалогу
    void NextDialogue()
    {
        if (currentDialogueIndex < dialogues.Count - 1)
        {
            // Логируем завершение текущего диалога
            LogDialogueCompletion(currentDialogueIndex);

            // Обновляем размер персонажа для текущего диалога
            if (charactersData != null)
            {
                charactersData.UpdateCharacterSize(dialogues[currentDialogueIndex].id, false);
            }

            currentDialogueIndex++;
            DisplayDialogue(currentDialogueIndex);
        }
        else
        {
            // Логируем завершение последнего диалога
            LogDialogueCompletion(currentDialogueIndex);

            // Логируем, что диалоги закончились
            Debug.Log("All dialogues have ended.");

            // Показываем сообщение о завершении диалогов и переходим на другую сцену
            StartCoroutine(EndDialogueAndSwitchScene());
        }
    }

    // Корутину для анимации текста
    IEnumerator TypeText(string text)
    {
        isTyping = true;
        dialogueText.text = ""; // Сначала очищаем текстовое поле
        foreach (char letter in text.ToCharArray())
        {
            dialogueText.text += letter; // Добавляем букву по одной
            yield return new WaitForSeconds(typingSpeed); // Ждем указанное время перед добавлением следующей буквы
        }
        isTyping = false;
    }

    // Обработчик изменения языка
    void OnLanguageChanged(Dropdown dropdown)
    {
        selectedLanguage = dropdown.options[dropdown.value].text;
        LoadDialogues(selectedLanguage, indexText);
        currentDialogueIndex = 0; // Сброс индекса диалога при смене языка
        DisplayDialogue(currentDialogueIndex);
    }

    // Метод для запуска диалогов
    public void StartDialogue()
    {
        LoadDialogues(selectedLanguage, indexText); // Загрузка диалогов для текущего языка
        currentDialogueIndex = 0; // Сброс индекса диалога
        DisplayDialogue(currentDialogueIndex); // Начало диалога
    }

    // Корутину для показа конечного текста и перехода на другую сцену
    IEnumerator EndDialogueAndSwitchScene()
    {
        // Показ конечного текста
        yield return TypeText(endText);

        // Переход на другую сцену после завершения текста
        if (!string.IsNullOrEmpty(endSceneName))
        {
            SceneManager.LoadScene(endSceneName); // Переход на указанную сцену
        }
        else
        {
            Debug.LogError("End scene name is not set!");
        }
    }

    // Логируем завершение диалога
    void LogDialogueCompletion(int index)
    {
        if (index >= 0 && index < dialogues.Count)
        {
            Dialogue dialogue = dialogues[index];
            Debug.Log($"Dialogue with ID {dialogue.id} and text '{dialogue.text}' has ended.");
        }
    }
}

// Класс для хранения информации о диалоге
[System.Serializable]
public class Dialogue
{
    public int id;
    public string text;
}

// Класс для хранения списка диалогов и конечного текста
[System.Serializable]
public class DialogueList
{
    public List<Dialogue> texts;
    public string endText;
}
