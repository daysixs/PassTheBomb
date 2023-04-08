using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class MyNetworkManager : NetworkManager
{
    [Header("Player")]
    [SerializeField] private int minPlayers = 2;

    [Header("Game")]
    [SerializeField] private GameObject roundSystem = null;

    public List<MyNetworkPlayer> roomPlayers { get; } = new List<MyNetworkPlayer>();

    public static event System.Action OnClientConnected;

    public static event System.Action OnClientDisconnected;

    public static event System.Action OnServerStopped;

    public static event System.Action OnServerReadied;

    #region server

    public override void OnClientConnect()
    {
        base.OnClientConnect();
        Debug.Log("You have connected to the server");

        OnClientConnected?.Invoke();
    }

    public override void OnStopServer()
    {
        base.OnStopServer();
        Debug.Log("The server has been stopped");
        roomPlayers.Clear();

        OnClientDisconnected?.Invoke();
    }

    public override void OnServerConnect(NetworkConnectionToClient conn)
    {
        if (numPlayers >= maxConnections)
        {
            conn.Disconnect();
            return;
        }
    }

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        base.OnServerAddPlayer(conn);
        MyNetworkPlayer player = conn.identity.GetComponent<MyNetworkPlayer>();

        //IsReadyToStart();

        //GameObject roundSystemInstance = Instantiate(roundSystem);

        player.setDisplayName($"Player {numPlayers}");

        Color displayColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));

        player.setDisplayColor(displayColor);
    }

    private bool IsReadyToStart()
    {
        if (numPlayers < minPlayers && numPlayers > minPlayers) { return false; }

        foreach (var player in roomPlayers)
        {
            if (numPlayers != 2)
            {
                return false;
            }
        }
        return true;
    }

    #endregion server
}