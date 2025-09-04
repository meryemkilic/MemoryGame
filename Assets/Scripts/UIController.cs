using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{

    [SerializeField] private GameObject difficultyPanel;

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