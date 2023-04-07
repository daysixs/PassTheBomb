using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class RoundSystem : NetworkBehaviour
{
    [SerializeField] private Animator anim = null;
    private MyNetworkManager room;

    public void CountDownFinished()
    {
        anim.enabled = false;
    }

    #region server

    public override void OnStartServer()
    {
        base.OnStartServer();
        //CheckToStartRound(conn);
    }

    [ServerCallback]
    public void StartRound()
    {
        RpcStartRound();
    }

    private void CheckToStartRound(NetworkConnection conn)
    {
        if (room.roomPlayers.Count == 2)
        {
            anim.enabled = true;
            RpcStartCountdown();
        }
    }

    #endregion server

    #region client

    [ClientRpc]
    private void RpcStartCountdown()
    {
        anim.enabled = true;
    }

    [ClientRpc]
    private void RpcStartRound()
    {
        Debug.Log("Start");
    }

    #endregion client
}