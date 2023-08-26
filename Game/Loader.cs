using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loader : MonoBehaviour
{
    public string[] sceneNames;
    public GameObject loadingScreen;
    public string firstSceneName;

    private void Start()
    {
        loadingScreen.SetActive(true);
        StartCoroutine(LoadScenesAsync());
    }

    private IEnumerator LoadScenesAsync()
    {
        AsyncOperation[] sceneLoads = new AsyncOperation[sceneNames.Length];

        for (int i = 0; i < sceneNames.Length; i++)
        {
            sceneLoads[i] = SceneManager.LoadSceneAsync(sceneNames[i], LoadSceneMode.Additive);
            sceneLoads[i].allowSceneActivation = false;
        }

        bool allScenesLoaded = false;
        while (!allScenesLoaded)
        {
            allScenesLoaded = true;

            for (int i = 0; i < sceneLoads.Length; i++)
            {
                if (!sceneLoads[i].isDone)
                {
                    if (sceneLoads[i].progress >= 0.9f)
                    {
                        sceneLoads[i].allowSceneActivation = true;
                    }

                    allScenesLoaded = false;
                }
            }

            yield return null;
        }

        TransitionToFirstScene(); 
    }

    private void TransitionToFirstScene()
    {
        SceneManager.LoadScene(firstSceneName, LoadSceneMode.Single);
    }
}
