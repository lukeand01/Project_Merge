using UnityEngine.Advertisements;
using UnityEngine;

//using GoogleMobileAds.Api;


public class AdHandler : MonoBehaviour
{
    //need to create the banner
    //nbeed to show ad after every stage but for the first one.

    public void RequestRewardAd(RewardType reward)
    {
        if (reward.ToString() == RewardType.UsePowerDestroy.ToString())
        {
            UIHandler.instance.inputUI.CallPowerFromAd(PowerType.DestroyTarget);
            return;
        }
        if (reward.ToString() == RewardType.UsePowerUpgrade.ToString())
        {
            UIHandler.instance.inputUI.CallPowerFromAd(PowerType.Upgrade);
            return;
        }
        if (reward.ToString() == RewardType.UsePowerWipe.ToString())
        {
            UIHandler.instance.inputUI.CallPowerFromAd(PowerType.Wipe);
            return;
        }

        if (reward.ToString() == RewardType.GetAnotherHealth.ToString())
        {
            GameHandler.instance.StartTheGameFromWhereItWas();
            return;
        }
    }
    /*

    public bool debugShouldNotShowAd;


   // BannerView bannerView;
   // InterstitialAd interstitial;
   // RewardedInterstitialAd rewardedAd;
   // Reward actualReward; //THIS MIGHT BE CHANGED BECASUE THOSE REWARDS MIGHT NEED TO CHANGED INSIDE THE MOBAD.

    string adUnitId = "";

    public bool isShowingAd { get; private set; } = false;


    private void Awake()
    {
        //MobileAds.Initialize(initStatus => { });

        //i am using test still.
        //for testing = adUnitId = "ca-app-pub-3940256099942544/9257395921";
        adUnitId = "pub-3563085082651862";


        //RequestBanner();
        //LoadInterstitialAd();
        //LoadRewardAd();
    }

    private void Start()
    {
        //RequestBanner();
    }


    //this is called by mainmenu always.
    public void RequestBanner()
    {
        //THis is the android version.      


        //bannerView = new BannerView(adUnitId, AdSize.SmartBanner, AdPosition.Top);

        //AdRequest request = new AdRequest.Build();

        //bannerView.LoadAd(request);
    }

    #region INTERSTITIAL AD
    //interstitial ads are for between levels.
    //i want that before loading the next scene we show then and once its done we close them. everytime we go from one scene to another.
    void LoadInterstitialAd()
    {

        
        if (interstitial != null)
        {
            interstitial.Destroy();
            interstitial = null;
        }
        

       // var adRequest = new AdRequest();

        

        // send the request to load the ad.
        
        InterstitialAd.Load(adUnitId, adRequest,
            (InterstitialAd ad, LoadAdError error) =>
            {
                // if error is not null, the load request failed.
                if (error != null || ad == null)
                {
                    Debug.LogError("interstitial ad failed to load an ad " +
                                   "with error : " + error);
                    return;
                }



                interstitial = ad;
            });

        
    }

    [ContextMenu("DEBUG SHOW INTE_AD")]
    public void RequestInterstitial()
    {
        
        if (interstitial != null && interstitial.CanShowAd())
        {
            //DebugErrorText.Log("Showing interstitial ad.");
            interstitial.Show();
            isShowingAd = true;
            interstitial.OnAdFullScreenContentClosed += OnInterstitialAdEnd;
        }
        else
        {
            Debug.LogError("Interstitial ad is not ready yet.");
        }
        

    }

    void OnInterstitialAdEnd()
    {

        isShowingAd = false;
        interstitial.OnAdFullScreenContentClosed -= OnInterstitialAdEnd;
    }


    #endregion


    #region REWARD AD

    //these ads works as reward to gain another chance at teh game or for making double the amount of coins.

    public void LoadRewardAd()
    {

        if (rewardedAd != null)
        {
            rewardedAd.Destroy();
            rewardedAd = null;
        }

        //DebugErrorText.Log("Loading the rewarded interstitial ad.");

        // create our request used to load the ad.
        var adRequest = new AdRequest();
        adRequest.Keywords.Add("unity-admob-sample");

        // send the request to load the ad.
        RewardedInterstitialAd.Load(adUnitId, adRequest,
            (RewardedInterstitialAd ad, LoadAdError error) =>
            {
                // if error is not null, the load request failed.
                if (error != null || ad == null)
                {
                    Debug.LogError("rewarded interstitial ad failed to load an ad " +
                                   "with error : " + error);
                    return;
                }



                rewardedAd = ad;
            });
    }

    public void RequestRewardAd(RewardType rewardId, double rewardAmount = 0)
    {

        if (debugShouldNotShowAd)
        {
            DebugRewardWithNoAd(rewardId);

            return;
        }


        if (rewardedAd != null && rewardedAd.CanShowAd())
        {
            Reward rewardValue = new Reward();
            rewardValue.Type = rewardId.ToString();
            rewardValue.Amount = rewardAmount;

            isShowingAd = true;



            rewardedAd.OnAdFullScreenContentClosed += OnRewardAdEnd;
            actualReward = rewardValue;


            rewardedAd.Show((Reward reward) =>
            {

                // TODO: Reward the user.
                //DebugErrorText.Log(String.Format(rewardMsg, reward.Type, reward.Amount));
            });
        }
    }


    void DebugRewardWithNoAd(RewardType reward)
    {
        if (reward.ToString() == RewardType.UsePowerDestroy.ToString())
        {
            UIHandler.instance.inputUI.CallPowerFromAd(PowerType.DestroyTarget);
            return;
        }
        if (reward.ToString() == RewardType.UsePowerUpgrade.ToString())
        {
            UIHandler.instance.inputUI.CallPowerFromAd(PowerType.Upgrade);
            return;
        }
        if (reward.ToString() == RewardType.UsePowerWipe.ToString())
        {
            UIHandler.instance.inputUI.CallPowerFromAd(PowerType.Wipe);
            return;
        }

        if (reward.ToString() == RewardType.GetAnotherHealth.ToString())
        {
            GameHandler.instance.StartTheGameFromWhereItWas();
            return;
        }
    }

    public void OnRewardAdEnd()
    {
        //different ones from whatever is to be gained.
        //you may gain another life. and you may gain double 

        isShowingAd = false;
        rewardedAd.OnAdFullScreenContentClosed -= OnRewardAdEnd;


        Reward reward = actualReward;

        if (reward == null)
        {

            return;
        }

        if(reward.Type == RewardType.UsePowerDestroy.ToString())
        {
            UIHandler.instance.inputUI.CallPowerFromAd(PowerType.DestroyTarget);          
            return;
        }
        if (reward.Type == RewardType.UsePowerUpgrade.ToString())
        {
            UIHandler.instance.inputUI.CallPowerFromAd(PowerType.Upgrade);
            return;
        }
        if (reward.Type == RewardType.UsePowerWipe.ToString())
        {
            UIHandler.instance.inputUI.CallPowerFromAd(PowerType.Wipe);
            return;
        }

        if(reward.Type == RewardType.GetAnotherHealth.ToString())
        {
            GameHandler.instance.StartTheGameFromWhereItWas();
            return;
        }

    }

    #endregion


    //i need to wait for ad.
    */
}

public enum RewardType
{
    Nothing,
    UsePowerWipe,
    UsePowerUpgrade,
    UsePowerDestroy,
    GetAnotherHealth
}
