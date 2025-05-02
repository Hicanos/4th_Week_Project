using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;

//��� ĳ������ �⺻ ������ ����
public class BaseController : MonoBehaviour
{
    protected Rigidbody2D rigid;
    protected AnimationHandler animationHandler;
    protected StatHandler statHandler;

    //SerializeField = ����ȭ, ��: ��ü�� ���¸� ���߿� ����, ����, �籸���� �� �ִ� ������� ��ȯ
    //private�� ����ϴ� ������ ����Ƽ�� �ν����� â�� ����
    [SerializeField] private SpriteRenderer characterRenderer; //��������Ʈ �¿���� �غ�
    [SerializeField] private Transform weaponPivot; //���⸦ ��ġ��ų ���� ��ġ



    //�̵� ����
    protected Vector2 movementDirection = Vector2.zero; 
    public Vector2 MovementDirection { get { return movementDirection; } }


    //�ٶ󺸴� ����
    protected Vector2 lookDirection = Vector2.zero;
    public Vector2 LookDirection { get { return lookDirection; } }

    //�˹�
    private Vector2 knockback = Vector2.zero; //�˹� ����
    private float knockbackDuration = 0.0f; //�˹� ���� �ð�

    [SerializeField] public WeaponHandler WeaponPrefab;
    protected WeaponHandler weaponHandler;

    protected bool isAttacking;
    private float timeSinceLastAttack = float.MaxValue;

    //����޼���� ����-Player Controller���� overrides
    protected virtual void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        animationHandler = GetComponent<AnimationHandler>();
        statHandler = GetComponent<StatHandler>();

        if (WeaponPrefab != null)
            weaponHandler = Instantiate(WeaponPrefab, weaponPivot);
        else
            weaponHandler = GetComponentInChildren<WeaponHandler>();
    }

    protected virtual void Start()
    {

    }

    protected virtual void Update()
    {
        HandleAction();
        Rotate(lookDirection);
        HandleAttackDelay();
    }

    protected virtual void FixedUpdate()
    {
        Movment(movementDirection); //��� �����ϵ���
        if (knockbackDuration > 0.0f)
        {
            knockbackDuration -= Time.fixedDeltaTime; //�˹�Ǹ� �˹�ð� ����
        }
    }

    protected virtual void HandleAction()
    {

    }

    private void Movment(Vector2 direction)
    {
        //�̵� �ӵ� ����*���ȼӵ�
        direction = direction * statHandler.Speed;
        //�˹� �߿��� �̵��ӵ��� ���Ҵ��ϰ�, �˹� �������� �̵���
        if (knockbackDuration > 0.0f)
        {
            direction *= 0.2f;
            direction += knockback;
        }

        //rigid, ���� ��ü�� �˹� ���� ����
        rigid.velocity = direction;
        animationHandler.Move(direction);
    }

    private void Rotate(Vector2 direction)
    {
        //��ź��Ʈ�����⼭��
        //��ź��Ʈ�� �������. �׷��ϱ�, �ٴڿ� �ִ� x�� ���� ������ ������ ���
        float rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        bool isLeft = Mathf.Abs(rotZ) > 90f;

        //�̰� �¿����
        characterRenderer.flipX = isLeft;

        //WeaponPivot�� ������, ȸ��ó�� (������ ���缭!)
        if (weaponPivot != null)
        {
            weaponPivot.rotation = Quaternion.Euler(0, 0, rotZ);
        }

        weaponHandler?.Rotate(isLeft);
    }

    public void ApplyKnockback(Transform other, float power, float duration)
    {
        knockbackDuration = duration;
        //������ �о
        //���� ����. normalized = ������ ũ�⸦ 1�� �������(��, �ʿ��Ѱ� ���� ��)
        knockback = -(other.position - transform.position).normalized * power;
    }

    private void HandleAttackDelay()
    {
        if(weaponHandler == null) return;

        if (timeSinceLastAttack <= weaponHandler.Delay)
        {
            timeSinceLastAttack += Time.deltaTime;
        }

        if (isAttacking && timeSinceLastAttack > weaponHandler.Delay)
        {
            timeSinceLastAttack = 0;
            Attack();
        }

    }
    protected virtual void Attack()
    {
        if (lookDirection != Vector2.zero)
            weaponHandler?.Attack();
    }
}
