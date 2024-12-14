using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InputManager : MonoBehaviour
{
    // Ссылка на поле ввода имени (InputField)
    public InputField nameInputField;

    // Ссылка на текстовое поле для отображения имени
    public Text nameDisplayText;

    // Массив скриптов, которые будут запускаться после ввода имени
    public MonoBehaviour[] scriptsToExecute;

    // Имя игрока
    private string playerName;

    // Название сцены, при которой скрипт должен запуститься
    public string targetSceneName;

    private bool scriptsExecuted = false; // Проверка, были ли уже запущены скрипты

    private string currentSceneName = ""; // Переменная для отслеживания текущей активной сцены

    void Start()
    {
        // Проверка на null и отладка
        if (nameInputField == null)
        {
            Debug.LogError("nameInputField is not assigned!");
        }
        if (nameDisplayText == null)
        {
            Debug.LogError("nameDisplayText is not assigned!");
        }
        if (scriptsToExecute == null)
        {
            Debug.LogError("scriptsToExecute is not assigned!");
        }

        // Отключаем все скрипты из массива при старте
        DisableAllScripts();
    }

    void Update()
    {
        // Получаем текущую активную сцену
        Scene activeScene = SceneManager.GetActiveScene();

        // Проверяем, изменилась ли сцена
        if (currentSceneName != activeScene.name)
        {
            currentSceneName = activeScene.name;

            // Если сцена соответствует целевой сцене и скрипты еще не были запущены
            if (currentSceneName == targetSceneName && !scriptsExecuted)
            {
                // Проверяем состояние InputField и запускаем скрипты
                ExecuteInputFieldCheck();
                scriptsExecuted = true;
            }
        }

        // Проверяем, заполнено ли поле ввода и нажата ли клавиша Enter
        if (Input.GetKeyDown(KeyCode.Return) && !string.IsNullOrEmpty(nameInputField?.text))
        {
            SaveNameAndExecuteScripts();
        }
    }

    // Метод для проверки и запуска скриптов после ввода имени
    void ExecuteInputFieldCheck()
    {
        // Если поле ввода активно, ожидаем ввода имени
        if (nameInputField != null && nameInputField.gameObject.activeSelf)
        {
            // Ждем нажатия клавиши Enter
            Debug.Log("InputField is active, waiting for name input...");
        }
        else
        {
            Debug.LogWarning("nameInputField is null or not active");
        }
    }

    // Метод для сохранения имени и запуска скриптов
    public void SaveNameAndExecuteScripts()
    {
        // Получаем текст из InputField
        if (nameInputField != null)
        {
            playerName = nameInputField.text;
        }

        // Проверяем, чтобы имя не было пустым
        if (!string.IsNullOrEmpty(playerName))
        {
            // Отображаем только имя в текстовом поле
            if (nameDisplayText != null)
            {
                nameDisplayText.text = playerName;
            }

            // Скрываем поле ввода, так как имя введено
            if (nameInputField != null)
            {
                nameInputField.gameObject.SetActive(false);
            }

            // Запускаем скрипты в массиве
            ExecuteScripts();

            // Вызов новой функции после выполнения скриптов
            OnScriptsExecuted();
        }
        else
        {
            // Если поле ввода пустое, выводим сообщение об ошибке
            if (nameDisplayText != null)
            {
                nameDisplayText.text = "Please enter a valid name!";
            }
        }
    }

    // Метод для отключения всех скриптов в массиве
    private void DisableAllScripts()
    {
        if (scriptsToExecute != null)
        {
            foreach (MonoBehaviour script in scriptsToExecute)
            {
                if (script != null)
                {
                    script.enabled = false; // Отключаем скрипты
                }
                else
                {
                    Debug.LogWarning("Script in scriptsToExecute is null");
                }
            }
        }
    }

    // Метод для запуска всех скриптов в массиве
    private void ExecuteScripts()
    {
        if (scriptsToExecute != null)
        {
            foreach (MonoBehaviour script in scriptsToExecute)
            {
                if (script != null)
                {
                    script.enabled = true; // Включаем скрипты
                }
                else
                {
                    Debug.LogWarning("Script in scriptsToExecute is null");
                }
            }
        }
    }

    // НОВАЯ ФУНКЦИЯ: Выполняется после того, как все скрипты были запущены
    private void OnScriptsExecuted()
    {
        // Здесь можно добавить любую дополнительную логику, например:
        Debug.Log("All scripts have been executed successfully.");

        // Пример дополнительного действия: выводим специальное сообщение на экран
        if (nameDisplayText != null)
        {
            nameDisplayText.text += "\nWelcome, " + playerName + "!";
        }
    }
}
