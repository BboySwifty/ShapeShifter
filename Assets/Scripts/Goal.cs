using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Goal : MonoBehaviour
{
    public GameObject FruitsParent;
    public AudioClip winClip;

    private bool _activated = false;
    private Animator animator;
    private int fruitCount = 0;

    private void Start()
    {
        animator = GetComponent<Animator>();

        Fruit[] fruits = FruitsParent.GetComponentsInChildren<Fruit>();
        foreach(Fruit fruit in fruits)
        {
            fruitCount++;
            fruit.Collected += FruitCollected;
        }
        if(fruitCount == 0)
            Activate();
    }

    // Start is called before the first frame update
    void Activate()
    {
        _activated = true;
        animator.SetTrigger("FruitsCollected");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController controller = collision.GetComponent<PlayerController>();

        if (controller != null && _activated)
        {
            controller.PlaySound(winClip);
            GameManager.Instance.NextLevel();
        }
    }

    void FruitCollected(object state, EventArgs e)
    {
        fruitCount--;
        if (fruitCount <= 0)
        {
            Activate();
        }
    }
}
