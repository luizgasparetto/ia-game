using UnityEngine;

public class Guardian2D : MonoBehaviour
{
    public enum State
    {
        Patrol,
        Alert,
        Chase,
        Attack,
        CallReinforcement,
        Retreat,
        Dead
    }

    [Header("References")]
    [SerializeField] private Transform _target;

    [Header("Stats")]
    [SerializeField] private readonly float _maxHealth = 100f;
    [SerializeField] private readonly float _currentHealth = 100f;
    [SerializeField] private readonly float _moveSpeed = 3f;

    private State _currentState = State.Patrol;

    private const float AlertRange = 20f;
    private const float ChaseRange = 10f;
    private const float MeleeRange = 3f;
    private const float RangedRange = 10f;

    private void Update()
    {
        if (!_target) return;

        float distance = Vector2.Distance(transform.position, _target.position);

        if (_currentHealth <= 0f)
        {
            TransitionTo(State.Dead);
            return;
        }

        _currentState = HandleState(_currentState, distance);
    }

    private State HandleState(State state, float distance) =>
        state switch
        {
            State.Patrol => HandlePatrol(distance),
            State.Alert => HandleAlert(distance),
            State.Chase => HandleChase(distance),
            State.Attack => HandleAttack(distance),
            State.CallReinforcement => HandleCallReinforcement(),
            State.Retreat => HandleRetreat(distance),
            State.Dead => HandleDead(),
            _ => state
        };

    private State HandlePatrol(float distance)
    {
        transform.position += Vector3.right * Mathf.Sin(Time.time) * (_moveSpeed * 0.002f);

        if (distance <= AlertRange) return State.Alert;
        return State.Patrol;
    }

    private State HandleAlert(float distance)
    {
        Debug.Log("Guardião: em alerta!");
        if (distance <= ChaseRange) return State.Chase;
        if (distance > AlertRange) return State.Patrol;
        return State.Alert;
    }

    private State HandleChase(float distance)
    {
        MoveTowards(_target.position);

        if (_currentHealth <= 20f) return State.Retreat;
        if (_currentHealth <= 40f) return State.CallReinforcement;

        if (distance <= MeleeRange || distance <= RangedRange)
            return State.Attack;

        if (distance > AlertRange) return State.Patrol;

        return State.Chase;
    }

    private State HandleAttack(float distance)
    {
        if (distance <= MeleeRange)
            Debug.Log("Guardião: ataque melee");
        else if (distance <= RangedRange)
            Debug.Log("Guardião: ataque ranged");

        return State.Chase;
    }

    private State HandleCallReinforcement()
    {
        Debug.Log("Guardião: chamando reforços!");
        return State.Chase;
    }

    private State HandleRetreat(float distance)
    {
        Vector3 dir = (transform.position - _target.position).normalized;
        transform.position += dir * (_moveSpeed * Time.deltaTime);

        _currentHealth = Mathf.Min(_maxHealth, _currentHealth + 6f * Time.deltaTime);

        if (distance < ChaseRange || _currentHealth >= 40f)
            return State.Chase;

        return State.Retreat;
    }

    private State HandleDead()
    {
        Debug.Log("Guardião: morreu!");
        return State.Dead;
    }

    private void TransitionTo(State newState)
    {
        if (_currentState == newState) return;
        _currentState = newState;
    }

    private void MoveTowards(Vector3 position)
    {
        Vector3 dir = (position - transform.position).normalized;
        transform.position += dir * (_moveSpeed * Time.deltaTime);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, AlertRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, ChaseRange);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, MeleeRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, RangedRange);
    }
}
