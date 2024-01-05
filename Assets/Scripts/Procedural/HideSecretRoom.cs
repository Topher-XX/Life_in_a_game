using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HideSecretRoom : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }


    public void SetCeilingSize(int length, int height)
    {
        //spriteRenderer = GetComponent<SpriteRenderer>();

        spriteRenderer.size = new Vector2(length, height);
        GetComponent<BoxCollider2D>().size = new Vector2(length, height);

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GetComponent<Animation>().Play("Fade");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GetComponent<Animation>().Play("Unfade");
        }
    }

}
