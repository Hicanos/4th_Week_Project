using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DoorInteract : InteractionManager
{
    bool isOpen = false;
    public Collider2D Door;
    public GameObject Collider;
    public Player player;


    //상호작용 가능한 것이 옆에 있으면, F키 누를 수 있도록 활성화 - Player의 불리언 CanInteract = true라면 상호작용 가능 상태
    //문이 열리면 Indoor의 콜라이더를 Trigger로 변경
    //문이 열리면 애니메이션도 작동해야함
    //현재: 애니메이터- 작동함
    //문제: 콜라이더 접촉은 됨. 지금 F눌렀을 때 Interact가 제대로 안 먹힘



    [SerializeField]
    protected Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        Door = GetComponent<Collider2D>();
    }
    public override void Interact()
    {
        Debug.Log("문 열렸니?");
        Collider2D DoorCollider = Collider.GetComponent<Collider2D>();
        //콜라이더가 존재하고 열려있으면 트리거를 켜고, 콜라이더가 없고 닫혀있으면 콜라이더를 끈다.

        if (DoorCollider != null && isOpen == false) //닫힌 문을 열어야함
        {
            isOpen = true;
            animator.SetBool("IsOpen", true);
            DoorCollider.isTrigger = true;
            Debug.Log("문 열었다");
            //문을 여는 애니메이션 작동

        }
        else if (isOpen == true) //열린 문 닫아야함
        {
            isOpen = false;
            animator.SetBool("IsOpen", false);
            DoorCollider.isTrigger = false;
            //문을 닫는 애니메이션 작동
            Debug.Log("문 닫앗다");
        }
        else
        {
            Debug.LogWarning(Collider.name + "콜라이더 컴포넌트를 찾을 수 없습니다.");

        }
    }






}

