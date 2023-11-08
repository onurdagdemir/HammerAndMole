using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSystem
{
    public delegate void ExperienceEventHandler();
    public static event ExperienceEventHandler OnExpChanged;
    public static event ExperienceEventHandler OnLevelUp;

    private int level;
    private int exp;
    private int expMax;

    public void AddExperience(int amount)
    {
        level = PlayerPrefs.GetInt("playerLevel");
        exp = PlayerPrefs.GetInt("playerExp");
        expMax = level * 30;

        exp += amount;
        while(exp >= expMax)
        {
            level++;
            exp -= expMax;
            expMax = level * 30;
            OnLevelUp();
        }
        PlayerPrefs.SetInt("expAmount", amount);
        PlayerPrefs.SetInt("playerLevel", level);
        PlayerPrefs.SetInt("playerExp", exp);
        PlayerPrefs.SetInt("playerExpMax", expMax);
        OnExpChanged();
    }

}
