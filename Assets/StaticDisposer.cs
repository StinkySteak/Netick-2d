using UnityEngine;

public static class StaticDisposer
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void Main()
    {
        LevelManager.Instance = null;
        PlayerManager.Instance = null;
        GUIManager.Instance = null;

        Player.LocalPlayer = null;
    }
}
