using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoScreenManager : MonoBehaviour
{
    private int gameType;
    private int gameKeySet;
    private string levelName;
    private string hammerName;
    private string moleName;
    private int playerType;
    public Text levelTxt;
    public Text won1Txt;
    public Text won2Txt;
    public Text roundTxt;
    public Text hammerNameTxt;
    public Text moleNameTxt;

    public Text rightBarNameTxt;
    public Text leftBarNameTxt;
    // Start is called before the first frame update
    void Start()
    {
        gameKeySet = PlayerPrefs.GetInt("gameKeySet");
        gameType = PlayerPrefs.GetInt("gameType");
        playerType = PlayerPrefs.GetInt("playerType");

        if (gameType != 0)
        {
            if (playerType == 0)
            {
                hammerName = PlayerPrefs.GetString("hammerName");
                moleName = PlayerPrefs.GetString("moleName");
            }
            else if (playerType == 1)
            {
                moleName = PlayerPrefs.GetString("hammerName");
                hammerName = PlayerPrefs.GetString("moleName");
            }
        }
        else
        {
            if (playerType == 0)
            {
                hammerName = PlayerPrefs.GetString("hammerName");
                moleName = Lean.Localization.LeanLocalization.GetTranslationText("Mole");
            }
            else if (playerType == 1)
            {
                moleName = PlayerPrefs.GetString("hammerName");
                hammerName = Lean.Localization.LeanLocalization.GetTranslationText("Hammer");
            }
        }

        if (gameType == 0 && playerType == 0)
        {
            roundTxt.text = PlayerPrefs.GetInt("roundSingle").ToString();
            won1Txt.text = PlayerPrefs.GetInt("wonSingle1").ToString();
            won2Txt.text = PlayerPrefs.GetInt("wonSingle2").ToString();
        }
        else if (gameType == 0 && playerType == 1)
        {
            roundTxt.text = PlayerPrefs.GetInt("roundSingle").ToString();
            won2Txt.text = PlayerPrefs.GetInt("wonSingle1").ToString();
            won1Txt.text = PlayerPrefs.GetInt("wonSingle2").ToString();
        }
        else if (gameType == 1 && playerType == 0)
        {
            roundTxt.text = PlayerPrefs.GetInt("roundMulti").ToString();
            won1Txt.text = PlayerPrefs.GetInt("wonMulti1").ToString();
            won2Txt.text = PlayerPrefs.GetInt("wonMulti2").ToString();
        }
        else if (gameType == 1 && playerType == 1)
        {
            roundTxt.text = PlayerPrefs.GetInt("roundMulti").ToString();
            won2Txt.text = PlayerPrefs.GetInt("wonMulti1").ToString();
            won1Txt.text = PlayerPrefs.GetInt("wonMulti2").ToString();
        }
        hammerNameTxt.text = hammerName;
        moleNameTxt.text = moleName;

        if(gameType == 0 && playerType == 1)
        {
            if (gameKeySet == 1)
            {
                leftBarNameTxt.text = hammerName;
                rightBarNameTxt.text = moleName;
            }
            else if (gameKeySet == 0)
            {
                rightBarNameTxt.text = hammerName;
                leftBarNameTxt.text = moleName;
            }
        }
        else
        {
            if (gameKeySet == 0)
            {
                leftBarNameTxt.text = hammerName;
                rightBarNameTxt.text = moleName;
            }
            else if (gameKeySet == 1)
            {
                rightBarNameTxt.text = hammerName;
                leftBarNameTxt.text = moleName;
            }
        }



        if (gameType == 0)
        {

            levelName = PlayerPrefs.GetString("levelName");

            if (levelName == "EASY")
            {
                levelTxt.text = Lean.Localization.LeanLocalization.GetTranslationText("Easy");
            }
            if (levelName == "NORMAL")
            {
                levelTxt.text = Lean.Localization.LeanLocalization.GetTranslationText("Normal");
            }
            if (levelName == "HARD")
            {
                levelTxt.text = Lean.Localization.LeanLocalization.GetTranslationText("Hard");
            }
        }
        if (gameType == 1)
        {
            levelTxt.text = Lean.Localization.LeanLocalization.GetTranslationText("MultiPlayer");
        }

        Destroy(gameObject);
    }

}
