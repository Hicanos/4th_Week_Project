using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potal : MonoBehaviour
{
    [Header("이동할 씬")]
    public string targetSceneName;

    // 해당 포탈 이름 넣어둘 곳
    [Header("다음 씬에서 스폰될 지점의 ID")]    
    public string targetSpawnPointID;

    //다시 되돌아올 포인트
    [Header("이 지점의 고유 ID")]
    public string thisPointID;

    //외부 스크립트에서 이 지점의 정보를 가져갈 수 있도록 속성 추가
    public string GetTargetSceneName()
    {
        return targetSceneName;
    }

    public string GetTargetSpawnPointID()
    {
        return targetSpawnPointID;
    }

    public string GetThisPointID()
    {
        return thisPointID;
    }

    // 필요하다면 플레이어가 이 지점에 닿거나 상호작용했을 때 호출될 메서드 추가
    // 예: InteractionHandler에서 F키 눌렀을 때 이 메서드를 호출하도록
    public void ActivateTransition()
    {
        if (GameManager.Instance != null)
        {
            // GameManager를 통해 씬 전환 요청
            GameManager.Instance.TransitionToScene(targetSceneName, targetSpawnPointID);
            Debug.Log("게임매니저를 이용해 씬이 변경됨");
        }
        else
        {
            Debug.LogError("GameManager 인스턴스를 찾을 수 없습니다. 씬에 GameManager 오브젝트가 있나요?");
        }
    }
}
