using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform target;
    float offsetX;
    float offsetY;

    void Start()
    {
        if (target == null) return;

        //ī�޶� ������-Ÿ��(�÷��̾�)�� ������ ��ġ �����
        offsetX = transform.position.x - target.position.x;
        offsetY = transform.position.y - target.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null) return;

        //transform.position�� ���� ���� �� �����Ƿ� posó�� �� �� �����ؾ���
        Vector3 pos = transform.position;

        //�÷��̾�� ���� �����ϸ� �����δ�.
        pos.x = target.position.x + offsetX;
        pos.y = target.position.y + offsetY;

        //�׷��� ���� �� �ٽ� �ִ� �۾��� �ʿ�
        transform.position = pos;
    }
}
