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
    [SerializeField] private Transform puzzleCenter;

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
    internal void GetPuzzle()
    {
        Debug.Log("GetPuzzle called");
    }

    internal void DespawnPuzzle()
    {
        Debug.Log("DespawnPuzzle called");
    }
}
