using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class LanguageManager : MonoBehaviour
{
    public static LanguageManager Instance { get; private set; }

    public Dropdown languageDropdown; // Ссылка на Dropdown для выбора языка
    private Dictionary<string, string> localizedTexts = new Dictionary<string, string>();
    private string currentLanguage = "en"; // Язык по умолчанию
    private string languageFolderPath; // Путь к папке с JSON-файлами

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            languageFolderPath = Application.streamingAssetsPath; // Правильный путь к папке StreamingAssets
            InitializeLanguageDropdown();
            LoadLanguage(currentLanguage);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Инициализация Dropdown и добавление языков
    private void InitializeLanguageDropdown()
    {
        if (languageDropdown != null)
        {
            languageDropdown.ClearOptions();
            List<string> languageOptions = new List<string> { "en", "ru" }; // Можно добавить больше языков
            languageDropdown.AddOptions(languageOptions);
            languageDropdown.onValueChanged.AddListener(OnLanguageChanged);
        }
    }

    // Загрузка JSON-файла для выбранного языка
    public void LoadLanguage(string languageCode)
    {
        currentLanguage = languageCode;
        string filePath = Path.Combine(languageFolderPath, $"{currentLanguage}.json");

        if (File.Exists(filePath))
        {
            string jsonContent = File.ReadAllText(filePath);
            LocalizationData data = JsonUtility.FromJson<LocalizationData>(jsonContent);
            PopulateDictionary(data);

            // Обновляем все тексты через LanguageFinder
            UpdateAllFinders();
        }
        else
        {
            Debug.LogError($"Language file not found: {filePath}");
        }
    }

    // Заполнение словаря текстов из JSON
    private void PopulateDictionary(LocalizationData data)
    {
        localizedTexts.Clear();
        foreach (var entry in data.translations)
        {
            localizedTexts[entry.key] = entry.value;
        }
    }

    // Получение локализованного текста по ключу
    public string GetLocalizedText(string key)
    {
        if (localizedTexts.TryGetValue(key, out string value))
        {
            return value;
        }
        return key; // Если ключ не найден, возвращаем сам ключ
    }

    // Обновление всех текстов на сцене
    public void UpdateAllFinders()
    {
        foreach (LanguageFinder finder in FindObjectsOfType<LanguageFinder>())
        {
            finder.UpdateText();
        }
    }

    // Метод для смены языка через Dropdown
    private void OnLanguageChanged(int index)
    {
        string selectedLanguage = languageDropdown.options[index].text;
        LoadLanguage(selectedLanguage);
    }

    // Структура данных для хранения пар ключ-значение
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

    // Получение текущего языка
    public string CurrentLanguage => currentLanguage;
}
