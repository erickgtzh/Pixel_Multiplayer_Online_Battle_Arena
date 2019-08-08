using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Score
{
    public static int _scoreDeaths;
    public static int _scoreKills;
    public static string _userName = "username";

    public static void EnemyKilled()
    {
        _scoreKills++;
    }

    public static void PlayerDie()
    {
        _scoreDeaths++;
    }

    public static int GetKills()
    {
        return _scoreKills;
    }

    public static int GetDeaths()
    {
        return _scoreDeaths;
    }

    public static string GetUsername()
    {
        return _userName;
    }
}
