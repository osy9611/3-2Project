using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string Name;
    public AudioClip Clip;
    private AudioSource Source;

    public float Volum;
    public bool Loop;

    public void SetSource(AudioSource _source)
    {
        Source = _source;
        Source.clip = Clip;
        Source.loop = Loop;
    }

    public void SetVolume()
    {
        Source.volume = Volum;
    }

    public void Play()
    {
        Source.Play();
    }

    public void OnePlay()
    {
        if(!Source.isPlaying)
        {
            Source.Play();
        }
    }

    public void Stop()
    {
        Source.Stop();
    }

    public void SetLoop()
    {
        Source.loop = true;
    }

    public void SetLoopCancel()
    {
        Source.loop = false;
    }
}


public class AudioManager : MonoBehaviour
{
    //static public AudioManager instance;
    [SerializeField]
    public List<Sound> sounds;

    // Start is called before the first frame update
    void Start()
    {
        for(int i=0;i<sounds.Count;i++)
        {
            GameObject soundObject = new GameObject("사운드 파일 이름 :" + i + "=" + sounds[i].Name);
            sounds[i].SetSource(soundObject.AddComponent<AudioSource>());
            soundObject.transform.SetParent(this.transform);

            sounds[i].SetVolume();
        }
        
    }

    public void Play(string _name)
    {
        for (int i = 0; i < sounds.Count; i++)
        {
            if (_name == sounds[i].Name)
            {
                sounds[i].Play();
                return;
            }
        }
    }


    public void OnePlay(string _name)
    {
        for (int i = 0; i < sounds.Count; i++)
        {
            if (_name == sounds[i].Name)
            {
                sounds[i].OnePlay();
                return;
            }
        }
    }

    public void Stop(string _name)
    {
        for (int i = 0; i < sounds.Count; i++)
        {
            if (_name == sounds[i].Name)
            {
                sounds[i].Stop();
                return;
            }
        }
    }

    public void SetLoop(string _name)
    {
        for (int i = 0; i < sounds.Count; i++)
        {
            if (_name == sounds[i].Name)
            {
                sounds[i].SetLoop();
                return;
            }
        }
    }

    public void SetLoopCancel(string _name)
    {
        for (int i = 0; i < sounds.Count; i++)
        {
            if (_name == sounds[i].Name)
            {
                sounds[i].SetLoopCancel();
                return;
            }
        }
    }

    public void SetVolume(string _name, float _Volume)
    {
        for (int i = 0; i < sounds.Count; i++)
        {
            if (_name == sounds[i].Name)
            {
                sounds[i].Volum = _Volume;
                sounds[i].SetVolume();
                return;
            }
        }
    }

    public void OnePlay(int num)
    {
         sounds[num].OnePlay();
    }

    public void Play(int num)
    {
        sounds[num].Play();
    }

   
}
