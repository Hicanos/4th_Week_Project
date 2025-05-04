using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInteract : InteractionManager
{
    bool isOpen = false;
    public GameObject Door;
    public Player player;


    //��ȣ�ۿ� ������ ���� ���� ������, FŰ ���� �� �ֵ��� Ȱ��ȭ - Player�� �Ҹ��� CanInteract = true��� ��ȣ�ۿ� ���� ����
    //���� ������ Indoor�� �ݶ��̴��� Trigger�� ����
    //���� ������ �ִϸ��̼ǵ� �۵��ؾ���



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
                //���� ���� �ִϸ��̼� �۵�

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
                Debug.LogWarning(Door.name + "�ݶ��̴� ������Ʈ�� ã�� �� �����ϴ�.");
            }
        }
        else
        {
            Debug.LogWarning("��� ������Ʈ�� ã�� �� �����ϴ�.");
        }
    }
}

