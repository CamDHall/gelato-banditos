using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    public static AudioManager Instance;

    AudioSource source;
    public AudioClip laser, astroCrack, splat, whip;

    private void Start()
    {
        Instance = this;
        source = GetComponent<AudioSource>();
    }

    public void Laser()
    {
        source.PlayOneShot(laser);
    }

    public void AstroCrack()
    {
        source.PlayOneShot(astroCrack);
    }

    public void BanditoSplat()
    {
        source.PlayOneShot(splat);
    }

    public void Whip()
    {
        source.volume = 2;
        source.PlayOneShot(whip);
    }

    public void ChangeVolume(float dist)
    {
        source.volume = 1 - (dist / 1500);
    }
}
