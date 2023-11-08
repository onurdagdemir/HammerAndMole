using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnlineInfoScreenManager : MonoBehaviour
{
    private int gameKeySet;
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
        playerType = PlayerPrefs.GetInt("playerType");
        gameKeySet = PlayerPrefs.GetInt("gameKeySet");
        roundTxt.text = PlayerPrefs.GetInt("roundOnline").ToString();
        won1Txt.text = PlayerPrefs.GetInt("wonOnline1").ToString();
        won2Txt.text = PlayerPrefs.GetInt("wonOnline2").ToString();


        hammerNameTxt.text = PlayerPrefs.GetString("hammerOnlineName");
        moleNameTxt.text = PlayerPrefs.GetString("moleOnlineName");

        levelTxt.text = Lean.Localization.LeanLocalization.GetTranslationText("Online");

        if(playerType == 0)
        {
            if (gameKeySet == 0)
            {
                leftBarNameTxt.text = PlayerPrefs.GetString("hammerOnlineName");
                rightBarNameTxt.text = PlayerPrefs.GetString("moleOnlineName");
            }
            else if (gameKeySet == 1)
            {
                rightBarNameTxt.text = PlayerPrefs.GetString("hammerOnlineName");
                leftBarNameTxt.text = PlayerPrefs.GetString("moleOnlineName");
            }
        }
        else if(playerType == 1)
        {
            if (gameKeySet == 1)
            {
                leftBarNameTxt.text = PlayerPrefs.GetString("hammerOnlineName");
                rightBarNameTxt.text = PlayerPrefs.GetString("moleOnlineName");
            }
            else if (gameKeySet == 0)
            {
                rightBarNameTxt.text = PlayerPrefs.GetString("hammerOnlineName");
                leftBarNameTxt.text = PlayerPrefs.GetString("moleOnlineName");
            }
        }

        Destroy(gameObject);

    }
}
