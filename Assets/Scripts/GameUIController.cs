using UnityEngine;

public class GameUIController : MonoBehaviour
{
    [SerializeField] private GameObject difficultySelectionPanel;
    [SerializeField] private GameObject gamePanel;

    public void OnDifficultySelected(int level)
    {
        difficultySelectionPanel.SetActive(false);
        gamePanel.SetActive(true);

        GameManager.Instance.GenerateLevel(level);
    }
}