using Netick;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Util for Creating [NetworkBehaviour] Singleton for NON-DontDestroyOnLoad
/// </summary>
/// <typeparam name="T"></typeparam>
public class NetworkSingleton<T> : NetworkBehaviour where T : Component
{
    public static T Instance;

    private void Awake()
    {
        Instance = GetComponent<T>();
    }
}
