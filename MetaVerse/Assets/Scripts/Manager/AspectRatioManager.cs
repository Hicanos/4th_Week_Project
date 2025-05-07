using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AspectRatioManager : MonoBehaviour
{
    public float targetAspect = 16f / 9f; // 기본 목표 비율 (16:9)



    void Update()
    {
        SetCameraAspectRatio(targetAspect);

    }

    // 외부에서 호출하여 목표 비율을 변경하는 함수
    public void SetTargetAspectRatio(float newAspect)
    {
        targetAspect = newAspect;
    }

    void SetCameraAspectRatio(float currentTargetAspect)
    {
        // 현재 화면 비율 계산
        float currentAspect = (float)Screen.width / Screen.height;

        // 목표 비율과 현재 비율 비교
        if (currentAspect > currentTargetAspect) // 화면이 목표보다 넓은 경우 (필러박스)
        {
            float newWidth = currentTargetAspect / currentAspect;
            Camera.main.rect = new Rect((1f - newWidth) / 2f, 0f, newWidth, 1f);
        }
        else // 화면이 목표보다 좁거나 같은 경우 (레터박스 또는 딱 맞음)
        {
            float newHeight = currentAspect / currentTargetAspect;
            Camera.main.rect = new Rect(0f, (1f - newHeight) / 2f, 1f, newHeight);
        }
    }
}
