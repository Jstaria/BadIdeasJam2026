using Unity.VisualScripting;
using UnityEngine;

public class PlayerThrow : MonoBehaviour
{
    public bool HasDart = false;
    [SerializeField] private Transform dartPos;
    [SerializeField] private Transform carousel;

    private GameObject currentDart;

    private void Update()
    {
        if (currentDart != null)
            currentDart.transform.position = dartPos.position;
    }

    public void PickUpDart(GameObject dartPrefab)
    {
        if (currentDart == null)
            currentDart = Instantiate(dartPrefab, dartPos);

        currentDart.SetActive(true);

        HasDart = true;
    }

    public void ThrowDart()
    {
        if (!HasDart) return;

        Dart dart = Instantiate(currentDart, Camera.main.transform.position + Camera.main.transform.forward * 3, Quaternion.identity).GetComponent<Dart>();
        dart.GetComponent<BoxCollider>().enabled = true;

        currentDart.SetActive(false);
        dart.Throw();
        HasDart = false;
    }
}
