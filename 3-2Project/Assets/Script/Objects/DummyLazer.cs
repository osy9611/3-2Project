using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyLazer : MonoBehaviour
{
    public Player player;
    RaycastHit2D hit;
    public Transform HitPoint;
    public Vector2 LayHit;
    public Transform OriginPos;
    public Transform Target;
    LineRenderer laser;
    public bool RazerOn;

    float Range;
    int Mask;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
        laser = GetComponent<LineRenderer>();
        Target = OriginPos;
        Range = Vector2.Distance(transform.position, Target.position);
    }

    private void OnDisable()
    {
        laser.SetPosition(1, Target.position);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Mask = 1 << 12;
        Mask = ~Mask;
        hit = Physics2D.Raycast(transform.position, transform.up, Range, Mask);
        laser.SetPosition(0, transform.position);

        if (hit.collider != null)
        {
            laser.SetPosition(1, hit.point);
        }
        else
        {
            laser.SetPosition(1, Target.position);
        }
    }
}
