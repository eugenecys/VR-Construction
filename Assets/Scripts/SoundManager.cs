using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundManager : Singleton<SoundManager>
{
    private List<AudioSource> _audioSource;
    //Change the number of audio sources as needed
    private int _audioSourceCount = 10;

    //Wheel whirring sound
    public AudioClip wheelSound { get; private set; }
    public AudioClip machinegunSound { get; private set; }
    public AudioClip laserCannonSound { get; private set; }
    public AudioClip hydraulicSound { get; private set; }
    public AudioClip attachSound { get; private set; }

    public void playSound(AudioClip sound)
    {
        foreach (AudioSource AS in _audioSource)
        {
            if (!AS.isPlaying)
            {
                AS.PlayOneShot(sound);
                break;
            }
        }
    }

    public void playSound(AudioClip sound, int index)
    {
        if (index > _audioSourceCount)
        {
            index = _audioSourceCount;
        }
        _audioSource[index - 1].PlayOneShot(sound);
    }

    public void playSound(AudioClip sound, int index, float volume)
    {
        if (index > _audioSourceCount)
        {
            index = _audioSourceCount;
        }
        _audioSource[index - 1].PlayOneShot(sound, volume);
    }

    public void stopSound(int index)
    {
        if (index > _audioSourceCount)
        {
            index = _audioSourceCount;
        }
        StartCoroutine(fadeSound(index, 1));
    }

    public void stopAllSounds()
    {
        foreach (AudioSource AS in _audioSource)
        {
             AS.Stop();
        }
    }

    // Use this for initialization
    void Awake()
    {
        _audioSource = new List<AudioSource>();
        for (int i = 0; i < _audioSourceCount; i++)
        {
            AudioSource temp = gameObject.AddComponent<AudioSource>();
            _audioSource.Add(temp);
        }

        //Loads a file from Resources/Sounds folder
        //wheelSound = _loadSoundClip("wheelSound", 0);

    }

    private AudioClip _loadSoundClip(string filename, int i)
    {
        AudioClip clip = Resources.Load("Sounds/" + filename) as AudioClip;
        return clip;
    }
    
    IEnumerator fadeSound(int index, float time)
    {
        AudioSource AS = _audioSource[index - 1];
        float delta = AS.volume / time;
        while (AS.volume > 0.05f)
        {
            AS.volume -= delta * Time.deltaTime;
            yield return null;
        }
        AS.Stop();
        AS.volume = 1;
    }

}