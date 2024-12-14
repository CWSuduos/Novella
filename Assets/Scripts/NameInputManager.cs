using UnityEngine;
using UnityEngine.UI;

public class NameInputManager : MonoBehaviour
{
    public GameObject nameInputPanel; // Панель с текстовым полем и кнопкой
    public InputField nameInputField; // Поле для ввода имени
    public Button submitButton; // Кнопка для подтверждения ввода
    public DialogueManager dialogueManager; // Скрипт DialogueManager, который нужно запустить
    [SerializeField]
    private GameObject[] objectsToHide; // Массив объектов, которые нужно скрыть до ввода имени

    void Start()
    {
        // Скрываем панель при старте
        nameInputPanel.SetActive(true);

        // Скрываем объекты, которые не должны быть видны до ввода имени
        SetObjectsVisibility(false);

        // Подключаем обработчик кнопки
        submitButton.onClick.AddListener(OnSubmitButtonClicked);

        // Проверяем, сохранено ли имя игрока
        if (PlayerPrefs.HasKey("PlayerName"))
        {
            // Если имя сохранено, показываем объекты и запускаем диалог сразу
            nameInputPanel.SetActive(false);
            SetObjectsVisibility(true);
            if (dialogueManager != null)
            {
                dialogueManager.StartDialogue();
            }
        }
    }

    void OnSubmitButtonClicked()
    {
        string playerName = nameInputField.text;

        if (!string.IsNullOrEmpty(playerName))
        {
            // Сохраняем имя игрока
            PlayerPrefs.SetString("PlayerName", playerName);

            // Сохраняем изменения
            PlayerPrefs.Save();

            // Скрываем панель после ввода имени
            nameInputPanel.SetActive(false);

            // Показываем скрытые объекты
            SetObjectsVisibility(true);

            // Запускаем DialogueManager
            if (dialogueManager != null)
            {
                dialogueManager.StartDialogue(); // Запуск диалога
            }
        }
        else
        {
            // Логика для отображения ошибки, если имя не введено
            Debug.Log("Please enter a valid name.");
        }
    }

    // Метод для управления видимостью объектов
    void SetObjectsVisibility(bool isVisible)
    {
        Debug.Log($"Setting objects visibility to {isVisible}"); // Добавляем отладочное сообщение
        foreach (GameObject obj in objectsToHide)
        {
            if (obj != null)
            {
                Debug.Log($"Setting visibility for {obj.name} to {isVisible}"); // Отладочное сообщение для каждого объекта
                obj.SetActive(isVisible);
            }
            else
            {
                Debug.LogWarning("Object in objectsToHide is null");
            }
        }
    }
}
