using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameTypeManager : MonoBehaviour
{
    
    private int gameType;
    private string gameTypeName;

    private int easyLevel = 30;
    private int normalLevel = 20;
    private int hardLevel = 10;

    // Start is called before the first frame update
    private void Awake()
    {
        if (!PlayerPrefs.HasKey("gameFrame"))
        {
            Application.targetFrameRate = 75;
        }
        else
        {
            Application.targetFrameRate = PlayerPrefs.GetInt("gameFrame");
        }
        PlayerPrefs.SetInt("OnlineConnection", 0);
    }


    public void SinglePlayerPrefs()
    {
        gameType = 0;
        gameTypeName = "SinglePlayer";
        PlayerPrefs.SetInt("gameType", gameType);
        PlayerPrefs.SetString("gameTypeName", gameTypeName);
    }

    public void MultiPlayerPrefs()
    {
        gameType = 1;
        gameTypeName = "MultiPlayer";
        PlayerPrefs.SetInt("gameType", gameType);
        PlayerPrefs.SetString("gameTypeName", gameTypeName);
    }

    public void OnlinePrefs()
    {
        gameType = 2;
        gameTypeName = "Online";
        PlayerPrefs.SetInt("gameType", gameType);
        PlayerPrefs.SetString("gameTypeName", gameTypeName);
        PlayerPrefs.SetString("levelName", "ONLINE");
    }

    public void EasyPrefs()
    {
        PlayerPrefs.SetInt("level", easyLevel);
        PlayerPrefs.SetString("levelName", "EASY");
    }

    public void NormalPrefs()
    {
        PlayerPrefs.SetInt("level", normalLevel);
        PlayerPrefs.SetString("levelName", "NORMAL");
    }

    public void HardPrefs()
    {
        PlayerPrefs.SetInt("level", hardLevel);
        PlayerPrefs.SetString("levelName", "HARD");
    }

    public void hammerPrefs()
    {
        PlayerPrefs.SetInt("playerType", 0);
    }

    public void molePrefs()
    {
        PlayerPrefs.SetInt("playerType", 1);
    }


    public void Forest()
    {
        PlayerPrefs.SetInt("terrainType", 0);
    }
    public void SpaceEarth()
    {
        PlayerPrefs.SetInt("terrainType", 1);
    }
    public void SpaceMars()
    {
        PlayerPrefs.SetInt("terrainType", 2);
    }
    public void Mars()
    {
        PlayerPrefs.SetInt("terrainType", 3);
    }

    public void Turkish()
    {
        Lean.Localization.LeanLocalization.SetCurrentLanguageAll("Turkish");
        PlayerPrefs.SetString("Language", "Turkish");
    }

    public void English()
    {
        Lean.Localization.LeanLocalization.SetCurrentLanguageAll("English");
        PlayerPrefs.SetString("Language", "English");
    }

    public void German()
    {
        Lean.Localization.LeanLocalization.SetCurrentLanguageAll("German");
        PlayerPrefs.SetString("Language", "German");
    }
}
