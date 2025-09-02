using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public void OnPlayButtonClicked(int index)
    {
        SceneManager.LoadScene(index);
    }
}