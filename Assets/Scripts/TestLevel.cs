using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLevel : MonoBehaviour
{

    public void Add20()
    {
        LevelSystem levelSystem = new LevelSystem();
        levelSystem.AddExperience(20);
    }
    public void Add200()
    {
        LevelSystem levelSystem = new LevelSystem();
        levelSystem.AddExperience(200);
    }
    public void ResetLevel()
    {
        PlayerPrefs.SetInt("playerLevel", 1);
        PlayerPrefs.SetInt("playerExp", 0);
        PlayerPrefs.SetInt("playerExpMax", 30);
        LevelSystem levelSystem = new LevelSystem();
        levelSystem.AddExperience(0);
    }

}
