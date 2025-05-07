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

    public GameObject planeSelectionPanel; // ����� ���� UI �г�
    public Button[] selectPlaneButtons; // 1~3�� ����� ���� ��ư�� (�迭�� ����)

    void Start()
    {
        if (restartText == null)
            Debug.LogError("restart text is null");
        if (scoreText == null)
            Debug.LogError("score text is null");
        if (backMainText == null)
            Debug.LogError("backMain text is null");

        // �ʱ� ���� ����: ����� �ؽ�Ʈ ����
        if (restartText != null) restartText.gameObject.SetActive(false);
        // ���� �� ���� �ؽ�Ʈ ���� (���� ���� �� ǥ��)
        if (backMainText != null) backMainText.gameObject.SetActive(false);

        // ĳ���� ���� UI �ʱ� ���� ���� (���� �� Ȱ��ȭ)
        if (planeSelectionPanel != null) planeSelectionPanel.SetActive(true);

        //���ھ� �ؽ�Ʈ�� �ִ� ���� �θ���� �ϴ� �� ���� ����
        if (scoreText != null) scoreText.gameObject.transform.parent.gameObject.SetActive(false);
    }
    public void SetGamePlayUIActive(bool isActive)
    {
        // scoreText �� ���� �÷��� UI ��ҵ��� �θ� ������Ʈ�� Ȱ��ȭ/��Ȱ��ȭ
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
