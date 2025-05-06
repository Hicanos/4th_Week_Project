using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potal : MonoBehaviour
{
    [Header("�̵��� ��")]
    public string targetSceneName;

    // �ش� ��Ż �̸� �־�� ��
    [Header("���� ������ ������ ������ ID")]    
    public string targetSpawnPointID;

    //�ٽ� �ǵ��ƿ� ����Ʈ
    [Header("�� ������ ���� ID")]
    public string thisPointID;

    //�ܺ� ��ũ��Ʈ���� �� ������ ������ ������ �� �ֵ��� �Ӽ� �߰�
    public string GetTargetSceneName()
    {
        return targetSceneName;
    }

    public string GetTargetSpawnPointID()
    {
        return targetSpawnPointID;
    }

    public string GetThisPointID()
    {
        return thisPointID;
    }

    // �ʿ��ϴٸ� �÷��̾ �� ������ ��ų� ��ȣ�ۿ����� �� ȣ��� �޼��� �߰�
    // ��: InteractionHandler���� FŰ ������ �� �� �޼��带 ȣ���ϵ���
    public void ActivateTransition()
    {
        if (GameManager.Instance != null)
        {
            // GameManager�� ���� �� ��ȯ ��û
            GameManager.Instance.TransitionToScene(targetSceneName, targetSpawnPointID);
            Debug.Log("���ӸŴ����� �̿��� ���� �����");
        }
        else
        {
            Debug.LogError("GameManager �ν��Ͻ��� ã�� �� �����ϴ�. ���� GameManager ������Ʈ�� �ֳ���?");
        }
    }
}
