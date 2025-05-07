using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline;
using UnityEngine;

//�߻� Ŭ����
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
