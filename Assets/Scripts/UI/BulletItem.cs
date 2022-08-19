using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletItem : MonoBehaviour
{
    public SoldierInfo info;
    public SoldierType curType;

    public void SetContent(SoldierInfo soldierInfo, SoldierType type)
    {
        info = soldierInfo;
        curType = type;
    }

    public void CheckAttack(SoldierItem item)
    {
        Destroy(gameObject);
        if (!item.isUsed || item.isDestroyed)
        {
            return;
        }
        switch(curType)
        {
            case SoldierType.Gun:
                item.HP -= info.attackValue;
                break;
            case SoldierType.Cannon:
                item.HP -= info.attackValue;
                // 范围攻击
                int index = item.transform.GetSiblingIndex();
                int col = index % 3;
                Debug.LogError(col + " - " + index);
                if (index + 3 < SoldierManager.maxCount)
                {
                    // bottom
                    var t = item.transform.parent.GetChild(index + 3).GetComponent<SoldierItem>();
                    if (t.isUsed && !t.isDestroyed)
                    {
                        t.HP -= info.attackValue * 0.1f;
                    }
                }
                if (index - 3 >= 0)
                {
                    // top
                    Debug.LogError("top");
                    var t = item.transform.parent.GetChild(index - 3).GetComponent<SoldierItem>();
                    if (t.isUsed && !t.isDestroyed)
                    {
                        t.HP -= info.attackValue * 0.1f;
                    }
                }
                if (col - 1 >= 0)
                {
                    // left
                    Debug.LogError("left");
                    var t = item.transform.parent.GetChild(index - 1).GetComponent<SoldierItem>();
                    if (t.isUsed && !t.isDestroyed)
                    {
                        t.HP -= info.attackValue * 0.1f;
                    }
                }
                if (col + 1 < 3)
                {
                    // right
                    Debug.LogError("right");
                    var t = item.transform.parent.GetChild(index + 1).GetComponent<SoldierItem>();
                    if (t.isUsed && !t.isDestroyed)
                    {
                        t.HP -= info.attackValue * 0.1f;
                    }
                }
 

                break;
            case SoldierType.Cure:
                item.HP -= info.attackValue;
                break;
            default:
                break;
        }
    }
}
