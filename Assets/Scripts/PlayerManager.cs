using Netick;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using NetworkPlayer = Netick.NetworkPlayer;

public class PlayerManager : SimpleSingleton<PlayerManager>
{
    public Dictionary<PlayerRef, Player> SpawnedPlayers { get; set; } = new();
    public Dictionary<PlayerRef, PlayerSetup> SpawnedPlayersObj { get; set; } = new();

    public void AddSpawnedPlayer(PlayerRef newPlayer, Player newObj)
    {
        if (SpawnedPlayers.ContainsKey(newPlayer))
        {
            Debug.LogError($"[PlayerManager]: Key is Exist! Existed: {newPlayer.Id} New: {newPlayer.Id}");
            return;
        }

        print($"[PlayerManager]: key Added: {newPlayer.Id}");
        SpawnedPlayers.Add(newPlayer, newObj);
    }
    public void RemoveSpawnedPlayer(PlayerRef player)
    {
        SpawnedPlayers.Remove(player);
    }

    public void AddSpawnedPlayerObj(PlayerRef player, PlayerSetup obj)
    {
        SpawnedPlayersObj.Add(player, obj);
    }
    public void RemoveSpawnedPlayerObj(PlayerRef player)
    {
        SpawnedPlayersObj.Remove(player);
    }
}
