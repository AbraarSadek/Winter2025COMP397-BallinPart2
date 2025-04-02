using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UIElements;

public class Audio_Manager : MonoBehaviour
{

    [SerializeField] AudioSource sfxSource;
    [SerializeField] AudioSource backgroundSource;
    public AudioClip MainMenu1;
    public AudioClip MainMenu2;
    public AudioClip Gameplay;
    public AudioMixer audioMixer;

    
    public void PlayBackground(AudioClip audio)
    {
        backgroundSource.clip = audio;
        backgroundSource.Play();
    }
    public void PlayGameplay(AudioClip audio)
    {
        PlayBackground(Gameplay);
    }
    public void randomizeMainMenu()
    {
        int randomNumber = Random.Range(1, 3);
        switch(randomNumber)
        {
            case 1:
                PlayBackground(MainMenu1);
                break;
            case 2:
                PlayBackground(MainMenu2);
                break;
            default:
                PlayBackground(MainMenu2);
                break;
        }
    }
    public AudioMixer getAudioMixer()
    {
        return audioMixer;
    }

}
