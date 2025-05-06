using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

public static class SceneLoader
{
    public static void LoadTargetScene(string sceneName)
    {
        // ��ȿ�� �� �̸����� ������ Ȯ���ϴ� ������ �߰��� �� �־��.
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogError("����: �̵��� �� �̸��� �������� �ʰų�, �������� �ʾҽ��ϴ�!");
            return;
        }

        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }
}
