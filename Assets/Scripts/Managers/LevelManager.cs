using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Netick;
using NetworkPlayer = Netick.NetworkPlayer;

public class LevelManager : NetworkSingleton<LevelManager>
{
    public Transform SpawnPosition;

    public override void NetworkStart()
    {
        if (Object.IsOwner)
            Object.PermitInput(Sandbox.LocalPlayer);
    }

    /// <summary>
    /// Will Attempt to Spawn new [Player]
    /// </summary>
    /// <param name="player"></param>
    void SpawnPlayerExistance(NetworkPlayer player, PlayerRef playerRef)
    {
        if (Sandbox.IsClient) return;

        var playerPlaceHolder = Sandbox.NetworkInstantiate(Sandbox.GetPrefab("Player"), Vector2.zero, Quaternion.identity, player);

        if (playerPlaceHolder.TryGetComponent(out Player component))
            component.PlayerId = playerRef;
    }

    /// <summary>
    /// Will Attempt to Spawn new [PlayerSetup]
    /// </summary>
    /// <param name="player"></param>
    void SpawnPlayer(NetworkPlayer player, PlayerRef playerRef)
    {
        if (Sandbox.IsClient) return;

        var playerObj = Sandbox.NetworkInstantiate(Sandbox.GetPrefab("PlayerController"), SpawnPosition.position, Quaternion.identity, player);

        if (playerObj.TryGetComponent(out PlayerSetup playerSetupComponent))
            playerSetupComponent.PlayerId = playerRef;
    }

    /// <summary>
    /// Only calls this once from OnClientConnected/OnSceneLoaded(Server)
    /// </summary>
    /// <param name="player"></param>
    /// <param name="playerRef"></param>
    public void SpawnNewPlayer(NetworkPlayer player, PlayerRef playerRef)
    {
        SpawnPlayerExistance(player, playerRef);
        SpawnPlayer(player, playerRef);
    }

    /// <summary>
    /// Will Attempt to Despawn player completely from the game both [Player] [PlayerSetup]
    /// <para>ONLY use this for OnClientDisconnected</para>
    /// </summary>
    void DespawnPlayer(PlayerRef player)
    {
        if (Sandbox.IsClient) return;

        if (PlayerManager.Instance.SpawnedPlayersObj.TryGetValue(player, out var playerObj))
            Sandbox.Destroy(playerObj.Object);

        Sandbox.Destroy(PlayerManager.Instance.SpawnedPlayers[player].Object);
    }

    /// <summary>
    /// Will Attempt to Despawn player completely from the game both [Player] [PlayerSetup]
    /// <para>ONLY use this for OnClientDisconnected</para>
    /// </summary>
    public void DestroyPlayer(PlayerRef playerRef)
    {
        if (!Sandbox.IsServer) return;

        PlayerManager.Instance.SpawnedPlayersObj.TryGetValue(playerRef, out var playerObj);

        Sandbox.Destroy(playerObj.Object);

        PlayerManager.Instance.SpawnedPlayers[playerRef].OnDespawned();
    }

    [Rpc(source: RpcPeers.Everyone, target: RpcPeers.Everyone, isReliable: true)]
    public void RPC_RequestRespawn(PlayerRef playerRef)
    {
        print("RPC_RequestRespawn by: " + Sandbox.GetRpcCaller());

        PlayerManager.Instance.SpawnedPlayers.TryGetValue(playerRef, out var expectedPlayer);

        if (PlayerManager.Instance.SpawnedPlayersObj.ContainsKey(playerRef)) // Player is still Alive
            return;

        SpawnPlayer(expectedPlayer.InputSource, playerRef);
    }








    public void OnNetworkPlayerConnected(NetworkPlayer player, PlayerRef playerRef)
    {
        SpawnNewPlayer(player, playerRef);
    }

    public void OnNetworkPlayerDisconnected(PlayerRef player)
    {
        DespawnPlayer(player);
    }
}
