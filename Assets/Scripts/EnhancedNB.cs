using Netick;
using UnityEngine;

public class EnhancedNB : NetworkBehaviour
{
    /// <summary>
    /// 
    /// </summary>
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

[Networked]
public struct PlayerRef
{
    public int Id;

    public bool IsValid => Id > 0;

    public static PlayerRef None => default;

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
