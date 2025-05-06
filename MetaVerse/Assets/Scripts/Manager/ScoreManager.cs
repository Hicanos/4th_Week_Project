using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance; // �̱��� ����

    [HideInInspector] //�ν����Ϳ��� �ʿ����
    public int highScore = 0; // ���� �ְ� ����

    [HideInInspector]
    public int currentMiniGameScore = 0; // ���� �÷��� ���� �̴ϰ��� ����

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        
            //����� �ְ� ������ �ҷ����� ����
            highScore = PlayerPrefs.GetInt("HighScore", 0); 

        }
        else
        {
            Destroy(gameObject); // �̹� �ν��Ͻ��� �ִٸ� �ı�
        }
    }

    // ���� �̴ϰ��� ������ ������Ű�� �޼��� (MiniGameManager���� ȣ��)
    public void AddScore(int amount)
    {
        currentMiniGameScore += amount;

    }

    // �̴ϰ��� ���� �� ���� ���� �ʱ�ȭ (MiniGameManager���� ȣ��)
    public void ResetCurrentMiniGameScore()
    {
        currentMiniGameScore = 0;
        Debug.Log("[ScoreManager] ���� �̴ϰ��� ���� �ʱ�ȭ.");
    }

    // �̴ϰ��� ���� �� ���� ������ �޾ƿͼ� ó���ϴ� �޼��� (MiniGameManager���� ȣ��)
    public void EndMiniGame()
    {
        Debug.Log($"[ScoreManager] �̴ϰ��� ����. ���� ����: {currentMiniGameScore}");

        // �ְ� ���� ������Ʈ
        if (currentMiniGameScore > highScore)
        {
            highScore = currentMiniGameScore;
            Debug.Log($"[ScoreManager] �ְ� ���� ������Ʈ: {highScore}");
            
            PlayerPrefs.SetInt("HighScore", highScore); // ����: PlayerPrefs�� ����
            PlayerPrefs.Save(); // ������� ����
        }

    }

    // ���� �̴ϰ��� ������ �ܺο� �����ϴ� �޼��� (UIManager ���� ȣ���Ͽ� ���� ǥ��)
    public int GetCurrentScore()
    {
        return currentMiniGameScore;
    }

    // �ְ� ������ �ܺο� �����ϴ� �޼���
    public int GetHighScore()
    {
        return highScore;
    }
}
