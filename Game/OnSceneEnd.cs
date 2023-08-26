using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class OnSceneEnd : MonoBehaviour
{
    public PlayableDirector cutsceneDirector;
    public string targetSceneName;

    public void CutsceneEnded()
    {
        Debug.Log("Cutscene Ended");
        SceneManager.LoadScene(targetSceneName);
    }
}
