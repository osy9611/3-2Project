using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Player player;
    public float Speed;
    public float Distance;
    public float Height;
    public float MinDistance;
    public float MaxDistance;
    public float DistanceSpeed;
    Vector3 CameraPos;
    Vector3 PlayerPos;

    public float Amount;
    public float Duration;
    Vector3 Pos;
    public bool ZoomOn;
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        Distance = MinDistance;
    }

    public void ZoomIn()
    {
        if (Distance <= MinDistance)
        {
            Distance += DistanceSpeed;
        }
    }

    public void ZoomOut()
    {
        if (Distance >= MaxDistance)
        {
            Distance -= DistanceSpeed;
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
            transform.localPosition = (Vector3)Random.insideUnitCircle * Amount + Pos;

            timer += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = Pos;
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

        CameraPos = new Vector3(transform.position.x, player.gameObject.transform.position.y + Height, Distance);
        Pos = new Vector3(player.transform.position.x, player.gameObject.transform.position.y + Height, Distance);
        gameObject.transform.position = Vector3.Lerp(CameraPos, player.gameObject.transform.position, Speed * Time.smoothDeltaTime);
    }
}
