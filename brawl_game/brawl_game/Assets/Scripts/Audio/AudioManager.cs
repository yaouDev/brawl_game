using UnityEngine;
using UnityEngine.Audio;
using System;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public Sound[] sounds;

    private void Awake()
    {
        instance ??= this;

        foreach(Sound s in sounds)
        {
            AddAudioSource(s, gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    public void Play(string soundName)
    {
        Play(soundName, sounds);
    }

    public void Play(string soundName, Sound[] collection)
    {
        Sound s = Array.Find(collection, sound => sound.name == soundName);

        if(s != null)
        {
            s.source.PlayOneShot(s.clip);
        }
        else
        {
            Debug.LogWarning("Couldn't find sound with name: " + soundName + "in collection " + collection);
        }
    }

    public void AddAudioSource(Sound s, GameObject target)
    {
        s.source = target.AddComponent<AudioSource>();
        s.source.clip = s.clip;

        s.source.volume = s.volume;
        s.source.pitch = s.pitch;
        s.source.loop = s.loop;
        s.source.outputAudioMixerGroup = s.mixerGroup;
    }
}
