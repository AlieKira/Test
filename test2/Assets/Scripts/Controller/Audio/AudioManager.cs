using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : BaseManager
{    private const string Sound_Prefix = "Sounds/";
    //public const string Sound_Alert = "Alert";
    //public const string Sound_ArrowShoot = "ArrowShoot";
    //public const string Sound_Bg_Fast = "Bg(fast)";
    //public const string Sound_Bg_Moderate = "Bg(moderate)";
    //public const string Sound_ButtonClick = "ButtonClick";
    //public const string Sound_Miss = "Miss";
    //public const string Sound_ShootPerson = "ShootPerson";
    //public const string Sound_Timer = "Timer";

    private AudioSource bgAudioSource;
    private AudioSource normalAudioSource;

    private bool isPlayBgSound = false;
    private bool isPlayNormalSound = false;

    private string nomalSoundName;
    private string bgSoundName;

    public float bgSoundValue;
    public float normalSoundValue;

    public override void OnInit()
    {
        GameObject audioSourceGO = new GameObject("AudioSource(GameObject)");
        bgAudioSource = audioSourceGO.AddComponent<AudioSource>();
        normalAudioSource = audioSourceGO.AddComponent<AudioSource>();

        PlaySound(bgAudioSource, LoadSound(Audios.Sound_Bg_Moderate), bgSoundValue, true);
    }

    public void OnUpdate()
    {
        if (isPlayBgSound)
        {
            PlaySound(bgAudioSource, LoadSound(bgSoundName), bgSoundValue, true);
            isPlayBgSound = false;
        }

        if (isPlayNormalSound)
        {
            PlaySound(bgAudioSource, LoadSound(nomalSoundName), normalSoundValue, false);
            isPlayNormalSound = false;
        }
    }

    public void PlayBgSound(string soundName)
    {
        isPlayBgSound = true;
        bgSoundName = soundName;
    }
    public void PlayNormalSound(string soundName)
    {
        isPlayNormalSound = true;
        nomalSoundName = soundName;
    }

    private void PlaySound(AudioSource audioSource, AudioClip clip, float volume, bool loop = false)
    {
        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.loop = loop;
        audioSource.Play();
    }
    private AudioClip LoadSound(string soundsName)
    {
        return Resources.Load<AudioClip>(Sound_Prefix + soundsName);
    }
}
