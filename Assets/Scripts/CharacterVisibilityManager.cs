using UnityEngine;
using UnityEngine.UI;

public class CharacterVisibilityManager : MonoBehaviour
{
    public GameObject[] objectsToHide; // Массив объектов, которые нужно скрыть
    public Text nameTextField;         // Поле для отображения имени персонажа (не InputField)

    private string lastText = "";      // Хранение предыдущего состояния текста
    private bool objectsVisible = false; // Флаг для отслеживания видимости объектов

    void Start()
    {
        // Изначально скрываем объекты
        SetObjectsVisibility(false);
    }

    void Update()
    {
        // Проверяем, изменился ли текст
        if (nameTextField.text != lastText)
        {
            lastText = nameTextField.text;  // Обновляем сохранённый текст

            // Если текстовое поле заполнено и объекты скрыты, показываем объекты
            if (!string.IsNullOrEmpty(nameTextField.text) && !objectsVisible)
            {
                SetObjectsVisibility(true);
            }
            else if (string.IsNullOrEmpty(nameTextField.text) && objectsVisible)
            {
                // Если текстовое поле пустое и объекты видимы, скрываем объекты
                SetObjectsVisibility(false);
            }
        }
    }

    // Метод для изменения видимости объектов
    void SetObjectsVisibility(bool isVisible)
    {
        foreach (GameObject obj in objectsToHide)
        {
            if (obj != null)
            {
                obj.SetActive(isVisible);
            }
        }
        objectsVisible = isVisible; // Обновляем флаг состояния видимости объектов
    }
}
