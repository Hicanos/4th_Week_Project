using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderLayer : MonoBehaviour
{
    //Triger Stay 상태일 때 player의 order in Layer 감소
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
                    playerRenderer.sortingOrder = spriteRenderer.sortingOrder - 1; //playerRenderer를 현재 spriteRenderer의 orderLayer-1
                    isHiding = true;
                }
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        // 플레이어가 트리거를 벗어났을 때 원래 상태로 복원
        if (collision.CompareTag("Player") && isHiding)
        {
            var playerRenderer = collision.GetComponentInChildren<SpriteRenderer>();

            if (playerRenderer != null)
            {
                playerRenderer.sortingOrder = 200; // 기본값으로 복원
                isHiding = false;
            }
        }
    }
}

