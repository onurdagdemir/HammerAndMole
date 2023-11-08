using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class OnlinePointAnmManager : MonoBehaviourPunCallbacks
{
    public Animator upAnimator;
    public Animator leftAnimator;
    public Animator rightAnimator;
    public Animator downAnimator;
    private int playerType;

    // Start is called before the first frame update
    void Start()
    {

        UpPointAnimation.OnHammer += UpPointAnmHammer;
        LeftPointAnimation.OnHammer += LeftPointAnmHammer;
        RightPointAnimation.OnHammer += RightPointAnmHammer;
        DownPointAnimation.OnHammer += DownPointAnmHammer;
        UpPointAnimation.OnMole += UpPointAnmMole;
        LeftPointAnimation.OnMole += LeftPointAnmMole;
        RightPointAnimation.OnMole += RightPointAnmMole;
        DownPointAnimation.OnMole += DownPointAnmMole;

        playerType = PlayerPrefs.GetInt("playerType");
    }

    private void OnDestroy()
    {
        UpPointAnimation.OnHammer -= UpPointAnmHammer;
        LeftPointAnimation.OnHammer -= LeftPointAnmHammer;
        RightPointAnimation.OnHammer -= RightPointAnmHammer;
        DownPointAnimation.OnHammer -= DownPointAnmHammer;
        UpPointAnimation.OnMole -= UpPointAnmMole;
        LeftPointAnimation.OnMole -= LeftPointAnmMole;
        RightPointAnimation.OnMole -= RightPointAnmMole;
        DownPointAnimation.OnMole -= DownPointAnmMole;
    }

    private void UpPointAnmHammer()
    {
        if (PhotonNetwork.IsMasterClient && playerType == 0)
        {
            upAnimator.SetTrigger("UpHammerPoint");
            photonView.RPC("UpPointRPC", RpcTarget.Others, 1);
        }
        else if (PhotonNetwork.IsMasterClient && playerType == 1)
        {
            upAnimator.SetTrigger("UpMolePoint");
            photonView.RPC("UpPointRPC", RpcTarget.Others, 0);
        }
    }
    private void LeftPointAnmHammer()
    {
        if (PhotonNetwork.IsMasterClient && playerType == 0)
        {
            leftAnimator.SetTrigger("LeftHammerPoint");
            photonView.RPC("LeftPointRPC", RpcTarget.Others, 1);
        }
        else if (PhotonNetwork.IsMasterClient && playerType == 1)
        {
            leftAnimator.SetTrigger("LeftMolePoint");
            photonView.RPC("LeftPointRPC", RpcTarget.Others, 0);
        }
    }
    private void RightPointAnmHammer()
    {
        if (PhotonNetwork.IsMasterClient && playerType == 0)
        {
            rightAnimator.SetTrigger("RightHammerPoint");
            photonView.RPC("RightPointRPC", RpcTarget.Others, 1);
        }
        else if (PhotonNetwork.IsMasterClient && playerType == 1)
        {
            rightAnimator.SetTrigger("RightMolePoint");
            photonView.RPC("RightPointRPC", RpcTarget.Others, 0);
        }
    }
    private void DownPointAnmHammer()
    {
        if (PhotonNetwork.IsMasterClient && playerType == 0)
        {
            downAnimator.SetTrigger("DownHammerPoint");
            photonView.RPC("DownPointRPC", RpcTarget.Others, 1);
        }
        else if (PhotonNetwork.IsMasterClient && playerType == 1)
        {
            downAnimator.SetTrigger("DownMolePoint");
            photonView.RPC("DownPointRPC", RpcTarget.Others, 0);
        }
    }

    private void UpPointAnmMole()
    {
        if (PhotonNetwork.IsMasterClient && playerType == 0)
        {
            upAnimator.SetTrigger("UpMolePoint");
            photonView.RPC("UpPointRPC", RpcTarget.Others, 0);
        }
        else if (PhotonNetwork.IsMasterClient && playerType == 1)
        {
            upAnimator.SetTrigger("UpHammerPoint");
            photonView.RPC("UpPointRPC", RpcTarget.Others, 1);
        }

    }
    private void LeftPointAnmMole()
    {
        if (PhotonNetwork.IsMasterClient && playerType == 0)
        {
            leftAnimator.SetTrigger("LeftMolePoint");
            photonView.RPC("LeftPointRPC", RpcTarget.Others, 0);
        }
        else if (PhotonNetwork.IsMasterClient && playerType == 1)
        {
            leftAnimator.SetTrigger("LeftHammerPoint");
            photonView.RPC("LeftPointRPC", RpcTarget.Others, 1);
        }
    }
    private void RightPointAnmMole()
    {
        if (PhotonNetwork.IsMasterClient && playerType == 0)
        {
            rightAnimator.SetTrigger("RightMolePoint");
            photonView.RPC("RightPointRPC", RpcTarget.Others, 0);
        }
        else if (PhotonNetwork.IsMasterClient && playerType == 1)
        {
            rightAnimator.SetTrigger("RightHammerPoint");
            photonView.RPC("RightPointRPC", RpcTarget.Others, 1);
        }
    }
    private void DownPointAnmMole()
    {
        if (PhotonNetwork.IsMasterClient && playerType == 0)
        {
            downAnimator.SetTrigger("DownMolePoint");
            photonView.RPC("DownPointRPC", RpcTarget.Others, 0);
        }
        else if (PhotonNetwork.IsMasterClient && playerType == 1)
        {
            downAnimator.SetTrigger("DownHammerPoint");
            photonView.RPC("DownPointRPC", RpcTarget.Others, 1);
        }
    }



    [PunRPC]
    private void UpPointRPC(int type)
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            if (type == 0)
            {
                upAnimator.SetTrigger("UpHammerPoint");
            }
            else if (type == 1)
            {
                upAnimator.SetTrigger("UpMolePoint");
            }
        }
    }

    [PunRPC]
    private void LeftPointRPC(int type)
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            if (type == 0)
            {
                leftAnimator.SetTrigger("LeftHammerPoint");
            }
            else if (type == 1)
            {
                leftAnimator.SetTrigger("LeftMolePoint");
            }
        }
    }

    [PunRPC]
    private void RightPointRPC(int type)
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            if (type == 0)
            {
                rightAnimator.SetTrigger("RightHammerPoint");
            }
            else if (type == 1)
            {
                rightAnimator.SetTrigger("RightMolePoint");
            }
        }
    }

    [PunRPC]
    private void DownPointRPC(int type)
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            if (type == 0)
            {
                downAnimator.SetTrigger("DownHammerPoint");
            }
            else if (type == 1)
            {
                downAnimator.SetTrigger("DownMolePoint");
            }
        }
    }
}
