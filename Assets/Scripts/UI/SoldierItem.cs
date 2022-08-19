using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

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
        indexText.text = (transform.GetSiblingIndex() + 1).ToString();
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
    }

    public void DestroySelf()
    {
        isDestroyed = true;
        hpSlider.value = 0;
        destroyGo.SetActive(true);
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
        levelText.text = soldierInfo.level.ToString();
        hpSlider.value = HP / soldierInfo.maxHp;

        cd -= Time.deltaTime;
        if (cd <= 0)
        {
            cd = soldierInfo.cd;
            Attack();
        }
    }

    private void Attack()
    {
        // 寻找最近的Item
        SoldierItem attackItem = null;
        if (focusItem == null || !focusItem.isUsed || focusItem.isDestroyed)
        {
            if (soldierInfo.attackEnemy)
            {
                if (teamType == TeamType.Red)
                {
                    for (int i = 0; i < SoldierUI.instance.blueSoldierList.Count; i++)
                    {
                        if (SoldierUI.instance.blueSoldierList[i].isUsed && !SoldierUI.instance.blueSoldierList[i].isDestroyed)
                        {
                            attackItem = SoldierUI.instance.blueSoldierList[i];
                            break;
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < SoldierUI.instance.redSoldierList.Count; i++)
                    {
                        if (SoldierUI.instance.redSoldierList[i].isUsed && !SoldierUI.instance.redSoldierList[i].isDestroyed)
                        {
                            attackItem = SoldierUI.instance.redSoldierList[i];
                            break;
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
                            && SoldierUI.instance.redSoldierList[i].HP < SoldierUI.instance.redSoldierList[i].soldierInfo.maxHp)
                        {
                            attackItem = SoldierUI.instance.redSoldierList[i];
                            break;
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < SoldierUI.instance.blueSoldierList.Count; i++)
                    {
                        if (SoldierUI.instance.blueSoldierList[i].isUsed && !SoldierUI.instance.blueSoldierList[i].isDestroyed
                            && SoldierUI.instance.blueSoldierList[i].HP < SoldierUI.instance.blueSoldierList[i].soldierInfo.maxHp)
                        {
                            attackItem = SoldierUI.instance.blueSoldierList[i];
                            break;
                        }
                    }
                }
            }
        } else
        {
            attackItem = focusItem;
        }

        if (attackItem != null)
        {
            BulletItem bulletItem = Instantiate(bulletItemPrefab);
            bulletItem.transform.SetParent(SoldierUI.instance.transform, false);
            bulletItem.transform.position = transform.position;
            bulletItem.SetContent(soldierInfo, soldierType);
            float time = Vector3.Distance(attackItem.transform.position, transform.position) / 800.0f;
            DOTween.To(() => bulletItem.transform.position, x => bulletItem.transform.position = x, attackItem.transform.position, time).OnComplete(() =>
            {
                bulletItem.CheckAttack(attackItem);
            });
        }
    }

}
