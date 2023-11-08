using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KostebekHareket : MonoBehaviour {

    public Transform[] moleHoles;
    private float upSpeed = 3.6f;
    private float downSpeed = 1.6f;
    private float[] maxUpHeights = { 1f, 0.9f, 0.4f, 0.3f, 0.95f };
    private float maxUpHeight;
    private float[] waitTimesUp = { 0.30f, 0.10f, 0.32f };
    private float[] waitTimesLUp = { 0.05f, 0.03f, 0.08f };
    private float waitTimeUp;
    private float waitTimeLUp;

    private bool isMovingUp;
    private float startY;
    private float targetY;
    public int levelCarpan;
    private bool isMoveMole = false;
    private bool isGameOver = false;


    private void Start()
    {
        GameOverScript.OnGameOver += GameOverCase;
        levelCarpan = PlayerPrefs.GetInt("level") / 10;
        startY = transform.position.y;
        MoveMoleRandom();
    }

    private void MoveMoleRandom()
    {
        if(!isGameOver)
        {
            int randomHoleIndex = Random.Range(0, moleHoles.Length);
            Transform randomHole = moleHoles[randomHoleIndex];
            transform.position = new Vector3(randomHole.position.x, transform.position.y, randomHole.position.z);
            maxUpHeight = maxUpHeights[Random.Range(0, maxUpHeights.Length)];
            waitTimeUp = waitTimesUp[Random.Range(0, waitTimesUp.Length)] * levelCarpan;
            waitTimeLUp = waitTimesLUp[Random.Range(0, waitTimesLUp.Length)];
            targetY = randomHole.position.y + maxUpHeight;

            isMovingUp = true;
        }


    }

    private void Update()
    {
        if (isMovingUp)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, targetY, transform.position.z), upSpeed * Time.deltaTime);

            if (transform.position.y >= targetY)
            {
                //isMovingUp = false;
                if(maxUpHeight >= 0.9f)
                {
                Invoke("MoveMoleDown", waitTimeUp);
                } else
                {
                Invoke("MoveMoleDown", waitTimeLUp);
                }
            }

        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, startY, transform.position.z), downSpeed * Time.deltaTime);


                if (transform.position.y <= startY)
                {
                    if(!isMoveMole)
                    {
                    MoveMoleRandom();
                    isMoveMole = true;
                    Invoke(nameof(MoveMoleReset), 0.5f);
                    }

                }
            
        }
    }

    private void MoveMoleDown()
    {
        isMovingUp = false;
    }

    private void MoveMoleReset()
    {
        isMoveMole = false;
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