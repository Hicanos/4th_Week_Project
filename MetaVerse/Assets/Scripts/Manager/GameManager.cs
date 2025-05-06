using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    // 다음 씬에서 플레이어가 스폰될 지점의 ID를 저장 (예: 포탈 이름 등)
    [HideInInspector] // 인스펙터에 노출되지 않도록 숨김
    public string nextSpawnPointID = "";

    // 이전에 있었던 씬의 이름 (돌아갈 때 활용)
    [HideInInspector]
    public string previousSceneName = "";

    [HideInInspector] // 게임 데이터로 관리될 캐릭터 이름
    public string characterName = "Player"; // 기본값

    public GameObject playerPrefab;


    //미니게임 관련 설정
    [HideInInspector] // 게임 데이터로 관리될 미니게임 최고 점수
    public int highScore = 0;

    // 현재 플레이 중인 미니게임 점수 (임시 저장)
    [HideInInspector]
    public int currentMiniGameScore = 0;


    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); //파괴 금지 처리
                                    
            //씬을 로드할때마다 OnSceneLoaded 호출
            SceneManager.sceneLoaded += OnSceneLoaded;

        }
        else
        {
            // 이미 다른 인스턴스가 있다면 새로 생성된 오브젝트는 파괴
            Destroy(gameObject);
        }
    }

    void OnDestroy()
    {
        // 오브젝트가 파괴되면 이벤트 등록 해제
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // 씬이 로드될 때마다 호출되는 메서드
    // 해당 씬에 캐릭트 프리펩을 생성해야함.
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"[GameManager] 씬 로드 완료: {scene.name}");

        //현재 씬에 플레이어가 있는지 확인 (DontDestroyOnLoad로 넘어온 플레이어 포함)
        PlayerInteraction existingPlayer = FindObjectOfType<PlayerInteraction>();

        if (scene.name == "StartScene") return; //씬 이름이 StartScene이면 생성금지

        //존재 플레이어==null이고 프리펩은 존재한다면 >생성
        if (existingPlayer == null && playerPrefab != null)
        {
            Debug.Log("[GameManager] 씬 진입. 플레이어 생성 시작.");

            // GameManager의 playerPrefab을 사용하여 플레이어 인스턴스 생성
            GameObject playerGO = Instantiate(playerPrefab);

            // 생성된 플레이어 오브젝트의 위치를 초기 스폰 위치로 설정
            // 스폰지점 ID를 가지고 와서 넘김

            // 생성된 플레이어에 캐릭터 이름 설정
            SetNameOnPlayer(playerGO, characterName);

            //메인카메라 태그로 찾기
            GameObject mainCameraGO = GameObject.FindGameObjectWithTag("MainCamera");

            if (mainCameraGO != null)
            {
                FollowCamera followCameraScript = mainCameraGO.GetComponent<FollowCamera>();
                if (followCameraScript != null)
                {
                    // 찾은 FollowCamera 스크립트의 SetTarget 메서드를 호출하여 생성된 플레이어의 Transform 할당
                    followCameraScript.SetTarget(playerGO.transform);
                    Debug.Log("[GameManager] 생성된 플레이어 오브젝트를 FollowCamera의 타겟으로 설정했습니다.");
                }
                else
                {
                    Debug.LogWarning("[GameManager] 메인 카메라 오브젝트에 FollowCamera 스크립트가 붙어있지 않습니다.");
                }
            }
            else
            {
                Debug.LogWarning("[GameManager] 'MainCamera' 태그를 가진 카메라 오브젝트를 씬에서 찾을 수 없습니다.");

            }
        }
        else if (existingPlayer != null)
        {
            Debug.Log("[GameManager] 씬 진입. 플레이어 오브젝트가 이미 존재합니다.");
            // 정상적인 플레이라면 존재할 수 없음.
            // 만약을 위해서(...) 설정
            SetNameOnPlayer(existingPlayer.gameObject, characterName);
        }

    }

    public void SetCharacterName(string name)
    {
        characterName = name;
        Debug.Log($"[GameManager] 캐릭터 이름 설정됨: {characterName}");
    }

    // 플레이어 오브젝트에 이름 표시
    private void SetNameOnPlayer(GameObject playerObject, string name)
    {

        Transform nameTextTransform = playerObject.transform.Find("NameCanvas/PlayerName");
        TMPro.TMP_Text nameText = null;

        //입력창에 입력하면.
        if (nameTextTransform != null)
        {
            nameText = nameTextTransform.GetComponent<TMPro.TMP_Text>();
        }


        if (nameText != null)
        {
            nameText.text = name;
            Debug.Log($"[GameManager] 플레이어 오브젝트에 이름 '{name}' 표시.");
        }
        else
        {
            Debug.LogWarning($"[GameManager] 플레이어 Prefab에서 이름 표시용 TMP Text 컴포넌트를 찾을 수 없습니다! 이름 표시 불가.");
            Debug.LogWarning($"경로 확인 필요: playerObject.transform.Find(\"NameCanvas / PlayerName\")");
        }
    }


    public void TransitionToScene(string sceneName, string spawnPointID)
    {
        // 이전 씬 이름 저장
        previousSceneName = SceneManager.GetActiveScene().name;

        // 다음 씬에서 사용될 스폰 지점 ID 저장
        nextSpawnPointID = spawnPointID;

        Debug.Log($"씬 전환 요청: {previousSceneName} -> {sceneName}, 다음 스폰 지점 ID: {nextSpawnPointID}");

        // 씬 로드 실행
        SceneManager.LoadScene(sceneName);
    }

    // 플레이어가 스폰된 후 호출하여 정보 초기화 (플레이어 스크립트에서 호출)
    public void ClearSpawnPoint()
    {
        nextSpawnPointID = "";
        // previousSceneName은 돌아갈 때 필요할 수 있음
    }

}
