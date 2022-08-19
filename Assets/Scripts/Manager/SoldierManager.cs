using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierInfo
{
    public int level = 1;
    public float maxHp;
    public float attackValue;
    public float cd;
    public bool attackEnemy = true;
}

public class SoldierManager
{
    public List<bool> redSoliderActiveStateList = new List<bool>();
    public List<bool> blueSoliderActiveStateList = new List<bool>();

    public bool isDirty;
    public const int maxCount = 15;

    public const int maxPlayerCount = 12;

    public void Init()
    {
        for(int i = 0; i < maxCount; i++)
        {
            redSoliderActiveStateList.Add(true);
            blueSoliderActiveStateList.Add(true);
        }
        isDirty = true;
    }

    public void SetSoldierState(TeamType type, int index, bool state)
    {
        if(index <0 || index >= maxCount)
        {
            return;
        }
        if(type == TeamType.Red)
        {
            redSoliderActiveStateList[index] = state;
        } else
        {
            blueSoliderActiveStateList[index] = state;
        }
        isDirty = true;
    }
}
