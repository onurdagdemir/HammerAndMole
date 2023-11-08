using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelWindowMenu : MonoBehaviour
{
    public Text levelTxt;
    public Text experienceTxt;
    public Slider experienceSlider;

    private int level;
    private int exp;
    private int expMax;

    void Awake()
    {

        if (!PlayerPrefs.HasKey("playerLevel"))
        {
            PlayerPrefs.SetInt("playerLevel", 1);
            PlayerPrefs.SetInt("playerExp", 0);
            PlayerPrefs.SetInt("playerExpMax", 30);
        }
        GetValues();
    }

    private void Start()
    {
        LevelSystem.OnExpChanged += GetValues;
        LevelSystem.OnLevelUp += OnLevelUpTest;
    }


    public void GetValues()
    {
        level = PlayerPrefs.GetInt("playerLevel");
        exp = PlayerPrefs.GetInt("playerExp");
        expMax = PlayerPrefs.GetInt("playerExpMax");
        levelTxt.text = level.ToString();
        experienceTxt.text = exp + "/" + expMax;
        experienceSlider.normalizedValue = (float)exp / expMax;
    }

    private void OnDestroy()
    {
        LevelSystem.OnExpChanged -= GetValues;
        LevelSystem.OnLevelUp -= OnLevelUpTest;
    }
    private void OnLevelUpTest()
    {

    }

}
