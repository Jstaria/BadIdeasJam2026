using Unity.Collections;
using UnityEditor.Animations;
using UnityEngine;

public enum PuzzleState
{
    Spawn,
    Despawn
}

public class Platform : MonoBehaviour
{
    [SerializeField] private Animator floorTileAnimator;
    [SerializeField] public Transform puzzleCenter;
    [SerializeField] private GameObject instantiatedPuzzlePrefab;

    public void LowerPlatformAnim(PuzzleState state)
    {
        switch (state)
        {
            case PuzzleState.Spawn:
                floorTileAnimator.SetTrigger("GetPuzzle");
                break;

            case PuzzleState.Despawn:
                floorTileAnimator.SetTrigger("ReturnPuzzle");
                break;

            default:
                break;
        }
    }

    /// <summary>
    /// Spawns puzzle on platform center
    /// </summary>
    public void GetPuzzle()
    {
        Debug.Log("GetPuzzle called");
        PuzzleManager.Instance.GetPuzzle(this, out instantiatedPuzzlePrefab);
    }

    public void DespawnPuzzle()
    {
        Debug.Log("DespawnPuzzle called");
        if (instantiatedPuzzlePrefab != null)
        {
            Destroy(instantiatedPuzzlePrefab);
            instantiatedPuzzlePrefab = null;
        }
    }
}
