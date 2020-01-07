using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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

    public Boss boss;
    void Awake()
    {
        video = GetComponent<VideoPlayer>();
        player = FindObjectOfType<Player>();
        Time.timeScale = 0;
        if(data.Count>=1)
        {
            video.clip = data[VideoCnt].Clip;
        }
        else
        {
            Time.timeScale = 1;
            FadeAni.SetBool("FadeOn", false);
            video.clip = null;
            rawImage.enabled = false;
            Fade.SetActive(false);
            Fade.SetActive(true);
            Bgm.source.clip = Bgm.bgms[3].Clip;
            Bgm.bgmName = Bgm.bgms[3].Name;
            Bgm.source.Play();
            player.PlayOn = true;
            PlayGame = true;
        }
       
        VideoCnt++;
    }

  
    public void GameSet()
    {
        Bgm.BgmFadeOn = true;
        Bgm.SceneChange = true;
    }

    public void DelayGameSet(float time)
    {
        Invoke("GameSet", time);
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
            player.PlayOn = true;
            PlayGame = true;

            if(boss !=null)
            {
                boss.Complete();
            }
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
                if (SceneManager.GetActiveScene().name == "BossStage")
                {
                    Scene.DelayMainTitle(1.5f);                  
                }
                else
                {
                    Scene.BossStageDelay(1.5f);
                }
                    
                Bgm.BgmFadeOn = false;                
            }           
        }
    }
}
