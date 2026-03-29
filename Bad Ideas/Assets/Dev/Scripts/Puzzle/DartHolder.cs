
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DartHolder : MonoBehaviour
{
    [SerializeField] private GameObject dartPrefab;
    [SerializeField] private PlayerThrow pt;
    [SerializeField] private List<GameObject> balloons;
    [SerializeField] private GameMasterSpeech speech;

    private void Awake()
    {
        CheckBalloons();
    }

    private void CheckBalloons()
    {
        StartCoroutine(CheckBalloonsHeartbeat());
    }

    private IEnumerator CheckBalloonsHeartbeat()
    {
        while (true)
        {
            balloons = balloons.FindAll(b => b != null);

            if (balloons.Count == 0)
            {
                speech.PlayDialogue("Puzzle3Solved");
                break;
            }

            yield return new WaitForSeconds(0.5f);
        }
    }

    public void GivePlayerDart()
    {
        if (pt.HasDart) return;
        pt.PickUpDart(dartPrefab);
    }
}
