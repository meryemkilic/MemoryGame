using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public void OnPlayButtonClicked()
    {
        SceneManager.LoadScene("GameScene");

    }

    /*
    public void onSettingsButtonClicked()
    {
        
    }
    */

}