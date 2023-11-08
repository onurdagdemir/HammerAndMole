using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class OnlineHealthManager : MonoBehaviourPunCallbacks
{
    public delegate void PointEventHandler();
    public static event PointEventHandler OnHammerPoint;
    public static event PointEventHandler OnMolePoint;
    public static event PointEventHandler OnExpGained;

    private int gameKeySet;
    private int playerType;
    private int moleMaxHealth = 100;
    private int hammerMaxHealth = 100;
    private int moleCurrentHealth;
    private int hammerCurrentHealth;
    public int damageAmount = 5;
    public GameOverScript GameOverScreen;

    public Text leftCurrentHealthValue;
    public Text rightCurrentHealthValue;

    private int isGameOver = 0;
    private int round;
    private int won1;
    private int won2;

    public GameObject GameOverText;
    public GameObject VictoryText;

    [SerializeField]
    AudioClip moleAttackSoundEffect;
    [SerializeField]
    AudioClip hammerAttackSoundEffect;

    // Start is called before the first frame update

    private void Awake()
    {
        playerType = PlayerPrefs.GetInt("playerType");
        gameKeySet = PlayerPrefs.GetInt("gameKeySet");
        moleCurrentHealth = moleMaxHealth;
        hammerCurrentHealth = hammerMaxHealth;

        round = PlayerPrefs.GetInt("roundOnline");
        won1 = PlayerPrefs.GetInt("wonOnline1");
        won2 = PlayerPrefs.GetInt("wonOnline2");


    }
    void Start()
    {
        GamePointHandler.OnCollision += HandleCollision;
        GamePointHandler.OnMoleAttack += HandleMoleAttack;
        TimeCounter.OnTimeOver += TimeIsOver;

        isGameOver = 0;
        PlayerPrefs.SetInt("isGameOver", isGameOver);
    }

    private void TimeIsOver()
    {
        GameOverMenu();
    }

    private void HandleCollision()
    {
        // Tokmakla çarpışma algılandığında canı azalt
        if (moleCurrentHealth > 0 && isGameOver == 0 && PhotonNetwork.IsMasterClient)
        {
            moleCurrentHealth -= damageAmount;
            OnHammerPoint();
            photonView.RPC("hammerAttackMaster", RpcTarget.Others);
            AudioSource.PlayClipAtPoint(hammerAttackSoundEffect, Camera.main.transform.position);
            if(playerType == 0)
            {
                if (gameKeySet == 0)
                {
                    rightCurrentHealthValue.text = moleCurrentHealth.ToString();
                }
                else if (gameKeySet == 1)
                {
                    leftCurrentHealthValue.text = moleCurrentHealth.ToString();
                }
            }
            else if (playerType == 1)
            {
                if (gameKeySet == 1)
                {
                    rightCurrentHealthValue.text = moleCurrentHealth.ToString();
                }
                else if (gameKeySet == 0)
                {
                    leftCurrentHealthValue.text = moleCurrentHealth.ToString();
                }
            }

        }
        if (moleCurrentHealth <= 0)
        {
            GameOverMenu();
        }
    }


    [PunRPC]
    private void hammerAttackMaster()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            if (moleCurrentHealth > 0 && isGameOver == 0)
            {
                moleCurrentHealth -= damageAmount;
                AudioSource.PlayClipAtPoint(hammerAttackSoundEffect, Camera.main.transform.position);
                if (playerType == 0)
                {
                    if (gameKeySet == 0)
                    {
                        rightCurrentHealthValue.text = moleCurrentHealth.ToString();
                    }
                    else if (gameKeySet == 1)
                    {
                        leftCurrentHealthValue.text = moleCurrentHealth.ToString();
                    }
                }
                else if (playerType == 1)
                {
                    if (gameKeySet == 1)
                    {
                        rightCurrentHealthValue.text = moleCurrentHealth.ToString();
                    }
                    else if (gameKeySet == 0)
                    {
                        leftCurrentHealthValue.text = moleCurrentHealth.ToString();
                    }
                }
            }
            if (moleCurrentHealth <= 0)
            {
                GameOverMenu();
            }
        }

    }


    private void HandleMoleAttack()
    {
        moleAttack();
    }

    private void moleAttack()
    {
        if (hammerCurrentHealth > 0 && isGameOver == 0 && PhotonNetwork.IsMasterClient)
        {
            hammerCurrentHealth -= damageAmount;
            OnMolePoint();
            photonView.RPC("moleAttackMaster", RpcTarget.Others);
            AudioSource.PlayClipAtPoint(moleAttackSoundEffect, Camera.main.transform.position);
            if (playerType == 0)
            {
                if (gameKeySet == 1)
                {
                    rightCurrentHealthValue.text = hammerCurrentHealth.ToString();
                }
                else if (gameKeySet == 0)
                {
                    leftCurrentHealthValue.text = hammerCurrentHealth.ToString();
                }
            }
            else if (playerType == 1)
            {
                if (gameKeySet == 0)
                {
                    rightCurrentHealthValue.text = hammerCurrentHealth.ToString();
                }
                else if (gameKeySet == 1)
                {
                    leftCurrentHealthValue.text = hammerCurrentHealth.ToString();
                }
            }
        }

        if (hammerCurrentHealth <= 0)
        {
            GameOverMenu();
        }
    }

    [PunRPC]
    private void moleAttackMaster()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            if (hammerCurrentHealth > 0 && isGameOver == 0)
            {
                hammerCurrentHealth -= damageAmount;
                AudioSource.PlayClipAtPoint(moleAttackSoundEffect, Camera.main.transform.position);
                if (playerType == 0)
                {
                    if (gameKeySet == 1)
                    {
                        rightCurrentHealthValue.text = hammerCurrentHealth.ToString();
                    }
                    else if (gameKeySet == 0)
                    {
                        leftCurrentHealthValue.text = hammerCurrentHealth.ToString();
                    }
                }
                else if (playerType == 1)
                {
                    if (gameKeySet == 0)
                    {
                        rightCurrentHealthValue.text = hammerCurrentHealth.ToString();
                    }
                    else if (gameKeySet == 1)
                    {
                        leftCurrentHealthValue.text = hammerCurrentHealth.ToString();
                    }
                }
            }
            if (hammerCurrentHealth <= 0)
            {
                GameOverMenu();
            }
        }

    }


    private void OnDestroy()
    {
        GamePointHandler.OnCollision -= HandleCollision;
        GamePointHandler.OnMoleAttack -= HandleMoleAttack;
        TimeCounter.OnTimeOver -= TimeIsOver;
    }

    private void GameOverMenu()
    {
        isGameOver = 1;
        round += 1;
        PlayerPrefs.SetInt("isGameOver", isGameOver);
        PlayerPrefs.SetInt("roundOnline", round);

        if (moleCurrentHealth < hammerCurrentHealth)
        {
            if(playerType == 0)
            {
                GameOverText.SetActive(false);
                VictoryText.SetActive(true);
            }

            GameOverScreen.Setup(PlayerPrefs.GetString("hammerOnlineName") + Lean.Localization.LeanLocalization.GetTranslationText("Wins"));
            won1 += 1;

            PlayerPrefs.SetInt("wonOnline1", won1);

            if (playerType == 0)
            {
                OnExpGained();
            }   
        }
        else if (hammerCurrentHealth < moleCurrentHealth)
        {
            if (playerType == 1)
            {
                GameOverText.SetActive(false);
                VictoryText.SetActive(true);
            }

            GameOverScreen.Setup(PlayerPrefs.GetString("moleOnlineName") + Lean.Localization.LeanLocalization.GetTranslationText("Wins"));
            won2 += 1;
            PlayerPrefs.SetInt("wonOnline2", won2);

            if (playerType == 1)
            {
                OnExpGained();
            }      
        }
        else
        {
            GameOverScreen.Setup("Draw!");
        }

        Destroy(gameObject);
    }
}
