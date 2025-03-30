using UnityEngine;

public class AudioPlayerLevel : MonoBehaviour
{
    public AudioClip audioClip;
    public Audio_Manager AudioManager;

    void Start()
    {
        AudioManager.PlayBackground(audioClip);

    }
}
