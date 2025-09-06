using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject difficultySelectionPanel;
    [SerializeField] private GameObject gamePanel;
    [SerializeField] private GameObject startPanel;
    [SerializeField] private Button playButton;
    [SerializeField] private Button easyButton;
    [SerializeField] private Button mediumButton;
    [SerializeField] private Button hardButton;

    private void Start()
    {
        playButton.onClick.AddListener(OnPlayButtonClicked);
        easyButton.onClick.AddListener(() => OnDifficultySelected(2, 2));
        mediumButton.onClick.AddListener(() => OnDifficultySelected(2, 3));
        hardButton.onClick.AddListener(() => OnDifficultySelected(2, 4));

    }

    public void OnPlayButtonClicked()
    {
        startPanel.SetActive(false);
        difficultySelectionPanel.SetActive(true);
    }

    public void OnDifficultySelected(int gridHeight, int gridWidth)
    {
        difficultySelectionPanel.SetActive(false);
        gamePanel.SetActive(true);
        GameManager.Instance.GenerateLevel(gridHeight, gridWidth);
    }
}