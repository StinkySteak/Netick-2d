using Netick;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A Placeholder Player Data, good for Nickname, Kill, Death, Score, etc ... 
/// <para>Which will not Despawned Until the Player Left</para>
/// </summary>
public class Player : EnhancedNB
{
    public static Player LocalPlayer;
    public static NetworkSandbox LocalSandbox => LocalPlayer.Object.Sandbox;

    [Networked] public State PlayerState { get; set; } = State.Despawned;

    public enum State
    {
        Despawned,
        Spawned
    }

    public void OnDespawned()
    {
        // print("OnDespawned");
        PlayerState = State.Despawned;
    }

    [OnChanged(nameof(PlayerState))]
    void OnPlayerStateChanged(State prevState)
    {
        // print("OnPlayerStateChanged");

        if (PlayerState == State.Despawned)
            PlayerManager.Instance.SpawnedPlayersObj.Remove(PlayerId);
    }

    [Rpc(source: RpcPeers.InputSource, target: RpcPeers.Owner, isReliable: true)]
    public void RPC_RequestRespawn() // PlayerRef playerRef
    {
        print($"RPC_RequestRespawn by: {Sandbox.RpcSource}");

        if (!PlayerManager.Instance.SpawnedPlayers.TryGetValue(PlayerId, out var expectedPlayer))
            return;

        if (PlayerManager.Instance.SpawnedPlayersObj.ContainsKey(PlayerId)) // Player is still Alive
            return;

        LevelManager.Instance.SpawnPlayer(expectedPlayer.InputSource, PlayerId);
    }

    public override void NetworkStart()
    {
        print($"IsLocalPlayer: {Object.IsInputSource}");

        if (Object.IsInputSource)
            LocalPlayer = this;
    }

    protected override void OnPlayerIdAssigned()
    {
        print($"Player Spawned: IsLocalPlayer: {Object.IsInputSource} PlayerId: {PlayerId.Id} Tick: {Sandbox.Tick}");

        PlayerManager.Instance.AddSpawnedPlayer(PlayerId, this);
    }
}
