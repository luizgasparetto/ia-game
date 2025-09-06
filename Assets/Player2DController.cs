using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player2DController : MonoBehaviour
{
    public float velocidade = 5f;
    Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Vector2 dir = new Vector2(h, v).normalized;
        rb.linearVelocity = dir * velocidade;

        rb.angularVelocity = 0f;
        rb.SetRotation(0f);
    }

    void OnCollisionStay2D(Collision2D _)
    {
        rb.angularVelocity = 0f;
        rb.SetRotation(0f);
    }
}
