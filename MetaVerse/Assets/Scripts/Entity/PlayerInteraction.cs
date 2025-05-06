using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private GameObject currentInteractableObj = null;

    [SerializeField]
    private GameObject Popup;


    private void Start()
    {
        // 씬이 로드될 때 GameManager에 저장된 스폰 지점 정보 확인
        if (GameManager.Instance != null && !string.IsNullOrEmpty(GameManager.Instance.nextSpawnPointID))
        {
            Debug.Log($"GameManager에 저장된 스폰 지점 ID 발견: {GameManager.Instance.nextSpawnPointID}");

            // 저장된 ID와 일치하는 Potal을 씬에서 찾기
            Potal[] spawnPoints = FindObjectsOfType<Potal>();
            Potal targetSpawnPoint = null;

            foreach (Potal tp in spawnPoints)
            {
                // This Point ID가 GameManager에 저장된 ID와 일치하는지 확인
                if (tp.GetThisPointID() == GameManager.Instance.nextSpawnPointID)
                {
                    targetSpawnPoint = tp;
                    break;
                }
            }

            if (targetSpawnPoint != null)
            {
                // 찾은 스폰 지점 위치로 플레이어 이동
                transform.position = targetSpawnPoint.transform.position;
                Debug.Log($"플레이어를 스폰 지점 '{GameManager.Instance.nextSpawnPointID}' 위치로 이동했습니다.");

                // 스폰 정보 사용 완료 후 초기화
                GameManager.Instance.ClearSpawnPoint();
            }
            else
            {
                Debug.LogWarning($"ID '{GameManager.Instance.nextSpawnPointID}'를 가진 스폰 지점을 현재 씬에서 찾을 수 없습니다. 기본 위치에 스폰됩니다.");
                // 저장된 스폰 지점을 찾지 못했을 경우 (예: 씬에 해당 ID의 포탈이 없거나 첫 진입 씬),
                // 플레이어는 기본적으로 씬에 배치된 위치나 특정 초기 위치에 스폰됩니다.
                GameManager.Instance.ClearSpawnPoint(); // 정보는 초기화
            }
        }
        else
        {
            Debug.Log("GameManager에 저장된 스폰 지점 정보가 없습니다. 기본 위치에 스폰됩니다.");
            // GameManager 인스턴스가 없거나 nextSpawnPointID가 비어있을 경우
            // 플레이어를 기본적으로 씬에 배치된 위치나 특정 초기 위치에 스폰
        }
    }


    // Update 함수로 플레이어의 입력(F키)을 감지하고 상호작용을 실행
    void Update()
    {
        // 상호작용 가능한 오브젝트가 범위 내에 있고 (currentInteractableObject != null),
        // F 키를 눌렀는지 확인
        if (currentInteractableObj != null && Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log($"[상호작용 핸들러] F 키 입력 감지! 상호작용 대상: {currentInteractableObj.name}");            

            // 상호작용 대상 오브젝트에 어떤 컴포넌트(Potal, DoorInteract 등)가 붙어있는지 확인

            Potal potalComponent = currentInteractableObj.GetComponent<Potal>();
            DoorInteract doorInteractComponent = currentInteractableObj.GetComponent<DoorInteract>();

            // 어떤 종류의 상호작용 오브젝트인지 확인하고 해당 로직을 실행
            if (potalComponent != null)
            {
                // 씬 전환을 담당하는 Potal 컴포넌트가 있는 경우
                Debug.Log($"[상호작용 핸들러] -> 씬 전환 지점 '{currentInteractableObj.name}' 활성화.");
                // Potal 스크립트의 ActivateTransition() 메서드를 호출하여 씬 전환을 시작
                potalComponent.ActivateTransition();
            }
            else if (doorInteractComponent != null)
            {
                // 문과 상호작용하는 DoorInteract 컴포넌트가 있는 경우
                Debug.Log($"[상호작용 핸들러] -> 문 '{currentInteractableObj.name}' 상호작용 활성화.");
                // DoorInteract 스크립트의 상호작용 메서드를 호출
                // 문 열림 애니메이션, 충돌체 변경 등은 DoorInteract 스크립트 자체에서 처리
                doorInteractComponent.Interact();
            }
            else
            {
                // currentInteractableObject가 설정되었지만, 예상치 못한 컴포넌트만 있는 경우 (예외 처리)
                // 보통 상호작용 오브젝트엔 저 둘 중 하나가 붙어있음(둘 다 X)
                Debug.LogWarning($"[상호작용 핸들러] 상호작용 가능한 오브젝트 '{currentInteractableObj.name}'에 인식된 상호작용 컴포넌트(Potal, DoorInteract)가 없습니다.");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 진입한 오브젝트가 상호작용 가능한 컴포넌트(Potal 또는 DoorInteract)를 가지고 있는지 확인
        Potal potalComponent = collision.GetComponent<Potal>();
        DoorInteract doorInteractComponent = collision.GetComponent<DoorInteract>();

        // 진입한 오브젝트가 상호작용 가능하고, 현재 다른 상호작용 대상을 추적하고 있지 않다면,
        // =currentInteractableObject가 null인 상태

        if ((potalComponent != null || doorInteractComponent != null) && currentInteractableObj == null)
        {
            // 이 오브젝트를 현재 상호작용 대상으로 설정.
            currentInteractableObj = collision.gameObject;
            Debug.Log($"[상호작용 핸들러] 상호작용 범위 진입: {collision.name} (현재 대상: {currentInteractableObj.name})");
            Popup.SetActive(true);
            //F 버튼 누를 수 있는 팝업 보이게 함
        }
    }

    // 다른 오브젝트의 콜라이더(또는 트리거)가 이 오브젝트의 트리거 콜라이더에서 벗어났을 때 호출
    private void OnTriggerExit2D(Collider2D collision)
    {
        // 범위에서 벗어난 오브젝트가 현재 추적 중인 상호작용 대상과 동일한지 확인
        // 여러 개의 트리거가 겹쳐 있을 때,
        // 현재 대상 오브젝트가 아닌 다른 오브젝트가 먼저 나가더라도 currentInteractableObject가 해제되지 않도록 함
        if (collision.gameObject == currentInteractableObj)
        {
            // 현재 상호작용 대상을 해제합니다.
            currentInteractableObj = null;
            Debug.Log($"[상호작용 핸들러] 상호작용 범위 벗어남: {collision.name}");
            Popup.SetActive(false);
            
        }
        //벗어난 오브젝트가 현재 추적 대상이 아니라면 아무것도 하지 않음
    }
}
