using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderLayer : MonoBehaviour
{
    //Triger Stay ������ �� player�� order in Layer ����
    private SpriteRenderer spriteRenderer;
    private bool isHiding = false;
    private GameObject player;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if(!isHiding)
            {
                player = collision.gameObject;
                var playerRenderer = player.GetComponentInChildren<SpriteRenderer>();

                if (playerRenderer != null)
                {
                    playerRenderer.sortingOrder = spriteRenderer.sortingOrder - 1; //playerRenderer�� ���� spriteRenderer�� orderLayer-1
                    isHiding = true;
                }
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        // �÷��̾ Ʈ���Ÿ� ����� �� ���� ���·� ����
        if (collision.CompareTag("Player") && isHiding)
        {
            var playerRenderer = collision.GetComponentInChildren<SpriteRenderer>();

            if (playerRenderer != null)
            {
                playerRenderer.sortingOrder = 200; // �⺻������ ����
                isHiding = false;
            }
        }
    }
}

