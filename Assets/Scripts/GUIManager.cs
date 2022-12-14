using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIManager : SimpleSingleton<GUIManager>
{
    public void StartClientHosted()
    {
        ConnectionManager.Instance.StartHost();
    }
    public void StartClientHostedExtraPeer()
    {
        ConnectionManager.Instance.StartHostExtraPeer();
    }
    public void StartClient()
    {
        ConnectionManager.Instance.StartClient();
    }

    public void RequestRespawn()
    {
        if (Player.LocalPlayer == null)
            return;

        Player.LocalPlayer.RPC_RequestRespawn(); // 
    }
}
