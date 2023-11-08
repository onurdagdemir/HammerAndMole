using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnlineStageManager : MonoBehaviour
{
    [SerializeField]
    AudioClip buttonClickSoundEffect;

    public Sprite ForestSprite;
    public Sprite SpaceEarthSprite;
    public Sprite SpaceMarsSprite;
    public Sprite MarsSprite;

    public GameObject StageSelectMenu;
    public Button stageButton;


    // Start is called before the first frame update
    void Awake()
    {
        Image StageButtonImage = stageButton.GetComponent<Image>();
        StageButtonImage.sprite = ForestSprite;
        stageButton.GetComponentInChildren<Text>().text = Lean.Localization.LeanLocalization.GetTranslationText("SelectStage");
        PlayerPrefs.SetInt("terrainType", 0);
    }


        public void Forest()
    {
        Image StageButtonImage = stageButton.GetComponent<Image>();
        StageButtonImage.sprite = ForestSprite;
        stageButton.GetComponentInChildren<Text>().text = Lean.Localization.LeanLocalization.GetTranslationText("Forest");
        PlayerPrefs.SetInt("terrainType", 0);
        AudioSource.PlayClipAtPoint(buttonClickSoundEffect, Camera.main.transform.position);
        StageSelectMenu.SetActive(false);
    }
    public void SpaceEarth()
    {
        Image StageButtonImage = stageButton.GetComponent<Image>();
        StageButtonImage.sprite = SpaceEarthSprite;
        stageButton.GetComponentInChildren<Text>().text = Lean.Localization.LeanLocalization.GetTranslationText("SpaceEarth");
        PlayerPrefs.SetInt("terrainType", 1);
        AudioSource.PlayClipAtPoint(buttonClickSoundEffect, Camera.main.transform.position);
        StageSelectMenu.SetActive(false);
    }
    public void SpaceMars()
    {
        Image StageButtonImage = stageButton.GetComponent<Image>();
        StageButtonImage.sprite = SpaceMarsSprite;
        stageButton.GetComponentInChildren<Text>().text = Lean.Localization.LeanLocalization.GetTranslationText("SpaceMars");
        PlayerPrefs.SetInt("terrainType", 2);
        AudioSource.PlayClipAtPoint(buttonClickSoundEffect, Camera.main.transform.position);
        StageSelectMenu.SetActive(false);
    }
    public void Mars()
    {
        Image StageButtonImage = stageButton.GetComponent<Image>();
        StageButtonImage.sprite = MarsSprite;
        stageButton.GetComponentInChildren<Text>().text = "Mars";
        PlayerPrefs.SetInt("terrainType", 3);
        AudioSource.PlayClipAtPoint(buttonClickSoundEffect, Camera.main.transform.position);
        StageSelectMenu.SetActive(false);
    }

    public void StageSelectMenuOpen()
    {
        AudioSource.PlayClipAtPoint(buttonClickSoundEffect, Camera.main.transform.position);
        StageSelectMenu.SetActive(true);
    }

    public void StageSelectMenuClose()
    {
        AudioSource.PlayClipAtPoint(buttonClickSoundEffect, Camera.main.transform.position);
        StageSelectMenu.SetActive(false);
    }

}
