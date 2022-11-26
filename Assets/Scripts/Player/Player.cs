using Netick;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : EnhancedNB
{
    public static Player LocalPlayer;

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
    public void RPC_RequestRespawn(PlayerRef playerRef) // PlayerRef playerRef
    {
        print("RPC_RequestRespawn by: " + playerRef);

        PlayerManager.Instance.SpawnedPlayers.TryGetValue(PlayerId, out var expectedPlayer);

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
