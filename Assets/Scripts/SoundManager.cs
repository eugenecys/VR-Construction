using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundManager : Singleton<SoundManager>
{
    private List<AudioSource> _audioSource;
    //Change the number of audio sources as needed
    private int _audioSourceCount = 10;

	// sfx sounds
    public AudioClip wheelSound { get; private set; }
    public AudioClip machinegunSound { get; private set; }
    public AudioClip laserSound { get; private set; }
    public AudioClip hydraulicSound { get; private set; }
    public AudioClip attachSound { get; private set; }
    public AudioClip cannonSound { get; private set; }
	public AudioClip powerUpSound { get; private set; }
	public AudioClip powerDownSound { get; private set; }
    public AudioClip trashSound { get; private set; }
    public AudioClip releaseSound { get; private set; }
    public AudioClip pickupSound { get; private set; }
	public AudioClip lightOnSound { get; private set; }

    // bgm sounds
    public AudioClip buildBGM { get; private set; }
	public AudioClip greenBGM { get; private set; }
	public AudioClip redBGM { get; private set; }
	public AudioClip cityBGM { get; private set; }

	// dialogue sounds
	public AudioClip startDialogue { get; private set; }
	public AudioClip tutorialDialogue01 { get; private set; }
	public AudioClip tutorialDialogue02 { get; private set; }
	public AudioClip tutorialDialogueEnd { get; private set; }
	public AudioClip selectBaseDialogue { get; private set; }
	public AudioClip constructionDialogue { get; private set; }
	public AudioClip deployDialogue { get; private set; }
	public AudioClip cityDialogue01 { get; private set; }
	public AudioClip cityDialogue02 { get; private set; }

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
		machinegunSound = _loadSoundClip("SFX/MachineGun-Loop", 0);
		laserSound = _loadSoundClip("SFX/Laser", 0);
		attachSound = _loadSoundClip("SFX/ConstructionRoom-Drop", 0);
		cannonSound = _loadSoundClip("SFX/Cannon-SingleShot", 0);
		trashSound = _loadSoundClip("SFX/ConstructionRoom-Trash", 0);
		releaseSound = _loadSoundClip("SFX/ConstructionRoom-ButtonRelease", 0);
		pickupSound = _loadSoundClip("SFX/ConstructionRoom-Pickup", 0);
		lightOnSound = _loadSoundClip ("SFX/LightOn", 0);
		powerUpSound = _loadSoundClip ("SFX/PowerUp01", 0);
		powerDownSound = _loadSoundClip ("SFX/PowerDown01", 0);

		// bgms 
		buildBGM = _loadSoundClip ("BGM/ConstructionRoom-Drone", 0);
		greenBGM = _loadSoundClip ("BGM/ConstructionRoom-Green", 0);
		redBGM = _loadSoundClip ("BGM/ConstructionRoom-Red", 0);
		cityBGM = _loadSoundClip ("BGM/City-BGM", 0);

		// dialogues
		startDialogue = _loadSoundClip ("Dialogue/Robotic/Start", 0);
		tutorialDialogue01 = _loadSoundClip ("Dialogue/Robotic/Tutorial01", 0);
		tutorialDialogue02 = _loadSoundClip ("Dialogue/Robotic/Tutorial02", 0);
		tutorialDialogueEnd = _loadSoundClip ("Dialogue/Robotic/TutorialEnd", 0);
		selectBaseDialogue = _loadSoundClip ("Dialogue/Robotic/SelectBase", 0);
		constructionDialogue = _loadSoundClip ("Dialogue/Robotic/Construction", 0);
		deployDialogue = _loadSoundClip ("Dialogue/Robotic/Deploy", 0);
		cityDialogue01 = _loadSoundClip ("Dialogue/Robotic/City01", 0);
		cityDialogue01 = _loadSoundClip ("Dialogue/Robotic/City02", 0);

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
		GameObject go = Instantiate(Resources.Load("Prefabs/sound"),emitter.position,Quaternion.identity) as GameObject;
        //go.transform.parent = emitter;

        //Create the source
		AudioSource source = go.GetComponent<AudioSource>();
        source.clip = clip;
        source.Play();

        StartCoroutine(AudioDestory(go, source.clip.length));
        //Destroy(go, clip.length+5f);
    }

    IEnumerator AudioDestory(GameObject go, float t)
    {
        yield return new WaitForSeconds(t);
		Destroy (go);
    }

}