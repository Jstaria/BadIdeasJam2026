using UnityEngine;

public class Dart : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    private string board = "RedBlock";

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag(board))
        {
            rb.constraints = RigidbodyConstraints.FreezeAll;
            rb.isKinematic = true;

            transform.SetParent(collision.transform);
        }
    }

    private void Update()
    {
        if (rb.linearVelocity.sqrMagnitude > 0.001f)
        {
            transform.LookAt(transform.position - rb.linearVelocity);
        }
    }

    public void Throw()
    {
        rb.isKinematic = false;

        Vector3 target = Camera.main.transform.position + Camera.main.transform.forward * 10f;
        Vector3 dir = (target - transform.position).normalized;

        rb.AddForce(dir * 10f, ForceMode.Impulse);
    }
}
