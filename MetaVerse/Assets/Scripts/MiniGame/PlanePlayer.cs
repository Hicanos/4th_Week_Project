using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanePlayer : MonoBehaviour
{
    private Animator characterAnimator;
    Animator animator;
    Rigidbody2D rigbody;

    public float flapForce = 6f;
    public float forwardSpeed = 3f;
    public bool isDead = false;
    public bool isFlap = false;

    float deathCooldown = 0f;

    public bool godMode = false;

    MiniGameManager miniGameManager;


    void Start()
    {
        miniGameManager = MiniGameManager.Instance;        
        rigbody = transform.GetComponent<Rigidbody2D>();

        if (rigbody == null) Debug.LogError("Not Founded Animator");
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead)
        {
            if (deathCooldown <= 0)
            {
                if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
                {
                    miniGameManager.RestartGame();
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
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (godMode) return;
        if (isDead) return;

        isDead = true;
        deathCooldown = 1f;

        animator.SetInteger("IsDie", 1);
        miniGameManager.GameOver();
    }

    public void SetCharacterAnimator(Animator animator)
    {
        characterAnimator = animator;
        if (characterAnimator == null) Debug.LogWarning("[PlanePlayer] ĳ���� �ִϸ����Ͱ� null�� �����Ǿ����ϴ�.");
        else Debug.Log("[PlanePlayer] ĳ���� �ִϸ����� ���� �Ϸ�.");
    }

    public void SetPlaneAnimationType(int type)
    {
        Debug.Log($"[PlanePlayer] ����� �ִϸ��̼� Ÿ�� ����: {type}");
        // �ִϸ������� �Ķ���͸� �����Ͽ� �ִϸ��̼� ����
        if (characterAnimator != null)
        {
            characterAnimator.SetInteger("PlaneType", type);
        }
        else
        {
            Debug.LogWarning("[PlanePlayer] ĳ���� �ִϸ����Ͱ� �����ϴ�.");
        }
    }
}
