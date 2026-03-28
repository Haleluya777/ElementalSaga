using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    [SerializeField] private GameObject LoadingScene;
    private float currentProgress;
    private float timer;

    public void SceneChange(string sceneName)
    {
        LoadingScene.SetActive(true);
        StartCoroutine(LoadingAsync(sceneName));
    }

    private IEnumerator LoadingAsync(string sceneName)
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);
        asyncOperation.allowSceneActivation = false;

        timer = 0f;

        while (!asyncOperation.isDone)
        {
            float realProgress = Mathf.Clamp01(asyncOperation.progress / 0.9f);

            timer += Time.deltaTime;
            currentProgress = Mathf.Min(realProgress, timer / .2f);

            if (asyncOperation.progress >= 0.9f && timer >= .2f)
            {
                asyncOperation.allowSceneActivation = true;
                //this.gameObject.SetActive(false);   
            }

            yield return null;
        }
        //this.gameObject.SetActive(false);
    }
}
