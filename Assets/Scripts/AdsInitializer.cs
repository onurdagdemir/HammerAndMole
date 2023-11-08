using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdsInitializer : MonoBehaviour
{
    private bool isInterstitialReady = false;

    private int counter;

    private void Awake()
    {
        //InitializeAds();
    }

    private void Start()
    {
        //InvokeRepeating("InterstitialControl", 15.0f, 10.0f);
        //LoadInterstitialAd();
    }

    void OnApplicationPause(bool isPaused)
    {
        IronSource.Agent.onApplicationPause(isPaused);
    }

    private void InterstitialControl()
    {
        if (isInterstitialReady && counter % 6 == 0)
        {
           ShowInterstitialAd();
           isInterstitialReady = false;
        }
        else if (!isInterstitialReady)
        {
            LoadInterstitialAd();
        }
        counter += 1;
    }

    private void LoadInterstitialAd()
    {
        IronSource.Agent.loadInterstitial();
    }

    private void ShowInterstitialAd()
    {
        IronSource.Agent.showInterstitial("Main_Menu");
    }

    void InterstitialOnAdReadyEvent(IronSourceAdInfo adInfo)
    {
        isInterstitialReady = true;
    }

    public void InitializeAds()
    {
        IronSource.Agent.setConsent(true);
        IronSourceEvents.onSdkInitializationCompletedEvent += SdkInitializationCompletedEvent;

        IronSourceInterstitialEvents.onAdReadyEvent += InterstitialOnAdReadyEvent;


        //IronSource.Agent.setMetaData("is_test_suite", "enable");
        IronSource.Agent.init("1c04a4245", IronSourceAdUnits.REWARDED_VIDEO);
        IronSource.Agent.init("1c04a4245", IronSourceAdUnits.INTERSTITIAL);
        IronSource.Agent.init("1c04a4245", IronSourceAdUnits.BANNER);

        IronSource.Agent.validateIntegration();
    }

    private void SdkInitializationCompletedEvent()
    {
        IronSource.Agent.loadInterstitial();
        //IronSource.Agent.launchTestSuite();
    }

    private void OnDestroy()
    {
        IronSourceEvents.onSdkInitializationCompletedEvent -= SdkInitializationCompletedEvent;

        IronSourceInterstitialEvents.onAdReadyEvent -= InterstitialOnAdReadyEvent;

    }
}
