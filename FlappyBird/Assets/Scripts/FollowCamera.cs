using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform target;
    float offsetX;
    // Start is called before the first frame update
    void Start()
    {
        if (target == null) return;

        offsetX = transform.position.x - target.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        if(target == null) return;

        //transform.position은 직접 넣을 수 없으므로 pos처럼 한 번 저장해야함
        Vector3 pos = transform.position;
        pos.x = target.position.x + offsetX;

        //그러니 변조 후 다시 넣는 작업도 필요
        transform.position = pos;
    }
}
