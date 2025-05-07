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

    public enum MiniGameState { PlaneSelection, Playing, GameOver } // �̴ϰ��� ���� ����
    private MiniGameState currentState; //���� ����

    private string playerName; //ĳ���͸�-GameManager
    private GameObject characterVisualInstance = null; //������ �̹���/�̸��� ���� ������
    private PlanePlayer planePlayerScript; //PlanePlayer�޸� Ridingã��

    public GameObject ridingObject;
    public Transform characterMountPoint; //ridingObject���� �÷��̾�ĳ���Ͱ� ��ġ�� Transform

    private void Awake()
    {
        if (miniGameManager != null && miniGameManager != this)
        {
            Destroy(gameObject); // �ߺ� �ν��Ͻ� �ı�
            return;
        }
        miniGameManager = this;
        uiManager = FindObjectOfType<UIManager>();

        if (ridingObject != null)
        {
            planePlayerScript = ridingObject.GetComponent<PlanePlayer>();
            if (planePlayerScript == null)
            {
                Debug.LogError($"[MiniGameManager] Riding Object '{ridingObject.name}'�� PlanePlayer ��ũ��Ʈ�� �پ����� ����");
            }
        }
        else
        {
            Debug.LogError("[MiniGameManager] Riding Object�� �����������");
        }

        // �̴ϰ��� �ʱ� ���� ���� (���� ȭ��)
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
            // ���콺 ��ư Ŭ�� �� ���� �����
            if (Input.GetMouseButtonDown(0))
            {
                RestartGame();
            }

            // 'B' Ű �Է� �� ���� �޴��� �̵�
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
        //���� �������� ���� �̸��� �����ͼ� �ε���
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void AddScore(int score)
    {
        currentScore += score;
        uiManager.UpdateScore(currentScore);
    }


    public void SetupMiniGame(string playerName, GameObject characterVisualPrefab)
    {
        this.playerName = playerName; // ���� �Ŵ������� �Ѿ���� �̸� ����

        // ĳ���� �ð� ��� Prefab ���� ��ġ
        if (characterVisualPrefab != null && ridingObject != null)
        {
            // ������ ������ ĳ���� �ð� ��� �ν��Ͻ��� �ִٸ� �ı� (����� �� ��)
            if (characterVisualInstance != null)
            {
                Destroy(characterVisualInstance);
            }

            // ĳ���ͽ�������Ʈ �� �̸�'��' ����
            characterVisualInstance = Instantiate(characterVisualPrefab);

            // ĳ���� ������Ʈ ��ġ (Riding �� �ڽ�����)
            Transform mountPoint = characterMountPoint != null ? characterMountPoint : ridingObject.transform;
            characterVisualInstance.transform.SetParent(mountPoint, false); // �θ� ���� (worldPositionStays = false)
            characterVisualInstance.transform.localPosition = Vector3.zero; // �θ� ���� ��ġ (0,0,0)�� �̵�

            // ĳ���� �̸� ǥ��
            Transform nameTextTransform = characterVisualInstance.transform.Find("NameCanvas/PlayerName");
            TMPro.TMP_Text nameText = null;
            if (nameTextTransform != null)
            {
                nameText = nameTextTransform.GetComponent<TMPro.TMP_Text>();
            }
            if (nameText != null)
            {
                nameText.text = playerName;
                Debug.Log($"[MiniGameManager] ĳ���� �̸� '{playerName}' ǥ��.");
            }
            else
            {
                Debug.LogWarning("[MiniGameManager] ĳ���� �ð� ��� Prefab���� �̸� ǥ�ÿ� TMP Text ������Ʈ�� ã�� �� ����");
            }

            // PlanePlayer ��ũ��Ʈ���� ĳ���� �ִϸ����� ���� (RidingObject�� �پ��ִ� PlanePlayer)
            Animator characterAnimator = characterVisualInstance.GetComponentInChildren<Animator>(); // ĳ���� �ν��Ͻ����� �ִϸ����� ã��
            if (planePlayerScript != null)
            {
                planePlayerScript.SetCharacterAnimator(characterAnimator); // PlanePlayer�� SetCharacterAnimator �޼��� �߰�
                Debug.Log("[MiniGameManager] PlanePlayer���� ĳ���� �ִϸ����� ����.");
            }
            else
            {
                Debug.LogWarning("[MiniGameManager] PlanePlayer ��ũ��Ʈ�� ���� �ִϸ����͸� ���� �Ұ�");
            }


            Debug.Log($"[MiniGameManager] ĳ���� '{playerName}' �̴ϰ��� �غ� �Ϸ�.");

        }
        else
        {
            Debug.LogError("[MiniGameManager] ĳ���� �ð� ��� Prefab �Ǵ� Riding Object�� ����/���޵��� ����");
        }

        // �ʱ� ���´� ����� ���� ȭ��
        SetState(MiniGameState.PlaneSelection);
    }

    private void SetState(MiniGameState newState)
    {
        currentState = newState;
        Debug.Log($"[MiniGameManager] �̴ϰ��� ���� ����: {currentState}");

        // ���¿� ���� UI �� ���� ���� Ȱ��ȭ/��Ȱ��ȭ
        switch (currentState)
        {
            case MiniGameState.PlaneSelection:
                uiManager.SetPlaneSelectionUIActive(true); // ����� ���� UI ǥ��
                uiManager.SetGamePlayUIActive(false); // ���� �÷��� UI ����
                uiManager.SetBackToMainUIActive(false); // ���� UI ����
                                                        // RidingObject�� ���� ��Ȱ��ȭ (���� �߿� �������� �ʵ���)
                if (ridingObject != null && ridingObject.GetComponent<Rigidbody2D>() != null)
                {
                    ridingObject.GetComponent<Rigidbody2D>().isKinematic = true; // ���� ���� �� �޵���
                    ridingObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero; // �ӵ� 0
                }
                break;

            case MiniGameState.Playing:
                uiManager.SetPlaneSelectionUIActive(false); // ����� ���� UI ����
                uiManager.SetGamePlayUIActive(true); // ���� �÷��� UI ǥ��
                uiManager.SetBackToMainUIActive(false); // ���� UI ����
                // ���� ���� ���� ����
                StartGame();
                break;

            case MiniGameState.GameOver:
                uiManager.SetRestart(); // ����� �ؽ�Ʈ ǥ��
                uiManager.SetBackToMainUIActive(true); // ���� �� ���� UI ǥ�� (�Ǵ� ��ư)
                                                       // RidingObject�� ���� �ٽ� Ȱ��ȭ (�׾��� �� ����������)
                if (ridingObject != null && ridingObject.GetComponent<Rigidbody2D>() != null)
                {
                    ridingObject.GetComponent<Rigidbody2D>().isKinematic = false; // ���� ���� �ٽ� �޵���
                }
                // ���� ���� ���� ���� (PlanePlayer���� GameOver�� ȣ����)
                break;
        }
    }

    private void StartGame()
    {
        currentScore = 0; // ���� �ʱ�ȭ
        uiManager.UpdateScore(currentScore); // ���� UI ������Ʈ
        uiManager.SetRestart(); // ����� UI ���� (���� ���� �� �ٽ� ��Ÿ��)
        uiManager.SetBackToMainUIActive(false); // ���� UI ����

    }

    // ����� ���� ��ư Ŭ�� �� ȣ��� �޼��� (UI ��ư �̺�Ʈ�� ����)
    public void SelectPlane(int type)
    {
        if (currentState != MiniGameState.PlaneSelection) return; // ���� ���°� �ƴϸ� ����

        Debug.Log($"[MiniGameManager] ����� Ÿ�� ���õ�: {type}");
        // ���õ� Ÿ���� PlanePlayer ��ũ��Ʈ�� ����
        if (planePlayerScript != null)
        {
            planePlayerScript.SetPlaneAnimationType(type); // PlanePlayer�� SetPlaneAnimationType �޼��� �߰� �ʿ�
        }
        else
        {
            Debug.LogWarning("[MiniGameManager] PlanePlayer ��ũ��Ʈ�� �����ϴ�. �ִϸ��̼� Ÿ�� ���� �Ұ�.");
        }

        // ���¸� Playing���� �����Ͽ� ���� ����
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