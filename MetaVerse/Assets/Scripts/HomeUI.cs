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

        //버튼 중에서 "이름"에 해당하는 버튼 찾기
        startButton = transform.Find("StartButton").GetComponent<Button>();
        exitButton = transform.Find("ExitButton").GetComponent<Button>();

        //onClick, 즉 클릭하면 해당 버튼에 반응을 줌
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
