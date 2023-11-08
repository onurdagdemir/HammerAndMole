using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    public delegate void PointEventHandler();
    public static event PointEventHandler OnHammerPoint;
    public static event PointEventHandler OnMolePoint;
    public static event PointEventHandler OnExpGained;

    private int moleMaxHealth = 100;
    private int hammerMaxHealth = 100;
    private int moleCurrentHealth;
    private int hammerCurrentHealth;
    public int damageAmount = 5;
    public GameOverScript GameOverScreen;

    public Text leftCurrentHealthValue;
    public Text rightCurrentHealthValue;
    private int gameKeySet;
    private int gameType;

    private int isGameOver = 0;
    private int round;
    private int won1;
    private int won2;
    private string hammerName;
    private string moleName;
    private int playerType;

    public GameObject GameOverText;
    public GameObject VictoryText;

    [SerializeField]
    AudioClip moleAttackSoundEffect;
    [SerializeField]
    AudioClip hammerAttackSoundEffect;

    // Start is called before the first frame update

    private void Awake()
    {
        gameKeySet = PlayerPrefs.GetInt("gameKeySet");
        gameType = PlayerPrefs.GetInt("gameType");
        moleCurrentHealth = moleMaxHealth;
        hammerCurrentHealth = hammerMaxHealth;
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
        } else
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


        if (gameType == 0)
        {
            round = PlayerPrefs.GetInt("roundSingle");
            won1 = PlayerPrefs.GetInt("wonSingle1");
            won2 = PlayerPrefs.GetInt("wonSingle2");
        }
        else if (gameType == 1 && playerType == 0)
        {
            round = PlayerPrefs.GetInt("roundMulti");
            won1 = PlayerPrefs.GetInt("wonMulti1");
            won2 = PlayerPrefs.GetInt("wonMulti2");
        }
        else if (gameType == 1 && playerType == 1)
        {
            round = PlayerPrefs.GetInt("roundMulti");
            won2 = PlayerPrefs.GetInt("wonMulti1");
            won1 = PlayerPrefs.GetInt("wonMulti2");
        }

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
            if (moleCurrentHealth > 0 && isGameOver == 0)
            {
              moleCurrentHealth -= damageAmount;
              AudioSource.PlayClipAtPoint(hammerAttackSoundEffect, Camera.main.transform.position);

            if (playerType == 0 || gameType == 1)
            {
                OnHammerPoint();
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
                OnMolePoint();
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

    private void HandleMoleAttack()
    {
        moleAttack();
    }

    private void OnDestroy()
    {
        GamePointHandler.OnCollision -= HandleCollision;
        GamePointHandler.OnMoleAttack -= HandleMoleAttack;
        TimeCounter.OnTimeOver -= TimeIsOver;
    }


    private void moleAttack()
    {
        if(hammerCurrentHealth > 0 && isGameOver == 0) { 
        hammerCurrentHealth -= damageAmount;
        AudioSource.PlayClipAtPoint(moleAttackSoundEffect, Camera.main.transform.position);

            if (playerType == 0 || gameType == 1)
            {
                OnMolePoint();
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
                OnHammerPoint();
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

    private void GameOverMenu()
    {
        isGameOver = 1;
        round += 1;
        PlayerPrefs.SetInt("isGameOver", isGameOver);

        if(gameType == 0)
        {
            PlayerPrefs.SetInt("roundSingle", round);
        } else if(gameType == 1)
        {
            PlayerPrefs.SetInt("roundMulti", round);
        }

        if (moleCurrentHealth < hammerCurrentHealth)
        {
            won1 += 1;

            if (gameType == 0 && playerType == 0)
            {
                GameOverText.SetActive(false);
                VictoryText.SetActive(true);
                PlayerPrefs.SetInt("wonSingle1", won1);
            }
            else if (gameType == 0 && playerType == 1)
            {
                won2 += 1;
                GameOverText.SetActive(true);
                VictoryText.SetActive(false);
                PlayerPrefs.SetInt("wonSingle2", won2);
            }
            else if (gameType == 1 && playerType == 0)
            {
                PlayerPrefs.SetInt("wonMulti1", won1);
            }
            else if (gameType == 1 && playerType == 1)
            {
                PlayerPrefs.SetInt("wonMulti2", won1);
            }

            GameOverScreen.Setup(hammerName + Lean.Localization.LeanLocalization.GetTranslationText("Wins"));

            if (gameType == 0 && playerType == 0)
            {
                OnExpGained();
            }
        }
        else if(hammerCurrentHealth < moleCurrentHealth)
        {
            won2 += 1;
            if (gameType == 0 && playerType == 0)
            {
                GameOverText.SetActive(true);
                VictoryText.SetActive(false);
                PlayerPrefs.SetInt("wonSingle2", won2);
            }
            else if (gameType == 0 && playerType == 1)
            {
                won1 += 1;
                GameOverText.SetActive(false);
                VictoryText.SetActive(true);
                PlayerPrefs.SetInt("wonSingle1", won1);
            }
            else if (gameType == 1 && playerType == 0)
            {
                PlayerPrefs.SetInt("wonMulti2", won2);
            }
            else if (gameType == 1 && playerType == 1)
            {
                PlayerPrefs.SetInt("wonMulti1", won2);
            }

            GameOverScreen.Setup(moleName + Lean.Localization.LeanLocalization.GetTranslationText("Wins"));

            if (gameType == 0 && playerType == 1)
            {
                OnExpGained();
            }

        } else
        {
            GameOverScreen.Setup("Draw!");
        }

        Destroy(gameObject);


    }


}
