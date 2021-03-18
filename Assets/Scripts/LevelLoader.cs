using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator animator;

    public AnimationClip sceneOpenAnimation, sceneCloseAnimation;

    private void Start()
    {
        animator.Play(sceneOpenAnimation.name);
    }

    public void ChangeScene(int sceneToLoad)
    {
        StartCoroutine(Change(sceneToLoad));
    }

    IEnumerator Change(int sceneIndex)
    {
        Debug.Log("Changing scene");
        //Play animation
        animator.Play(sceneCloseAnimation.name);
        //Wait
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        //Load Scene
        SceneManager.LoadScene(sceneIndex);
    }

    public void QuitGame()
    {
        StartCoroutine(Quit());
    }

    IEnumerator Quit()
    {
        Debug.Log("Quitting game");
        //Play animation
        animator.Play(sceneCloseAnimation.name);
        //Wait
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        //Quit Game
        Application.Quit();
    }
}
