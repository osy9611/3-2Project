using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMManager : MonoBehaviour
{
    public List<AudioClip> Clip;
    public AudioSource source;
    public int Count;
    public float DisCountVolume;
    public void FadeOn()
    {
        Invoke("VolumeFade", 0.3f);
    }

    public void VolumeFade()
    {
        if(source.volume>0)
        {
            source.volume -= DisCountVolume;
        }
    }

    private void OnDisable()
    {
        if(Clip.Count!=1)
        {
            source.clip = Clip[Count];
        }
    }

    
}
