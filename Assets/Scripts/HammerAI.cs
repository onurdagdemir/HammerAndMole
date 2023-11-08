using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerAI : MonoBehaviour
{
    Animator animator;
    public GameObject MolePlayer;
    private bool isAnimating = false;
    private bool isGameOver = false;
    private bool canHit = false;
    private bool canHitTimer = true;
    private int trueHitChance = 90;
    private float hitWaitTime;
    private float[] waitBeforeHit = { 0.35f, 0.15f, 0.27f, 0.2f, 0.25f };
    

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        GameOverScript.OnGameOver += GameOverCase;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isGameOver && !isAnimating)
        {
            if (MolePlayer.transform.position.y > 1.6f)
            {
                if (canHitTimer)
                {
                    HitTimer();
                }

                if (canHit)
                {
                    if (MolePlayer.transform.position.x > 0 && MolePlayer.transform.position.z > 0) //down
                    {
                        if (UnityEngine.Random.Range(0, 100) <= trueHitChance)
                        {
                            Hit(0);
                        }
                        else
                        {
                            Hit(UnityEngine.Random.Range(0, 4));
                        }

                        isAnimating = true;
                        Invoke("ResetIsAnimating", 1f);
                        canHitTimer = true;
                        canHit = false;
                    }

                    if (MolePlayer.transform.position.x < 0 && MolePlayer.transform.position.z > 0) //right
                    {
                        if (UnityEngine.Random.Range(0, 100) <= trueHitChance)
                        {
                            Hit(1);
                        }
                        else
                        {
                            Hit(UnityEngine.Random.Range(0, 4));
                        }
                        isAnimating = true;
                        Invoke("ResetIsAnimating", 1f);
                        canHitTimer = true;
                        canHit = false;
                    }

                    if (MolePlayer.transform.position.x < 0 && MolePlayer.transform.position.z < 0) //up
                    {
                        if (UnityEngine.Random.Range(0, 100) <= trueHitChance)
                        {
                            Hit(2);
                        }
                        else
                        {
                            Hit(UnityEngine.Random.Range(0, 4));
                        }
                        isAnimating = true;
                        Invoke("ResetIsAnimating", 1f);
                        canHitTimer = true;
                        canHit = false;
                    }

                    if (MolePlayer.transform.position.x > 0 && MolePlayer.transform.position.z < 0) //left
                    {
                        if (UnityEngine.Random.Range(0, 100) <= trueHitChance)
                        {
                            Hit(3);
                        }
                        else
                        {
                            Hit(UnityEngine.Random.Range(0, 4));
                        }
                        isAnimating = true;
                        Invoke("ResetIsAnimating", 1f);
                        canHitTimer = true;
                        canHit = false;
                    }
                }
                
            }
            else
            {
                canHit = false;
                canHitTimer = true;
            }
        }
    }

    private void HitTimer()
    {
        canHitTimer = false;
        hitWaitTime = waitBeforeHit[UnityEngine.Random.Range(0, waitBeforeHit.Length)];
        Invoke("HitControl", hitWaitTime);
    }

    private void HitControl()
    {
        canHit = true;
    }

    private void Hit(int position)
    {
        switch(position)
        {
            case 0:
                animator.SetTrigger("DownPressed");
                break;
            case 1:
                animator.SetTrigger("RightPressed");
                break;
            case 2:
                animator.SetTrigger("UpPressed");
                break;
            case 3:
                animator.SetTrigger("LeftPressed");
                break;
        }
    }

    private void ResetIsAnimating()
    {
        isAnimating = false;
    }

    private void GameOverCase()
    {
        isGameOver = true;
    }

    private void OnDestroy()
    {
        GameOverScript.OnGameOver -= GameOverCase;
    }
}
