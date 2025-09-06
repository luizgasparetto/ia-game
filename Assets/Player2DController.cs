using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player2DController : MonoBehaviour
{
    public float velocidade = 5f;
    Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        // trava rota��o via f�sica
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Vector2 dir = new Vector2(h, v).normalized;
        rb.linearVelocity = dir * velocidade;

        // hard lock extra (caso alguma colis�o tente girar)
        rb.angularVelocity = 0f;
        rb.SetRotation(0f); // mant�m Z = 0
    }

    void OnCollisionStay2D(Collision2D _)
    {
        // redund�ncia de seguran�a (se colidir e tentar girar)
        rb.angularVelocity = 0f;
        rb.SetRotation(0f);
    }
}
