using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{


    private Transform target;
    float offsetX;
    float offsetY;

    private bool isReadyToFollow = false;

    void Update()
    {
        if (target == null) return;

        if (isReadyToFollow && target != null)
        {
            //transform.position�� ���� ���� �� �����Ƿ� posó�� �� �� �����ؾ���
            Vector3 pos = transform.position;

            //�÷��̾�� ���� �����ϸ� �����δ�.
            pos.x = target.position.x + offsetX;
            pos.y = target.position.y + offsetY;

            //���� ���� ���� ����ȭ�� ���� �ִ�ġ ����
            pos.x= Mathf.Clamp(pos.x, -2, 15);
            pos.y = Mathf.Clamp(pos.y, -10, 5);

            //�׷��� ���� �� �ٽ� �ִ� �۾��� �ʿ�
            transform.position = pos;
        }            
    }


    //Awake�� Start������ ã�� �������Ƿ� SetTarget�� �̿��� GameManager���� ����
    public void SetTarget(Transform newTarget)
    {
        if (newTarget != null)
        {
            target = newTarget;
            isReadyToFollow = true;

            offsetX = transform.position.x - target.position.x;
            offsetY = transform.position.y - target.position.y;

            Debug.Log($"[FollowCamera] �ܺο��� ī�޶� Ÿ���� '{newTarget.gameObject.name}'���� �����Ǿ����ϴ�.");
            Debug.Log($"[FollowCamera] ������ ��� �Ϸ�: offsetX={offsetX}, offsetY={offsetY}");
        }
        else
        {
            target = null;
            isReadyToFollow = false;
            Debug.LogWarning("[FollowCamera] �ܺο��� ī�޶� Ÿ���� null�� �����Ǿ����ϴ�. ī�޶� ����ϴ�.");
        }
    }

}