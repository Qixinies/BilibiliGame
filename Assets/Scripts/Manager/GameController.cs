using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController manager = null;
    public SoldierManager soldierMan = new SoldierManager();
    private void Awake()
    {
        if(manager == null)
        {
            manager = this;
        } else if(manager != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        soldierMan.Init();
    }
}
