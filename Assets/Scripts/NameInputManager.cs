using UnityEngine;
using UnityEngine.UI;

public class NameInputManager : MonoBehaviour
{
    public GameObject nameInputPanel; // ������ � ��������� ����� � �������
    public InputField nameInputField; // ���� ��� ����� �����
    public Button submitButton; // ������ ��� ������������� �����
    public DialogueManager dialogueManager; // ������ DialogueManager, ������� ����� ���������
    [SerializeField]
    private GameObject[] objectsToHide; // ������ ��������, ������� ����� ������ �� ����� �����

    void Start()
    {
        // �������� ������ ��� ������
        nameInputPanel.SetActive(true);

        // �������� �������, ������� �� ������ ���� ����� �� ����� �����
        SetObjectsVisibility(false);

        // ���������� ���������� ������
        submitButton.onClick.AddListener(OnSubmitButtonClicked);

        // ���������, ��������� �� ��� ������
        if (PlayerPrefs.HasKey("PlayerName"))
        {
            // ���� ��� ���������, ���������� ������� � ��������� ������ �����
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
            // ��������� ��� ������
            PlayerPrefs.SetString("PlayerName", playerName);

            // ��������� ���������
            PlayerPrefs.Save();

            // �������� ������ ����� ����� �����
            nameInputPanel.SetActive(false);

            // ���������� ������� �������
            SetObjectsVisibility(true);

            // ��������� DialogueManager
            if (dialogueManager != null)
            {
                dialogueManager.StartDialogue(); // ������ �������
            }
        }
        else
        {
            // ������ ��� ����������� ������, ���� ��� �� �������
            Debug.Log("Please enter a valid name.");
        }
    }

    // ����� ��� ���������� ���������� ��������
    void SetObjectsVisibility(bool isVisible)
    {
        Debug.Log($"Setting objects visibility to {isVisible}"); // ��������� ���������� ���������
        foreach (GameObject obj in objectsToHide)
        {
            if (obj != null)
            {
                Debug.Log($"Setting visibility for {obj.name} to {isVisible}"); // ���������� ��������� ��� ������� �������
                obj.SetActive(isVisible);
            }
            else
            {
                Debug.LogWarning("Object in objectsToHide is null");
            }
        }
    }
}
