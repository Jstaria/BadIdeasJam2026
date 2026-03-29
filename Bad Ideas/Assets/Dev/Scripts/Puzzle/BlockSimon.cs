using System.Collections.Generic;
using UnityEngine;

public class BlockSimon : MonoBehaviour
{
    public List<Rigidbody> blocks;
    [SerializeField] private GameMasterSpeech speech;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        
        foreach (var block in blocks)
        {
            block.constraints = RigidbodyConstraints.None;
            block.AddExplosionForce(500, other.transform.position, 50);
        }

        GetComponent<BoxCollider>().enabled = false;

        speech.PlayDialogue("TowerKnockedOver");    
    }

    public void RemoveBlock(GameObject block)
    {
        blocks.Remove(block.GetComponent<Rigidbody>());
        Destroy(block);

        if (blocks.Count == 0)
        {
            speech.PlayDialogue("Puzzle2Solved");
        }
    }
}
