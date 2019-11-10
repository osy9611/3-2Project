using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BGM
{
    public string Name;
    public AudioClip Clip;
    public float Volume;
}

public class BGMManager : MonoBehaviour
{
    [SerializeField]
    public List<BGM> bgms;
    public AudioSource source;
    public int Count;
    public float DisCountVolume;
    public bool BgmFadeOn;

    public bool SceneChange;

    public string bgmName;

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
        if(bgms.Count!=1)
        {
            source.clip = bgms[Count].Clip;
        }
    }

    public void Play(string name)
    {
        for(int i=0;i<bgms.Count;i++)
        {
            if(bgms[i].Name==name)
            {
                BgmFadeOn = false;
                Debug.Log("들어옴");
                source.clip = bgms[i].Clip;
                source.volume =bgms[i].Volume;
                source.Play();               
            }
        }
    }

    private void Update()
    {
        if(!SceneChange && BgmFadeOn)
        {
            if(source.volume>0)
            {
                FadeOn();
            }
            else
            {
                Play(bgmName);
            }
           
        }
    }

}
