using System.Collections;
using System.Collections.Generic;
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

    private Vector3 prevBlockPostion; //���� ��� ��ġ
    private Vector3 desiredPosition; //������ ��ǥ ��ġ
    private Vector3 stackBounds = new Vector2(BoundSize, BoundSize); //���� ��� �ʺ�

    //���ο� ����� ����� ���� ����
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

        //Lerp - �������� (���۰�-���� ������ ���� t����(0~1)�� ����Ͽ� ��ȯ)
        //Lerp(������ġ, ������ġ, �������ۼ�������)
        transform.position = Vector3.Lerp(transform.position, desiredPosition, StackMovingSpeed * Time.deltaTime);
    }

    bool Spawn_Block()
    {
        //���� �� ����
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
        newTrans.parent = this.transform; //���� ���� ����� �θ��� Transform�� ����� Transform���� ������
        newTrans.localPosition = prevBlockPostion + Vector3.up; //y�� scale�� 1�̱� ������ up�� �ᵵ �ٷ� ���� �ö�
        newTrans.localRotation = Quaternion.identity; //ȸ���� ���� �ʱⰪ
        newTrans.localScale = new Vector3(stackBounds.x, 1, stackBounds.y);//���� ���� ���� ����� ���� localscale ����

        stackCount++;

        desiredPosition = Vector3.down * stackCount;
        blockTransition = 0f;

        lastBlock = newTrans;

        isMovingX = !isMovingX;

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
}
