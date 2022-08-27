using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoldierUI : MonoBehaviour
{
    public static SoldierUI instance = null;

    public List<SoldierItem> redSoldierList;
    public List<SoldierItem> blueSoldierList;

    public InputField input;
    public Button addBtn;
    public Button removeBtn;

    public InputField indexInput;
    public Button attackBtn;

    public Text redTotalAttackText;
    public Text redTotalCureText;
    public Text blueTotalAttackText;
    public Text blueTotalCureText;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        addBtn.onClick.AddListener(() =>
        {
            try
            {
                if (input.text.StartsWith("A"))
                {
                    int index = int.Parse(input.text.Replace("A", "")) - 1;
                    if (index >= 0 && index < SoldierManager.maxCount)
                    {
                        if(!GameController.manager.soldierMan.CanEnterTeam(TeamType.Red))
                        {
                            Debug.LogError("红方队伍已满员");
                        }
                        else if (redSoldierList[index].isDestroyed)
                        {
                            Debug.LogError("红方当前位置已经被摧毁");
                        }
                        else if (redSoldierList[index].isUsed)
                        {
                            Debug.LogError("红方当前位置已经被使用");
                        }
                        else
                        {
                            GameController.manager.soldierMan.SetSoldierState(TeamType.Red, index, true);
                        }
                    }
                }
                else if (input.text.StartsWith("B"))
                {
                    int index = int.Parse(input.text.Replace("B", ""));
                    if (index >= 0 && index < SoldierManager.maxCount)
                    {
                        if (!GameController.manager.soldierMan.CanEnterTeam(TeamType.Blue))
                        {
                            Debug.LogError("蓝方队伍已满员");
                        }
                        else if (blueSoldierList[index].isDestroyed)
                        {
                            Debug.LogError("蓝方当前位置已经被摧毁");
                        }
                        else if (blueSoldierList[index].isUsed)
                        {
                            Debug.LogError("蓝方当前位置已经被使用");
                        }
                        else
                        {
                            GameController.manager.soldierMan.SetSoldierState(TeamType.Blue, index, true);
                        }
                    }
                }
            }
            catch
            {

            }
        });

        removeBtn.onClick.AddListener(() =>
        {
            try
            {
                if (input.text.StartsWith("A"))
                {
                    int index = int.Parse(input.text.Replace("A", "")) - 1;
                    if (index >= 0 && index < SoldierManager.maxCount)
                    {
                        if (redSoldierList[index].isDestroyed)
                        {
                            Debug.LogError("红方当前位置已经被摧毁");
                        }
                        else if (redSoldierList[index].isUsed)
                        {
                            Debug.LogError("红方当前位置已经被使用");
                        }
                        else
                        {
                            GameController.manager.soldierMan.SetSoldierState(TeamType.Red, index, false);
                        }
                    }
                }
                else if (input.text.StartsWith("B"))
                {
                    int index = int.Parse(input.text.Replace("B", ""));
                    if (index >= 0 && index < SoldierManager.maxCount)
                    {
                        if (blueSoldierList[index].isDestroyed)
                        {
                            Debug.LogError("蓝方当前位置已经被摧毁");
                        }
                        else if (blueSoldierList[index].isUsed)
                        {
                            Debug.LogError("蓝方当前位置已经被使用");
                        }
                        else
                        {
                            GameController.manager.soldierMan.SetSoldierState(TeamType.Blue, index, false);
                        }
                    }
                }
            }
            catch
            {

            }
        });

        attackBtn.onClick.AddListener(() =>
        {
            try
            {
                int index = int.Parse(indexInput.text) - 1;
                if (index >= 0 & index < SoldierManager.maxCount)
                {
                    for (int i = 0; i < redSoldierList.Count; i++)
                    {
                        redSoldierList[i].SetFocus(blueSoldierList[index]);
                    }

                    for (int i = 0; i < blueSoldierList.Count; i++)
                    {
                        blueSoldierList[i].SetFocus(redSoldierList[index]);
                    }
                }
            }
            catch
            {

            }
        });
    }

    public void Update()
    {
        redTotalAttackText.text = "累计伤害 : " + GameController.manager.soldierMan.redTotalAttack.ToString("0.0");
        redTotalCureText.text = "累计治疗 : " + Mathf.Abs(GameController.manager.soldierMan.redTotalCure).ToString("0.0");
        blueTotalAttackText.text = "累计伤害 : " + GameController.manager.soldierMan.blueTotalAttack.ToString("0.0");
        blueTotalCureText.text = "累计治疗 : " + Mathf.Abs(GameController.manager.soldierMan.blueTotalCure).ToString("0.0");

        if (!GameController.manager.soldierMan.isDirty)
        {
            return;
        }
        GameController.manager.soldierMan.isDirty = false;
        for (int i = 0; i < redSoldierList.Count; i++)
        {
            if (GameController.manager.soldierMan.redSoliderActiveStateList[i])
            {
                redSoldierList[i].SetContent();
            }
            else
            {
                redSoldierList[i].EmptyContent();
            }
        }
        for (int i = 0; i < blueSoldierList.Count; i++)
        {
            if (GameController.manager.soldierMan.blueSoliderActiveStateList[i])
            {
                blueSoldierList[i].SetContent();
            }
            else
            {
                blueSoldierList[i].EmptyContent();
            }
        }
    }
}
