using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Player player;
    public float Speed;
    public float Distance;
    public float StoreDistance;
    public float ColDistance;
    public float DistanceSpeed;
    public float Height;
    public float StoreHeight;
    public float ColHeight;
    public float HeightSpeed;

    Vector3 CameraPos;
    Vector3 PlayerPos;

    public float Amount;
    public float Duration;
    Vector3 Pos;
    public bool ZoomOn;
    public bool HeightChangeOn;
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        ColDistance = Distance;
        ColHeight = Height;
    }

    public void CheckDistance(float distance)
    {
        if (Distance >= distance)
        {
            ColDistance = distance;
            ZoomOn = true;
        }
        else
        {
            ColDistance = distance;
            ZoomOn = false;
        }
    }

    public void CheckHeight(float height)
    {
        if(Height >= height)
        {
            ColHeight = height;
            HeightChangeOn = true;
        }
        else
        {
            ColHeight = height;
            HeightChangeOn = false;
        }
    }

    public void ZoomIn()
    {
        if (Distance <= ColDistance)
        {
            Distance += DistanceSpeed;
        }
    }

    public void ZoomOut()
    {
        if (Distance >= ColDistance)
        {
            Distance -= DistanceSpeed;
        }
    }

    public void HeightUp()
    {
        if (Height <= ColHeight)
        {
            Height += HeightSpeed;
        }
    }
    public void HeightDown()
    {
        if (Height >= ColHeight)
        {
            Height -= HeightSpeed;
        }
    }

    public void SaveZoomSet()
    {
        if (StoreDistance > Distance)
        {
            ColDistance = StoreDistance;
            ZoomOn = false;
        }
        else if (StoreDistance < Distance)
        {
            ColDistance = StoreDistance;
            ZoomOn = true;
        }
    }

    public void SaveHeightSet()
    {
        if (StoreHeight > Height)
        {
            ColHeight = StoreHeight;
            HeightChangeOn = false;
        }
        else if (StoreHeight < Height)
        {
            ColHeight = StoreHeight;
            HeightChangeOn = true;
        }
    }

    public void CameraShake()
    {
        StartCoroutine(Shake());
    }
    
    IEnumerator Shake()
    {
        yield return new WaitForSeconds(0.1f);
        float timer = 0;
        while (timer <= Duration)
        {
            transform.localPosition = Random.insideUnitSphere * Amount + transform.position;

            timer += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = CameraPos;
    }

    void FixedUpdate()
    {
        if (ZoomOn)
        {
            Invoke("ZoomOut", 0.1f);
        }
        else
        {
            Invoke("ZoomIn", 0.1f);
        }

        if(HeightChangeOn)
        {
            Invoke("HeightDown", 0.1f);
        }
        else
        {
            Invoke("HeightUp", 0.1f);
        }
        CameraPos = new Vector3(transform.position.x, player.gameObject.transform.position.y + Height, Distance);
        gameObject.transform.position = Vector3.Lerp(CameraPos, player.gameObject.transform.position, Speed * Time.smoothDeltaTime);
    }
}