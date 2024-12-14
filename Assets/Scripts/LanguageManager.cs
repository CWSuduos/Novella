using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class LanguageManager : MonoBehaviour
{
    public static LanguageManager Instance { get; private set; }

    public Dropdown languageDropdown; // ������ �� Dropdown ��� ������ �����
    private Dictionary<string, string> localizedTexts = new Dictionary<string, string>();
    private string currentLanguage = "en"; // ���� �� ���������
    private string languageFolderPath; // ���� � ����� � JSON-�������

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            languageFolderPath = Application.streamingAssetsPath; // ���������� ���� � ����� StreamingAssets
            InitializeLanguageDropdown();
            LoadLanguage(currentLanguage);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // ������������� Dropdown � ���������� ������
    private void InitializeLanguageDropdown()
    {
        if (languageDropdown != null)
        {
            languageDropdown.ClearOptions();
            List<string> languageOptions = new List<string> { "en", "ru" }; // ����� �������� ������ ������
            languageDropdown.AddOptions(languageOptions);
            languageDropdown.onValueChanged.AddListener(OnLanguageChanged);
        }
    }

    // �������� JSON-����� ��� ���������� �����
    public void LoadLanguage(string languageCode)
    {
        currentLanguage = languageCode;
        string filePath = Path.Combine(languageFolderPath, $"{currentLanguage}.json");

        if (File.Exists(filePath))
        {
            string jsonContent = File.ReadAllText(filePath);
            LocalizationData data = JsonUtility.FromJson<LocalizationData>(jsonContent);
            PopulateDictionary(data);

            // ��������� ��� ������ ����� LanguageFinder
            UpdateAllFinders();
        }
        else
        {
            Debug.LogError($"Language file not found: {filePath}");
        }
    }

    // ���������� ������� ������� �� JSON
    private void PopulateDictionary(LocalizationData data)
    {
        localizedTexts.Clear();
        foreach (var entry in data.translations)
        {
            localizedTexts[entry.key] = entry.value;
        }
    }

    // ��������� ��������������� ������ �� �����
    public string GetLocalizedText(string key)
    {
        if (localizedTexts.TryGetValue(key, out string value))
        {
            return value;
        }
        return key; // ���� ���� �� ������, ���������� ��� ����
    }

    // ���������� ���� ������� �� �����
    public void UpdateAllFinders()
    {
        foreach (LanguageFinder finder in FindObjectsOfType<LanguageFinder>())
        {
            finder.UpdateText();
        }
    }

    // ����� ��� ����� ����� ����� Dropdown
    private void OnLanguageChanged(int index)
    {
        string selectedLanguage = languageDropdown.options[index].text;
        LoadLanguage(selectedLanguage);
    }

    // ��������� ������ ��� �������� ��� ����-��������
    [System.Serializable]
    public class LocalizationData
    {
        public List<LocalizationEntry> translations;
    }

    [System.Serializable]
    public class LocalizationEntry
    {
        public string key;
        public string value;
    }

    // ��������� �������� �����
    public string CurrentLanguage => currentLanguage;
}
