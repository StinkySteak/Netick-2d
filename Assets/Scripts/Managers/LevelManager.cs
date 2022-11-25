using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Netick;
using Network = Netick.Network;
using NetworkPlayer = Netick.NetworkPlayer;
using static Netick.Network;
using Newtonsoft.Json.Linq;

public class LevelManager : NetworkSingleton<LevelManager>
{
    public Transform SpawnPosition;

    /// <summary>
    /// Will Attempt to Spawn player
    /// </summary>
    /// <param name="player"></param>
    void SpawnPlayer(NetworkPlayer player, PlayerRef playerRef)
    {
        if (Sandbox.IsClient) return;
        // print($"Attempt to Spawn Player by: {Sandbox.LocalPlayer}");
        var playerPlaceHolder = Sandbox.NetworkInstantiate(Sandbox.GetPrefab("Player"), Vector2.zero, Quaternion.identity, player);
        var playerObj = Sandbox.NetworkInstantiate(Sandbox.GetPrefab("PlayerController"), SpawnPosition.position, Quaternion.identity, player);

        if (playerPlaceHolder.TryGetComponent(out Player component))
        {
            component.PlayerId = playerRef;
        }

        if (playerObj.TryGetComponent(out PlayerSetup playerSetupComponent))
        {
            playerSetupComponent.PlayerId = playerRef;
        }
    }

    /// <summary>
    /// Will Attempt to Spawn player
    /// </summary>
    /// <param name="player"></param>
    void DespawnPlayer(PlayerRef player)
    {
        // print($"Attempt to Despawn Player by: {Sandbox.LocalPlayer}");

        if (Sandbox.IsClient) return;

        PlayerManager.Instance.SpawnedPlayers.TryGetValue(player, out var playerExistance);
        PlayerManager.Instance.SpawnedPlayersObj.TryGetValue(player, out var playerObj);

        Sandbox.Destroy(playerExistance.Object);
        Sandbox.Destroy(playerObj.Object);
    }

    public void OnNetworkPlayerConnected(NetworkPlayer player, PlayerRef playerRef)
    {
        SpawnPlayer(player, playerRef);
    }

    public void OnNetworkPlayerDisconnected(PlayerRef player)
    {
        DespawnPlayer(player);
    }
}
