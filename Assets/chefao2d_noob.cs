using UnityEngine;

public class chefao2d_noob : MonoBehaviour
{
    public enum est_chefe
    {
        paradin,
        corre,
        bate,
        especial,
        fugir_curar,
        foi_de_base
    }

    public est_chefe atual = est_chefe.paradin;

    [Header("refs")]
    public Transform alvo;   // player

    [Header("stats")]
    public float vida = 100f;
    public float velocidade = 2.8f;

    [Header("timers")]
    public float cd_especial = 6f;
    float timer_cd = 0f;

    // faixas
    float raioChase = 15f;
    float melee = 3f;
    float rangedMax = 10f;

    void Update()
    {
        if (!alvo) return;

        float dist = Vector2.Distance(transform.position, alvo.position);
        if (timer_cd > 0f) timer_cd -= Time.deltaTime;
        if (vida <= 0f) atual = est_chefe.foi_de_base;

        switch (atual)
        {
            case est_chefe.paradin:
                if (dist < raioChase) atual = est_chefe.corre;
                break;

            case est_chefe.corre:
                IrAte(alvo.position);

                if (vida <= 20f) { atual = est_chefe.fugir_curar; break; }
                if (vida <= 50f && timer_cd <= 0f) { atual = est_chefe.especial; break; }

                if (dist < melee || (dist >= melee && dist <= rangedMax))
                    atual = est_chefe.bate;
                break;

            case est_chefe.bate:
                if (dist < melee) Debug.Log("Chefão: ataque melee");
                else if (dist <= rangedMax) Debug.Log("Chefão: ataque ranged");
                atual = est_chefe.corre;
                break;

            case est_chefe.especial:
                Debug.Log("Chefão: ATAQUE ESPECIAL!");
                timer_cd = cd_especial;
                atual = est_chefe.corre;
                break;

            case est_chefe.fugir_curar:
                Vector3 dir = (transform.position - alvo.position).normalized;
                transform.position += dir * (velocidade * Time.deltaTime);
                vida = Mathf.Min(100f, vida + 8f * Time.deltaTime);
                if (dist < 4f || vida >= 50f) atual = est_chefe.corre;
                break;

            case est_chefe.foi_de_base:
                Debug.Log("Chefão: morreu!");
                break;
        }
    }

    void IrAte(Vector3 p)
    {
        Vector3 dir = (p - transform.position).normalized;
        transform.position += dir * (velocidade * Time.deltaTime);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, raioChase);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, melee);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, rangedMax);
    }
}
