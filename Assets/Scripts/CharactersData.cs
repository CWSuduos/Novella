using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // ��� ������������� UI ���������

[System.Serializable]
public class CharacterData
{
    public string characterName;
    public List<int> dialogueIndices;
    public GameObject characterObject; // ������ �� ������ ��������� � �����
}

public class CharactersData : MonoBehaviour
{
    public List<CharacterData> charactersData; // ������ ������ ����������
    public float sizeChangeDuration = 0.5f; // ����������������� �������� ��������� �������
    public Text endingDialogueText; // ��������� ���� ��� ������������ �������
    public string defaultEndingDialogue = "����������� ������: ��� ��������� ��������� ���� �������!"; // ������ �� ���������

    private string customEndingDialogue; // ���������������� ����������� ������

    // ����� ��� ���������� ������� ��������� � ����������� �� ���������� �������
    public void UpdateCharacterSize(int dialogueIndex, bool isActive)
    {
        bool anyCharacterActive = false;

        foreach (var characterData in charactersData)
        {
            if (characterData.dialogueIndices.Contains(dialogueIndex))
            {
                // �������� ������ ������� ��������� � ����������� �� ���������� �������
                StartCoroutine(ChangeCharacterSize(characterData.characterObject, isActive));
                anyCharacterActive = true;
            }
        }

        // ���� �� ���� �� ���������� �� �������, ���������� ����������� ������
        if (!anyCharacterActive)
        {
            ShowEndingDialogue();
        }
    }

    // �������� ��� �������� ��������� ������� ���������
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
                yield return null; // ������ WaitForSeconds ���������� null ��� ������ ������������������
            }

            character.transform.localScale = endSize; // ��������, ��� �������� ������ ����� ����������
        }
    }

    // ����� ��� ������ ������������ �������
    private void ShowEndingDialogue()
    {
        if (endingDialogueText != null)
        {
            endingDialogueText.text = string.IsNullOrEmpty(customEndingDialogue) ? defaultEndingDialogue : customEndingDialogue;
        }
    }

    // ����� ��� ��������� ����������������� ������������ �������
    public void SetCustomEndingDialogue(string dialogue)
    {
        customEndingDialogue = dialogue;
    }
}
