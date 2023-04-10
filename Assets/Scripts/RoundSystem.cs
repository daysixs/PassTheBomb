using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class RoundSystem : NetworkBehaviour
{
    [SerializeField] private Animator anim = null;
    private MyNetworkManager room;

    private MyNetworkManager Room
    {
        get
        {
            if (room != null) { return room; }

            return room = MyNetworkManager.singleton as MyNetworkManager;
        }
    }

    public void CountDownFinished()
    {
        anim.enabled = false;
    }

    #region server

    public override void OnStartServer()
    {
        MyNetworkManager.OnServerAllJoined += CheckToStartRound;
    }

    [ServerCallback]
    public void StartRound()
    {
        RpcStartRound();
    }

    [Server]
    private void CheckToStartRound(NetworkConnectionToClient conn)
    {
        if (Room.numPlayers == Room.maxPlayers)
        {
            anim.enabled = true;
            RpcStartCountdown();
            Debug.Log("Hi");
        }
        else
        {
            return;
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