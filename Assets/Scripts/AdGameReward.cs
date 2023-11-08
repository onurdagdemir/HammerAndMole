using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdGameReward : MonoBehaviour
{
    [SerializeField]
    AudioClip buttonClickSoundEffect;

    public GameObject RewardScreenBack;
    public Text amount;
    private int expAmount;
    private int gameType;
    private int randomRewardChance;
    private bool isRewardReady = false;

    private const int rewardedAdDisplayChance = 40;

    // Start is called before the first frame update
    void Start()
    {
        IronSourceRewardedVideoEvents.onAdRewardedEvent += RewardedVideoOnAdRewardedEvent;

        randomRewardChance = Random.Range(1, 101);
        RewardScreenBack.SetActive(false);

        gameType = PlayerPrefs.GetInt("gameType");
        expAmount = PlayerPrefs.GetInt("playerLevel") * 20;
        amount.text = "+" + expAmount + "P";
        //GameOverScript.OnGameOver += DelayShowRewardScreen;
    }

    void OnApplicationPause(bool isPaused)
    {
        IronSource.Agent.onApplicationPause(isPaused);
    }

    public void ShowRewardedAd()
    {
        if (isRewardReady)
        {
            IronSource.Agent.showRewardedVideo("Game_Over");
        }
    }

    void RewardedVideoOnAdRewardedEvent(IronSourcePlacement placement, IronSourceAdInfo adInfo)
    {
        LevelSystem levelSystem = new LevelSystem();
        levelSystem.AddExperience(expAmount);
        Destroy(RewardScreenBack);
    }

    private void DelayShowRewardScreen()
    {
        isRewardReady = IronSource.Agent.isRewardedVideoAvailable();

        IronSource.Agent.loadBanner(IronSourceBannerSize.BANNER, IronSourceBannerPosition.TOP, (string)"Game_Over");

        StartCoroutine(ShowRewardScreen());
    }

    IEnumerator ShowRewardScreen()
    {
        yield return new WaitForSecondsRealtime(1f);
        if (randomRewardChance <= rewardedAdDisplayChance && gameType == 0 && isRewardReady)
        {
            RewardScreenBack.SetActive(true);
        }
    }

    public void CloseRewardScreen()
    {
        AudioSource.PlayClipAtPoint(buttonClickSoundEffect, Camera.main.transform.position);
        Destroy(RewardScreenBack);
    }


    private void OnDestroy()
    {
        //GameOverScript.OnGameOver -= DelayShowRewardScreen;
        IronSourceRewardedVideoEvents.onAdRewardedEvent -= RewardedVideoOnAdRewardedEvent;
        //IronSource.Agent.destroyBanner();
    }

}
