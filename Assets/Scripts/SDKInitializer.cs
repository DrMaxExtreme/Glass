using System.Collections;
using UnityEngine;
using Agava.YandexGames;

public class SDKInitializer : MonoBehaviour
{
    private void Awake()
    {
        YandexGamesSdk.CallbackLogging = true;
    }

    private IEnumerator Start()
    {
#if !UNITY_EDITOR
        yield return YandexGamesSdk.Initialize();
#endif
        yield break;
    }
}
