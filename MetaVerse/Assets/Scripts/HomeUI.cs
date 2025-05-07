using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HomeUI : BaseUI
{
    Button startButton;
    Button exitButton;
    protected override UIState GetUIState()
    {
        return UIState.Home;
    }

    public override void Init(StackManager stackManager)
    {
        base.Init(stackManager);

        //��ư �߿��� "�̸�"�� �ش��ϴ� ��ư ã��
        startButton = transform.Find("StartButton").GetComponent<Button>();
        exitButton = transform.Find("ExitButton").GetComponent<Button>();

        //onClick, �� Ŭ���ϸ� �ش� ��ư�� ������ ��
        startButton.onClick.AddListener(OnClickStartButton);
        exitButton.onClick.AddListener(OnClickExitButton);
    }

    void OnClickStartButton()
    {
        stackManager.OnClickStart();
    }

    void OnClickExitButton()
    {
        stackManager.OnClickExit();
    }

}
