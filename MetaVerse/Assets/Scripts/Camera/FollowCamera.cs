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
            //transform.position은 직접 넣을 수 없으므로 pos처럼 한 번 저장해야함
            Vector3 pos = transform.position;

            //플레이어와 간격 유지하며 움직인다.
            pos.x = target.position.x + offsetX;
            pos.y = target.position.y + offsetY;

            //가장 맵이 넓은 메인화면 내의 최대치 설정
            pos.x= Mathf.Clamp(pos.x, -2, 15);
            pos.y = Mathf.Clamp(pos.y, -10, 5);

            //그러니 변조 후 다시 넣는 작업도 필요
            transform.position = pos;
        }            
    }


    //Awake와 Start에서도 찾지 못했으므로 SetTarget을 이용해 GameManager에서 세팅
    public void SetTarget(Transform newTarget)
    {
        if (newTarget != null)
        {
            target = newTarget;
            isReadyToFollow = true;

            offsetX = transform.position.x - target.position.x;
            offsetY = transform.position.y - target.position.y;

            Debug.Log($"[FollowCamera] 외부에서 카메라 타겟이 '{newTarget.gameObject.name}'으로 설정되었습니다.");
            Debug.Log($"[FollowCamera] 오프셋 계산 완료: offsetX={offsetX}, offsetY={offsetY}");
        }
        else
        {
            target = null;
            isReadyToFollow = false;
            Debug.LogWarning("[FollowCamera] 외부에서 카메라 타겟이 null로 설정되었습니다. 카메라가 멈춥니다.");
        }
    }

}