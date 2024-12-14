using UnityEngine;
using UnityEngine.UI;

public class LanguageFinder : MonoBehaviour
{
    public Text targetText; // ��������� ������, ������� ����� �����������
    public string key; // ���� ��� ������ �������� � JSON

    private string currentText = ""; // ������� �������� ������

    void Start()
    {
        if (targetText != null && !string.IsNullOrEmpty(key))
        {
            UpdateText(); // ��� ������� �������� �����
        }
    }

    // ����� ��� ���������� ������ �� ������ ���������� �����
    public void UpdateText()
    {
        string newText = LanguageManager.Instance.GetLocalizedText(key);
        if (currentText != newText) // ���� ����� ����������, �������� ���
        {
            targetText.text = newText;
            currentText = newText;
        }
    }
}
