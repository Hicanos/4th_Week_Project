using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class NPCInteraction : MonoBehaviour
{
    TextMeshProUGUI Talktext;
    public GameObject TalkBalloon;

    Dictionary<int, string> Talkbox = new();

    private void Start()
    {
        Talktext = transform.Find("Talk").GetComponentInChildren<TextMeshProUGUI>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        TalkBalloon.SetActive(true);
        Talktext.text = Talkbox[0];
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        TalkBalloon.SetActive(false);
        Talktext.text = null;   
    }



    public void TalkList(Dictionary<int, string> list)
    {
        Talkbox[0] = "��, ������!";
        Talkbox[1] = "���� ó����?";
        Talkbox[2] = "�� ���� �����̾�.";

    }

}
