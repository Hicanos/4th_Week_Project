using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : InteractionManager
{
    public bool CanInteract;

    //태그가 설정된 오브젝트가 상호작용 가능한 오브젝트라면 = CanInteract true
    private GameObject interactObj { get; set; }

    public override void Interact()
    {

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //게임오브젝트의 태그가 Interact라면, 상호작용 가능.
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
