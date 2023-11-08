using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftPointAnimation : MonoBehaviour
{
    public delegate void LeftPointAnmEventHandler();
    public static event LeftPointAnmEventHandler OnHammer;
    public static event LeftPointAnmEventHandler OnMole;
    Animator animator;
    private bool isCollision = false;
    private int gameType;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        HealthManager.OnHammerPoint += HammerPointAnimation;
        HealthManager.OnMolePoint += MolePointAnimation;
        OnlineHealthManager.OnHammerPoint += HammerPointAnimation;
        OnlineHealthManager.OnMolePoint += MolePointAnimation;
        gameType = PlayerPrefs.GetInt("gameType");
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Mole")
        {
            isCollision = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Mole")
        {
            isCollision = false;
        }
    }

    public void HammerPointAnimation()
    {
        if (isCollision && gameType !=2)
        {
            animator.SetTrigger("LeftHammerPoint");
        }
        else if (isCollision && gameType == 2)
        {
            OnHammer();
        }

    }

    public void MolePointAnimation()
    {
        if (isCollision && gameType != 2)
        {
            animator.SetTrigger("LeftMolePoint");
        }
        else if (isCollision && gameType == 2)
        {
            OnMole();
        }
    }

    private void OnDestroy()
    {
        HealthManager.OnHammerPoint -= HammerPointAnimation;
        HealthManager.OnMolePoint -= MolePointAnimation;
        OnlineHealthManager.OnHammerPoint -= HammerPointAnimation;
        OnlineHealthManager.OnMolePoint -= MolePointAnimation;
    }
}
