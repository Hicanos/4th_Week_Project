using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // TMP_InputField 사용 시 필요
using UnityEngine.UI;

public class CharacterCreationManager : MonoBehaviour
{
    public TMP_InputField nameInputField; // 이름 입력 필드
    public Button createButton; // 캐릭터 생성 버튼

    public GameObject WarningMsg;

    // 메인 씬
    private const string MainSceneName = "MainScene"; 

    void Start()
    {
        // 버튼 클릭 이벤트 연결
        if (createButton != null)
        {
            createButton.onClick.AddListener(OnCreateButtonClick);
        }
        else
        {
            Debug.LogError("[CharacterCreationManager] Create Button이 연결되지 않았습니다!");
        }

        // InputField가 연결되었는지 확인
        if (nameInputField == null)
        {
            Debug.LogError("[CharacterCreationManager] Name Input Field가 연결되지 않았습니다!");
        }
    }

    void OnDestroy()
    {
        // 버튼 이벤트 연결 해제 (씬이 파괴될 때)
        if (createButton != null)
        {
            createButton.onClick.RemoveListener(OnCreateButtonClick);
        }
    }

    // 생성 버튼 클릭 시 호출될 메서드
    public void OnCreateButtonClick()
    {
        if (nameInputField == null) return;

        string characterName = nameInputField.text;

        // 입력된 이름이 비어있지 않은지 확인
        if (string.IsNullOrWhiteSpace(characterName))
        {
            Debug.LogWarning("[CharacterCreationManager] 캐릭터 이름을 입력해주세요!");
            WarningMsg.SetActive(true);

            return;
        }

        // GameManager 인스턴스가 있는지 확인하고 이름 저장
        if (GameManager.Instance != null)
        {
            GameManager.Instance.SetCharacterName(characterName);
            Debug.Log($"[CharacterCreationManager] GameManager에 이름 '{characterName}' 저장 완료. 메인 씬으로 이동합니다.");

            // 메인 씬으로 이동 요청-시작지점 스폰
            GameManager.Instance.TransitionToScene(MainSceneName, "SpawnPotal"); // 기본 스폰 지점 없음으로 전달

        }
        else
        {
            Debug.LogError("[CharacterCreationManager] GameManager 인스턴스를 찾을 수 없습니다! 씬 전환 불가.");
        }
    }
}
