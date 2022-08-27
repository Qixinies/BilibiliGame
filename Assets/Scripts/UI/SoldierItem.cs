using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public enum SoldierType
{
    Gun = 0,
    Cannon,
    Cure
}

public enum TeamType
{
    Red = 0,
    Blue
}

public class SoldierItem : MonoBehaviour
{
    public TeamType teamType;
    public SoldierType soldierType;
    public List<GameObject> contentGoList;
    public Text userNameText;
    public Text indexText;
    public GameObject destroyGo;
    public Text levelText;
    public Slider hpSlider;

    public SoldierInfo soldierInfo;
    public bool isDestroyed;
    public bool isUsed;
    public BulletItem bulletItemPrefab;
    public SoldierItem focusItem;
    public int index;

    private float totalAttack;
    private float levelUpValue = 20;
    private float cd;

    public float HP
    {
        get
        {
            return hp;
        }
        set
        {
            if (isDestroyed || !isUsed)
            {
                return;
            }
            hp = value;
            if (hp <= 0)
            {
                DestroySelf();
            }
        }
    }
    private float hp;

    public void SetFocus(SoldierItem item)
    {
        focusItem = item;
    }

    private void Start()
    {
        EmptyContent();
        index = transform.GetSiblingIndex();
        indexText.text = (index + 1).ToString();
        isDestroyed = false;
        destroyGo.SetActive(false);
    }

    public void SetContent()
    {
        if (isDestroyed || isUsed)
        {
            return;
        }
        isUsed = true;
        for (int i = 0; i < contentGoList.Count; i++)
        {
            contentGoList[i].gameObject.SetActive((int)soldierType == i);
        }
        userNameText.text = "用户 " + Random.Range(0, 1000);
        switch (soldierType)
        {
            case SoldierType.Gun:
                soldierInfo = new SoldierInfo()
                {
                    maxHp = 100,
                    attackValue = 2,
                    cd = 1
                };
                break;
            case SoldierType.Cannon:
                soldierInfo = new SoldierInfo()
                {
                    maxHp = 60,
                    attackValue = 5,
                    cd = 2
                };
                break;
            case SoldierType.Cure:
                soldierInfo = new SoldierInfo()
                {
                    maxHp = 80,
                    attackValue = -3,
                    cd = 3,
                    attackEnemy = false
                };
                break;
            default:
                break;
        }
        HP = soldierInfo.maxHp;
        cd = soldierInfo.cd;
    }

    public void EmptyContent()
    {
        if (isDestroyed)
        {
            return;
        }
        isUsed = false;
        for (int i = 0; i < contentGoList.Count; i++)
        {
            contentGoList[i].gameObject.SetActive(false);
        }
        userNameText.text = "暂无用户";
        totalAttack = 0;
        cd = 0;
    }

    public void DestroySelf()
    {
        isDestroyed = true;
        hpSlider.value = 0;
        destroyGo.SetActive(true);
    }

    private void CheckLevelUp()
    {
        if(totalAttack >= levelUpValue)
        {
            totalAttack -= levelUpValue;
            UpgradeLevel();
        }
    }
    

    private void Update()
    {
        if (isDestroyed || soldierInfo == null)
        {
            return;
        }
        if (HP <= 0)
        {
            DestroySelf();
        }
        levelText.text = "Lv." + soldierInfo.level.ToString();
        hpSlider.value = HP / soldierInfo.maxHp;

        cd -= Time.deltaTime;
        if (cd <= 0)
        {
            cd = soldierInfo.cd;
            Attack();
            CheckLevelUp();
        }
    }

    private void Attack()
    {
        // 寻找最近的Item
        SoldierItem attackItem = null;
        // 如果锁定的item无效则更新
        if (focusItem != null && (!focusItem.isUsed || focusItem.isDestroyed))
        {
            focusItem = null;
        }

        if (focusItem == null)
        {
            int minDistance = int.MaxValue;
            if (soldierInfo.attackEnemy)
            {
                if (teamType == TeamType.Red)
                {
                    for (int i = 0; i < SoldierUI.instance.blueSoldierList.Count; i++)
                    {

                        if (SoldierUI.instance.blueSoldierList[i].isUsed && !SoldierUI.instance.blueSoldierList[i].isDestroyed
                            && GetDistance(SoldierUI.instance.blueSoldierList[i].index) < minDistance)
                        {
                            minDistance = GetDistance(SoldierUI.instance.blueSoldierList[i].index);
                            attackItem = SoldierUI.instance.blueSoldierList[i];
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < SoldierUI.instance.redSoldierList.Count; i++)
                    {
                        if (SoldierUI.instance.redSoldierList[i].isUsed && !SoldierUI.instance.redSoldierList[i].isDestroyed
                            && GetDistance(SoldierUI.instance.redSoldierList[i].index) < minDistance)
                        {
                            minDistance = GetDistance(SoldierUI.instance.redSoldierList[i].index);
                            attackItem = SoldierUI.instance.redSoldierList[i];
                        }
                    }
                }
            }
            else
            {
                if (teamType == TeamType.Red)
                {
                    for (int i = 0; i < SoldierUI.instance.redSoldierList.Count; i++)
                    {
                        if (SoldierUI.instance.redSoldierList[i].isUsed && !SoldierUI.instance.redSoldierList[i].isDestroyed
                            && SoldierUI.instance.redSoldierList[i].HP < SoldierUI.instance.redSoldierList[i].soldierInfo.maxHp
                            && GetDistance(SoldierUI.instance.redSoldierList[i].index) < minDistance)
                        {
                            minDistance = GetDistance(SoldierUI.instance.redSoldierList[i].index);
                            attackItem = SoldierUI.instance.redSoldierList[i];
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < SoldierUI.instance.blueSoldierList.Count; i++)
                    {
                        if (SoldierUI.instance.blueSoldierList[i].isUsed && !SoldierUI.instance.blueSoldierList[i].isDestroyed
                            && SoldierUI.instance.blueSoldierList[i].HP < SoldierUI.instance.blueSoldierList[i].soldierInfo.maxHp
                            && GetDistance(SoldierUI.instance.blueSoldierList[i].index) < minDistance)
                        {
                            minDistance = GetDistance(SoldierUI.instance.blueSoldierList[i].index);
                            attackItem = SoldierUI.instance.blueSoldierList[i];
                        }
                    }
                }
            }
        }
        else
        {
            attackItem = focusItem;
        }

        if (attackItem != null)
        {
            BulletItem bulletItem = Instantiate(bulletItemPrefab);
            bulletItem.transform.SetParent(SoldierUI.instance.transform, false);
            bulletItem.transform.position = transform.position;
            bulletItem.SetContent(soldierInfo, soldierType);
            float time = Vector3.Distance(attackItem.transform.position, transform.position) / 1600.0f;
            DOTween.To(() => bulletItem.transform.position, x => bulletItem.transform.position = x, attackItem.transform.position, time).OnComplete(() =>
            {
                float attackValue = bulletItem.CheckAttack(attackItem);
                totalAttack += attackValue;
                if(soldierInfo.attackEnemy)
                {
                    if(teamType == TeamType.Red)
                    {
                        GameController.manager.soldierMan.redTotalAttack += attackValue;
                    } else
                    {
                        GameController.manager.soldierMan.blueTotalAttack += attackValue;
                    }
                } else
                {
                    if (teamType == TeamType.Red)
                    {
                        GameController.manager.soldierMan.redTotalCure += attackValue;
                    }
                    else
                    {
                        GameController.manager.soldierMan.blueTotalCure += attackValue;
                    }
                }
                if (attackItem.isDestroyed)
                {
                    UpgradeLevel();
                }
            }).SetEase(Ease.Linear);
        }
    }

    private void UpgradeLevel()
    {
        soldierInfo.level += 1;
        HP = soldierInfo.maxHp;
    }



    private int GetDistance(int targetIndex)
    {
        if (targetIndex < 0 || targetIndex >= SoldierManager.maxCount)
        {
            return int.MaxValue;
        }
        int curRow = index / 3;
        int curCol = index % 3;
        int targetRow = targetIndex / 3;
        int targetCol = targetIndex % 3;
        return Mathf.Abs(targetRow - curRow) + (teamType == TeamType.Red ? (2 - curCol) + targetCol : curCol + (2 - targetCol));
    }

}