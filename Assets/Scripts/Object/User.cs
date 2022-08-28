using System;
using UnityEngine;

public class User
{
    public string username;
    public Sprite avatar;
    public TeamType teamType;
    public int index;

    public User(string username, Sprite avatar, TeamType teamType, int index)
    {
        this.username = username;
        this.avatar = avatar;
        this.teamType = teamType;
        this.index = index;
    }
}
