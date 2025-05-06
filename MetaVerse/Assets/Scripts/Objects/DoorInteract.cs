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


    //��ȣ�ۿ� ������ ���� ���� ������, FŰ ���� �� �ֵ��� Ȱ��ȭ - Player�� �Ҹ��� CanInteract = true��� ��ȣ�ۿ� ���� ����
    //���� ������ Indoor�� �ݶ��̴��� Trigger�� ����
    //���� ������ �ִϸ��̼ǵ� �۵��ؾ���
    //����: �ִϸ�����- �۵���
    //����: �ݶ��̴� ������ ��. ���� F������ �� Interact�� ����� �� ����



    [SerializeField]
    protected Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        Door = GetComponent<Collider2D>();
    }
    public override void Interact()
    {
        Debug.Log("�� ���ȴ�?");
        Collider2D DoorCollider = Collider.GetComponent<Collider2D>();
        //�ݶ��̴��� �����ϰ� ���������� Ʈ���Ÿ� �Ѱ�, �ݶ��̴��� ���� ���������� �ݶ��̴��� ����.

        if (DoorCollider != null && isOpen == false) //���� ���� �������
        {
            isOpen = true;
            animator.SetBool("IsOpen", true);
            DoorCollider.isTrigger = true;
            Debug.Log("�� ������");
            //���� ���� �ִϸ��̼� �۵�

        }
        else if (isOpen == true) //���� �� �ݾƾ���
        {
            isOpen = false;
            animator.SetBool("IsOpen", false);
            DoorCollider.isTrigger = false;
            //���� �ݴ� �ִϸ��̼� �۵�
            Debug.Log("�� �ݾѴ�");
        }
        else
        {
            Debug.LogWarning(Collider.name + "�ݶ��̴� ������Ʈ�� ã�� �� �����ϴ�.");

        }
    }






}

