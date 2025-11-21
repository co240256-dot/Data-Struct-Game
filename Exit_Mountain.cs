using UnityEngine;

public class Exit_Mountain : MonoBehaviour
{
    public Collider2D[] mountainColliders;
    public Collider2D[] boundaryColliders;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        // Turn outside walls back ON, inner border OFF
        foreach (Collider2D mountain in mountainColliders)
            if (mountain != null) mountain.enabled = true;

        foreach (Collider2D boundary in boundaryColliders)
            if (boundary != null) boundary.enabled = false;

        var sr = collision.GetComponent<SpriteRenderer>();
        if (sr != null) sr.sortingOrder = 1;
    }
}
