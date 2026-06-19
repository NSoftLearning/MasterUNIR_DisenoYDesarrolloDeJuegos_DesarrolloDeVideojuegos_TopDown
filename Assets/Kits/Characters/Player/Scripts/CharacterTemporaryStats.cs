using System.Collections;
using UnityEngine;

public class CharacterTemporaryStats : MonoBehaviour
{
    [Header("Runtime Multipliers")]
    [SerializeField] private float damageMultiplier = 1f;
    [SerializeField] private float moveSpeedMultiplier = 1f;
    [SerializeField] private float attackSpeedMultiplier = 1f;
    [SerializeField] private bool isInvulnerable;

    private Coroutine damageBonusCoroutine;
    private Coroutine moveSpeedBonusCoroutine;
    private Coroutine attackSpeedBonusCoroutine;
    private Coroutine invulnerabilityCoroutine;

    public float DamageMultiplier => damageMultiplier;
    public float MoveSpeedMultiplier => moveSpeedMultiplier;
    public float AttackSpeedMultiplier => attackSpeedMultiplier;
    public bool IsInvulnerable => isInvulnerable;

    public void ApplyDamageBonus(float multiplier, float duration)
    {
        if (damageBonusCoroutine != null)
        {
            StopCoroutine(damageBonusCoroutine);
        }

        damageBonusCoroutine = StartCoroutine(DamageBonusRoutine(multiplier, duration));
    }

    public void ApplyMoveSpeedBonus(float multiplier, float duration)
    {
        if (moveSpeedBonusCoroutine != null)
        {
            StopCoroutine(moveSpeedBonusCoroutine);
        }

        moveSpeedBonusCoroutine = StartCoroutine(MoveSpeedBonusRoutine(multiplier, duration));
    }

    public void ApplyAttackSpeedBonus(float multiplier, float duration)
    {
        if (attackSpeedBonusCoroutine != null)
        {
            StopCoroutine(attackSpeedBonusCoroutine);
        }

        attackSpeedBonusCoroutine = StartCoroutine(AttackSpeedBonusRoutine(multiplier, duration));
    }

    public void ApplyInvulnerability(float duration)
    {
        if (invulnerabilityCoroutine != null)
        {
            StopCoroutine(invulnerabilityCoroutine);
        }

        invulnerabilityCoroutine = StartCoroutine(InvulnerabilityRoutine(duration));
    }

    private IEnumerator DamageBonusRoutine(float multiplier, float duration)
    {
        damageMultiplier = multiplier;

        yield return new WaitForSeconds(duration);

        damageMultiplier = 1f;
        damageBonusCoroutine = null;
    }

    private IEnumerator MoveSpeedBonusRoutine(float multiplier, float duration)
    {
        moveSpeedMultiplier = multiplier;

        yield return new WaitForSeconds(duration);

        moveSpeedMultiplier = 1f;
        moveSpeedBonusCoroutine = null;
    }

    private IEnumerator AttackSpeedBonusRoutine(float multiplier, float duration)
    {
        attackSpeedMultiplier = multiplier;

        yield return new WaitForSeconds(duration);

        attackSpeedMultiplier = 1f;
        attackSpeedBonusCoroutine = null;
    }

    private IEnumerator InvulnerabilityRoutine(float duration)
    {
        isInvulnerable = true;

        yield return new WaitForSeconds(duration);

        isInvulnerable = false;
        invulnerabilityCoroutine = null;
    }
}