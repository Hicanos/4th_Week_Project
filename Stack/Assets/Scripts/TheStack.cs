using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class TheStack : MonoBehaviour
{
    //��� ������ / �̵��ϴ� �� / ������ �ö󰡴� �ӵ� / ��� �̵� �ӵ� / �������� ����� ��������
    private const float BoundSize = 3.5f;
    private const float MovingBoundsSize = 3f;
    private const float StackMovingSpeed = 5.0f;
    private const float BlockMovingSpeed = 3.5f;
    private const float ErrorMargin = 0.1f;

    public GameObject originBlock = null;

    private Vector3 prevBlockPosition; //���� ��� ��ġ
    private Vector3 desiredPosition; //������ ��ǥ ��ġ
    private Vector3 stackBounds = new Vector2(BoundSize, BoundSize); //���� ��� �ʺ�

    //���ο� ����� ����� ���� ����
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

        //���ھ� �� �޺� ����
        bestScore = PlayerPrefs.GetInt(BestScoreKey, 0);
        bestCombo = PlayerPrefs.GetInt(BestComboKey, 0);

        prevColor = GetRandomColor();
        nextColor = GetRandomColor();

        prevBlockPosition = Vector3.down;
        Spawn_Block(); //�̰� ó�� ���õǴ� ��
        Spawn_Block(); //�̰� �� ������ ���õǴ� �����̴� ��
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
            {   //���� ���� ó��
                UpdateScore();
                isGameOver = true;
                GameOverEffect();
                UIManager.Instance.SetScoreUI();
            }
            
        }

        MoveBlock();

        //Lerp - �������� (���۰�-���� ������ ���� t����(0~1)�� ����Ͽ� ��ȯ)
        //Lerp(������ġ, ������ġ, �������ۼ�������)
        transform.position = Vector3.Lerp(
                             transform.position,
                             desiredPosition,
                             StackMovingSpeed * Time.deltaTime
                             );
    }

    bool Spawn_Block()
    {
        //���� �� ����
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
        newTrans.parent = this.transform; //���� ���� ����� �θ��� Transform�� ����� Transform���� ������
        newTrans.localPosition = prevBlockPosition + Vector3.up; //y�� scale�� 1�̱� ������ up�� �ᵵ �ٷ� ���� �ö�
        newTrans.localRotation = Quaternion.identity; //ȸ���� ���� �ʱⰪ
        newTrans.localScale = new Vector3(stackBounds.x, 1, stackBounds.y);//���� ���� ���� ����� ���� localscale ����

        stackCount++;

        desiredPosition = Vector3.down * stackCount;
        blockTransition = 0f;

        lastBlock = newTrans;

        isMovingX = !isMovingX;

        UIManager.Instance.UpdateScore();

        return true;        
    }

    Color GetRandomColor()
    {   //100~250���� ������ (100�̸��� �ʹ� ��ο� ���̶� ����)
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

        //���׸��� �� �÷����� �ٲ� (�����̾�-����, �÷� ���� ���Ե�)
        rn.material.color = applyColor;
        Camera.main.backgroundColor = applyColor-new Color(0.1f,0.1f,0.1f);

        //�߰��� �÷��� ���� �÷��� ���ٸ�, ���� �÷��� �����ϰ� �ٲ������
        if (applyColor.Equals(nextColor) == true)
        {
            prevColor = nextColor;
            nextColor = GetRandomColor();
        }

    }

    void MoveBlock()
    {
        blockTransition += Time.deltaTime * BlockMovingSpeed;


        //���� ? : 0���� �츮�� ������ ��������� ��ȯ�ϴ� ��, sin�� ����ϸ� �ϸ��� ��� ����������, PingPong�� ����� �����ϴٴ� ���� ����
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
            //���� ���� �߽� x�� - ������ �������� �߽� x�� = �߷������� �ϴ� ũ��
            float deltaX = prevBlockPosition.x - lastPosition.x;

            //���� ������ ��𿡼� ���������� ���ϱ�
            bool isNegativeNum = (deltaX < 0) ? true : false;

            deltaX = Mathf.Abs(deltaX); //Abs: ���밪
            
            if(deltaX > ErrorMargin)
            {
                stackBounds.x -= deltaX;
                if(stackBounds.x <= 0)
                {
                    return false;
                }

                //���� ����� ������x - �ֱ�������x / 2 = �߽ɰ�
                float middle = (prevBlockPosition.x - lastPosition.x) / 2f;
                lastBlock.localScale = new Vector3(stackBounds.x, 1, stackBounds.y);

                Vector3 tempPosition = lastBlock.localPosition;
                tempPosition.x = middle;
                lastBlock.localPosition = lastPosition = tempPosition;

                //������ �߽���
                float rubbleHalfScale = deltaX / 2f;

                //���� ������
                CreateRubble(new Vector3(
                    isNegativeNum
                    ? lastPosition.x + stackBounds.x / 2 + rubbleHalfScale
                    : lastPosition.x - stackBounds.x / 2 - rubbleHalfScale
                    , lastPosition.y
                    , lastPosition.z
                    ),
                    new Vector3(deltaX, 1, stackBounds.y)
                    );

                //deltaX�� errorMagin���� ũ�� = ��Ȯ���� �ʴ� = �޺� break
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

        //�ι�° ������ =>���� ���� ��ġ�� ��� �ٲ�� ������ ��ġ�� �ľ��ϰ� �ش� x, z���� ������.
        secondaryPosition = (isMovingX)? lastBlock.localPosition.x : lastBlock.localPosition.z;

        return true;
    }

    //���� ����
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


    //ó�� ���۰� ������ ������ ���·� �籸�� 
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
