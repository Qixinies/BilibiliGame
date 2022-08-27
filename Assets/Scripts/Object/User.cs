using System;
using UnityEngine;

public class User
{
    string username;
    Sprite avatar;
    TeamType teamType;
    int index;

    public User(string username, Sprite avatar, TeamType teamType, int index)
    {
        this.username = username;
        this.avatar = avatar;
        this.teamType = teamType;
        this.index = index;
    }
}
