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
                //deltaTime: ������ �ð��� ������ ��ġ�� ���� ����!
                // �������� ���� ���� �� �ε巯��������.
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
        //������ �׳� ����
        if (isDead) return;

        //���� ������� ��
        //velocity = ���ӵ�
        //velocity��� ����ü�� rigbody�� ������ ��
        Vector3 velocity = rigbody.velocity;
        
        //�Ȱ��� �ӵ��� ������ �����ϵ��� ��
        velocity.x = forwardSpeed;

        if (isFlap)
        {
            //���� �õ� ��, y���� Force
            velocity.y += flapForce;
            isFlap = false; //���� �پ����� �ٽ� false��
        }

        //������ �� �ٽ� ���� �ڸ��� �� �־��ֱ�.
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
