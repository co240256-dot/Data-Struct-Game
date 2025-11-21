using UnityEngine;

public class Entry_Mountain : MonoBehaviour
{
    public Collider2D[] mountainColliders;   // outside walls / roof
    public Collider2D[] boundaryColliders;   // inner orange border

    private void Start()
    {
        // Outside the mountain by default:
        // walls ON, inner boundary OFF
        foreach (var c in mountainColliders)
            if (c != null) c.enabled = true;

        foreach (var c in boundaryColliders)
            if (c != null) c.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        // Let player go behind the mountain
        foreach (Collider2D mountain in mountainColliders)
            if (mountain != null) mountain.enabled = false;

        // Lock them inside with the orange border
        foreach (Collider2D boundary in boundaryColliders)
            if (boundary != null) boundary.enabled = true;

        var sr = collision.GetComponent<SpriteRenderer>();
        if (sr != null) sr.sortingOrder = 15;
    }
}
