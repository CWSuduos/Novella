using UnityEngine;

public class SceneInitializer : MonoBehaviour
{
    void Start()
    {
        // Проверяем, есть ли скрипты для выполнения
        if (UIManager.scriptsToRunOnLoad != null)
        {
            // Включаем скрипты, которые были переданы из предыдущей сцены
            foreach (MonoBehaviour script in UIManager.scriptsToRunOnLoad)
            {
                if (script != null)
                {
                    script.enabled = true;
                }
            }

            // Очищаем массив скриптов, чтобы предотвратить повторное выполнение
            UIManager.scriptsToRunOnLoad = null;

            Debug.Log("Все скрипты успешно запущены на новой сцене.");
        }
    }
}
