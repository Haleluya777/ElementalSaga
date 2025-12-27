using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public void SceneChange(int sceneNum)
    {
        SceneManager.LoadScene(sceneNum);
    }
}
