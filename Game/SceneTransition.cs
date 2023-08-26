using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public string cutsceneSceneName;

    public void ChangeScene()
    {
        SceneManager.LoadScene(cutsceneSceneName);
    }
}
