using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameTypeManager : MonoBehaviour
{
    public GameObject singlePlayerMole;
    public GameObject multiPlayerMole;
    public GameObject hammer;
    public GameObject hammerPlayer;
    public GameObject serverManager;
    public GameObject HealthManager;
    public GameObject OnlineHealthManager;
    public GameObject startCountDownManager;
    public GameObject infoScreenManager;
    public GameObject onlineInfoScreenManager;
    public GameObject onlinePointAnmManager;
    public GameObject levelBackground;
    private int gameType;
    private int gameKeySet;
    private int playerType;

    public GameObject rightJoystick;
    public GameObject leftJoystick;

    // Start is called before the first frame update
    void Start()
    {
        Lean.Localization.LeanLocalization.SetCurrentLanguageAll(PlayerPrefs.GetString("Language"));
        gameType = PlayerPrefs.GetInt("gameType");
        gameKeySet = PlayerPrefs.GetInt("gameKeySet");
        playerType = PlayerPrefs.GetInt("playerType");
        if (gameType == 0)
        {
            if(playerType == 0)
            {
                hammer.SetActive(true);
                Destroy(hammerPlayer);
                singlePlayerMole.SetActive(true);
                Destroy(multiPlayerMole);
            } else if (playerType == 1)
            {
                hammerPlayer.SetActive(true);
                Destroy(hammer);
                multiPlayerMole.SetActive(true);
                Destroy(singlePlayerMole);
            }
            Destroy(serverManager);
            HealthManager.SetActive(true);
            Destroy(OnlineHealthManager);
            Destroy(onlineInfoScreenManager);
            Destroy(onlinePointAnmManager);
            levelBackground.SetActive(true);

            if (gameKeySet == 0)
            {
                Destroy(rightJoystick);
            }
            else if(gameKeySet == 1)
            {
                Destroy(leftJoystick);
            }

        }
        else if (gameType == 1)
        {
            Destroy(singlePlayerMole);
            Destroy(hammerPlayer);
            multiPlayerMole.SetActive(true);
            hammer.SetActive(true);
            Destroy(serverManager);
            HealthManager.SetActive(true);
            Destroy(OnlineHealthManager);
            Destroy(onlineInfoScreenManager);
            Destroy(onlinePointAnmManager);
            Destroy(levelBackground);
        }
        else if (gameType == 2)
        {
            Destroy(singlePlayerMole);
            Destroy(multiPlayerMole);
            Destroy(hammer);
            Destroy(hammerPlayer);
            serverManager.SetActive(true);
            Destroy(HealthManager);
            OnlineHealthManager.SetActive(true);
            Destroy(startCountDownManager);
            Destroy(infoScreenManager);
            onlinePointAnmManager.SetActive(true);
            levelBackground.SetActive(true);

            if (gameKeySet == 0)
            {
                Destroy(rightJoystick);
            }
            else if (gameKeySet == 1)
            {
                Destroy(leftJoystick);
            }
        }

        Destroy(gameObject);
    }

}
