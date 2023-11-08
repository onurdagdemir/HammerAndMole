using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class GamePointHandler : MonoBehaviourPunCallbacks
{

    public delegate void CollisionEventHandler();
    public static event CollisionEventHandler OnCollision;

    public delegate void MoleEventHandler();
    public static event MoleEventHandler OnMoleAttack;

    private float currentTime;
    private bool isMoleAttacked = false;
    private float lastAttackTimer;
    private float lastHammerAttackTimer;
    private int levelCarpan; // start fonksiyonunda multiplayer ise 2f olarak işaretlendi.

    private int gameType;
    private int playerType;
    private bool isMasterPlayer;
    private float moleAttackTime;

    void Start()
    {
        GameOverScript.OnGameOver += DestroyOnGameOver;

        gameType = PlayerPrefs.GetInt("gameType");
        playerType = PlayerPrefs.GetInt("playerType");

        if (gameType == 0)
        {
            levelCarpan = PlayerPrefs.GetInt("level") / 10;
        }
        else
        {
            levelCarpan = 1;
        }

        // master durumuna göre mole atak süresi kısalır veya artar
        if(gameType == 2)
        {
            isMasterPlayer = PhotonNetwork.IsMasterClient;

            if (isMasterPlayer)
            {
                if(playerType == 0)
                {
                    moleAttackTime = 0.20f;
                }
                else
                {
                    moleAttackTime = 0.45f;
                }
            }
            else
            {
                if (playerType == 0)
                {
                    moleAttackTime = 0.45f;
                }
                else
                {
                    moleAttackTime = 0.20f;
                }
            }
        }
        else if (gameType == 0 && playerType == 1)
        {
            moleAttackTime = 0.45f / levelCarpan;
        }
        else
        {
            moleAttackTime = 0.3f;
        }
    }
    private void DestroyOnGameOver()
    {
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        GameOverScript.OnGameOver -= DestroyOnGameOver;
    }

        private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Hammer")
        {
            if(Time.time - lastHammerAttackTimer >= 0.2f)
            {
                OnCollision();
                lastHammerAttackTimer = Time.time;
            }

        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "MoleAttackHeight")
        {
            currentTime = currentTime += Time.deltaTime;
            if (currentTime >= moleAttackTime * levelCarpan && isMoleAttacked == false && Time.time - lastAttackTimer >= 1f)
            {
                OnMoleAttack();
                isMoleAttacked = true;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "MoleAttackHeight")
        {
            currentTime = 0f;
            lastAttackTimer = Time.time;
            isMoleAttacked = false;
        }
    }
}