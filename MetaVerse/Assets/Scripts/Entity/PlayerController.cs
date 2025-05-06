using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : BaseController
{
    private Camera camera;
    private bool IsLeft;


    //ī�޶�: 4f~8f���� ����- �ܾƿ�. �� �̻��� clamp.  
    //Math.Lerp�� �ε巴��.
    //���� �Ϸ��� �߾��µ� �����ε�

    protected override void Start()
    {
        base.Start();
        camera = Camera.main;
    }

    protected override void HandleAction()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        movementDirection = new Vector2(horizontal, vertical).normalized;

        //���� ������ ���� �ִٸ�, �ø�.  

        if (movementDirection.x < 0)
        {
            IsLeft = true;
            characterRenderer.flipX = true;  
        }
        else if (movementDirection.x > 0)
        {
            IsLeft = false;
            characterRenderer.flipX = false; 
        }
    }
}
