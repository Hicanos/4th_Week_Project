using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheStack : MonoBehaviour
{
    //블록 사이즈 / 이동하는 양 / 스피드 / 성공으로 취급할 오차범위
    private const float BoundSize = 3.5f;
    private const float MovingBoundsSize = 3f;
    private const float StackMovingSpeed = 5.0f;
    private const float BlockMovingSpeed = 3.5f;
    private const float ErrorMargin = 0.1f;

    public GameObject originBlock = null;
    private Vector3 prevBlockPostiion;
    private Vector3 desirePosition;
    private Vector3 stackBounds = new Vector2(BoundSize, BoundSize);

    //새로운 블록을 만들기 위한 변수
    Transform lastBlock = null;
    float blockTransition = 0f;
    float secondaryPosition = 0f;

    int stackCount = -1;
    int comboCount = 0;

    void Start()
    {
        if(originBlock == null)
        {
            Debug.Log("OriginBlock is Null");
            return;
        }

        prevBlockPostiion = Vector3.down;
        Spawn_Block();
    }

    void Update()
    {
        if(Input.GetMouseButton(0))
        {
            Spawn_Block();
        }

        transform.position = Vector3.Lerp(transform.position, desirePosition, StackMovingSpeed * Time.deltaTime);
    }

    bool Spawn_Block()
    {
        if(lastBlock != null)
           prevBlockPostiion = lastBlock.localPosition;

        GameObject newBlock = null;
        Transform newTrans = null;

        newBlock = Instantiate(originBlock);

        if(newBlock == null)
        {
            Debug.Log("NewBlock Instantiate Failed");
            return false;
        }

        newTrans = newBlock.transform;
        newTrans.parent = this.transform; //새로 생긴 블록의 부모의 Transform을 블록의 Transform으로 가져옴
        newTrans.localPosition = prevBlockPostiion + Vector3.up; //y값 scale이 1이기 때문에 up만 써도 바로 위로 올라감
        newTrans.localRotation = Quaternion.identity; //회전이 없는 초기값
        newTrans.localScale = new Vector3(stackBounds.x, 1, stackBounds.y);//새로 생긴 블럭의 사이즈에 맞춰 localscale 설정

        stackCount++;

        desirePosition = Vector3.down * stackCount;
        blockTransition = 0f;

        lastBlock = newTrans;

        return true;        
    }

}
