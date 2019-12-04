using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalMoon : MonoBehaviour
{
    public List<GameObject> Moons;
    public List<GameObject> SetMoons;

    public int MoonCount;
    Player player;
    public bool FinalMoonOn;

    [Header("달 생성 딜레이를 입력하세요")]
    public float MoonDelay;

    [Header("달이 생성될 간격을 입력하세요")]
    public float radius;
    float Angle;
    float AngleInterval;

    //회전시킬 오브젝트
    public GameObject WheelObj;
    public float TurnSpeed;

    Boss boss;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
        boss = GetComponent<Boss>();

        AngleInterval = (Mathf.PI*2)/Moons.Count;       
        for(int i=0;i<Moons.Count;i++)
        {
            Angle += AngleInterval;
            float x = radius * Mathf.Cos(Angle);
            float y = radius * Mathf.Sin(Angle);
            Moons[i].transform.position = new Vector2(x, y); 
        }

        for(int i=0;i<Moons.Count/2;i++)
        {
            SetMoons.Add(Moons[i]);
            SetMoons.Add(Moons[i+6]);
        }
    }

    public void MoonOn()
    {
        CancelInvoke("MoonOn");
        boss.Audio.Play(24);
        SetMoons[MoonCount].SetActive(true);
        MoonCount++;
    }

    public void DelayMoonOn(float time)
    {
        if(SetMoons.Count !=MoonCount)
        {
            Invoke("MoonOn", time);
        }
        else
        {
            FinalMoonOn = false;           
            StartCoroutine(Restart());
          
        }        
    }

    public void ResetMoon()
    {
        CancelInvoke("MoonOn");
        for (int i=0;i<SetMoons.Count;i++)
        {
            SetMoons[i].SetActive(false);
        }
    }
    
    IEnumerator Restart()
    {
        boss.bossAniManager.AttackOn();
        yield return new WaitForSeconds(1.01f);
        player.LightCheck(player.MaxLightCount, false);
        yield return new WaitForSeconds(2.0f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

   
    // Update is called once per frame
    void Update()
    {
        WheelObj.transform.Rotate(Vector3.forward, TurnSpeed * Time.deltaTime);

        if (FinalMoonOn)
        {
            DelayMoonOn(MoonDelay);
        }
    }
}
