using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundHandler : MonoBehaviour
{
    [Separator("REFERENCES")]
    [SerializeField] Transform soundContainer;

    [Separator("VOLUME")]
    [SerializeField] float sfxVolume;
    [SerializeField] float bgVolume;

    AudioSource bdSource;
    [SerializeField] AudioSource deathSound;
    [SerializeField] AudioSource bonusMusic;

    private void Awake()
    {
        bdSource = GetComponent<AudioSource>();
        bdSource.volume = bgVolume;



    }


   

    public void ChangeBgMusic(AudioClip clip)
    {
        bdSource.clip = clip;   
        bdSource.Play();
    }


    public void ControlDeathAudioSource(bool isTurnedOn)
    {
        deathSound.enabled = isTurnedOn;

        if (isTurnedOn)
        {
            //deathSound.Stop();
           // deathSound.Play();
        }

    }

    public void ControlBonusAudioSource(bool isTurnedOn)
    {
        bonusMusic.enabled = isTurnedOn;
    }


    public void CreateSFX(AudioClip clip, float modifier = 1)
    {

        GameObject newObject = new GameObject();
        newObject.transform.SetParent(soundContainer);
        AudioSource source = newObject.AddComponent<AudioSource>();
        newObject.AddComponent<DestroySelf>().SetUpDestroy(clip.length + 0.1f);
        source.clip = clip;
        source.volume = sfxVolume * modifier;
        source.Play();
    }



}
