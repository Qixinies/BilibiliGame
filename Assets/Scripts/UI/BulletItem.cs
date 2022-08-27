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

    public float CheckAttack(SoldierItem item)
    {
        Destroy(gameObject);
        if (!item.isUsed || item.isDestroyed)
        {
            return 0;
        }
        float result = 0;
        switch(curType)
        {
            case SoldierType.Gun:
                result = info.attackValue * (1.0f + 0.4f * info.level);
                item.HP -= result;
                break;
            case SoldierType.Cannon:
                result = info.attackValue * (1.0f + 0.3f * info.level);
                item.HP -= result;
                // 范围攻击
                int index = item.transform.GetSiblingIndex();
                int col = index % 3;
                if (index + 3 < SoldierManager.maxCount)
                {
                    // bottom
                    var t = item.transform.parent.GetChild(index + 3).GetComponent<SoldierItem>();
                    if (t.isUsed && !t.isDestroyed)
                    {
                        result += info.attackValue * 0.1f * (1.0f + 0.1f * info.level);
                        t.HP -= info.attackValue * 0.1f * (1.0f + 0.1f * info.level);
                    }
                }
                if (index - 3 >= 0)
                {
                    // top
                    var t = item.transform.parent.GetChild(index - 3).GetComponent<SoldierItem>();
                    if (t.isUsed && !t.isDestroyed)
                    {
                        result += info.attackValue * 0.1f * (1.0f + 0.1f * info.level);
                        t.HP -= info.attackValue * 0.1f * (1.0f + 0.1f * info.level);
                    }
                }
                if (col - 1 >= 0)
                {
                    // left
                    var t = item.transform.parent.GetChild(index - 1).GetComponent<SoldierItem>();
                    if (t.isUsed && !t.isDestroyed)
                    {
                        result += info.attackValue * 0.1f * (1.0f + 0.1f * info.level);
                        t.HP -= info.attackValue * 0.1f * (1.0f + 0.1f * info.level);
                    }
                }
                if (col + 1 < 3)
                {
                    // right
                    var t = item.transform.parent.GetChild(index + 1).GetComponent<SoldierItem>();
                    if (t.isUsed && !t.isDestroyed)
                    {
                        result += info.attackValue * 0.1f * (1.0f + 0.1f * info.level);
                        t.HP -= info.attackValue * 0.1f * (1.0f + 0.1f * info.level);
                    }
                }
                break;
            case SoldierType.Cure:
                result = info.attackValue * (1.0f + 0.5f * info.level);
                item.HP -= result;
                break;
            default:
                break;
        }
        return result;
    }
}
