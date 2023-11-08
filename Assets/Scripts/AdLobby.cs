using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdLobby : MonoBehaviour
{
    private bool isInterstitialReady = false;

    // Start is called before the first frame update
    void Start()
    {
        IronSourceInterstitialEvents.onAdReadyEvent += InterstitialOnAdReadyEvent;

        //IronSource.Agent.loadInterstitial();
        //Invoke(nameof(ShowInterstitialAd), 30f);
    }

    void InterstitialOnAdReadyEvent(IronSourceAdInfo adInfo)
    {
        isInterstitialReady = true;
    }

    private void ShowInterstitialAd()
    {
        if(isInterstitialReady)
        {
            IronSource.Agent.showInterstitial("Settings");
        }

    }

    private void OnDestroy()
    {
        IronSourceInterstitialEvents.onAdReadyEvent -= InterstitialOnAdReadyEvent;
    }
}
