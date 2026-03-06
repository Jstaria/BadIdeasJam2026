using AYellowpaper.SerializedCollections;
using QFSW.QC;
using System.Collections;
using UnityEngine;

public class PlatformManager : MonoBehaviour
{
    [SerializedDictionary("Platform Name", "Platform Object")]
    [SerializeField] private SerializedDictionary<string, Platform> platforms;

    [Command]
    public void SpawnAll() => StartCoroutine(SpawnAllCoroutine());

    private IEnumerator SpawnAllCoroutine()
    {
        foreach (var platform in platforms.Values)
        {
            platform.LowerPlatformAnim(PuzzleState.Spawn);
            yield return new WaitForSeconds(0.5f); 
        }
    }

    [Command]
    public void SpawnPuzzleOnPlatform(string platformName) => LowerPlatform(platformName, PuzzleState.Spawn);
    [Command]
    public void DespawnPuzzleOnPlatform(string platformName) => LowerPlatform(platformName, PuzzleState.Despawn);

    private void LowerPlatform(string name, PuzzleState state)
    {
        if (platforms.TryGetValue(name, out Platform platform))
        {
            platform.LowerPlatformAnim(state);
        }
        else
        {
            Debug.LogWarning($"Platform '{name}' not found.");
        }
    }
}
