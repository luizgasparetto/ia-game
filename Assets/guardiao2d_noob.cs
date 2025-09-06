using UnityEngine;

public class guardiao2d_noob : MonoBehaviour
{
    public enum estadozin
    {
        patrulha,
        alerta,
        persegui,
        atack,
        chama_ajuda,
        recua,
        morto
    }

    public estadozin atual = estadozin.patrulha;

    [Header("refs")]
    public Transform alvo;   // player
    public float vida = 100f;

    [Header("movimento")]
    public float velocidade = 3f;

    // faixas
    float raioAlerta = 20f;
    float raioChase = 10f;
    float melee = 3f;
    float rangedMax = 10f;

    void Update()
    {
        if (!alvo) return;

        float dist = Vector2.Distance(transform.position, alvo.position);
        if (vida <= 0f) atual = estadozin.morto;

        switch (atual)
        {
            case estadozin.patrulha:
                // patrulha boba: movimento de vaivém
                transform.position += Vector3.right * Mathf.Sin(Time.time) * (velocidade * 0.002f);
                if (dist <= raioAlerta) atual = estadozin.alerta;
                break;

            case estadozin.alerta:
                Debug.Log("Guardião: em alerta!");
                if (dist <= raioChase) atual = estadozin.persegui;
                else if (dist > raioAlerta) atual = estadozin.patrulha;
                break;

            case estadozin.persegui:
                IrAte(alvo.position);

                if (vida <= 20f) { atual = estadozin.recua; break; }
                if (vida <= 40f) { atual = estadozin.chama_ajuda; break; }

                if (dist < melee || (dist >= melee && dist <= rangedMax))
                    atual = estadozin.atack;

                if (dist > raioAlerta) atual = estadozin.patrulha;
                break;

            case estadozin.atack:
                if (dist < melee) Debug.Log("Guardião: ataque melee");
                else if (dist <= rangedMax) Debug.Log("Guardião: ataque ranged");
                atual = estadozin.persegui;
                break;

            case estadozin.chama_ajuda:
                Debug.Log("Guardião: chamando reforços!");
                atual = estadozin.persegui;
                break;

            case estadozin.recua:
                Vector3 dir = (transform.position - alvo.position).normalized;
                transform.position += dir * (velocidade * Time.deltaTime);
                vida = Mathf.Min(100f, vida + 6f * Time.deltaTime);
                if (dist < raioChase || vida >= 40f) atual = estadozin.persegui;
                break;

            case estadozin.morto:
                Debug.Log("Guardião: morreu!");
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
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, raioAlerta);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, raioChase);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, melee);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, rangedMax);
    }
}
