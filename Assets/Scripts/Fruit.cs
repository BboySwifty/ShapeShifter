using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : MonoBehaviour
{
    public AudioClip collectedClip;
    public event EventHandler Collected;

    private Animator animator;
    private CircleCollider2D collider2d;

    private void Start()
    {
        animator = GetComponent<Animator>();
        collider2d = GetComponent<CircleCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController controller = collision.GetComponent<PlayerController>();

        if (controller != null)
        {
            collider2d.enabled = false;
            animator.SetTrigger("Collected");
            controller.PlaySound(collectedClip);
            Collected?.Invoke(this, EventArgs.Empty);
            Destroy(gameObject, 0.5f);
        }
    }
}
