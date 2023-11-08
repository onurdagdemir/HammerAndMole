    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class MolePlayerControl : MonoBehaviour
    {
        public Transform[] moleHoles;
        private float upSpeed = 5f;
        private float downSpeed = 2f;
        private float maxUpHeight = 0.95f;
        private float startY;
        private float targetY;
        private bool isMovingUp = false;
        private int holeIndex = 4;

        private bool upPressed = false;
        private bool downPressed = false;
        private bool rightPressed = false;
        private bool leftPressed = false;
        private int gameKeySet;

        private bool isPlatformPC = true;
        float horizontalInput;
        float verticalInput;
        public Joystick joystickLeft;
        public Joystick joystickRight;

        private bool isGameOver = false;

        // Start is called before the first frame update
        void Start()
        {
        GameOverScript.OnGameOver += GameOverCase;
        startY = transform.position.y;
        gameKeySet = PlayerPrefs.GetInt("gameKeySet");

        if (PlayerPrefs.GetInt("playerType") == 1 && PlayerPrefs.GetInt("gameType") == 0)
        {
            upSpeed = 5.3f;
            downSpeed = 2.3f;

            if(gameKeySet == 0)
            {
                gameKeySet = 1;
            } else if(gameKeySet == 1)
            {
                gameKeySet = 0;
            }
        }

#if UNITY_STANDALONE
        // Bilgisayarda çalışıyorsa tuşlar aktif
        isPlatformPC = true;
#elif UNITY_ANDROID
        // Android cihazda çalışıyorsa joystick aktif
        isPlatformPC = false;
#endif
        }

    private void MoveMoleHole(Transform selectedHole)
        {
            transform.position = new Vector3(selectedHole.position.x, transform.position.y, selectedHole.position.z);
            targetY = selectedHole.position.y + maxUpHeight;
            isMovingUp = true;
        }

        // Update is called once per frame
        void Update()
        {

        if (isPlatformPC == false)
        {
            if (gameKeySet == 0)
            {
                horizontalInput = joystickRight.Horizontal;
                verticalInput = joystickRight.Vertical;
            }
            if (gameKeySet == 1)
            {
                horizontalInput = joystickLeft.Horizontal;
                verticalInput = joystickLeft.Vertical;
            }


            if (verticalInput >= 0.7f)
            {
                upPressed = true;
            }
            else
            {
                upPressed = false;
            }

            if (verticalInput <= -0.7f)
            {
                downPressed = true;
            }
            else
            {
                downPressed = false;
            }

            if (horizontalInput >= 0.7f)
            {
                rightPressed = true;
            }
            else
            {
                rightPressed = false;
            }

            if (horizontalInput <= -0.7f)
            {
                leftPressed = true;
            }
            else
            {
                leftPressed = false;
            }

        }

        if (isPlatformPC == true)
        {
            if (gameKeySet == 0)
            {
                if (Input.GetKey(KeyCode.UpArrow))
                {
                    upPressed = true;
                }
                else
                {
                    upPressed = false;
                }

                if (Input.GetKey(KeyCode.DownArrow))
                {
                    downPressed = true;
                }
                else
                {
                    downPressed = false;
                }

                if (Input.GetKey(KeyCode.RightArrow))
                {
                    rightPressed = true;
                }
                else
                {
                    rightPressed = false;
                }

                if (Input.GetKey(KeyCode.LeftArrow))
                {
                    leftPressed = true;
                }
                else
                {
                    leftPressed = false;
                }
            }

            if (gameKeySet == 1)
            {
                if (Input.GetKey(KeyCode.W))
                {
                    upPressed = true;
                }
                else
                {
                    upPressed = false;
                }

                if (Input.GetKey(KeyCode.S))
                {
                    downPressed = true;
                }
                else
                {
                    downPressed = false;
                }

                if (Input.GetKey(KeyCode.D))
                {
                    rightPressed = true;
                }
                else
                {
                    rightPressed = false;
                }

                if (Input.GetKey(KeyCode.A))
                {
                    leftPressed = true;
                }
                else
                {
                    leftPressed = false;
                }
            }
        }

            if (transform.position.y <= startY)
            {
                holeIndex = 4;
            }


                if (isMovingUp == false)
                {
                    if (upPressed && holeIndex != 0 && holeIndex != 1 && holeIndex != 3)
                    {
                        MoveMoleHole(moleHoles[2]);
                        holeIndex = 2;
                    }
                    else if (leftPressed && holeIndex != 0 && holeIndex != 1 && holeIndex != 2)
                    {
                        MoveMoleHole(moleHoles[3]);
                        holeIndex = 3;
                    }
                    else if (downPressed && holeIndex != 1 && holeIndex != 2 && holeIndex != 3)
                    {
                        MoveMoleHole(moleHoles[0]);
                        holeIndex = 0;
                    }
                    else if (rightPressed && holeIndex != 0 && holeIndex != 2 && holeIndex != 3)
                    {
                        MoveMoleHole(moleHoles[1]);
                        holeIndex = 1;
                    }
            }


                if (isMovingUp && !isGameOver)
                {
                    transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, targetY, transform.position.z), upSpeed * Time.deltaTime);

                        isMovingUp = false;

                }

            else
            {
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, startY, transform.position.z), downSpeed * Time.deltaTime);
            }

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
