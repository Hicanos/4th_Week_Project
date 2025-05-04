using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : InteractionManager
{
    public bool CanInteract;

    //�±װ� ������ ������Ʈ�� ��ȣ�ۿ� ������ ������Ʈ��� = CanInteract true
    private GameObject interactObj { get; set; }

    public override void Interact()
    {

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //���ӿ�����Ʈ�� �±װ� Interact���, ��ȣ�ۿ� ����.
        if (collision.CompareTag("Interact"))
        {
            CanInteract = true;            
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        CanInteract = false;
    }
}
