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

        //카메라 포지션-타겟(플레이어)의 포지션 위치 잡아줌
        offsetX = transform.position.x - target.position.x;
        offsetY = transform.position.y - target.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null) return;

        //transform.position은 직접 넣을 수 없으므로 pos처럼 한 번 저장해야함
        Vector3 pos = transform.position;

        //플레이어와 간격 유지하며 움직인다.
        pos.x = target.position.x + offsetX;
        pos.y = target.position.y + offsetY;

        //그러니 변조 후 다시 넣는 작업도 필요
        transform.position = pos;
    }
}
