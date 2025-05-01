using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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

    private Vector3 prevBlockPosition; //이전 블록 위치
    private Vector3 desiredPosition; //스택의 목표 위치
    private Vector3 stackBounds = new Vector2(BoundSize, BoundSize); //현재 블록 너비

    //새로운 블록을 만들기 위한 변수
    Transform lastBlock = null;
    float blockTransition = 0f;
    float secondaryPosition = 0f;

    int stackCount = -1;
    public int Score { get { return stackCount; } }
    int comboCount = 0;
    public int ComboCount { get { return comboCount; } }

    public int maxCombo = 0;
    public int MaxCombo { get => maxCombo; }

    public Color prevColor;
    public Color nextColor;

    bool isMovingX = true;

    int bestScore = 0;
    public int BestScore { get => bestScore; }

    int bestCombo = 0;
    public int BestCombo { get => bestCombo; }

    private const string BestScoreKey = "BestScore";
    private const string BestComboKey = "BestCombo";

    private bool isGameOver = true;


    void Start()
    {
        if(originBlock == null)
        {
            Debug.Log("OriginBlock is Null");
            return;
        }

        //스코어 및 콤보 저장
        bestScore = PlayerPrefs.GetInt(BestScoreKey, 0);
        bestCombo = PlayerPrefs.GetInt(BestComboKey, 0);

        prevColor = GetRandomColor();
        nextColor = GetRandomColor();

        prevBlockPosition = Vector3.down;
        Spawn_Block(); //이건 처음 세팅되는 블럭
        Spawn_Block(); //이건 그 다음에 세팅되는 움직이는 블럭
    }

    void Update()
    {
        if(isGameOver == true) { return; }

        if(Input.GetMouseButtonDown(0))
        {
            if (PlaceBlock())
            {
                Spawn_Block();
            }
            else
            {   //게임 오버 처리
                UpdateScore();
                isGameOver = true;
                GameOverEffect();
                UIManager.Instance.SetScoreUI();
            }
            
        }

        MoveBlock();

        //Lerp - 선형보간 (시작값-끝값 사이의 값을 t비율(0~1)로 계산하여 반환)
        //Lerp(현재위치, 목적위치, 일정한퍼센테이지)
        transform.position = Vector3.Lerp(
                             transform.position,
                             desiredPosition,
                             StackMovingSpeed * Time.deltaTime
                             );
    }

    bool Spawn_Block()
    {
        //이전 블럭 저장
        if(lastBlock != null)
           prevBlockPosition = lastBlock.localPosition;

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
        newTrans.localPosition = prevBlockPosition + Vector3.up; //y값 scale이 1이기 때문에 up만 써도 바로 위로 올라감
        newTrans.localRotation = Quaternion.identity; //회전이 없는 초기값
        newTrans.localScale = new Vector3(stackBounds.x, 1, stackBounds.y);//새로 생긴 블럭의 사이즈에 맞춰 localscale 설정

        stackCount++;

        desiredPosition = Vector3.down * stackCount;
        blockTransition = 0f;

        lastBlock = newTrans;

        isMovingX = !isMovingX;

        UIManager.Instance.UpdateScore();

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

    bool PlaceBlock()
    {
        Vector3 lastPosition = lastBlock.localPosition;

        if (isMovingX)
        {
            //이전 블럭의 중심 x값 - 마지막 포지션의 중심 x값 = 잘려나가야 하는 크기
            float deltaX = prevBlockPosition.x - lastPosition.x;

            //남은 조각이 어디에서 떨어지는지 정하기
            bool isNegativeNum = (deltaX < 0) ? true : false;

            deltaX = Mathf.Abs(deltaX); //Abs: 절대값
            
            if(deltaX > ErrorMargin)
            {
                stackBounds.x -= deltaX;
                if(stackBounds.x <= 0)
                {
                    return false;
                }

                //이전 블록의 포지션x - 최근포지션x / 2 = 중심값
                float middle = (prevBlockPosition.x - lastPosition.x) / 2f;
                lastBlock.localScale = new Vector3(stackBounds.x, 1, stackBounds.y);

                Vector3 tempPosition = lastBlock.localPosition;
                tempPosition.x = middle;
                lastBlock.localPosition = lastPosition = tempPosition;

                //러블의 중심점
                float rubbleHalfScale = deltaX / 2f;

                //러블 생성기
                CreateRubble(new Vector3(
                    isNegativeNum
                    ? lastPosition.x + stackBounds.x / 2 + rubbleHalfScale
                    : lastPosition.x - stackBounds.x / 2 - rubbleHalfScale
                    , lastPosition.y
                    , lastPosition.z
                    ),
                    new Vector3(deltaX, 1, stackBounds.y)
                    );

                //deltaX가 errorMagin보다 크다 = 정확하지 않다 = 콤보 break
                comboCount = 0;
            }
            else
            {
                ComboCheck();
                lastBlock.localPosition = prevBlockPosition + Vector3.up;
            }

        }
        else
        {
            float deltaZ = prevBlockPosition.z - lastPosition.z;
            bool isNegativeNum = (deltaZ < 0) ? true: false;

            deltaZ = Mathf.Abs(deltaZ);
            if(deltaZ > ErrorMargin)
            {
                stackBounds.y-= deltaZ;
                if(stackBounds.y <= 0)
                {
                    return false;
                }

                float middle = (prevBlockPosition.z + lastPosition.z) / 2f;
                lastBlock.localScale = new Vector3(stackBounds.x, 1, stackBounds.y);

                Vector3 tempPosition = lastBlock.localPosition;
                tempPosition.z = middle;
                lastBlock.localPosition= lastPosition = tempPosition;

                float rubbleHalfScale = deltaZ / 2f;
                CreateRubble(
                    new Vector3(
                        lastPosition.x,
                        lastPosition.y,
                        isNegativeNum
                        ? lastPosition.z + stackBounds.y / 2 + rubbleHalfScale
                        : lastPosition.z - stackBounds.y / 2 - rubbleHalfScale),
                    new Vector3(stackBounds.x, 1, deltaZ)
                    );

                comboCount = 0;
            }
            else
            {
                lastBlock.localPosition = prevBlockPosition + Vector3.up;
                ComboCheck();
            }
        }

        //두번째 포지션 =>이전 블럭의 위치가 계속 바뀌기 때문에 위치를 파악하고 해당 x, z값을 저장함.
        secondaryPosition = (isMovingX)? lastBlock.localPosition.x : lastBlock.localPosition.z;

        return true;
    }

    //러블 생성
    void CreateRubble(Vector3 pos, Vector3 scale)
    {
        GameObject go = Instantiate(lastBlock.gameObject);
        go.transform.parent = this.transform;

        go.transform.localPosition = pos;
        go.transform.localScale = scale;
        go.transform.localRotation = Quaternion.identity;

        go.AddComponent<Rigidbody>();
        go.name = "Rubble";
    }

    void ComboCheck()
    {
        comboCount++;
        if(comboCount > maxCombo)
            maxCombo = comboCount;

        if( (comboCount % 5) == 0)
        {
            Debug.Log("5Combo!");
            stackBounds += new Vector3(0.5f, 0.5f);
            stackBounds.x=
                (stackBounds.x > BoundSize) ? BoundSize : stackBounds.x;
            stackBounds.y =
                (stackBounds.y > BoundSize) ? BoundSize : stackBounds.y;
        }
    }

    void UpdateScore()
    {
        if(bestScore < stackCount)
        {
            bestScore = stackCount;
            bestCombo = MaxCombo;

            PlayerPrefs.SetInt(BestScoreKey, bestScore);
            PlayerPrefs.SetInt(BestComboKey, bestCombo);
        }
    }

    void GameOverEffect()
    {
        int childCount =this.transform.childCount;

        for(int i = 1; i < 20; i++)
        {
            if (childCount < i) break;

            GameObject go = transform.GetChild(childCount - i).gameObject;

            if (go.name.Equals("Rubble")) continue;

            Rigidbody rigid = go.AddComponent<Rigidbody>();
            rigid.AddForce(
                (Vector3.up * Random.Range(0, 10f) + Vector3.right * (Random.Range(0, 10) - 5f)) *100f
                );
        }
    }


    //처음 시작과 완전히 동일한 형태로 재구성 
    public void Restart()
    {
        int childCount = transform.childCount;

        for(int i = 0; i < childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

        isGameOver = false;
        lastBlock = null;
        desiredPosition = Vector3.zero;
        stackBounds = new Vector3(BoundSize, BoundSize);

        stackCount = -1;
        isMovingX = true;
        blockTransition = 0f;
        secondaryPosition = 0f;

        comboCount = 0;
        maxCombo = 0;

        prevBlockPosition = Vector3.down;

        prevColor = GetRandomColor();
        nextColor = GetRandomColor();

        Spawn_Block();
        Spawn_Block();
    }

}
