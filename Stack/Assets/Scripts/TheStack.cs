using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheStack : MonoBehaviour
{
    //��� ������ / �̵��ϴ� �� / ���ǵ� / �������� ����� ��������
    private const float BoundSize = 3.5f;
    private const float MovingBoundsSize = 3f;
    private const float StackMovingSpeed = 5.0f;
    private const float BlockMovingSpeed = 3.5f;
    private const float ErrorMargin = 0.1f;

    public GameObject originBlock = null;
    private Vector3 prevBlockPostiion;
    private Vector3 desirePosition;
    private Vector3 stackBounds = new Vector2(BoundSize, BoundSize);

    //���ο� ����� ����� ���� ����
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
        newTrans.parent = this.transform; //���� ���� ����� �θ��� Transform�� ����� Transform���� ������
        newTrans.localPosition = prevBlockPostiion + Vector3.up; //y�� scale�� 1�̱� ������ up�� �ᵵ �ٷ� ���� �ö�
        newTrans.localRotation = Quaternion.identity; //ȸ���� ���� �ʱⰪ
        newTrans.localScale = new Vector3(stackBounds.x, 1, stackBounds.y);//���� ���� ���� ����� ���� localscale ����

        stackCount++;

        desirePosition = Vector3.down * stackCount;
        blockTransition = 0f;

        lastBlock = newTrans;

        return true;        
    }

}
