using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheStack : MonoBehaviour
{
    //블록 사이즈 / 이동하는 양 / 스택이 올라가는 속도 / 블록 이동 속도 / 성공으로 취급할 오차범위
    private const float BoundSize = 3.5f;
    private const float MovingBoundsSize = 3f;
    private const float StackMovingSpeed = 5.0f;
    private const float BlockMovingSpeed = 3.5f;
    private const float ErrorMargin = 0.1f;

    public GameObject originBlock = null;

    private Vector3 prevBlockPostion; //이전 블록 위치
    private Vector3 desiredPosition; //스택의 목표 위치
    private Vector3 stackBounds = new Vector2(BoundSize, BoundSize); //현재 블록 너비

    //새로운 블록을 만들기 위한 변수
    Transform lastBlock = null;
    float blockTransition = 0f;
    float secondaryPosition = 0f;

    int stackCount = -1;
    int comboCount = 0;

    public Color prevColor;
    public Color nextColor;

    bool isMovingX = true;

    void Start()
    {
        if(originBlock == null)
        {
            Debug.Log("OriginBlock is Null");
            return;
        }
        prevColor = GetRandomColor();
        nextColor = GetRandomColor();

        prevBlockPostion = Vector3.down;
        Spawn_Block();
        Spawn_Block();
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Spawn_Block();
        }

        MoveBlock();

        //Lerp - 선형보간 (시작값-끝값 사이의 값을 t비율(0~1)로 계산하여 반환)
        //Lerp(현재위치, 목적위치, 일정한퍼센테이지)
        transform.position = Vector3.Lerp(transform.position, desiredPosition, StackMovingSpeed * Time.deltaTime);
    }

    bool Spawn_Block()
    {
        //이전 블럭 저장
        if(lastBlock != null)
           prevBlockPostion = lastBlock.localPosition;

        GameObject newBlock = null;
        Transform newTrans = null;

        newBlock = Instantiate(originBlock);

        if(newBlock == null)
        {
            Debug.Log("NewBlock Instantiate Failed");
            return false;
        }

        ColorChange(newBlock);

        newTrans = newBlock.transform;
        newTrans.parent = this.transform; //새로 생긴 블록의 부모의 Transform을 블록의 Transform으로 가져옴
        newTrans.localPosition = prevBlockPostion + Vector3.up; //y값 scale이 1이기 때문에 up만 써도 바로 위로 올라감
        newTrans.localRotation = Quaternion.identity; //회전이 없는 초기값
        newTrans.localScale = new Vector3(stackBounds.x, 1, stackBounds.y);//새로 생긴 블럭의 사이즈에 맞춰 localscale 설정

        stackCount++;

        desiredPosition = Vector3.down * stackCount;
        blockTransition = 0f;

        lastBlock = newTrans;

        isMovingX = !isMovingX;

        return true;        
    }

    Color GetRandomColor()
    {   //100~250까지 랜덤값 (100미만은 너무 어두운 색이라 제외)
        float r = Random.Range(100f, 250f) / 255f;
        float g = Random.Range(100f, 250f) / 255f;
        float b = Random.Range(100f, 250f) / 255f;

        return new Color(r, g, b);
    }

    void ColorChange(GameObject go)
    {
        Color applyColor = Color.Lerp(prevColor, nextColor, (stackCount % 11) / 10f);
        
        Renderer rn = go.GetComponent<Renderer>();

        if(rn == null)
        {
            Debug.Log("Renderer is Null");
            return;
        }

        //마테리얼 중 컬러값을 바꿈 (마테이얼-재질, 컬러 등이 포함됨)
        rn.material.color = applyColor;
        Camera.main.backgroundColor = applyColor-new Color(0.1f,0.1f,0.1f);

        //추가될 컬러가 다음 컬러와 같다면, 다음 컬러를 랜덤하게 바꿔줘야함
        if (applyColor.Equals(nextColor) == true)
        {
            prevColor = nextColor;
            nextColor = GetRandomColor();
        }

    }

    void MoveBlock()
    {
        blockTransition += Time.deltaTime * BlockMovingSpeed;


        //핑퐁 ? : 0부터 우리가 지정한 사이즈까지 순환하는 값, sin을 사용하면 완만한 곡선이 가능하지만, PingPong은 양수만 가능하다는 점에 주의
        float movePosition = Mathf.PingPong(blockTransition, BoundSize)-BoundSize/2;

        if (isMovingX)
        {
            lastBlock.localPosition = new Vector3(
                movePosition * MovingBoundsSize, stackCount, secondaryPosition);
        }
        else
        {
            lastBlock.localPosition = new Vector3(
                secondaryPosition, stackCount, -movePosition * MovingBoundsSize);
        }
    }
}
