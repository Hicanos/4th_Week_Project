using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private GameObject currentInteractableObj = null;

    [SerializeField]
    private GameObject Popup;


    private void Start()
    {
        // ���� �ε�� �� GameManager�� ����� ���� ���� ���� Ȯ��
        if (GameManager.Instance != null && !string.IsNullOrEmpty(GameManager.Instance.nextSpawnPointID))
        {
            Debug.Log($"GameManager�� ����� ���� ���� ID �߰�: {GameManager.Instance.nextSpawnPointID}");

            // ����� ID�� ��ġ�ϴ� Potal�� ������ ã��
            Potal[] spawnPoints = FindObjectsOfType<Potal>();
            Potal targetSpawnPoint = null;

            foreach (Potal tp in spawnPoints)
            {
                // This Point ID�� GameManager�� ����� ID�� ��ġ�ϴ��� Ȯ��
                if (tp.GetThisPointID() == GameManager.Instance.nextSpawnPointID)
                {
                    targetSpawnPoint = tp;
                    break;
                }
            }

            if (targetSpawnPoint != null)
            {
                // ã�� ���� ���� ��ġ�� �÷��̾� �̵�
                transform.position = targetSpawnPoint.transform.position;
                Debug.Log($"�÷��̾ ���� ���� '{GameManager.Instance.nextSpawnPointID}' ��ġ�� �̵��߽��ϴ�.");

                // ���� ���� ��� �Ϸ� �� �ʱ�ȭ
                GameManager.Instance.ClearSpawnPoint();
            }
            else
            {
                Debug.LogWarning($"ID '{GameManager.Instance.nextSpawnPointID}'�� ���� ���� ������ ���� ������ ã�� �� �����ϴ�. �⺻ ��ġ�� �����˴ϴ�.");
                // ����� ���� ������ ã�� ������ ��� (��: ���� �ش� ID�� ��Ż�� ���ų� ù ���� ��),
                // �÷��̾�� �⺻������ ���� ��ġ�� ��ġ�� Ư�� �ʱ� ��ġ�� �����˴ϴ�.
                GameManager.Instance.ClearSpawnPoint(); // ������ �ʱ�ȭ
            }
        }
        else
        {
            Debug.Log("GameManager�� ����� ���� ���� ������ �����ϴ�. �⺻ ��ġ�� �����˴ϴ�.");
            // GameManager �ν��Ͻ��� ���ų� nextSpawnPointID�� ������� ���
            // �÷��̾ �⺻������ ���� ��ġ�� ��ġ�� Ư�� �ʱ� ��ġ�� ����
        }
    }


    // Update �Լ��� �÷��̾��� �Է�(FŰ)�� �����ϰ� ��ȣ�ۿ��� ����
    void Update()
    {
        // ��ȣ�ۿ� ������ ������Ʈ�� ���� ���� �ְ� (currentInteractableObject != null),
        // F Ű�� �������� Ȯ��
        if (currentInteractableObj != null && Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log($"[��ȣ�ۿ� �ڵ鷯] F Ű �Է� ����! ��ȣ�ۿ� ���: {currentInteractableObj.name}");            

            // ��ȣ�ۿ� ��� ������Ʈ�� � ������Ʈ(Potal, DoorInteract ��)�� �پ��ִ��� Ȯ��

            Potal potalComponent = currentInteractableObj.GetComponent<Potal>();
            DoorInteract doorInteractComponent = currentInteractableObj.GetComponent<DoorInteract>();

            // � ������ ��ȣ�ۿ� ������Ʈ���� Ȯ���ϰ� �ش� ������ ����
            if (potalComponent != null)
            {
                // �� ��ȯ�� ����ϴ� Potal ������Ʈ�� �ִ� ���
                Debug.Log($"[��ȣ�ۿ� �ڵ鷯] -> �� ��ȯ ���� '{currentInteractableObj.name}' Ȱ��ȭ.");
                // Potal ��ũ��Ʈ�� ActivateTransition() �޼��带 ȣ���Ͽ� �� ��ȯ�� ����
                potalComponent.ActivateTransition();
            }
            else if (doorInteractComponent != null)
            {
                // ���� ��ȣ�ۿ��ϴ� DoorInteract ������Ʈ�� �ִ� ���
                Debug.Log($"[��ȣ�ۿ� �ڵ鷯] -> �� '{currentInteractableObj.name}' ��ȣ�ۿ� Ȱ��ȭ.");
                // DoorInteract ��ũ��Ʈ�� ��ȣ�ۿ� �޼��带 ȣ��
                // �� ���� �ִϸ��̼�, �浹ü ���� ���� DoorInteract ��ũ��Ʈ ��ü���� ó��
                doorInteractComponent.Interact();
            }
            else
            {
                // currentInteractableObject�� �����Ǿ�����, ����ġ ���� ������Ʈ�� �ִ� ��� (���� ó��)
                // ���� ��ȣ�ۿ� ������Ʈ�� �� �� �� �ϳ��� �پ�����(�� �� X)
                Debug.LogWarning($"[��ȣ�ۿ� �ڵ鷯] ��ȣ�ۿ� ������ ������Ʈ '{currentInteractableObj.name}'�� �νĵ� ��ȣ�ۿ� ������Ʈ(Potal, DoorInteract)�� �����ϴ�.");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ������ ������Ʈ�� ��ȣ�ۿ� ������ ������Ʈ(Potal �Ǵ� DoorInteract)�� ������ �ִ��� Ȯ��
        Potal potalComponent = collision.GetComponent<Potal>();
        DoorInteract doorInteractComponent = collision.GetComponent<DoorInteract>();

        // ������ ������Ʈ�� ��ȣ�ۿ� �����ϰ�, ���� �ٸ� ��ȣ�ۿ� ����� �����ϰ� ���� �ʴٸ�,
        // =currentInteractableObject�� null�� ����

        if ((potalComponent != null || doorInteractComponent != null) && currentInteractableObj == null)
        {
            // �� ������Ʈ�� ���� ��ȣ�ۿ� ������� ����.
            currentInteractableObj = collision.gameObject;
            Debug.Log($"[��ȣ�ۿ� �ڵ鷯] ��ȣ�ۿ� ���� ����: {collision.name} (���� ���: {currentInteractableObj.name})");
            Popup.SetActive(true);
            //F ��ư ���� �� �ִ� �˾� ���̰� ��
        }
    }

    // �ٸ� ������Ʈ�� �ݶ��̴�(�Ǵ� Ʈ����)�� �� ������Ʈ�� Ʈ���� �ݶ��̴����� ����� �� ȣ��
    private void OnTriggerExit2D(Collider2D collision)
    {
        // �������� ��� ������Ʈ�� ���� ���� ���� ��ȣ�ۿ� ���� �������� Ȯ��
        // ���� ���� Ʈ���Ű� ���� ���� ��,
        // ���� ��� ������Ʈ�� �ƴ� �ٸ� ������Ʈ�� ���� �������� currentInteractableObject�� �������� �ʵ��� ��
        if (collision.gameObject == currentInteractableObj)
        {
            // ���� ��ȣ�ۿ� ����� �����մϴ�.
            currentInteractableObj = null;
            Debug.Log($"[��ȣ�ۿ� �ڵ鷯] ��ȣ�ۿ� ���� ���: {collision.name}");
            Popup.SetActive(false);
            
        }
        //��� ������Ʈ�� ���� ���� ����� �ƴ϶�� �ƹ��͵� ���� ����
    }
}
