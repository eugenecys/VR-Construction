using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundManager : Singleton<SoundManager>
{
    private List<AudioSource> _audioSource;
    //Change the number of audio sources as needed
    private int _audioSourceCount = 10;

    public List<GameObject> sfxToBeRemove = new List<GameObject>();

	// sfx sounds
    public AudioClip wheelSound { get; private set; }
    public AudioClip machinegunSound { get; private set; }
    public AudioClip laserSound { get; private set; }
    public AudioClip hydraulicSound { get; private set; }
    public AudioClip attachSound { get; private set; }
    public AudioClip cannonSound { get; private set; }
    public AudioClip trashSound { get; private set; }
    public AudioClip releaseSound { get; private set; }
    public AudioClip pickupSound { get; private set; }
	public AudioClip lightOnSound { get; private set; }
    public AudioClip[] explosions { get; private set; }

    // bgm sounds
    public AudioClip buildBGM { get; private set; }
	public AudioClip greenBGM { get; private set; }
	public AudioClip redBGM { get; private set; }

	// dialogue sounds
	public AudioClip startDialogue { get; private set; }
	public AudioClip selectBaseDialogue { get; private set; }
	public AudioClip constructionDialogue { get; private set; }
	public AudioClip deployDialogue { get; private set; }
	public AudioClip cityDialogue { get; private set; }

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
        wheelSound = _loadSoundClip("SFX/ConstructionRoom-Engine_Running", 0);
		machinegunSound = _loadSoundClip("SFX/Cannon-SingleShot", 0);
		laserSound = _loadSoundClip("SFX/Laser", 0);
		attachSound = _loadSoundClip("SFX/ConstructionRoom-Drop", 0);
		cannonSound = _loadSoundClip("SFX/Cannon-SingleShot", 0);
		trashSound = _loadSoundClip("SFX/ConstructionRoom-Trash", 0);
		releaseSound = _loadSoundClip("SFX/ConstructionRoom-ButtonRelease", 0);
		pickupSound = _loadSoundClip("SFX/ConstructionRoom-Pickup", 0);
		lightOnSound = _loadSoundClip ("SFX/LightOn", 0);
		buildBGM = _loadSoundClip ("SFX/ConstructionRoom-Drone", 0);
		greenBGM = _loadSoundClip ("SFX/ConstructionRoom-Green", 0);
		redBGM = _loadSoundClip ("SFX/ConstructionRoom-Red", 0);
		startDialogue = _loadSoundClip ("Dialogue/StartDialogue", 0);
		selectBaseDialogue = _loadSoundClip ("Dialogue/SelectBaseDialogue", 0);
		constructionDialogue = _loadSoundClip ("Dialogue/ConstructionDialogue", 0);
		deployDialogue = _loadSoundClip ("Dialogue/DeployDialogue", 0);
		cityDialogue = _loadSoundClip ("Dialogue/CityDialogue", 0);
        explosions[0] = _loadSoundClip("SFX/Explosion01", 0);
        explosions[1] = _loadSoundClip("SFX/Explosion02", 0);
        explosions[2] = _loadSoundClip("SFX/Explosion03", 0);
        explosions[3] = _loadSoundClip("SFX/Explosion04", 0);
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

    public void AudioPlay(AudioClip clip, Transform emitter)
    {
        //Create an empty game object
        GameObject go = new GameObject("Audio: " + clip.name);
        go.transform.position = emitter.position;
        go.transform.parent = emitter;

        //Create the source
        AudioSource source = go.AddComponent<AudioSource>();
        source.clip = clip;
        source.Play();

        StartCoroutine(AudioDestory(go, source.clip.length));
        //Destroy(go, clip.length+5f);
    }

    IEnumerator AudioDestory(GameObject go, float t)
    {
        yield return new WaitForSeconds(t);
        sfxToBeRemove.Add(go);

    }

    void RemoveSFX()
    {
        foreach (GameObject go in sfxToBeRemove)
        {
            Destroy(go);
        }
        sfxToBeRemove.Clear();
    }

}