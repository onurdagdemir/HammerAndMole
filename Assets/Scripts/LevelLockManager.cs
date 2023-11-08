using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelLockManager : MonoBehaviour
{
    public Button spaceEarth;
    public Button spaceMars;
    public Button mars;
    public Text spaceEarthTxt;
    public Text spaceMarsTxt;
    public Text marsTxt;
    public Text spaceEarthLock;
    public Text spaceMarsLock;
    public Text marsLock;

    private int level;

    private void Start()
    {
        controlLevels();
    }

    public void controlLevels()
    {
        level = PlayerPrefs.GetInt("playerLevel");

        if (level < 3)
        {
            spaceEarth.interactable = false;
            spaceEarthTxt.text = Lean.Localization.LeanLocalization.GetTranslationText("LevelSE");
            spaceMars.interactable = false;
            spaceMarsTxt.text = Lean.Localization.LeanLocalization.GetTranslationText("LevelSM");
            mars.interactable = false;
            marsTxt.text = Lean.Localization.LeanLocalization.GetTranslationText("LevelM");
        }
        else if (level < 8)
        {
            spaceEarth.interactable = true;
            spaceEarthLock.enabled = false;
            spaceEarthTxt.text = Lean.Localization.LeanLocalization.GetTranslationText("SpaceEarth");
            spaceMars.interactable = false;
            spaceMarsTxt.text = Lean.Localization.LeanLocalization.GetTranslationText("LevelSM");
            mars.interactable = false;
            marsTxt.text = Lean.Localization.LeanLocalization.GetTranslationText("LevelM");

        }
        else if (level < 15)
        {
            spaceEarth.interactable = true;
            spaceEarthLock.enabled = false;
            spaceEarthTxt.text = Lean.Localization.LeanLocalization.GetTranslationText("SpaceEarth");
            spaceMars.interactable = true;
            spaceMarsLock.enabled = false;
            spaceMarsTxt.text = Lean.Localization.LeanLocalization.GetTranslationText("SpaceMars");
            mars.interactable = false;
            marsTxt.text = Lean.Localization.LeanLocalization.GetTranslationText("LevelM");
        }
        else
        {
            spaceEarth.interactable = true;
            spaceEarthLock.enabled = false;
            spaceEarthTxt.text = Lean.Localization.LeanLocalization.GetTranslationText("SpaceEarth");
            spaceMars.interactable = true;
            spaceMarsLock.enabled = false;
            spaceMarsTxt.text = Lean.Localization.LeanLocalization.GetTranslationText("SpaceMars");
            mars.interactable = true;
            marsLock.enabled = false;
            marsTxt.text = "Mars";
        }
    }
}
