using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    public AudioSource normalSource; 
    public AudioSource battleSource;  

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (!normalSource.isPlaying)
        {
            normalSource.Play();
        }

        battleSource.Stop();
    }

    public void SwitchToBattle()
    {
        normalSource.Pause();

        battleSource.Play();
    }

    public void SwitchToNormal()
    {
        battleSource.Stop();

        normalSource.UnPause();
    }
}