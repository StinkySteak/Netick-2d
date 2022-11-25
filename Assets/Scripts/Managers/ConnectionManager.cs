using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Netick;
using Network = Netick.Network;

public class ConnectionManager : NetworkEventsListner
{
    public static ConnectionManager Instance;
    public bool IsDedicatedServer;

    private void Awake()
    {
        Instance = this;
    }

    public int Port = 28080;

    public void StartHost()
    {
        Network.StartAsServer(Port);
    }
    public void StartHostExtraPeer()
    {
        Network.StartAsServerAndClient(Port);
    }
    public void StartClient()
    {
        Network.StartAsClient(Port)
            .Connect(Port, "127.0.0.1");
    }

    public override void OnClientConnected(NetworkSandbox sandbox, NetworkConnection client)
    {
        WriteConnection($"[Client: PeerId: {client.Id + 2} is Connected]");
        LevelManager.Instance.OnNetworkPlayerConnected(client, PlayerRef.Create(client.Id + 2));
    }

    public override void OnClientDisconnected(NetworkSandbox sandbox, NetworkConnection client)
    {
        WriteConnection($"[Client: PeerId: {client.Id + 2} is Disconnected]");
        LevelManager.Instance.OnNetworkPlayerDisconnected(PlayerRef.Create(client.Id + 2));
    }

    public override void OnSceneLoaded(NetworkSandbox sandbox)
    {
        if (Sandbox.IsServer)
            WriteConnection($"[Server: PeerId: {1} is Connected]");

        ///Execute this to spawn Player on the [Server]
        LevelManager.Instance.OnNetworkPlayerConnected(Sandbox.LocalPlayer, PlayerRef.Create(1));
    }

    public void WriteConnection(string text)
    {
        print(text);
    }
}
