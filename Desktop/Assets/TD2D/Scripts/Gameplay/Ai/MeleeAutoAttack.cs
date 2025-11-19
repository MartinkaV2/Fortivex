using UnityEngine;

/// <summary>
/// Melee unit automatic target detection + movement + facing + attack trigger.
/// Supports extended detection for ranged enemies.
/// </summary>
[RequireComponent(typeof(AttackMelee))]
public class MeleeAutoAttack : MonoBehaviour
{
    [Header("Ranges")]
    public float detectionRange = 1.5f;              // normál látótáv
    public float extraRangeAgainstRanged = 2.0f;     // ranged enemy ellen több
    public float attackRange = 1.0f;                 // milyen közel kell lenni az ütéshez

    [Header("Movement")]
    public float moveSpeed = 2f;                     // egység sebessége
    public LayerMask enemyLayer;

    private AttackMelee attack;
    private SpriteRenderer spriteRenderer;
    private Transform currentTarget;

    void Start()
    {
        attack = GetComponent<AttackMelee>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        FindTarget();
        MoveAndAttack();
    }

    private void FindTarget()
    {
        float maxCheckRange = detectionRange + extraRangeAgainstRanged;
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, maxCheckRange, enemyLayer);

        if (hits.Length == 0)
        {
            currentTarget = null;
            return;
        }

        float bestDist = float.MaxValue;
        Transform bestTarget = null;

        // 1. első kör: normál detection range-en belül a legközelebbi
        foreach (var h in hits)
        {
            float d = Vector2.Distance(transform.position, h.transform.position);
            if (d <= detectionRange && d < bestDist)
            {
                bestDist = d;
                bestTarget = h.transform;
            }
        }

        // 2. ha nincs melee range-en belül, nézz ranged ellenfelet extended range-ben
        if (bestTarget == null)
        {
            foreach (var h in hits)
            {
                float d = Vector2.Distance(transform.position, h.transform.position);

                if (d <= maxCheckRange)
                {
                    if (h.GetComponent<AttackRanged>() != null)
                    {
                        if (d < bestDist)
                        {
                            bestDist = d;
                            bestTarget = h.transform;
                        }
                    }
                }
            }
        }

        currentTarget = bestTarget;
    }

    private void MoveAndAttack()
    {
        if (currentTarget == null)
            return;

        // Sprite nézzen cél felé
        FaceTarget(currentTarget.position);

        float distance = Vector2.Distance(transform.position, currentTarget.position);

        if (distance > attackRange)
        {
            // 🔹 TÁMADÁSI TÁVON KÍVÜL → LÉPJEN ODA
            Vector3 dir = (currentTarget.position - transform.position).normalized;
            transform.position += dir * moveSpeed * Time.deltaTime;
        }
        else
        {
            // 🔹 ELÉG KÖZEL → TAMADÁS
            attack.TryAttack(currentTarget);
        }
    }

    private void FaceTarget(Vector3 targetPos)
    {
        if (spriteRenderer == null) return;

        float direction = targetPos.x - transform.position.x;
        if (Mathf.Approximately(direction, 0f)) return;

        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * (direction < 0 ? -1 : 1);
        transform.localScale = scale;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange + extraRangeAgainstRanged);

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
