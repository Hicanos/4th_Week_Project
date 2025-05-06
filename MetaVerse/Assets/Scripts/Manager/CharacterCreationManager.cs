using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // TMP_InputField ��� �� �ʿ�
using UnityEngine.UI;

public class CharacterCreationManager : MonoBehaviour
{
    public TMP_InputField nameInputField; // �̸� �Է� �ʵ�
    public Button createButton; // ĳ���� ���� ��ư

    public GameObject WarningMsg;

    // ���� ��
    private const string MainSceneName = "MainScene"; 

    void Start()
    {
        // ��ư Ŭ�� �̺�Ʈ ����
        if (createButton != null)
        {
            createButton.onClick.AddListener(OnCreateButtonClick);
        }
        else
        {
            Debug.LogError("[CharacterCreationManager] Create Button�� ������� �ʾҽ��ϴ�!");
        }

        // InputField�� ����Ǿ����� Ȯ��
        if (nameInputField == null)
        {
            Debug.LogError("[CharacterCreationManager] Name Input Field�� ������� �ʾҽ��ϴ�!");
        }
    }

    void OnDestroy()
    {
        // ��ư �̺�Ʈ ���� ���� (���� �ı��� ��)
        if (createButton != null)
        {
            createButton.onClick.RemoveListener(OnCreateButtonClick);
        }
    }

    // ���� ��ư Ŭ�� �� ȣ��� �޼���
    public void OnCreateButtonClick()
    {
        if (nameInputField == null) return;

        string characterName = nameInputField.text;

        // �Էµ� �̸��� ������� ������ Ȯ��
        if (string.IsNullOrWhiteSpace(characterName))
        {
            Debug.LogWarning("[CharacterCreationManager] ĳ���� �̸��� �Է����ּ���!");
            WarningMsg.SetActive(true);

            return;
        }

        // GameManager �ν��Ͻ��� �ִ��� Ȯ���ϰ� �̸� ����
        if (GameManager.Instance != null)
        {
            GameManager.Instance.SetCharacterName(characterName);
            Debug.Log($"[CharacterCreationManager] GameManager�� �̸� '{characterName}' ���� �Ϸ�. ���� ������ �̵��մϴ�.");

            // ���� ������ �̵� ��û-�������� ����
            GameManager.Instance.TransitionToScene(MainSceneName, "SpawnPotal"); // �⺻ ���� ���� �������� ����

        }
        else
        {
            Debug.LogError("[CharacterCreationManager] GameManager �ν��Ͻ��� ã�� �� �����ϴ�! �� ��ȯ �Ұ�.");
        }
    }
}
