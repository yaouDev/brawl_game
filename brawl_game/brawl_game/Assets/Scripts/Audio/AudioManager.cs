using UnityEngine;
using UnityEngine.Audio;
using System;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Range(0, 1)]
    public float panningStrength;
    public float pitchMin = 0.75f;
    public float pitchMax = 1.25f;

    public Sound[] sounds;

    private void Awake()
    {
        if (Extensions.CheckIfAlreadyExists(this))
        {
            Destroy(gameObject);
            return;
        }
        instance ??= this;

        foreach(Sound s in sounds)
        {
            AddAudioSource(s, gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    public void Play(string soundName, bool varyPitch, Vector3 currentPos)
    {
        Play(soundName, true, currentPos, sounds);
    }

    public void Play(string soundName, bool varyPitch, Vector3 currentPos, Sound[] collection)
    {
        Sound s = Array.Find(collection, sound => sound.name == soundName);

        if(s != null)
        {
            s.source.panStereo = currentPos.normalized.x * panningStrength;

            if (varyPitch) s.source.pitch = UnityEngine.Random.Range(pitchMin, pitchMax);
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
