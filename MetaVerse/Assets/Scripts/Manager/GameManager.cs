using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    // ���� ������ �÷��̾ ������ ������ ID�� ���� (��: ��Ż �̸� ��)
    [HideInInspector] // �ν����Ϳ� ������� �ʵ��� ����
    public string nextSpawnPointID = "";

    // ������ �־��� ���� �̸� (���ư� �� Ȱ��)
    [HideInInspector]
    public string previousSceneName = "";

    [HideInInspector] // ���� �����ͷ� ������ ĳ���� �̸�
    public string characterName = "Player"; // �⺻��

    public GameObject playerPrefab;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); //�ı� ���� ó��

            //���� �ε��Ҷ����� OnSceneLoaded ȣ��
            SceneManager.sceneLoaded += OnSceneLoaded;

        }
        else
        {
            // �̹� �ٸ� �ν��Ͻ��� �ִٸ� ���� ������ ������Ʈ�� �ı�
            Destroy(gameObject);
        }
    }

    void OnDestroy()
    {
        // ������Ʈ�� �ı��Ǹ� �̺�Ʈ ��� ���� (�߿�!)
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // ���� �ε�� ������ ȣ��Ǵ� �޼���
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"[GameManager] �� �ε� �Ϸ�: {scene.name}");

        //�ε�� ���� ���� ���̰�, ���� �÷��̾ �������� �ʾҴٸ� �÷��̾ ����
        if (scene.name == "MainScene")
        {
            //���� ���� �÷��̾ �ִ��� Ȯ�� (DontDestroyOnLoad�� �Ѿ�� �÷��̾� ����)
            PlayerInteraction existingPlayer = FindObjectOfType<PlayerInteraction>();

            //���� �÷��̾�==null�̰� �������� �����Ѵٸ� >����
            if (existingPlayer == null && playerPrefab != null)
            {
                Debug.Log("[GameManager] ���� �� ����. �÷��̾� ���� ����.");

                // GameManager�� playerPrefab�� ����Ͽ� �÷��̾� �ν��Ͻ� ����
                GameObject playerGO = Instantiate(playerPrefab);

                // ������ �÷��̾� ������Ʈ�� ��ġ�� �ʱ� ���� ��ġ�� ����
                // �������� ID�� ������ �ͼ� �ѱ�

                // ������ �÷��̾ ĳ���� �̸� ����
                SetNameOnPlayer(playerGO, characterName);


            }
            else if (existingPlayer != null)
            {
                Debug.Log("[GameManager] ���� �� ����. �÷��̾� ������Ʈ�� �̹� �����մϴ�.");
                // �������� �÷��̶�� ������ �� ����.
                // ������ ���ؼ�(...) ����
                SetNameOnPlayer(existingPlayer.gameObject, characterName);
            }

        }

    }

    public void SetCharacterName(string name)
    {
        characterName = name;
        Debug.Log($"[GameManager] ĳ���� �̸� ������: {characterName}");
    }

    // �÷��̾� ������Ʈ�� �̸� ǥ��
    private void SetNameOnPlayer(GameObject playerObject, string name)
    {

        Transform nameTextTransform = playerObject.transform.Find("NameCanvas/PlayerName"); // TODO: ���� ��� Ȯ��
        TMPro.TMP_Text nameText = null;

        //�Է�â�� �Է��ϸ�.
        if (nameTextTransform != null)
        {
            nameText = nameTextTransform.GetComponent<TMPro.TMP_Text>();
        }


        if (nameText != null)
        {
            nameText.text = name;
            Debug.Log($"[GameManager] �÷��̾� ������Ʈ�� �̸� '{name}' ǥ��.");
        }
        else
        {
            Debug.LogWarning($"[GameManager] �÷��̾� Prefab���� �̸� ǥ�ÿ� TMP Text ������Ʈ�� ã�� �� �����ϴ�! �̸� ǥ�� �Ұ�.");
            Debug.LogWarning($"��� Ȯ�� �ʿ�: playerObject.transform.Find(\"NameCanvas / PlayerName\")");
        }
    }


    public void TransitionToScene(string sceneName, string spawnPointID)
    {
        // ���� �� �̸� ����
        previousSceneName = SceneManager.GetActiveScene().name;

        // ���� ������ ���� ���� ���� ID ����
        nextSpawnPointID = spawnPointID;

        Debug.Log($"�� ��ȯ ��û: {previousSceneName} -> {sceneName}, ���� ���� ���� ID: {nextSpawnPointID}");

        // �� �ε� ����
        SceneManager.LoadScene(sceneName);
    }

    // �÷��̾ ������ �� ȣ���Ͽ� ���� �ʱ�ȭ (�÷��̾� ��ũ��Ʈ���� ȣ��)
    public void ClearSpawnPoint()
    {
        nextSpawnPointID = "";
        // previousSceneName�� ���ư� �� �ʿ��� �� ����
    }

}
