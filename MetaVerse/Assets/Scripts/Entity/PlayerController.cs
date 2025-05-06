using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : BaseController
{
    private Camera camera;
    private bool IsLeft;


    //카메라: 4f~8f까지 줌인- 줌아웃. 그 이상은 clamp.  
    //Math.Lerp로 부드럽게.
    //구현 하려고 했었는데 무리인듯

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

        //만약 왼쪽을 보고 있다면, 플립.  

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
