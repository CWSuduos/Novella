using UnityEngine;
using UnityEngine.SceneManagement; // Для управления сценами
using UnityEngine.UI; // Для работы с UI элементами
using System.Collections;

public class UIManager : MonoBehaviour
{
    public Button startButton; // Кнопка старта
    public Button exitButton;  // Кнопка выхода
    public MonoBehaviour[] scriptsToExecute; // Массив скриптов, которые нужно запустить на новой сцене
    private bool isLoadingScene = false;  // Флаг для проверки загрузки сцены

    // Статическое поле для хранения скриптов, доступное между сценами
    public static MonoBehaviour[] scriptsToRunOnLoad;

    void Start()
    {
        // Убедитесь, что этот объект не уничтожается при загрузке новой сцены
        DontDestroyOnLoad(gameObject);

        // Настроим кнопки
        startButton.onClick.AddListener(OnStartButtonClicked);
        exitButton.onClick.AddListener(OnExitButtonClicked);
    }

    // Функция для перехода на игровую сцену
    public void OnStartButtonClicked()
    {
        // Запускаем асинхронную загрузку сцены
        if (!isLoadingScene)
        {
            isLoadingScene = true;

            // Передаем массив скриптов в статическое поле
            scriptsToRunOnLoad = scriptsToExecute;

            StartCoroutine(LoadScene("FirstLocation")); // Замените "FirstLocation" на имя вашей сцены
        }
    }

    // Асинхронная загрузка сцены
    private IEnumerator LoadScene(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        // Ждем, пока сцена полностью загрузится
        while (!asyncLoad.isDone) 
        {
            yield return null;
        }
    }

    // Функция для выхода из приложения
    public void OnExitButtonClicked()
    {
        Application.Quit();
    }
}
