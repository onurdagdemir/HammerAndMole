using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SettingsScreenManager : MonoBehaviour
{
    public GameObject SettingsScreen;
    public GameObject NamesScreen;
    public InputField SingleNameInputField;
    public InputField MultiNameInputField;
    public InputField OnlineNameInputField;
    public InputField FSingleNameInputField;
    public InputField FMultiNameInputField;
    public InputField FOnlineNameInputField;

    public GameObject SingleFirstTimeName;
    public GameObject MultiFirstTimeName;
    public GameObject OnlineFirstTimeName;
    public GameObject TerrainSelectMenu;
    public GameObject PlayerTypeSelectMenu;
    public Text resetPointsInfoTxt;

    public Toggle checkBoxKeySet1;
    public Toggle checkBoxKeySet2;
    private int keySetType;

    [SerializeField]
    AudioClip buttonClickSoundEffect;

    void Start()
    {
        resetPointsInfoTxt.text = Lean.Localization.LeanLocalization.GetTranslationText("SettingsInfo");
        PlayerPrefs.SetInt("roundMulti", 0);
        PlayerPrefs.SetInt("wonMulti1", 0);
        PlayerPrefs.SetInt("wonMulti2", 0);

        keySetType = PlayerPrefs.GetInt("gameKeySet");

        if (keySetType == 0)
        {
            checkBoxKeySet1.isOn = true;
        }
        else if (keySetType == 1)
        {
            checkBoxKeySet2.isOn = true;
        }
    }

    public void SaveNames()
    {
        if(SingleNameInputField.text != "")
        {
            PlayerPrefs.SetString("hammerName", SingleNameInputField.text);
        }
        if (MultiNameInputField.text != "")
        {
            PlayerPrefs.SetString("moleName", MultiNameInputField.text);
        }
        if (OnlineNameInputField.text != "")
        {
            PlayerPrefs.SetString("onlinePlayerName", OnlineNameInputField.text);
        }
        NamesScreen.SetActive(false);
        AudioSource.PlayClipAtPoint(buttonClickSoundEffect, Camera.main.transform.position);
    }

    public void ResetPoints()
    {
        PlayerPrefs.SetInt("roundSingle", 0);
        PlayerPrefs.SetInt("wonSingle1", 0);
        PlayerPrefs.SetInt("wonSingle2", 0);
        resetPointsInfoTxt.text = Lean.Localization.LeanLocalization.GetTranslationText("SettingsInfoFB");
        AudioSource.PlayClipAtPoint(buttonClickSoundEffect, Camera.main.transform.position);
    }
    public void GoMenu()
    {
        SettingsScreen.SetActive(false);
        keySetPrefs();
        AudioSource.PlayClipAtPoint(buttonClickSoundEffect, Camera.main.transform.position);
    }

    public void GoBack()
    {
        NamesScreen.SetActive(false);
        AudioSource.PlayClipAtPoint(buttonClickSoundEffect, Camera.main.transform.position);
    }

    public void ChangeNames()
    {
        SingleNameInputField.text = PlayerPrefs.GetString("hammerName");
        MultiNameInputField.text = PlayerPrefs.GetString("moleName");
        OnlineNameInputField.text = PlayerPrefs.GetString("onlinePlayerName");
        NamesScreen.SetActive(true);
        AudioSource.PlayClipAtPoint(buttonClickSoundEffect, Camera.main.transform.position);
    }

    public void SaveSingleName()
    {
        if(FSingleNameInputField.text == "GoogleTestUser1010")
        {
            PlayerPrefs.SetInt("playerLevel", 20);
            PlayerPrefs.SetInt("playerExp", 0);
            PlayerPrefs.SetInt("playerExpMax", 600);
            PlayerPrefs.SetString("hammerName", FSingleNameInputField.text);
            SceneManager.LoadScene("StartScene");
        }
        else if (FSingleNameInputField.text != "")
        {
            PlayerPrefs.SetString("hammerName", FSingleNameInputField.text);
            SingleFirstTimeName.SetActive(false);
        }
        AudioSource.PlayClipAtPoint(buttonClickSoundEffect, Camera.main.transform.position);
    }
    public void SaveMultiName()
    {
        if (FMultiNameInputField.text != "")
        {
            PlayerPrefs.SetString("moleName", FMultiNameInputField.text);
            MultiFirstTimeName.SetActive(false);
            TerrainSelectMenu.SetActive(true);
        }
        AudioSource.PlayClipAtPoint(buttonClickSoundEffect, Camera.main.transform.position);
    }
    public void SaveOnlineName()
    {
        if (FOnlineNameInputField.text != "")
        {
            PlayerPrefs.SetString("onlinePlayerName", FOnlineNameInputField.text);
            OnlineFirstTimeName.SetActive(false);
            PlayerTypeSelectMenu.SetActive(true);
        }
        AudioSource.PlayClipAtPoint(buttonClickSoundEffect, Camera.main.transform.position);
    }

    public void GoBackF()
    {
        OnlineFirstTimeName.SetActive(false);
        MultiFirstTimeName.SetActive(false);
        AudioSource.PlayClipAtPoint(buttonClickSoundEffect, Camera.main.transform.position);
    }

    private void keySetPrefs()
    {
        if (checkBoxKeySet1.isOn)
        {
            keySetType = 0;
            PlayerPrefs.SetInt("gameKeySet", 0);
        }
        else
        {
            keySetType = 1;
            PlayerPrefs.SetInt("gameKeySet", 1);
        }
    }


}
