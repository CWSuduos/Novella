using UnityEngine;
using UnityEngine.UI; // Для работы с UI элементами, такими как Slider и Text

public class MusicManager : MonoBehaviour
{
    public AudioSource audioSource;         // Ссылка на компонент AudioSource
    public AudioClip[] musicClips;          // Массив с аудиофайлами (песнями)
    public Slider volumeSlider;             // Слайдер для изменения громкости
    public Text volumeText;                 // Текст для отображения значения громкости
    public bool isShuffled = true;          // Включить случайное проигрывание песен
    private int currentTrackIndex = 0;      // Индекс текущего трека

    void Awake()
    {
        // Убедитесь, что этот объект не уничтожается при загрузке новой сцены
        if (FindObjectsOfType<MusicManager>().Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        if (musicClips.Length > 0)
        {
            // Начинаем проигрывание первой песни
            PlayNextTrack();
        }

        // Присваиваем начальное значение громкости, если слайдер установлен
        if (volumeSlider != null)
        {
            volumeSlider.value = audioSource.volume;
            volumeSlider.onValueChanged.AddListener(ChangeVolume);
        }

        // Обновляем текст громкости при старте
        UpdateVolumeText();
    }

    void Update()
    {
        // Проверяем, если песня закончилась
        if (!audioSource.isPlaying)
        {
            PlayNextTrack();
        }
    }

    void PlayNextTrack()
    {
        if (isShuffled)
        {
            // Случайно выбираем следующий трек
            currentTrackIndex = Random.Range(0, musicClips.Length);
        }
        else
        {
            // Последовательно выбираем следующий трек
            currentTrackIndex = (currentTrackIndex + 1) % musicClips.Length;
        }

        // Устанавливаем новый трек и начинаем его проигрывать
        audioSource.clip = musicClips[currentTrackIndex];
        audioSource.Play();
    }

    // Метод для изменения громкости
    void ChangeVolume(float volume)
    {
        audioSource.volume = volume;
        UpdateVolumeText();
    }

    // Метод для обновления текста громкости
    void UpdateVolumeText()
    {
        if (volumeText != null)
        {
            // Отображаем громкость в процентах
            volumeText.text = "" + Mathf.RoundToInt(audioSource.volume * 100) + "%";
        }
    }
}
