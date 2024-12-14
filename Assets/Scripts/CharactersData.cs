using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Для использования UI элементов

[System.Serializable]
public class CharacterData
{
    public string characterName;
    public List<int> dialogueIndices;
    public GameObject characterObject; // Ссылка на объект персонажа в сцене
}

public class CharactersData : MonoBehaviour
{
    public List<CharacterData> charactersData; // Список данных персонажей
    public float sizeChangeDuration = 0.5f; // Продолжительность анимации изменения размера
    public Text endingDialogueText; // Текстовое поле для концовочного диалога
    public string defaultEndingDialogue = "Концовочный диалог: Все персонажи завершили свои реплики!"; // Строка по умолчанию

    private string customEndingDialogue; // Пользовательский концовочный диалог

    // Метод для обновления размера персонажа в зависимости от активности диалога
    public void UpdateCharacterSize(int dialogueIndex, bool isActive)
    {
        bool anyCharacterActive = false;

        foreach (var characterData in charactersData)
        {
            if (characterData.dialogueIndices.Contains(dialogueIndex))
            {
                // Изменяем размер объекта персонажа в зависимости от активности диалога
                StartCoroutine(ChangeCharacterSize(characterData.characterObject, isActive));
                anyCharacterActive = true;
            }
        }

        // Если ни один из персонажей не активен, показываем концовочный диалог
        if (!anyCharacterActive)
        {
            ShowEndingDialogue();
        }
    }

    // Корутину для плавного изменения размера персонажа
    private IEnumerator ChangeCharacterSize(GameObject character, bool isActive)
    {
        if (character != null)
        {
            Vector3 startSize = character.transform.localScale;
            Vector3 endSize = isActive ? new Vector3(7, 7, 1) : new Vector3(5, 5, 1);
            float elapsedTime = 0f;

            while (elapsedTime < sizeChangeDuration)
            {
                character.transform.localScale = Vector3.Lerp(startSize, endSize, (elapsedTime / sizeChangeDuration));
                elapsedTime += Time.deltaTime;
                yield return null; // Вместо WaitForSeconds используем null для лучшей производительности
            }

            character.transform.localScale = endSize; // Убедимся, что конечный размер точно установлен
        }
    }

    // Метод для показа концовочного диалога
    private void ShowEndingDialogue()
    {
        if (endingDialogueText != null)
        {
            endingDialogueText.text = string.IsNullOrEmpty(customEndingDialogue) ? defaultEndingDialogue : customEndingDialogue;
        }
    }

    // Метод для установки пользовательского концовочного диалога
    public void SetCustomEndingDialogue(string dialogue)
    {
        customEndingDialogue = dialogue;
    }
}
