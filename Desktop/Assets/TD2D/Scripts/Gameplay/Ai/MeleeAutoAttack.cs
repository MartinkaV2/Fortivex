using UnityEngine;
using System.Linq;

public class MeleeAutoAttack : MonoBehaviour
{
    public float detectionRange = 1.5f;
    public LayerMask enemyLayer;
    private AttackMelee attack;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        attack = GetComponent<AttackMelee>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>(); // vagy saját referenciát adj meg
    }

    void Update()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, detectionRange, enemyLayer);
        if (hits.Length > 0)
        {
            var target = hits
                .Select(h => h.transform)
                .OrderBy(t => Vector2.Distance(transform.position, t.position))
                .First();

            // 👉 Irányváltás
            FaceTarget(target);

            if (attack != null)
                attack.TryAttack(target);
        }
    }

    void FaceTarget(Transform target)
    {
        if (target == null) return;

        float direction = target.position.x - transform.position.x;

        // Ha spriteRenderer van
        if (spriteRenderer != null)
        {
            spriteRenderer.flipX = direction < 0;
        }
        else
        {
            // Vagy ha a sprite scale alapján fordul
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x) * (direction < 0 ? -1 : 1);
            transform.localScale = scale;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
