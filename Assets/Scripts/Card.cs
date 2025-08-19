using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    [Header("Card Settings")]
    [SerializeField] private int cardID;
    [SerializeField] private Sprite frontSprite;
    [SerializeField] private Sprite backSprite;

    [Header("Card State")]
    private bool isRevealed = false;
    private bool isMatched = false;
    private bool isInteractable = true;

    [Header("Components")]
    private CardFlip cardFlip;
    private Button cardButton;

    private void Awake()
    {
        cardFlip = GetComponent<CardFlip>();
        cardButton = GetComponent<Button>();

        if (cardButton != null)
            cardButton.onClick.AddListener(OnCardButtonClicked);
    }

    private void Start()
    {
        if (cardFlip != null)
            cardFlip.SetCardImmediate(false, frontSprite, backSprite);
    }

    public void OnCardButtonClicked()
    {
        if (!isInteractable || isMatched || isRevealed) return;

        // Ses: click
        SoundManager.Instance?.PlayClick();

        RevealCard();

        if (GameManager.Instance != null)
            GameManager.Instance.CardWasClicked(this);
    }

    public void RevealCard()
    {
        if (isRevealed || isMatched) return;
        isRevealed = true;
        SetCardState(true);
    }

    public void HideCard()
    {
        if (!isRevealed || isMatched) return;
        isRevealed = false;
        SetCardState(false);
    }

    public void SetMatched()
    {
        isMatched = true;
        isRevealed = true;
        isInteractable = false;
        SetCardState(true);

        if (cardFlip != null)
            StartCoroutine(cardFlip.PlayMatchCelebrate());
    }

    private void SetCardState(bool showFront)
    {
        if (cardFlip != null)
            cardFlip.FlipCard(showFront, frontSprite, backSprite);
    }

    public void SetInteractable(bool interactable)
    {
        isInteractable = interactable;
        if (cardButton != null)
            cardButton.interactable = interactable;
    }

    public void InitializeCard(int id, Sprite front, Sprite back)
    {
        cardID = id;
        frontSprite = front;
        backSprite = back;

        isRevealed = false;
        isMatched = false;
        isInteractable = true;

        if (cardFlip != null)
            cardFlip.SetCardImmediate(false, frontSprite, backSprite);
    }

    public int GetCardID() => cardID;
    public bool IsRevealed() => isRevealed;
    public bool IsMatched() => isMatched;
    public bool IsInteractable() => isInteractable;
}
