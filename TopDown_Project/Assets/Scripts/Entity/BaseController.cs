using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;

//모든 캐릭터의 기본 움직임 관리
public class BaseController : MonoBehaviour
{
    protected Rigidbody2D rigid;
    protected AnimationHandler animationHandler;
    protected StatHandler statHandler;

    //SerializeField = 직렬화, 즉: 개체의 상태를 나중에 저장, 전송, 재구성할 수 있는 방식으로 변환
    //private에 사용하는 것으로 유니티의 인스펙터 창에 노출
    [SerializeField] private SpriteRenderer characterRenderer; //스프라이트 좌우반전 준비
    [SerializeField] private Transform weaponPivot; //무기를 위치시킬 기준 위치



    //이동 방향
    protected Vector2 movementDirection = Vector2.zero; 
    public Vector2 MovementDirection { get { return movementDirection; } }


    //바라보는 방향
    protected Vector2 lookDirection = Vector2.zero;
    public Vector2 LookDirection { get { return lookDirection; } }

    //넉백
    private Vector2 knockback = Vector2.zero; //넉백 방향
    private float knockbackDuration = 0.0f; //넉백 지속 시간

    [SerializeField] public WeaponHandler WeaponPrefab;
    protected WeaponHandler weaponHandler;

    protected bool isAttacking;
    private float timeSinceLastAttack = float.MaxValue;

    //가상메서드로 제작-Player Controller에서 overrides
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
        Movment(movementDirection); //계속 동작하도록
        if (knockbackDuration > 0.0f)
        {
            knockbackDuration -= Time.fixedDeltaTime; //넉백되면 넉백시간 감소
        }
    }

    protected virtual void HandleAction()
    {

    }

    private void Movment(Vector2 direction)
    {
        //이동 속도 방향*스탯속도
        direction = direction * statHandler.Speed;
        //넉백 중에는 이동속도가 감소당하고, 넉백 방향으로 이동함
        if (knockbackDuration > 0.0f)
        {
            direction *= 0.2f;
            direction += knockback;
        }

        //rigid, 실제 객체에 넉백 방향 적용
        rigid.velocity = direction;
        animationHandler.Move(direction);
    }

    private void Rotate(Vector2 direction)
    {
        //역탄젠트가여기서왜
        //역탄젠트로 각도계산. 그러니까, 바닥에 있는 x와 빗변 사이의 각도를 계산
        float rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        bool isLeft = Mathf.Abs(rotZ) > 90f;

        //이건 좌우반전
        characterRenderer.flipX = isLeft;

        //WeaponPivot이 있으면, 회전처리 (각도에 맞춰서!)
        if (weaponPivot != null)
        {
            weaponPivot.rotation = Quaternion.Euler(0, 0, rotZ);
        }

        weaponHandler?.Rotate(isLeft);
    }

    public void ApplyKnockback(Transform other, float power, float duration)
    {
        knockbackDuration = duration;
        //상대방을 밀어냄
        //벡터 빼기. normalized = 벡터의 크기를 1로 만들어줌(즉, 필요한건 방향 뿐)
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
