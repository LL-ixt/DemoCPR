using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class SceneTransitionManager : MonoBehaviour
{
    public FadeScreen fadeScreen;
    public GameObject ovrRigObject;
    void Start()
    {
        //DontDestroyOnLoad(ovrRigObject);
    }
    public void GoToSceneAsync(string sceneName)
    {
        fadeScreen.gameObject.SetActive(true);
        StartCoroutine(GoToSceneRoutineAsync(sceneName));
    }

    IEnumerator GoToSceneRoutineAsync(string sceneName)
    {
        fadeScreen.FadeIn();
        yield return new WaitForSeconds(fadeScreen.fadeDuration);
        if (ovrRigObject != null)
        {
            ovrRigObject.SetActive(false); 
        }
        //Launch the new scene
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false;
        
        float timer = 0;
        while(timer <= fadeScreen.fadeDuration && !operation.isDone) 
        {
            timer += Time.deltaTime;
            yield return null;
        }

        operation.allowSceneActivation = true;
    }
}
