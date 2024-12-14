using UnityEngine;

public class SceneInitializer : MonoBehaviour
{
    void Start()
    {
        // ���������, ���� �� ������� ��� ����������
        if (UIManager.scriptsToRunOnLoad != null)
        {
            // �������� �������, ������� ���� �������� �� ���������� �����
            foreach (MonoBehaviour script in UIManager.scriptsToRunOnLoad)
            {
                if (script != null)
                {
                    script.enabled = true;
                }
            }

            // ������� ������ ��������, ����� ������������� ��������� ����������
            UIManager.scriptsToRunOnLoad = null;

            Debug.Log("��� ������� ������� �������� �� ����� �����.");
        }
    }
}
