using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetManager : MonoBehaviour
{
    public List<Prism> prism;
    public List<Prism> SetPrism;
    public RockFall[] rockFall;
    public StepFloor[] stepFloor;
    public Player player;
    // Start is called before the first frame update
    void Start()
    {
        rockFall = FindObjectsOfType<RockFall>();
        stepFloor = FindObjectsOfType<StepFloor>();
        player = GetComponent<Player>();
    }

    public void ResetObjects()
    {
       
        for (int i = 0; i < rockFall.Length; i++)
        {
            rockFall[i].gameObject.SetActive(false);
        }

        for(int i=0;i<stepFloor.Length;i++)
        {
            stepFloor[i].gameObject.SetActive(false);
        }
    }
    
    public void SetObjects()
    {
        for (int i = 0; i < prism.Count; i++)
        {
            prism[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < rockFall.Length; i++)
        {
            rockFall[i].gameObject.SetActive(true);
        }
        for (int i = 0; i < stepFloor.Length; i++)
        {
            stepFloor[i].gameObject.SetActive(true);
        }
    }
}
