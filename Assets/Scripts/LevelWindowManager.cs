using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelWindowManager : MonoBehaviour
{
    [SerializeField]
    AudioClip expGainedSoundEffect;

    public Text levelTxt;
    public Text experienceTxt;
    public Slider experienceSlider;
    public Text expGainedTxt;
    public Animator expGainedAnimator;
    public GameObject levelUpParticule;
    public GameObject levelUpText;

    private int level;
    private int exp;
    private int expMax;
    private string levelName;

    // Start is called before the first frame update
    void Awake()
    {
        levelName = PlayerPrefs.GetString("levelName");
        GetValues();
    }

    private void Start()
    {
        LevelSystem.OnExpChanged += GetValues;
        LevelSystem.OnExpChanged += ExpGainedAnimation;
        LevelSystem.OnLevelUp += LevelUpAnimation;
        HealthManager.OnExpGained += ExperienceGained;
        OnlineHealthManager.OnExpGained += ExperienceGained;
    }

    private void OnDestroy()
    {
        LevelSystem.OnExpChanged -= GetValues;
        LevelSystem.OnExpChanged -= ExpGainedAnimation;
        LevelSystem.OnLevelUp -= LevelUpAnimation;
        HealthManager.OnExpGained -= ExperienceGained;
        OnlineHealthManager.OnExpGained -= ExperienceGained;
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

    private void ExperienceGained()
    {
        if (levelName == "EASY")
        {
            LevelSystem levelSystem = new LevelSystem();
            levelSystem.AddExperience(10);
        }
        else if (levelName == "NORMAL")
        {
            LevelSystem levelSystem = new LevelSystem();
            levelSystem.AddExperience(20);
        }
        else if (levelName == "HARD" || levelName == "ONLINE")
        {
            LevelSystem levelSystem = new LevelSystem();
            levelSystem.AddExperience(30);
        }
    }


    public void ExpGainedAnimation()
    {
        expGainedTxt.text = "+" + PlayerPrefs.GetInt("expAmount");
        if (expGainedAnimator.gameObject.activeSelf)
        {
            expGainedAnimator.SetTrigger("ExpGained");
        }
        AudioSource.PlayClipAtPoint(expGainedSoundEffect, Camera.main.transform.position);
    }


    public void LevelUpAnimation()
    {
        levelUpParticule.SetActive(true);
        levelUpText.SetActive(true);
    }


}
