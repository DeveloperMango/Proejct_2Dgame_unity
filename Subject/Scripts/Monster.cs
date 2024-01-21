using System.Collections;
using UnityEngine;

public class Monster : MonoBehaviour
{
    [SerializeField] private Rigidbody2D target;
    [SerializeField] private Rigidbody2D monster;
    private SpriteRenderer spriter;

    private float minX, maxX, minY, maxY;
    public float moveSpeed;
    private float fleeDistance = 1.5f;

    private bool isFollowing = true;
    private Vector2 randomDestination;

    private int hitCount = 0;
    private int hitsToDie = 3;

    public GameObject[] deadObjects;
    public GameObject[] liveObejcts;

    private bool isLive = false;

    Rigidbody2D rigid;
    Animator anim;
    WaitForFixedUpdate wait;


    private bool isInvincible = false; // 무적 상태를 나타내는 변수
    private float invincibleDuration = 1.5f; // 무적 상태 지속 시간
    public float invincibleMoveSpeed = 15.0f; // 무적 상태일 때의 이동 속도




    void Awake()
    {
        Camera mainCamera = Camera.main;

        Vector2 min = mainCamera.ViewportToWorldPoint(new Vector2(0.01f, 0.05f));
        Vector2 max = mainCamera.ViewportToWorldPoint(new Vector2(0.99f, 0.95f));

        minX = min.x;
        maxX = max.x;
        minY = min.y;
        maxY = max.y;

        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriter = GetComponent<SpriteRenderer>();
        wait = new WaitForFixedUpdate();
        rigid.isKinematic = true;
    }

    void Update()
    {
        if (isLive)
        {
            return;
        }

        Vector2 moveDirection = Vector2.zero;


        // 몬스터가 무기에 한대 맞으면 바로 맞는걸 막기위해 무적상태 부여
        if (isInvincible)
        {
            // 타겟과 반대 방향으로 이동할 위치 계산
            Vector2 awayFromTarget = (monster.position - target.position).normalized;

            // 최대로 멀어질 수 있는 위치 계산
            Vector2 furthestPoint = monster.position + awayFromTarget * 10.0f; 

            // 화면 경계 내로 조정
            furthestPoint.x = Mathf.Clamp(furthestPoint.x, minX + 0.1f, maxX - 0.1f);
            furthestPoint.y = Mathf.Clamp(furthestPoint.y, minY + 0.1f, maxY - 0.1f);

            moveDirection = (furthestPoint - monster.position).normalized;

            rigid.MovePosition(monster.position + moveDirection * invincibleMoveSpeed * Time.deltaTime);
            return;
        }
        else if (target != null && BtnText.g_start == true)
        {
            float distanceToTarget = Vector2.Distance(monster.position, target.position);

            if (isFollowing)
            {
                Vector2 normDir = (target.position - monster.position).normalized;

                if (monster.position.x <= minX + 0.2f || monster.position.x >= maxX - 0.2f ||
                    monster.position.y <= minY + 0.2f || monster.position.y >= maxY - 0.2f)
                {
                    SetRandomDestination();
                    moveDirection = (randomDestination - monster.position).normalized;
                }
                else
                {
                    moveDirection = -(normDir);
                }

                if (distanceToTarget > fleeDistance * 2)
                {
                    isFollowing = false;
                }
            }
            else
            {
                if (Vector2.Distance(monster.position, randomDestination) < 0.5f || distanceToTarget < fleeDistance)
                {
                    SetRandomDestination();
                }

                moveDirection = (randomDestination - monster.position).normalized;

                if (distanceToTarget < fleeDistance)
                {
                    isFollowing = true;
                }
            }
        }

        rigid.MovePosition(monster.position + moveDirection * moveSpeed * Time.deltaTime);
    }

    void SetRandomDestination()
    {
        randomDestination = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (isInvincible) return; // 무적 상태일 때는 아무런 반응하지 않음

        if (other.CompareTag("Weapon"))
        {
            if (hitCount < hitsToDie)
            {
                Vector2 playerPos = GameObject.Find("Player").transform.position;
                Vector2 dirVec = ((Vector2)transform.position - playerPos).normalized;
                rigid.AddForce(dirVec.normalized * 1.5f, ForceMode2D.Impulse); // 여기에 직접 튕기는 반응 구현

                liveObejcts[hitCount].SetActive(false);
                deadObjects[hitCount].SetActive(true);
            }

            hitCount++;
            rigid.velocity = Vector2.zero;

            if (hitCount >= hitsToDie)
            {
                anim.SetBool("Dead", true);
                Die();
            }
            else
            {
                anim.SetTrigger("Hit");
                StartCoroutine(Invincibility());
            }
        }
    }

    IEnumerator Invincibility()
    {
        isInvincible = true;
        yield return new WaitForSeconds(invincibleDuration);
        isInvincible = false;
    }

    void Die()
    {
        isLive = true;
        rigid.velocity = Vector2.zero;

    }
}
