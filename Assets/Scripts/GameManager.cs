using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    [Header("Game Settings")]
    [SerializeField] private int gridWidth = 4;
    [SerializeField] private int gridHeight = 3;
    [SerializeField] private float cardRevealTime = 1f;
    [SerializeField] private Sprite[] cardSprites;   // ön yüzler
    [SerializeField] private Sprite backSprite;      // tek arka yüz

    [Header("UI References")]
    [SerializeField] private Transform cardParent;
    [SerializeField] private GameObject cardPrefab;

    private List<Card> allCards = new List<Card>();
    private List<Card> revealedCards = new List<Card>();
    private int totalPairs;
    private int matchedPairs = 0;
    private bool canSelectCards = true;
    private bool isCheckingMatch = false;

    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        Random.InitState(System.DateTime.Now.Millisecond);
        InitializeGame();
    }

    private void InitializeGame()
    {
        totalPairs = (gridWidth * gridHeight) / 2;
        CreateCards();
    }

    private void CreateCards()
    {
        foreach (Transform child in cardParent)
            Destroy(child.gameObject);
        allCards.Clear();

        List<int> cardIDs = new List<int>();
        for (int i = 0; i < totalPairs; i++)
        {
            cardIDs.Add(i);
            cardIDs.Add(i);
        }

        // shuffle
        for (int i = cardIDs.Count - 1; i > 0; i--)
        {
            int rand = Random.Range(0, i + 1);
            int temp = cardIDs[i];
            cardIDs[i] = cardIDs[rand];
            cardIDs[rand] = temp;
        }

        for (int i = 0; i < cardIDs.Count; i++)
        {
            GameObject cardObj = Instantiate(cardPrefab, cardParent);
            Card card = cardObj.GetComponent<Card>();
            if (card != null)
            {
                int id = cardIDs[i];
                Sprite front = cardSprites[id % cardSprites.Length];
                card.InitializeCard(id, front, backSprite);
                allCards.Add(card);
            }
        }
    }

    public void CardWasClicked(Card selectedCard)
    {
        if (!canSelectCards || isCheckingMatch) return;

        revealedCards.Add(selectedCard);

        if (revealedCards.Count == 2)
        {
            canSelectCards = false;
            isCheckingMatch = true;
            Invoke(nameof(CheckForMatch), cardRevealTime);
        }
    }

    private void CheckForMatch()
    {
        if (revealedCards.Count != 2) return;

        Card c1 = revealedCards[0];
        Card c2 = revealedCards[1];

        if (c1.GetCardID() == c2.GetCardID())
        {
            c1.SetMatched();
            c2.SetMatched();
            matchedPairs++;

            SoundManager.Instance?.PlayMatch();

            if (matchedPairs >= totalPairs)
                OnGameComplete();
        }
        else
        {
            c1.HideCard();
            c2.HideCard();

            SoundManager.Instance?.PlayMismatch();
        }

        revealedCards.Clear();
        canSelectCards = true;
        isCheckingMatch = false;
    }

    private void OnGameComplete()
    {
        Debug.Log("Game Complete!");
        SoundManager.Instance?.PlayVictory();
    }

    public void RestartGame()
    {
        Random.InitState(System.DateTime.Now.Millisecond);
        matchedPairs = 0;
        revealedCards.Clear();
        canSelectCards = true;
        isCheckingMatch = false;
        CancelInvoke();
        InitializeGame();
    }

    // Getters
    public int GetMatchedPairs() => matchedPairs;
    public int GetTotalPairs() => totalPairs;
    public bool CanSelectCards() => canSelectCards;
}
