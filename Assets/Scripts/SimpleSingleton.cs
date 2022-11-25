using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Util for Creating Singleton for NON-DontDestroyOnLoad MonoBehaviour
/// </summary>
/// <typeparam name="T"></typeparam>
public class SimpleSingleton<T> : MonoBehaviour where T : Component
{
    public static T Instance;

    private void Awake()
    {
        Instance = GetComponent<T>();
    }
}
