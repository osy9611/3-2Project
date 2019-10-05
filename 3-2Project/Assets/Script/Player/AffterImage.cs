using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AffterImage : MonoBehaviour
{
    public GameObject player;
    public float GhostDelay;
    private float GhostDelaySeconds;
    public GameObject Ghost;
    public bool MakeGhost;

    // Start is called before the first frame update
    void Start()
    {
        GhostDelaySeconds = GhostDelay;
    }

    // Update is called once per frame
    void Update()
    {
        if(MakeGhost)
        {
            StartCoroutine(GhostOn());
        }
    }

    IEnumerator GhostOn()
    {
        if(MakeGhost)
        {
            GameObject CurrentGhost = Instantiate(Ghost, transform.position, transform.rotation);
            Sprite CurrentSprite = GetComponent<SpriteRenderer>().sprite;
            CurrentGhost.transform.localScale = this.transform.localScale;
            CurrentGhost.GetComponent<SpriteRenderer>().sprite = CurrentSprite;
            CurrentGhost.transform.localScale = player.transform.localScale;
            Destroy(CurrentGhost, 0.5f);
            MakeGhost = false;
        }
      
        yield return new WaitForSeconds(GhostDelaySeconds);
        MakeGhost = false;
    }
}
