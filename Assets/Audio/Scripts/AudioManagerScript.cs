using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;

public class AudioManagerScript : MonoBehaviour
{
    [HideInInspector]
    public float maxVolume = 0.8f;
    public float MusicVolume = 0.6f;
    public float SoundVolume = 0.6f;

    public AudioClip[] songs;

    public AudioClip PickupClip;
    public AudioClip TurnFanfareClip;
    public AudioClip SpottedClip;
    public AudioClip UIClickClip;

    [HideInInspector]
    public List<AudioClip> playlist;

    private AudioSource _musicSource;

    private AudioSource audioSource
    {
        get { return GetComponent<AudioSource>(); }
    }
    private AudioClip playingTrack;

    private void playNextTrack()
    {
        if (playlist == null || playlist.Count == 0)
        {
            playlist = songs.ToList().OrderBy(x => Guid.NewGuid()).ToList();
        }

        playingTrack = playlist[0];
        playlist.Remove(playingTrack);
        audioSource.clip = playingTrack;
        
        audioSource.Play();
    }

    public void PlaySound(SoundType type)
    {
        switch (type)
        {
            case SoundType.Pickup:
                audioSource.PlayOneShot(PickupClip, SoundVolume);
                break;
            case SoundType.PlayerTurnFanfare:
                audioSource.PlayOneShot(TurnFanfareClip, SoundVolume);
                break;
            case SoundType.PlayerSpotted:
                audioSource.PlayOneShot(SpottedClip, SoundVolume);
                break;
            case SoundType.UIClick:
                audioSource.PlayOneShot(UIClickClip, SoundVolume);
                break;
        }
    }

	// Use this for initialization
	void Start () {
        
	    playNextTrack();
	}
	
	// Update is called once per frame
	void Update () {
	    if (!audioSource.isPlaying)
            playNextTrack();

	    if (Math.Abs(audioSource.volume - MusicVolume) > Double.Epsilon)
	        audioSource.volume = MusicVolume;
	}
}

public enum SoundType
{
    Pickup,
    PlayerTurnFanfare,
    PlayerSpotted,
    UIClick
}
