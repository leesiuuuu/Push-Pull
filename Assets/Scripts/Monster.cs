using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Monster : MonoBehaviour
{
    Rigidbody2D rigid;

    [SerializeField] float speed = 2f;

    [Header("Detection")]
    [SerializeField] float frontCheckDistance = 0.3f;
    [SerializeField] float groundCheckDistance = 0.5f;
    [SerializeField] LayerMask groundLayer;

    Vector2 moveDir = Vector2.right;

    LevelLoader levelLoader;
    BGMScript bs;

    Coroutine moveCoroutine;
    bool isDead = false;

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        levelLoader = FindObjectOfType<LevelLoader>();
        bs = FindObjectOfType<BGMScript>();

        moveCoroutine = StartCoroutine(Move());
    }

    IEnumerator Move()
    {
        while (true)
        {
            if (isDead)
                yield break;

            CheckDirection();

            rigid.velocity = new Vector2(moveDir.x * speed, rigid.velocity.y);

            yield return null;
        }
    }

    void CheckDirection()
    {
        Vector2 origin = transform.position;

        RaycastHit2D[] hits = Physics2D.RaycastAll(
            origin,
            moveDir,
            frontCheckDistance
        );

        bool frontBlocked = false;

        foreach (var hit in hits)
        {
            if (hit.collider == null) continue;

            if (hit.collider.GetComponent<NewPlayer1>() != null) continue;
            if (hit.collider.GetComponent<NewPlayer2>() != null) continue;
            if (hit.collider.transform == transform) continue;

            frontBlocked = true;
            break;
        }

        Vector2 groundOrigin = origin + moveDir * 0.3f;
        RaycastHit2D groundHit = Physics2D.Raycast(
            groundOrigin,
            Vector2.down,
            groundCheckDistance,
            groundLayer
        );

        if (frontBlocked || groundHit.collider == null)
        {
            Flip();
        }
    }

    void Flip()
    {
        moveDir = -moveDir;

        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDead) return;

        if (collision.gameObject.TryGetComponent<NewPlayer1>(out var player1))
        {
            KillPlayer(player1);
        }
        else if (collision.gameObject.TryGetComponent<NewPlayer2>(out var player2))
        {
            KillPlayer(player2);
        }
    }

    void KillPlayer(MonoBehaviour player)
    {
        player.SendMessage("Die");
        bs.FadeOut();
        levelLoader.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Die()
    {
        if (isDead) return;
        isDead = true;

        // 이동 정지
        if (moveCoroutine != null)
            StopCoroutine(moveCoroutine);

        rigid.velocity = Vector2.zero;

        // Z 회전 Lock 해제
        rigid.constraints &= ~RigidbodyConstraints2D.FreezeRotation;

    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(
            transform.position,
            transform.position + (Vector3)moveDir * frontCheckDistance
        );

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(
            transform.position + (Vector3)moveDir * 0.3f,
            transform.position + (Vector3)moveDir * 0.3f + Vector3.down * groundCheckDistance
        );
    }
}
