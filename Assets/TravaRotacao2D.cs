using UnityEngine;

[DisallowMultipleComponent]
public class TravaRotacao2D : MonoBehaviour
{
    Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb) rb.constraints |= RigidbodyConstraints2D.FreezeRotation;
    }

    void LateUpdate()
    {
        if (rb) { rb.angularVelocity = 0f; rb.SetRotation(0f); }
        else { transform.rotation = Quaternion.identity; }
    }
}
