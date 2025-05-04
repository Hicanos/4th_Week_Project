using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInteract : InteractionManager
{
    bool isOpen = false;
    public GameObject Door;
    public Player player;


    //상호작용 가능한 것이 옆에 있으면, F키 누를 수 있도록 활성화 - Player의 불리언 CanInteract = true라면 상호작용 가능 상태
    //문이 열리면 Indoor의 콜라이더를 Trigger로 변경
    //문이 열리면 애니메이션도 작동해야함



    protected Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    public override void Interact()
    {
        if (player.CanInteract==true && Input.GetKeyDown(KeyCode.F))
        {
            if (isOpen == false)
            {
                isOpen = true;
                animator.SetBool("IsOpen", true);
                //문을 여는 애니메이션 작동

            }
            else if (isOpen == true)
            {
                isOpen = false;
                animator.SetBool("IsOpen", false);
            }
            ChangeColliderTrigger();
        }

    }

    void ChangeColliderTrigger()
    {
        if(Door != null)
        {
            Collider2D DoorCollider = Door.GetComponent<Collider2D>();
            if (DoorCollider != null && isOpen ==true)
            {
                DoorCollider.isTrigger = true;
            }
            else if (DoorCollider != null && isOpen ==false)
            {
                DoorCollider.isTrigger = false;
            }
            else
            {
                Debug.LogWarning(Door.name + "콜라이더 컴포넌트를 찾을 수 없습니다.");
            }
        }
        else
        {
            Debug.LogWarning("대상 오브젝트를 찾을 수 없습니다.");
        }
    }
}

