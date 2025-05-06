using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

public static class SceneLoader
{
    public static void LoadTargetScene(string sceneName)
    {
        // 유효한 씬 이름인지 간단히 확인하는 로직을 추가할 수 있어요.
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogError("오류: 이동할 씬 이름이 존재하지 않거나, 설정되지 않았습니다!");
            return;
        }

        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }
}
