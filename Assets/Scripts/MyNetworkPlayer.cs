using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;

public class MyNetworkPlayer : NetworkBehaviour
{
    [SerializeField] private TMP_Text displayNameText = null;
    [SerializeField] private Renderer displayColorRenderer = null;

    [SyncVar(hook = nameof(HandleDisplayNameUpdate))]
    [SerializeField]
    private string displayName = "Mising Name";

    [SyncVar(hook = nameof(HandleDisplayColorUpdate))]
    [SerializeField]
    private Color displayColor = Color.black;

    #region server
    [Server]
    public void setDisplayName(string newDisplayName)
    {
        displayName = newDisplayName;
    }

    [Server]
    public void setDisplayColor(Color newDisplayColor)
    {
        displayColor = newDisplayColor;
    }

    [Command] // client calls a function on server
    private void CmdSetDisplayName(string newDisplayName)
    {
        // server authority to limit displayName into 2-20 letter length
        if(newDisplayName.Length < 2 || newDisplayName.Length > 20)
        {
            return;
        }
        RpcDisplayNewName(newDisplayName);
        setDisplayName(newDisplayName);
    }
    #endregion

    #region client
    private void HandleDisplayColorUpdate(Color oldColor, Color newColor)
    {
        displayColorRenderer.material.SetColor("_BaseColor", newColor);
    }
    private void HandleDisplayNameUpdate(string oldName, string newName)
    {
        displayNameText.text = newName;
    }

    [ContextMenu ("Set This Name")]
    private void SetThisName()
    {
        CmdSetDisplayName("My New Name");
    }

    [ClientRpc] // remote procedure call - server call a function on all clients  ////// targetRPC - server to target client
    private void RpcDisplayNewName(string newDisplayName)
    {
        Debug.Log(newDisplayName);
    }

    #endregion
}
