using System.Collections;
using UnityEngine;

public class CharacterTemporaryStats : MonoBehaviour
{
    [Header("Runtime Multipliers")]
    [SerializeField] private float damageMultiplier = 1f;
    [SerializeField] private float moveSpeedMultiplier = 1f;
    [SerializeField] private float attackSpeedMultiplier = 1f;
    [SerializeField] private bool isInvulnerable;

    private Life life;
    private Inmunity inmunityVisual;

    private Coroutine damageBonusCoroutine;
    private Coroutine moveSpeedBonusCoroutine;
    private Coroutine attackSpeedBonusCoroutine;
    private Coroutine invulnerabilityCoroutine;

    public float DamageMultiplier => damageMultiplier;
    public float MoveSpeedMultiplier => moveSpeedMultiplier;
    public float AttackSpeedMultiplier => attackSpeedMultiplier;
    public bool IsInvulnerable => isInvulnerable;

    private void Awake()
    {
        life = GetComponent<Life>();
        inmunityVisual = GetComponent<Inmunity>();
    }

    public void ApplyDamageBonus(float multiplier, float duration)
    {
        if (multiplier <= 0f || duration <= 0f)
            return;

        if (damageBonusCoroutine != null)
        {
            StopCoroutine(damageBonusCoroutine);
        }

        damageBonusCoroutine = StartCoroutine(DamageBonusRoutine(multiplier, duration));
    }

    public void ApplyMoveSpeedBonus(float multiplier, float duration)
    {
        if (multiplier <= 0f || duration <= 0f)
            return;

        if (moveSpeedBonusCoroutine != null)
        {
            StopCoroutine(moveSpeedBonusCoroutine);
        }

        moveSpeedBonusCoroutine = StartCoroutine(MoveSpeedBonusRoutine(multiplier, duration));
    }

    public void ApplyAttackSpeedBonus(float multiplier, float duration)
    {
        if (multiplier <= 0f || duration <= 0f)
            return;

        if (attackSpeedBonusCoroutine != null)
        {
            StopCoroutine(attackSpeedBonusCoroutine);
        }

        attackSpeedBonusCoroutine = StartCoroutine(AttackSpeedBonusRoutine(multiplier, duration));
    }

    public void ApplyInvulnerability(float duration)
    {
        if (duration <= 0f)
            return;

        if (life == null)
        {
            life = GetComponent<Life>();
        }

        if (inmunityVisual == null)
        {
            inmunityVisual = GetComponent<Inmunity>();
        }

        if (life == null)
        {
            Debug.LogWarning("Cannot apply invulnerability. Life component not found.");
            return;
        }

        if (invulnerabilityCoroutine != null)
        {
            StopCoroutine(invulnerabilityCoroutine);

            if (isInvulnerable)
            {
                life.RemoveInmunitySource();
                isInvulnerable = false;
            }
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
        life.AddInmunitySource();

        if (inmunityVisual != null)
        {
            inmunityVisual.PlayBlink(duration);
        }

        yield return new WaitForSeconds(duration);

        life.RemoveInmunitySource();
        isInvulnerable = false;

        invulnerabilityCoroutine = null;
    }

    private void OnDestroy()
    {
        if (isInvulnerable && life != null)
        {
            life.RemoveInmunitySource();
            isInvulnerable = false;
        }
    }
}