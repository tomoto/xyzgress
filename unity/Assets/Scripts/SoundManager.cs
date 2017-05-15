using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AudioClipExtension
{
    public static void Play(this AudioClip sound, float volume = 1.0f)
    {
        SoundManager.GetInstance().AudioSource.PlayOneShot(sound, volume);
    }

    public static IEnumerator PlayWithDelay(this AudioClip sound, float seconds, float volume = 1.0f)
    {
        yield return new WaitForSeconds(seconds);
        sound.Play(volume);
    }
}

public class SoundManager : MonoBehaviour {
    public static SoundManager GetInstance()
    {
        return GameObject.Find("SoundManager").GetComponent<SoundManager>();
    }

    public AudioSource AudioSource { get { return GetComponent<AudioSource>(); } }

    public AudioClip MenuMoveSound;
    public AudioClip MenuSelectSound;

    public AudioClip WalkSound;
    public AudioClip BumpSound;
    public AudioClip HackSound;
    public AudioClip BurstSound;
    public AudioClip LinkSound;
    public AudioClip CFCreatedSound;
    public AudioClip CFDestroyedSound;

    public AudioClip PortalDamagedSound;
    public AudioClip PortalDestroyedSound;
    public AudioClip PortalCapturedSound;
    public AudioClip PortalRechargedSound;

    public AudioClip DayUpSound;
    public AudioClip FinishSound;
    public AudioClip LevelUpSound;

    public AudioClip[] StartUpVoices;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
