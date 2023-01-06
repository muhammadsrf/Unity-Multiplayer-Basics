using Mirror;
using TMPro;
using UnityEngine;

public class MyNetworkPlayer : NetworkBehaviour
{
    [SerializeField] private TMP_Text displayNameText = null;
    [SerializeField] private Renderer displayColorRenderer = null;

    [SyncVar(hook = nameof(HandleDisplayNameUpdated))]
    [SerializeField]
    private string _displayName = "Missing Name";

    [SyncVar(hook = nameof(HandleDisplayColorUpdated))]
    [SerializeField]
    private Color _displayColor = Color.black;

    #region Server
    [Server]
    public void SetDisplayName(string newDisplayName)
    {
        _displayName = newDisplayName;
    }

    [Server]
    public void SetDisplayColor(Color newColor)
    {
        _displayColor = newColor;
    }

    [Command]
    private void CmdSetDisplayName(string newDisplayName)
    {
        if (newDisplayName.Length < 2 || newDisplayName.Length > 20) { return; }

        RpcLogNewName(newDisplayName);

        SetDisplayName(newDisplayName);
    }
    #endregion

    #region Client
    private void HandleDisplayColorUpdated(Color oldColor, Color newColor)
    {
        displayColorRenderer.material.SetColor("_BaseColor", newColor);
    }

    private void HandleDisplayNameUpdated(string oldName, string newName)
    {
        displayNameText.text = newName;
    }

    [ContextMenu("Set Display Name")]
    private void SetMyName()
    {
        CmdSetDisplayName("M");
    }

    [ClientRpc]
    private void RpcLogNewName(string newName)
    {
        Debug.Log(newName);
    }
    #endregion
}