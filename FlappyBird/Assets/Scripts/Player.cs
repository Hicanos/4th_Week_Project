using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    Animator animator;
    Rigidbody2D rigbody;

    public float flapForce = 6f;
    public float forwardSpeed = 3f;
    public bool isDead = false;
    public bool isFlap = false;
    
    float deathCooldown = 0f;

    public bool godMode = false;

    GameManager gameManager;


    void Start()
    {
        gameManager = GameManager.Instance;
        animator = transform.GetComponentInChildren<Animator>();
        rigbody = transform.GetComponent<Rigidbody2D>();

        if (animator == null) Debug.LogError("Not Founded Animator");
        if (rigbody == null) Debug.LogError("Not Founded Animator");
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead)
        {
            if(deathCooldown <= 0)
            {
                if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
                {
                    gameManager.RestartGame();
                }
            }
            else
            {
                //deltaTime: 동일한 시간에 동일한 위치에 도달 가능!
                // 프레임이 높을 수록 더 부드러워지긴함.
                deathCooldown -= Time.deltaTime;
            }

        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            {
                isFlap = true;
            }
        }
    }

    private void FixedUpdate()
    {
        //죽으면 그냥 리턴
        if (isDead) return;

        //여긴 살아있을 때
        //velocity = 가속도
        //velocity라는 구조체에 rigbody를 가져온 것
        Vector3 velocity = rigbody.velocity;
        
        //똑같은 속도로 앞으로 진행하도록 함
        velocity.x = forwardSpeed;

        if (isFlap)
        {
            //점프 시도 시, y값을 Force
            velocity.y += flapForce;
            isFlap = false; //점프 뛰었으니 다시 false로
        }

        //가져온 걸 다시 원래 자리에 쏙 넣어주기.
        rigbody.velocity = velocity;

        float angle = Mathf.Clamp((rigbody.velocity.y * 10f), -90, 90);
        transform.rotation =Quaternion.Euler(0, 0, angle);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (godMode) return;
        if (isDead) return;

        isDead = true;
        deathCooldown = 1f;

        animator.SetInteger("IsDie",1);
        gameManager.GameOver();
    }
}
