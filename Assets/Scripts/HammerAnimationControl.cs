using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HammerAnimationControl : MonoBehaviour
{

    Animator animator;
    private bool isAnimating;
    private bool upPressed = false;
    private bool downPressed = false;
    private bool rightPressed = false;
    private bool leftPressed = false;
    private int gameKeySet;

    private bool isPlatformPC = true;
    private float horizontalInput;
    private float verticalInput;
    public Joystick joystickLeft;
    public Joystick joystickRight;

    private bool touchPressed = false;
    private bool isGameOver = false;

    // Start is called before the first frame update
    void Start()
    {

        animator = GetComponent<Animator>();

        gameKeySet = PlayerPrefs.GetInt("gameKeySet");

        HammerTouchControl.upTouch += upPressedAction;
        HammerTouchControl.rightTouch += rightPressedAction;
        HammerTouchControl.leftTouch += leftPressedAction;
        HammerTouchControl.downTouch += downPressedAction;
        GameOverScript.OnGameOver += GameOver;


#if UNITY_STANDALONE
        // Bilgisayarda çalışıyorsa tuşlar aktif
        isPlatformPC = true;
#elif UNITY_ANDROID 
        // Android cihazda çalışıyorsa joystick aktif
        isPlatformPC = false;
#endif

    }

    private void OnDestroy()
    {
        HammerTouchControl.upTouch -= upPressedAction;
        HammerTouchControl.rightTouch -= rightPressedAction;
        HammerTouchControl.leftTouch -= leftPressedAction;
        HammerTouchControl.downTouch -= downPressedAction;
        GameOverScript.OnGameOver -= GameOver;
    }

    private void upPressedAction()
    {
        upPressed = true;
        touchPressed = true;
        Invoke(nameof(resetAction), 0.1f);
    }
    private void rightPressedAction()
    {
        rightPressed = true;
        touchPressed = true;
        Invoke(nameof(resetAction), 0.1f);
    }
    private void leftPressedAction()
    {
        leftPressed = true;
        touchPressed = true;
        Invoke(nameof(resetAction), 0.1f);
    }
    private void downPressedAction()
    {
        downPressed = true;
        touchPressed = true;
        Invoke(nameof(resetAction), 0.1f);
    }

    private void resetAction()
    {
        upPressed = false;
        rightPressed = false;
        leftPressed = false;
        rightPressed = false;
        touchPressed = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(isPlatformPC == false && !touchPressed)
        {
            if (gameKeySet == 0)
            {
                horizontalInput = joystickLeft.Horizontal;
                verticalInput = joystickLeft.Vertical;
            }
            if (gameKeySet == 1)
            {
                horizontalInput = joystickRight.Horizontal;
                verticalInput = joystickRight.Vertical;
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
        

        if(isPlatformPC == true && !touchPressed)
        {
            if (gameKeySet == 1)
            {
                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    upPressed = true;
                }
                else
                {
                    upPressed = false;
                }

                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    downPressed = true;
                }
                else
                {
                    downPressed = false;
                }

                if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    rightPressed = true;
                }
                else
                {
                    rightPressed = false;
                }

                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    leftPressed = true;
                }
                else
                {
                    leftPressed = false;
                }
            }

            if (gameKeySet == 0)
            {
                if (Input.GetKeyDown(KeyCode.W))
                {
                    upPressed = true;
                }
                else
                {
                    upPressed = false;
                }

                if (Input.GetKeyDown(KeyCode.S))
                {
                    downPressed = true;
                }
                else
                {
                    downPressed = false;
                }

                if (Input.GetKeyDown(KeyCode.D))
                {
                    rightPressed = true;
                }
                else
                {
                    rightPressed = false;
                }

                if (Input.GetKeyDown(KeyCode.A))
                {
                    leftPressed = true;
                }
                else
                {
                    leftPressed = false;
                }
            }
        }

       if (!isGameOver && isAnimating == false) {

        if (rightPressed)
        {
            animator.SetTrigger("RightPressed");
                isAnimating = true;
                Invoke("ResetIsAnimating", 0.5f);
            }
        if (leftPressed)
        {
            animator.SetTrigger("LeftPressed");
                isAnimating = true;
                Invoke("ResetIsAnimating", 0.5f);
            }
        if (upPressed)
        {
            animator.SetTrigger("UpPressed");
                isAnimating = true;
                Invoke("ResetIsAnimating", 0.5f);
            }
        if (downPressed)
        {
            animator.SetTrigger("DownPressed");
                isAnimating = true;
                Invoke("ResetIsAnimating", 0.5f);
            }
       }
    }

    private void ResetIsAnimating()
    {
        isAnimating = false;
    }

    private void GameOver()
    {
        isGameOver = true;
    }


}
