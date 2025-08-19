using UnityEngine;
using UnityEngine.SceneManagement;

public class NewMonoBehaviourScript : MonoBehaviour
{
    public void LoadGame(int index)
    {
        SceneManager.LoadScene(index);
    }
}
