using UnityEngine;

/// <summary>
/// Melee unit automatic target detection + facing + attack trigger.
/// If target has AttackRanged component, melee unit will use extended detection range
/// so it will try to chase and hit ranged enemies as well.
/// </summary>
[RequireComponent(typeof(AttackMelee))]
public class MeleeAutoAttack : MonoBehaviour
{
    [Tooltip("Default detection radius for melee enemies")]
    public float detectionRange = 1.5f;

    [Tooltip("Extra radius added when the potential target is a ranged unit")]
    public float extraRangeAgainstRanged = 2.0f;

    [Tooltip("Layer(s) that represent enemy units")]
    public LayerMask enemyLayer;

    private AttackMelee attack;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        attack = GetComponent<AttackMelee>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        float maxCheckRange = detectionRange + Mathf.Max(0f, extraRangeAgainstRanged);
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, maxCheckRange, enemyLayer);

        if (hits == null || hits.Length == 0)
            return;

        Transform chosen = null;
        float bestDist = float.MaxValue;

        // First pass: closest enemy within normal detection range
        foreach (var h in hits)
        {
            float d = Vector2.Distance(transform.position, h.transform.position);
            if (d <= detectionRange && d < bestDist)
            {
                bestDist = d;
                chosen = h.transform;
            }
        }

        // Second pass: if no close enemy, pick ranged enemy in extended range
        if (chosen == null)
        {
            foreach (var h in hits)
            {
                float d = Vector2.Distance(transform.position, h.transform.position);
                if (d <= maxCheckRange)
                {
                    var ranged = h.GetComponent<AttackRanged>();
                    if (ranged != null && d < bestDist)
                    {
                        bestDist = d;
                        chosen = h.transform;
                    }
                }
            }
        }

        if (chosen != null)
        {
            FaceTarget(chosen.position);

            // **IMPORTANT FIX** – use cooldown-controlled TryAttack, not Fire!
            attack.TryAttack(chosen);
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
    }
}
