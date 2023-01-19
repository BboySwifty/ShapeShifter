using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public Animator TransitionAnimator;
    public float TransitionTime;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public void NextLevel()
    {
        StartCoroutine(EndLevelTransition());
    }

    public void RestartLevel()
    {
        StartCoroutine(StartLevelTransition());
    }

    private IEnumerator StartLevelTransition()
    {
        TransitionAnimator.SetTrigger("Transition");
        yield return new WaitForSeconds(TransitionTime);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private IEnumerator EndLevelTransition()
    {
        TransitionAnimator.SetTrigger("Transition");
        yield return new WaitForSeconds(TransitionTime);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
