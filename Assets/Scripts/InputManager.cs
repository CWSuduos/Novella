using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InputManager : MonoBehaviour
{
    // ������ �� ���� ����� ����� (InputField)
    public InputField nameInputField;

    // ������ �� ��������� ���� ��� ����������� �����
    public Text nameDisplayText;

    // ������ ��������, ������� ����� ����������� ����� ����� �����
    public MonoBehaviour[] scriptsToExecute;

    // ��� ������
    private string playerName;

    // �������� �����, ��� ������� ������ ������ �����������
    public string targetSceneName;

    private bool scriptsExecuted = false; // ��������, ���� �� ��� �������� �������

    private string currentSceneName = ""; // ���������� ��� ������������ ������� �������� �����

    void Start()
    {
        // �������� �� null � �������
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

        // ��������� ��� ������� �� ������� ��� ������
        DisableAllScripts();
    }

    void Update()
    {
        // �������� ������� �������� �����
        Scene activeScene = SceneManager.GetActiveScene();

        // ���������, ���������� �� �����
        if (currentSceneName != activeScene.name)
        {
            currentSceneName = activeScene.name;

            // ���� ����� ������������� ������� ����� � ������� ��� �� ���� ��������
            if (currentSceneName == targetSceneName && !scriptsExecuted)
            {
                // ��������� ��������� InputField � ��������� �������
                ExecuteInputFieldCheck();
                scriptsExecuted = true;
            }
        }

        // ���������, ��������� �� ���� ����� � ������ �� ������� Enter
        if (Input.GetKeyDown(KeyCode.Return) && !string.IsNullOrEmpty(nameInputField?.text))
        {
            SaveNameAndExecuteScripts();
        }
    }

    // ����� ��� �������� � ������� �������� ����� ����� �����
    void ExecuteInputFieldCheck()
    {
        // ���� ���� ����� �������, ������� ����� �����
        if (nameInputField != null && nameInputField.gameObject.activeSelf)
        {
            // ���� ������� ������� Enter
            Debug.Log("InputField is active, waiting for name input...");
        }
        else
        {
            Debug.LogWarning("nameInputField is null or not active");
        }
    }

    // ����� ��� ���������� ����� � ������� ��������
    public void SaveNameAndExecuteScripts()
    {
        // �������� ����� �� InputField
        if (nameInputField != null)
        {
            playerName = nameInputField.text;
        }

        // ���������, ����� ��� �� ���� ������
        if (!string.IsNullOrEmpty(playerName))
        {
            // ���������� ������ ��� � ��������� ����
            if (nameDisplayText != null)
            {
                nameDisplayText.text = playerName;
            }

            // �������� ���� �����, ��� ��� ��� �������
            if (nameInputField != null)
            {
                nameInputField.gameObject.SetActive(false);
            }

            // ��������� ������� � �������
            ExecuteScripts();

            // ����� ����� ������� ����� ���������� ��������
            OnScriptsExecuted();
        }
        else
        {
            // ���� ���� ����� ������, ������� ��������� �� ������
            if (nameDisplayText != null)
            {
                nameDisplayText.text = "Please enter a valid name!";
            }
        }
    }

    // ����� ��� ���������� ���� �������� � �������
    private void DisableAllScripts()
    {
        if (scriptsToExecute != null)
        {
            foreach (MonoBehaviour script in scriptsToExecute)
            {
                if (script != null)
                {
                    script.enabled = false; // ��������� �������
                }
                else
                {
                    Debug.LogWarning("Script in scriptsToExecute is null");
                }
            }
        }
    }

    // ����� ��� ������� ���� �������� � �������
    private void ExecuteScripts()
    {
        if (scriptsToExecute != null)
        {
            foreach (MonoBehaviour script in scriptsToExecute)
            {
                if (script != null)
                {
                    script.enabled = true; // �������� �������
                }
                else
                {
                    Debug.LogWarning("Script in scriptsToExecute is null");
                }
            }
        }
    }

    // ����� �������: ����������� ����� ����, ��� ��� ������� ���� ��������
    private void OnScriptsExecuted()
    {
        // ����� ����� �������� ����� �������������� ������, ��������:
        Debug.Log("All scripts have been executed successfully.");

        // ������ ��������������� ��������: ������� ����������� ��������� �� �����
        if (nameDisplayText != null)
        {
            nameDisplayText.text += "\nWelcome, " + playerName + "!";
        }
    }
}
