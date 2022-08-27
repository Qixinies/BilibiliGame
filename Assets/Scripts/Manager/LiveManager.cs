using Liluo.BiliBiliLive;
using UnityEngine;
using System;
using System.Collections.Generic; 
/*
* TODO
* - [ ] 不能重复加入
* - [ ] 独立攻击系统
* - [ ] 独立操作日志
*/
public class LiveManager
{ 
    IBiliBiliLiveRequest req;
    List<User> userList = new List<User>();

    public void Destroy()
    {
        req.DisConnect();
        req = null;
    }

    public async void Init()
    {
        req = await BiliBiliLive.Connect(GameController.manager.roomId);
        req.OnDanmuCallBack += HandleDanmu;
    }

    public async void HandleDanmu(BiliBiliLiveDanmuData data)
    {
        Debug.Log($"<color=#60B8E0>弹幕</color> 用户名: {data.username}, 内容: {data.content}, 舰队等级: {data.guardLevel}");
        Sprite avatar = await BiliBiliLive.GetHeadSprite(data.userId);
        string content = data.content;
        if (content.StartsWith("A") || content.StartsWith("a") || content.StartsWith("B") || content.StartsWith("b"))
        {
            TeamType teamType;
            int index;
            if (content.StartsWith("A") || content.StartsWith("a"))
            {
                content = content.Replace("A", "");
                content = content.Replace("a", "");
                index = int.Parse(content) - 1;
                teamType = TeamType.Red;
            }
            else
            {
                content = content.Replace("B", "");
                content = content.Replace("b", "");
                index = int.Parse(content);
                teamType = TeamType.Blue;
            }
            User user = new User(data.username, avatar, teamType, index);
            var soldier = SoldierUI.instance.SetSoldier(teamType, index);
            soldier.userNameText.text = data.username;
        }
    }

}
