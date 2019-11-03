using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
[System.Serializable]
public class VideoData
{
    public string Name;
    public VideoClip Clip;
    public bool Start;
    public bool Done;    
}
public class CutSceneManager : MonoBehaviour
{
    [SerializeField]
    public List<VideoData> data;
    public VideoPlayer video;
    public RawImage rawImage;   
    public int VideoCnt = 0;

    public AudioManager Audio;
    public BGMManager Bgm;
    public SceneChange Scene;

    public GameObject Fade;
    public Animator FadeAni;
    bool BgmFadeOn;

    void Awake()
    {
        video = GetComponent<VideoPlayer>();
        Time.timeScale = 0;
        video.clip = data[VideoCnt].Clip;
        VideoCnt++;
    }

    public void Play()
    {
        Bgm.Count++;
        Bgm.gameObject.SetActive(false);
        rawImage.enabled = true;
        video.clip = data[VideoCnt].Clip;
        video.Play();
        VideoCnt++;
        Time.timeScale = 0;      
    }

    public void GameSet()
    {
        BgmFadeOn= true;
    }
    
    void Update()
    {
        if(video.clip !=null &&video.isPaused)
        {
            Time.timeScale = 1;
            FadeAni.SetBool("FadeOn", false);
            video.clip = null;
            rawImage.enabled=false;
            Fade.SetActive(false);
            Fade.SetActive(true);
            Bgm.gameObject.SetActive(true);
        }

        if(BgmFadeOn)
        {
            if(Bgm.source.volume>0)
            {
                Bgm.FadeOn();
            }
            else
            {
                FadeAni.SetBool("FadeOn", true);
                Scene.DelayMainTitle(1.5f);
                BgmFadeOn = false;                
            }           
        }
    }
}
