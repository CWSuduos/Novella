using UnityEngine;
using UnityEngine.UI;

public class LanguageFinder : MonoBehaviour
{
    public Text targetText; // Текстовый объект, который будет обновляться
    public string key; // Ключ для поиска перевода в JSON

    private string currentText = ""; // Текущее значение текста

    void Start()
    {
        if (targetText != null && !string.IsNullOrEmpty(key))
        {
            UpdateText(); // При запуске обновить текст
        }
    }

    // Метод для обновления текста на основе выбранного языка
    public void UpdateText()
    {
        string newText = LanguageManager.Instance.GetLocalizedText(key);
        if (currentText != newText) // Если текст отличается, обновить его
        {
            targetText.text = newText;
            currentText = newText;
        }
    }
}
