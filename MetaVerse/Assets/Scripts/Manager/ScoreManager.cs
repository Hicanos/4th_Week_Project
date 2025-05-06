using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance; // 싱글톤 패턴

    [HideInInspector] //인스펙터에선 필요없음
    public int highScore = 0; // 게임 최고 점수

    [HideInInspector]
    public int currentMiniGameScore = 0; // 현재 플레이 중인 미니게임 점수

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        
            //저장된 최고 점수를 불러오는 로직
            highScore = PlayerPrefs.GetInt("HighScore", 0); 

        }
        else
        {
            Destroy(gameObject); // 이미 인스턴스가 있다면 파괴
        }
    }

    // 현재 미니게임 점수를 증가시키는 메서드 (MiniGameManager에서 호출)
    public void AddScore(int amount)
    {
        currentMiniGameScore += amount;

    }

    // 미니게임 시작 시 현재 점수 초기화 (MiniGameManager에서 호출)
    public void ResetCurrentMiniGameScore()
    {
        currentMiniGameScore = 0;
        Debug.Log("[ScoreManager] 현재 미니게임 점수 초기화.");
    }

    // 미니게임 종료 시 최종 점수를 받아와서 처리하는 메서드 (MiniGameManager에서 호출)
    public void EndMiniGame()
    {
        Debug.Log($"[ScoreManager] 미니게임 종료. 최종 점수: {currentMiniGameScore}");

        // 최고 점수 업데이트
        if (currentMiniGameScore > highScore)
        {
            highScore = currentMiniGameScore;
            Debug.Log($"[ScoreManager] 최고 점수 업데이트: {highScore}");
            
            PlayerPrefs.SetInt("HighScore", highScore); // 예시: PlayerPrefs에 저장
            PlayerPrefs.Save(); // 변경사항 저장
        }

    }

    // 현재 미니게임 점수를 외부에 제공하는 메서드 (UIManager 등이 호출하여 점수 표시)
    public int GetCurrentScore()
    {
        return currentMiniGameScore;
    }

    // 최고 점수를 외부에 제공하는 메서드
    public int GetHighScore()
    {
        return highScore;
    }
}
