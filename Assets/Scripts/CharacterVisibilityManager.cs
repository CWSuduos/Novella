using UnityEngine;
using UnityEngine.UI;

public class CharacterVisibilityManager : MonoBehaviour
{
    public GameObject[] objectsToHide; // ������ ��������, ������� ����� ������
    public Text nameTextField;         // ���� ��� ����������� ����� ��������� (�� InputField)

    private string lastText = "";      // �������� ����������� ��������� ������
    private bool objectsVisible = false; // ���� ��� ������������ ��������� ��������

    void Start()
    {
        // ���������� �������� �������
        SetObjectsVisibility(false);
    }

    void Update()
    {
        // ���������, ��������� �� �����
        if (nameTextField.text != lastText)
        {
            lastText = nameTextField.text;  // ��������� ���������� �����

            // ���� ��������� ���� ��������� � ������� ������, ���������� �������
            if (!string.IsNullOrEmpty(nameTextField.text) && !objectsVisible)
            {
                SetObjectsVisibility(true);
            }
            else if (string.IsNullOrEmpty(nameTextField.text) && objectsVisible)
            {
                // ���� ��������� ���� ������ � ������� ������, �������� �������
                SetObjectsVisibility(false);
            }
        }
    }

    // ����� ��� ��������� ��������� ��������
    void SetObjectsVisibility(bool isVisible)
    {
        foreach (GameObject obj in objectsToHide)
        {
            if (obj != null)
            {
                obj.SetActive(isVisible);
            }
        }
        objectsVisible = isVisible; // ��������� ���� ��������� ��������� ��������
    }
}
