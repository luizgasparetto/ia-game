using UnityEngine;

public class Boss2D : MonoBehaviour
{
    public enum State
    {
        Idle,
        Chase,
        Attack,
        Special,
        RetreatHeal,
        Dead
    }

    [Header("References")]
    [SerializeField] private Transform _target;

    [Header("Stats")]
    [SerializeField] private readonly float _maxHealth = 100f;
    [SerializeField] private readonly float _currentHealth = 100f;
    [SerializeField] private readonly float _moveSpeed = 2.8f;

    [Header("Cooldowns")]
    [SerializeField] private readonly float _specialCooldown = 6f;
    private float _cooldownTimer = 0f;

    private State _currentState = State.Idle;

    private const float ChaseRange = 15f;
    private const float MeleeRange = 3f;
    private const float RangedRange = 10f;

    private void Update()
    {
        if (!_target) return;

        float distance = Vector2.Distance(transform.position, _target.position);
        if (_cooldownTimer > 0f) _cooldownTimer -= Time.deltaTime;

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
            State.Idle => HandleIdle(distance),
            State.Chase => HandleChase(distance),
            State.Attack => HandleAttack(distance),
            State.Special => HandleSpecial(),
            State.RetreatHeal => HandleRetreatHeal(distance),
            State.Dead => HandleDead(),
            _ => state
        };

    private State HandleIdle(float distance)
    {
        if (distance < ChaseRange) return State.Chase;
        return State.Idle;
    }

    private State HandleChase(float distance)
    {
        MoveTowards(_target.position);

        if (_currentHealth <= 20f) return State.RetreatHeal;
        if (_currentHealth <= 50f && _cooldownTimer <= 0f) return State.Special;

        if (distance <= MeleeRange || distance <= RangedRange)
            return State.Attack;

        return State.Chase;
    }

    private State HandleAttack(float distance)
    {
        if (distance <= MeleeRange)
            Debug.Log("Boss: ataque melee");
        else if (distance <= RangedRange)
            Debug.Log("Boss: ataque ranged");

        return State.Chase;
    }

    private State HandleSpecial()
    {
        Debug.Log("Boss: ATAQUE ESPECIAL!");
        _cooldownTimer = _specialCooldown;
        return State.Chase;
    }

    private State HandleRetreatHeal(float distance)
    {
        Vector3 dir = (transform.position - _target.position).normalized;
        transform.position += dir * (_moveSpeed * Time.deltaTime);

        _currentHealth = Mathf.Min(_maxHealth, _currentHealth + 8f * Time.deltaTime);

        if (distance < 4f || _currentHealth >= 50f)
            return State.Chase;

        return State.RetreatHeal;
    }

    private State HandleDead()
    {
        Debug.Log("Boss: morreu!");
        return State.Dead;
    }

    private void TransitionTo(State newState)
    {
        if (_currentState == newState) return;
        _currentState = newState;
    }

    private void MoveTowards(Vector3 targetPos)
    {
        Vector3 dir = (targetPos - transform.position).normalized;
        transform.position += dir * (_moveSpeed * Time.deltaTime);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, ChaseRange);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, MeleeRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, RangedRange);
    }
}
