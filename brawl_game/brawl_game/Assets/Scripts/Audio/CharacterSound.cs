using UnityEngine;

public class CharacterSound : MonoBehaviour
{
    private AudioManager am;

    public Sound[] characterSounds;

    private void Start()
    {
        am = AudioManager.instance;

        foreach(Sound s in characterSounds)
        {
            am.AddAudioSource(s, gameObject);
        }
    }
}
