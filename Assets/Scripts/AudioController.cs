using System;
using UnityEngine;
using UnityEngine.Audio;

public class AudioController : MonoBehaviour
{
    private AudioSource sfx;
    [SerializeField] private SoundType[] soundTypes;

    private void Start()
    {
        sfx = GetComponent<AudioSource>();
    }
    public void Play(Sounds sound)
    {
        SoundType item = Array.Find(soundTypes, i => i.soundName == sound);
        AudioClip clip = item.audioclip;
        if (clip != null)
        {
            sfx.PlayOneShot(clip);
        }
        else
        {
            Debug.Log("Clip not found for sound:" + sound);
        }
    }
}

[Serializable]
public class SoundType
{
    public Sounds soundName;
    public AudioClip audioclip;
}
public enum Sounds
{
    buttonClick,
    pickup
}
