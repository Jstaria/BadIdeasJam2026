using AYellowpaper.SerializedCollections;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public enum PuzzleType
{
    Normal,
    Mixed,
    Absurd
}

public class PuzzleManager : Singleton<PuzzleManager>
{
    [SerializedDictionary("Puzzle Type", "Puzzle Prefab List")]
    [SerializeField] private SerializedDictionary<PuzzleType, List<GameObject>> puzzlePrefabs;

    private int indexer;
    private PuzzleType currentPuzzleSet = PuzzleType.Normal;

    public PuzzleType CurrentPuzzleSet { 
        get => currentPuzzleSet;
        private set
        {
            indexer = 0;
            currentPuzzleSet = value;
        }
    }

    protected override void OnAwake()
    {
        for (int i = 0; i <= (int)PuzzleType.Absurd; i++)
        {
            if (puzzlePrefabs[(PuzzleType)i] == null) continue;
            if (puzzlePrefabs[(PuzzleType)i].Count == 0) continue;

            puzzlePrefabs[(PuzzleType)i].OrderBy(e => Random.value).ToList();
        }
    }

    public void GetPuzzle(Platform p, out GameObject puzzle)
    {
        if (puzzlePrefabs[currentPuzzleSet] == null || puzzlePrefabs[currentPuzzleSet].Count == 0)
        {
            Debug.LogWarning($"No puzzles found for {currentPuzzleSet} set.");
            puzzle = null;
            return;
        }

        puzzle = Instantiate(puzzlePrefabs[currentPuzzleSet][indexer++], p.transform);
        indexer = indexer % puzzlePrefabs[currentPuzzleSet].Count;
    }
}
