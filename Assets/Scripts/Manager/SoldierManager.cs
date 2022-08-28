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

    public float redTotalAttack;
    public float redTotalCure;
    public float blueTotalAttack;
    public float blueTotalCure;
    
    private int maxPlayerCount = 12;
    private int redCount = 0;
    private int blueCount = 0;
    private bool initFinish = false;

    public void Init()
    {
        for(int i = 0; i < maxCount; i++)
        {
            redSoliderActiveStateList.Add(false);
            blueSoliderActiveStateList.Add(false);
        }

        for (int i = 0; i < maxCount; i++)
        {
            SetSoldierState(TeamType.Red, i, false);
            SetSoldierState(TeamType.Blue, i, false);
        }

        isDirty = true;
        initFinish = true;
    }

    public bool CanEnterTeam(TeamType teamType)
    {
        return (teamType == TeamType.Red ? redCount : blueCount) < maxPlayerCount;
    }

    public void SetSoldierState(TeamType type, int index, bool state)
    {
        if(index < 0 || index >= maxCount)
        {
            return;
        }
        if(state && !CanEnterTeam(type))
        {
            return;
        }
        if(type == TeamType.Red)
        {
            redSoliderActiveStateList[index] = state;
            redCount += (state ? 1 : (initFinish ? -1 : 0));
        } else
        {
            blueSoliderActiveStateList[index] = state;
            blueCount += (state ? 1 : (initFinish ? -1 : 0));
        }
        isDirty = true;
    }
}
