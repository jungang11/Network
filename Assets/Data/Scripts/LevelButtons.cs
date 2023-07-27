using Photon.Pun;
using System;

public class LevelButtons : MonoBehaviourPunCallbacks
{
    public Status status;

    public void LevelButton(int level)
    {
        status.Init(level);
    }

    public void DecreaseHP(int level)
    {
        photonView.RPC("PunDecreaseHP", RpcTarget.All, level);
    }

    [PunRPC]
    public void PunDecreaseHP(int level)
    {
        if(PhotonNetwork.IsMasterClient)
            GameManager.Data.StatDict[level].hp--;

        status.Init(level);
    }
}
