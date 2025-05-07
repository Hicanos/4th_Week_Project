using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MiniGameManager : MonoBehaviour
{
    static MiniGameManager miniGameManager;
    public static MiniGameManager Instance { get { return miniGameManager; } }

    private int currentScore = 0;

    UIManager uiManager;
    public UIManager UIManager { get { return uiManager; } }

    public enum MiniGameState { PlaneSelection, Playing, GameOver } // 미니게임 상태 열거
    private MiniGameState currentState; //현재 상태

    private string playerName; //캐릭터명-GameManager
    private GameObject characterVisualInstance = null; //생성된 이미지/이름만 가진 프리팹
    private PlanePlayer planePlayerScript; //PlanePlayer달린 Riding찾기

    public GameObject ridingObject;
    public Transform characterMountPoint; //ridingObject에서 플레이어캐릭터가 배치될 Transform

    private void Awake()
    {
        if (miniGameManager != null && miniGameManager != this)
        {
            Destroy(gameObject); // 중복 인스턴스 파괴
            return;
        }
        miniGameManager = this;
        uiManager = FindObjectOfType<UIManager>();

        if (ridingObject != null)
        {
            planePlayerScript = ridingObject.GetComponent<PlanePlayer>();
            if (planePlayerScript == null)
            {
                Debug.LogError($"[MiniGameManager] Riding Object '{ridingObject.name}'에 PlanePlayer 스크립트가 붙어있지 않음");
            }
        }
        else
        {
            Debug.LogError("[MiniGameManager] Riding Object가 연결되지않음");
        }

        // 미니게임 초기 상태 설정 (선택 화면)
        SetState(MiniGameState.PlaneSelection);
    }

    private void Start()
    {
        uiManager.UpdateScore(0);
    }

    private void Update()
    {
        if (currentState == MiniGameState.GameOver)
        {
            // 마우스 버튼 클릭 시 게임 재시작
            if (Input.GetMouseButtonDown(0))
            {
                RestartGame();
            }

            // 'B' 키 입력 시 메인 메뉴로 이동
            if (Input.GetKeyDown(KeyCode.B))
            {
                BackToMain();
            }
        }
    }

    public void GameOver()
    {
        uiManager.SetRestart();
    }

    public void RestartGame()
    {
        //지금 실행중인 씬의 이름을 가져와서 로드함
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void AddScore(int score)
    {
        currentScore += score;
        uiManager.UpdateScore(currentScore);
    }


    public void SetupMiniGame(string playerName, GameObject characterVisualPrefab)
    {
        this.playerName = playerName; // 게임 매니저에서 넘어오는 이름 저장

        // 캐릭터 시각 요소 Prefab 생성 배치
        if (characterVisualPrefab != null && ridingObject != null)
        {
            // 기존에 생성된 캐릭터 시각 요소 인스턴스가 있다면 파괴 (재시작 시 등)
            if (characterVisualInstance != null)
            {
                Destroy(characterVisualInstance);
            }

            // 캐릭터스프라이트 및 이름'만' 생성
            characterVisualInstance = Instantiate(characterVisualPrefab);

            // 캐릭터 오브젝트 배치 (Riding 의 자식으로)
            Transform mountPoint = characterMountPoint != null ? characterMountPoint : ridingObject.transform;
            characterVisualInstance.transform.SetParent(mountPoint, false); // 부모 설정 (worldPositionStays = false)
            characterVisualInstance.transform.localPosition = Vector3.zero; // 부모 로컬 위치 (0,0,0)로 이동

            // 캐릭터 이름 표시
            Transform nameTextTransform = characterVisualInstance.transform.Find("NameCanvas/PlayerName");
            TMPro.TMP_Text nameText = null;
            if (nameTextTransform != null)
            {
                nameText = nameTextTransform.GetComponent<TMPro.TMP_Text>();
            }
            if (nameText != null)
            {
                nameText.text = playerName;
                Debug.Log($"[MiniGameManager] 캐릭터 이름 '{playerName}' 표시.");
            }
            else
            {
                Debug.LogWarning("[MiniGameManager] 캐릭터 시각 요소 Prefab에서 이름 표시용 TMP Text 컴포넌트를 찾을 수 없음");
            }

            // PlanePlayer 스크립트에게 캐릭터 애니메이터 전달 (RidingObject에 붙어있는 PlanePlayer)
            Animator characterAnimator = characterVisualInstance.GetComponentInChildren<Animator>(); // 캐릭터 인스턴스에서 애니메이터 찾기
            if (planePlayerScript != null)
            {
                planePlayerScript.SetCharacterAnimator(characterAnimator); // PlanePlayer에 SetCharacterAnimator 메서드 추가
                Debug.Log("[MiniGameManager] PlanePlayer에게 캐릭터 애니메이터 전달.");
            }
            else
            {
                Debug.LogWarning("[MiniGameManager] PlanePlayer 스크립트가 없어 애니메이터를 전달 불가");
            }


            Debug.Log($"[MiniGameManager] 캐릭터 '{playerName}' 미니게임 준비 완료.");

        }
        else
        {
            Debug.LogError("[MiniGameManager] 캐릭터 시각 요소 Prefab 또는 Riding Object가 연결/전달되지 않음");
        }

        // 초기 상태는 비행기 선택 화면
        SetState(MiniGameState.PlaneSelection);
    }

    private void SetState(MiniGameState newState)
    {
        currentState = newState;
        Debug.Log($"[MiniGameManager] 미니게임 상태 변경: {currentState}");

        // 상태에 따른 UI 및 게임 로직 활성화/비활성화
        switch (currentState)
        {
            case MiniGameState.PlaneSelection:
                uiManager.SetPlaneSelectionUIActive(true); // 비행기 선택 UI 표시
                uiManager.SetGamePlayUIActive(false); // 게임 플레이 UI 숨김
                uiManager.SetBackToMainUIActive(false); // 복귀 UI 숨김
                                                        // RidingObject의 물리 비활성화 (선택 중엔 움직이지 않도록)
                if (ridingObject != null && ridingObject.GetComponent<Rigidbody2D>() != null)
                {
                    ridingObject.GetComponent<Rigidbody2D>().isKinematic = true; // 물리 영향 안 받도록
                    ridingObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero; // 속도 0
                }
                break;

            case MiniGameState.Playing:
                uiManager.SetPlaneSelectionUIActive(false); // 비행기 선택 UI 숨김
                uiManager.SetGamePlayUIActive(true); // 게임 플레이 UI 표시
                uiManager.SetBackToMainUIActive(false); // 복귀 UI 숨김
                // 게임 시작 로직 실행
                StartGame();
                break;

            case MiniGameState.GameOver:
                uiManager.SetRestart(); // 재시작 텍스트 표시
                uiManager.SetBackToMainUIActive(true); // 메인 씬 복귀 UI 표시 (또는 버튼)
                                                       // RidingObject의 물리 다시 활성화 (죽었을 때 떨어지도록)
                if (ridingObject != null && ridingObject.GetComponent<Rigidbody2D>() != null)
                {
                    ridingObject.GetComponent<Rigidbody2D>().isKinematic = false; // 물리 영향 다시 받도록
                }
                // 게임 오버 로직 실행 (PlanePlayer에서 GameOver를 호출함)
                break;
        }
    }

    private void StartGame()
    {
        currentScore = 0; // 점수 초기화
        uiManager.UpdateScore(currentScore); // 점수 UI 업데이트
        uiManager.SetRestart(); // 재시작 UI 숨김 (게임 오버 시 다시 나타남)
        uiManager.SetBackToMainUIActive(false); // 복귀 UI 숨김

    }

    // 비행기 선택 버튼 클릭 시 호출될 메서드 (UI 버튼 이벤트에 연결)
    public void SelectPlane(int type)
    {
        if (currentState != MiniGameState.PlaneSelection) return; // 선택 상태가 아니면 무시

        Debug.Log($"[MiniGameManager] 비행기 타입 선택됨: {type}");
        // 선택된 타입을 PlanePlayer 스크립트에 전달
        if (planePlayerScript != null)
        {
            planePlayerScript.SetPlaneAnimationType(type); // PlanePlayer에 SetPlaneAnimationType 메서드 추가 필요
        }
        else
        {
            Debug.LogWarning("[MiniGameManager] PlanePlayer 스크립트가 없습니다. 애니메이션 타입 설정 불가.");
        }

        // 상태를 Playing으로 변경하여 게임 시작
        SetState(MiniGameState.Playing);
    }

    private void BackToMain()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            GameManager.Instance.TransitionToScene("MainScene", "PlaneGame");

        }
    }

}