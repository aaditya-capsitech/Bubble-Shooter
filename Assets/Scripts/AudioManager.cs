using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    private AudioSource audioSource;
    public AudioClip audioClip;

    private void Awake()
    {
        if(audioSource == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
            audioSource = GetComponent<AudioSource>();
            audioSource.clip = audioClip;
    }
    void Start()
    {
        
    }

    void Update()
    {
        
    }
    public void PlayAudio()
    {
        if (audioSource != null)
            audioSource.PlayOneShot(audioClip);
    }
}
