using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneManagementScript : MonoBehaviour
{

    [SerializeField]
    AudioClip buttonClickSoundEffect;

    public GameObject SettingsScreen;
    public GameObject TerrainSelectMenu;
    public GameObject LoadingScreen;
    public GameObject PlayerTypeSelectMenu;
    public GameObject SinglePlayerTypeSelectMenu;
    public GameObject MultiPlayerTypeSelectMenu;
    public Text MultiPlayerTypeInfoTxt;

    public GameObject SingleFirstTimeName;
    public GameObject MultiFirstTimeName;
    public GameObject OnlineFirstTimeName;
    public GameObject SingleLevelScreen;
    private int gameType;

    private void Awake()
    {
        //PlayerPrefs.DeleteAll();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (!PlayerPrefs.HasKey("hammerName"))
        {
            SingleFirstTimeName.SetActive(true);
        }
    }

    public void SettingsScreenOn()
    {
        SettingsScreen.SetActive(true);
        AudioSource.PlayClipAtPoint(buttonClickSoundEffect, Camera.main.transform.position);
    }

    public void GoOnline()
    {
        LoadingScreen.SetActive(true);
        AudioSource.PlayClipAtPoint(buttonClickSoundEffect, Camera.main.transform.position);
        Invoke(nameof(GoLobby), 0.5f);
    }


    private void GoLobby()
    {
        SceneManager.LoadScene("LobbyScene");
    }


    public void GoMenu()
    {
        TerrainSelectMenu.SetActive(false);
        PlayerTypeSelectMenu.SetActive(false);
        AudioSource.PlayClipAtPoint(buttonClickSoundEffect, Camera.main.transform.position);
    }

    public void BackToTerrain()
    {
        SingleLevelScreen.SetActive(false);
        SinglePlayerTypeSelectMenu.SetActive(false);
        MultiPlayerTypeSelectMenu.SetActive(false);
        TerrainSelectMenu.SetActive(true);
        AudioSource.PlayClipAtPoint(buttonClickSoundEffect, Camera.main.transform.position);
    }

    public void GoTerrainSelectMenu()
    {
        TerrainSelectMenu.SetActive(true);
        AudioSource.PlayClipAtPoint(buttonClickSoundEffect, Camera.main.transform.position);
    }

    public void MultiPlayer()
    {
        if (PlayerPrefs.HasKey("moleName"))
        {
            TerrainSelectMenu.SetActive(true);
        }
        else
        {
            MultiFirstTimeName.SetActive(true);
        }

        AudioSource.PlayClipAtPoint(buttonClickSoundEffect, Camera.main.transform.position);
    }

    public void Online()
    {
        if (PlayerPrefs.HasKey("onlinePlayerName"))
        {
            PlayerTypeSelectMenu.SetActive(true);
        }
        else
        {
            OnlineFirstTimeName.SetActive(true);
        }

        AudioSource.PlayClipAtPoint(buttonClickSoundEffect, Camera.main.transform.position);
    }

    public void SingleLevelSelectMenu()
    {
        SinglePlayerTypeSelectMenu.SetActive(false);
        SingleLevelScreen.SetActive(true);
        AudioSource.PlayClipAtPoint(buttonClickSoundEffect, Camera.main.transform.position);
    }


    public void LevelAfterTerrain()
    {
        gameType = PlayerPrefs.GetInt("gameType");
        if (gameType == 0)
        {
            SingleLevelScreen.SetActive(false);
            TerrainSelectMenu.SetActive(false);
            SinglePlayerTypeSelectMenu.SetActive(true);
        }
        else if (gameType == 1)
        {
            Lean.Localization.LeanLocalization.SetToken("MenuSingleName", PlayerPrefs.GetString("hammerName"), false);
            TerrainSelectMenu.SetActive(false);
            MultiPlayerTypeSelectMenu.SetActive(true);
        }

        AudioSource.PlayClipAtPoint(buttonClickSoundEffect, Camera.main.transform.position);
    }

    public void OnStartOfflineGame()
    {
        LoadingScreen.SetActive(true);
        AudioSource.PlayClipAtPoint(buttonClickSoundEffect, Camera.main.transform.position);
        Invoke(nameof(GoOfflineGame), 0.5f);
    }

    private void GoOfflineGame()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void OnExitButtonClicked()
    {
        AudioSource.PlayClipAtPoint(buttonClickSoundEffect, Camera.main.transform.position);
        Invoke(nameof(ExitGame), 0.5f);
    }
    private void ExitGame()
    {
        Application.Quit();
    }




}
