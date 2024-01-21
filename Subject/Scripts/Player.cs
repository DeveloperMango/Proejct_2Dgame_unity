using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Vector2 inputVec;
    Rigidbody2D rigid;
    SpriteRenderer spriter;
    Animator animator;


    public float speed;
    private float minX, maxX, minY, maxY;

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        Camera mainCamera = Camera.main;

        // 화면 좌하단과 우상단의 월드 좌표
        Vector2 min = mainCamera.ViewportToWorldPoint(new Vector2(0.01f, 0.05f));
        Vector2 max = mainCamera.ViewportToWorldPoint(new Vector2(0.99f, 0.95f));

        // 화면 밖으로 나가지 못하도록 X와 Y 위치를 제한
        minX = min.x;
        maxX = max.x;
        minY = min.y;
        maxY = max.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (BtnText.g_start)
        {
            // 게임 오브젝트의 현재 위치
            Vector2 currentPos = transform.position;

            currentPos.x = Mathf.Clamp(currentPos.x, minX, maxX);
            currentPos.y = Mathf.Clamp(currentPos.y, minY, maxY);

            transform.position = currentPos;

            inputVec.x = Input.GetAxis("Horizontal");
            inputVec.y = Input.GetAxis("Vertical");
        }
    }

    private void FixedUpdate()
    {
            Vector2 nextVec = inputVec.normalized * speed * Time.fixedDeltaTime;
            rigid.MovePosition(rigid.position + nextVec);
    }

    private void LateUpdate()
    {
        animator.SetFloat("speed", inputVec.magnitude);
        if (inputVec.x != 0)
        {
            spriter.flipX = inputVec.x < 0;
        }
    }

}
