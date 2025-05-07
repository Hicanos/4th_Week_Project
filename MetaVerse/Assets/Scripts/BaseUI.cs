using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline;
using UnityEngine;

//추상 클래스
public abstract class BaseUI : MonoBehaviour
{
    protected StackManager stackManager;

    public virtual void Init(StackManager uiManager)
    {
        this.stackManager = uiManager;
    }

    protected abstract UIState GetUIState();

    public void SetActive(UIState state)
    {
        gameObject.SetActive(GetUIState() == state);
    }
}
