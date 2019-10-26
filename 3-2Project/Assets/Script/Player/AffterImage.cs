using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.Experimental.U2D.Animation;

[System.Serializable]
public struct AfterData
{
    public GameObject obj;
    public GhostData data;
}

public class AffterImage : MonoBehaviour
{
    public GameObject player;
    public float GhostDelay;
    private float GhostDelaySeconds;
    public GameObject Ghost;
    public Transform GhostPool;
    public bool MakeGhost;

    public Transform[] Bonse;
    public int GhostNum;

    [SerializeField]
    public List<AfterData> Ghosts;
    // Start is called before the first frame update
    void Start()
    {
        GhostDelaySeconds = GhostDelay;

        for(int i=0;i< GhostNum; i++)
        {
            AfterData Dummy;
            Dummy.obj = Instantiate(Ghost, transform.position, transform.rotation);
            Dummy.data = Dummy.obj.GetComponent<GhostData>();
            Dummy.obj.transform.SetParent(GhostPool);
            Dummy.obj.SetActive(false);
            Ghosts.Add(Dummy);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if(MakeGhost)
        {
            AfterData data = Ghosts[0];
            if(data.obj.activeSelf)
            {
                data.obj.SetActive(false);
            }
            data.obj.transform.position = transform.position;
            data.obj.SetActive(true);
            Ghosts.RemoveAt(0);
            data.data.TransChange(Bonse);
            data.obj.transform.transform.localScale = this.transform.localScale;
            data.obj.transform.localScale = player.transform.localScale;
            Ghosts.Add(data);
            MakeGhost = false;
            Invoke("GhostOn", GhostDelaySeconds);
        }
    }

    public void GhostOn()
    {
        MakeGhost = false;
    }

}
