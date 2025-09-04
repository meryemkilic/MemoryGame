using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUIController : MonoBehaviour
{
    [SerializeField] private GameObject difficultySelectionPanel;
    [SerializeField] private GameObject gamePanel;
    [SerializeField] private GameObject startPanel;



    public void OnPlayButtonClicked()
    {
        startPanel.SetActive(false);
        difficultySelectionPanel.SetActive(true);
        

    }
    public void OnDifficultySelected(int level)
    {
        difficultySelectionPanel.SetActive(false);
        gamePanel.SetActive(true);

        GameManager.Instance.GenerateLevel(level);
    }
}