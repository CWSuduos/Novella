using UnityEngine;
using UnityEngine.SceneManagement; // ��� ���������� �������
using UnityEngine.UI; // ��� ������ � UI ����������
using System.Collections;

public class UIManager : MonoBehaviour
{
    public Button startButton; // ������ ������
    public Button exitButton;  // ������ ������
    public MonoBehaviour[] scriptsToExecute; // ������ ��������, ������� ����� ��������� �� ����� �����
    private bool isLoadingScene = false;  // ���� ��� �������� �������� �����

    // ����������� ���� ��� �������� ��������, ��������� ����� �������
    public static MonoBehaviour[] scriptsToRunOnLoad;

    void Start()
    {
        // ���������, ��� ���� ������ �� ������������ ��� �������� ����� �����
        DontDestroyOnLoad(gameObject);

        // �������� ������
        startButton.onClick.AddListener(OnStartButtonClicked);
        exitButton.onClick.AddListener(OnExitButtonClicked);
    }

    // ������� ��� �������� �� ������� �����
    public void OnStartButtonClicked()
    {
        // ��������� ����������� �������� �����
        if (!isLoadingScene)
        {
            isLoadingScene = true;

            // �������� ������ �������� � ����������� ����
            scriptsToRunOnLoad = scriptsToExecute;

            StartCoroutine(LoadScene("FirstLocation")); // �������� "FirstLocation" �� ��� ����� �����
        }
    }

    // ����������� �������� �����
    private IEnumerator LoadScene(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        // ����, ���� ����� ��������� ����������
        while (!asyncLoad.isDone) 
        {
            yield return null;
        }
    }

    // ������� ��� ������ �� ����������
    public void OnExitButtonClicked()
    {
        Application.Quit();
    }
}
