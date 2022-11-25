using Netick;
using UnityEngine;

/// <summary>
/// Improved Network Behaviour with PlayerRef(PlayerId) functionallity
/// </summary>
public class EnhancedNB : NetworkBehaviour
{
    [Networked, HideInInspector] public PlayerRef PlayerId { get; set; }
#if UNITY_EDITOR
    [ReadOnly]
#endif
    [SerializeField]
    private int LatestPlayerId;

    [OnChanged(nameof(PlayerId))]
    void OnPlayerIdChanged(PlayerRef previous)
    {
      //  print($"PlayerId changed to {PlayerId.Id} at: {Sandbox.Tick}");
        LatestPlayerId = PlayerId;
        OnPlayerIdAssigned();
    }

    protected virtual void OnPlayerIdAssigned()
    {

    }

}


/// <summary>
/// Useful Player ID Struct Wrapper. Do not Relate this to Connection ID
/// <para>Can be used for Mapping PlayerID/PlayerObject</para>
/// </summary>
[Networked]
public struct PlayerRef
{
    /// <summary>
    /// Raw Player Id
    /// </summary>
    public int Id;

    /// <summary>
    /// only more than 0 is considered as Valid
    /// </summary>
    public bool IsValid => Id > 0;

    /// <summary>
    /// Default Value
    /// </summary>
    public static PlayerRef None => default;

    /// <summary>
    /// Only Create on OnClientConnected OR By getting other PlayerRef Reference
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public static PlayerRef Create(int id) { return new PlayerRef() { Id = id }; }

    public static implicit operator string(PlayerRef playerId)
    {
        return $"[PlayerRef: {playerId.Id}]";
    }

    public static implicit operator int(PlayerRef playerId)
    {
        return playerId.Id;
    }
    public static bool operator ==(PlayerRef ref1, PlayerRef ref2)
    {
        return ref1.Id == ref2.Id;
    }
    public static bool operator !=(PlayerRef ref1, PlayerRef ref2)
    {
        return ref1.Id != ref2.Id;
    }
    public override bool Equals(object obj)
    {
        return base.Equals(obj);
    }
    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}
