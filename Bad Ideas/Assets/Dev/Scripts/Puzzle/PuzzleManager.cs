using AYellowpaper.SerializedCollections;
using UnityEngine;
using System.Collections.Generic;

public enum PuzzleType
{
    Normal,
    Mixed,
    Absurd
}

public class PuzzleManager : MonoBehaviour
{
    [SerializedDictionary("Puzzle Type", "Puzzle Prefab List")]
    [SerializeField] private SerializedDictionary<PuzzleType, List<GameObject>> platforms;
}
