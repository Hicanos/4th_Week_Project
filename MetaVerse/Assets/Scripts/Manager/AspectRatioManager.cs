using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AspectRatioManager : MonoBehaviour
{
    public float targetAspect = 16f / 9f; // �⺻ ��ǥ ���� (16:9)



    void Update()
    {
        SetCameraAspectRatio(targetAspect);

    }

    // �ܺο��� ȣ���Ͽ� ��ǥ ������ �����ϴ� �Լ�
    public void SetTargetAspectRatio(float newAspect)
    {
        targetAspect = newAspect;
    }

    void SetCameraAspectRatio(float currentTargetAspect)
    {
        // ���� ȭ�� ���� ���
        float currentAspect = (float)Screen.width / Screen.height;

        // ��ǥ ������ ���� ���� ��
        if (currentAspect > currentTargetAspect) // ȭ���� ��ǥ���� ���� ��� (�ʷ��ڽ�)
        {
            float newWidth = currentTargetAspect / currentAspect;
            Camera.main.rect = new Rect((1f - newWidth) / 2f, 0f, newWidth, 1f);
        }
        else // ȭ���� ��ǥ���� ���ų� ���� ��� (���͹ڽ� �Ǵ� �� ����)
        {
            float newHeight = currentAspect / currentTargetAspect;
            Camera.main.rect = new Rect(0f, (1f - newHeight) / 2f, 1f, newHeight);
        }
    }
}
