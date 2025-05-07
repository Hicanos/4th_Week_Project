using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI restartText;
    public TextMeshProUGUI backMainText;
    // Start is called before the first frame update

    public GameObject planeSelectionPanel; // 비행기 선택 UI 패널
    public Button[] selectPlaneButtons; // 1~3번 비행기 선택 버튼들 (배열로 연결)

    void Start()
    {
        if (restartText == null)
            Debug.LogError("restart text is null");
        if (scoreText == null)
            Debug.LogError("score text is null");
        if (backMainText == null)
            Debug.LogError("backMain text is null");

        // 초기 상태 설정: 재시작 텍스트 숨김
        if (restartText != null) restartText.gameObject.SetActive(false);
        // 메인 씬 복귀 텍스트 숨김 (게임 오버 시 표시)
        if (backMainText != null) backMainText.gameObject.SetActive(false);

        // 캐릭터 선택 UI 초기 상태 설정 (시작 시 활성화)
        if (planeSelectionPanel != null) planeSelectionPanel.SetActive(true);

        //스코어 텍스트가 있는 곳은 부모까지 일단 한 번에 꺼둠
        if (scoreText != null) scoreText.gameObject.transform.parent.gameObject.SetActive(false);
    }
    public void SetGamePlayUIActive(bool isActive)
    {
        // scoreText 등 게임 플레이 UI 요소들의 부모 오브젝트를 활성화/비활성화
        if (scoreText != null && scoreText.gameObject.transform.parent != null)
        {
            scoreText.gameObject.transform.parent.gameObject.SetActive(isActive);
        }        
    }
    public void SetRestart()
    {
        if (restartText != null) restartText.gameObject.SetActive(true);
    }

    public void UpdateScore(int score)
    {
        if (scoreText != null) scoreText.text = score.ToString();
    }

    public void SetBackToMainUIActive(bool isActive)
    {
        if (backMainText != null) backMainText.gameObject.SetActive(isActive);
    }

    public void SetPlaneSelectionUIActive(bool isActive)
    {
        if (planeSelectionPanel != null) planeSelectionPanel.SetActive(isActive);
    }
}
