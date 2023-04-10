using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class MyNetworkManager : NetworkManager
{
    [Header("Player")]
    [SerializeField] public int maxPlayers = 2;

    [Header("Game")]
    [SerializeField] private GameObject roundSystem = null;

    public List<MyNetworkPlayer> roomPlayers { get; } = new List<MyNetworkPlayer>();

    public static event System.Action OnClientConnected;

    public static event System.Action OnClientDisconnected;

    public static event System.Action OnServerStopped;

    public static event System.Action<NetworkConnectionToClient> OnServerAllJoined;

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
        OnServerStopped?.Invoke();
    }

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        base.OnServerAddPlayer(conn);

        MyNetworkPlayer player = conn.identity.GetComponent<MyNetworkPlayer>();

        player.setDisplayName($"Player {numPlayers}");

        Color displayColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));

        player.setDisplayColor(displayColor);

        try
        {
            OnServerAllJoined?.Invoke(conn);

            Debug.Log("Inside delegate invocation");

            GameObject roundSystemInstance = Instantiate(roundSystem);
            NetworkServer.Spawn(roundSystemInstance);
            Debug.Log("Game is starting");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error invoking OnServerAllJoined delegate: {e}");
        }
    }

    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        if (conn.identity != null)
        {
            MyNetworkPlayer player = conn.identity.GetComponent<MyNetworkPlayer>();
            roomPlayers.Remove(player);
        }

        base.OnServerDisconnect(conn);
    }

    #endregion server
}