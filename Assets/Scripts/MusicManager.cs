using UnityEngine;
using UnityEngine.UI; // ��� ������ � UI ����������, ������ ��� Slider � Text

public class MusicManager : MonoBehaviour
{
    public AudioSource audioSource;         // ������ �� ��������� AudioSource
    public AudioClip[] musicClips;          // ������ � ������������ (�������)
    public Slider volumeSlider;             // ������� ��� ��������� ���������
    public Text volumeText;                 // ����� ��� ����������� �������� ���������
    public bool isShuffled = true;          // �������� ��������� ������������ �����
    private int currentTrackIndex = 0;      // ������ �������� �����

    void Awake()
    {
        // ���������, ��� ���� ������ �� ������������ ��� �������� ����� �����
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
            // �������� ������������ ������ �����
            PlayNextTrack();
        }

        // ����������� ��������� �������� ���������, ���� ������� ����������
        if (volumeSlider != null)
        {
            volumeSlider.value = audioSource.volume;
            volumeSlider.onValueChanged.AddListener(ChangeVolume);
        }

        // ��������� ����� ��������� ��� ������
        UpdateVolumeText();
    }

    void Update()
    {
        // ���������, ���� ����� �����������
        if (!audioSource.isPlaying)
        {
            PlayNextTrack();
        }
    }

    void PlayNextTrack()
    {
        if (isShuffled)
        {
            // �������� �������� ��������� ����
            currentTrackIndex = Random.Range(0, musicClips.Length);
        }
        else
        {
            // ��������������� �������� ��������� ����
            currentTrackIndex = (currentTrackIndex + 1) % musicClips.Length;
        }

        // ������������� ����� ���� � �������� ��� �����������
        audioSource.clip = musicClips[currentTrackIndex];
        audioSource.Play();
    }

    // ����� ��� ��������� ���������
    void ChangeVolume(float volume)
    {
        audioSource.volume = volume;
        UpdateVolumeText();
    }

    // ����� ��� ���������� ������ ���������
    void UpdateVolumeText()
    {
        if (volumeText != null)
        {
            // ���������� ��������� � ���������
            volumeText.text = "" + Mathf.RoundToInt(audioSource.volume * 100) + "%";
        }
    }
}
