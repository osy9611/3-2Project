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

    public Player player;

    public AudioManager Audio;
    public BGMManager Bgm;
    public SceneChange Scene;

    public GameObject Fade;
    public Animator FadeAni;

    public bool PlayGame;

    void Awake()
    {
        video = GetComponent<VideoPlayer>();
        Time.timeScale = 0;
        video.clip = data[VideoCnt].Clip;
        VideoCnt++;
    }

    public void Play()
    {
        rawImage.enabled = true;
        video.clip = data[VideoCnt].Clip;
        video.Play();
        VideoCnt++;
        PlayGame = false;
        Time.timeScale = 0;      
    }

    public void GameSet()
    {
        Bgm.BgmFadeOn = true;
        Bgm.SceneChange = true;
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
            Bgm.source.clip= Bgm.bgms[0].Clip;
            Bgm.bgmName = Bgm.bgms[0].Name;
            Bgm.source.Play();
            PlayGame = true;
        }

        if (Bgm.BgmFadeOn && Bgm.SceneChange)
        {
            if(Bgm.source.volume>0)
            {
                Bgm.FadeOn();
                FadeAni.SetBool("End", true);
            }
            else
            {                
                Scene.DelayMainTitle(1.5f);
                Bgm.BgmFadeOn = false;                
            }           
        }
    }
}
