using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;
using System.Collections;



public class GameManager : SingletonBehaviour<GameManager>
{
    private int gridWidth;
    private int gridHeight;
    [SerializeField] private float cardRevealTime = 0.75f;
    [SerializeField] private CardData[] cardTypes;
    [SerializeField] private Card cardPrefab;
    [SerializeField] private Transform cardParent;


    private List<Card> allCards = new List<Card>();
    private List<Card> revealedCards = new List<Card>();
    private int totalPairs;
    private int matchedPairs = 0;
    private bool isCheckingMatch = false;


    protected override void Awake()
    {
        base.Awake();
    }
    private void Start()
    {
        InitializeGame();
    }

    private void InitializeGame()
    {
        totalPairs = (gridHeight * gridWidth) / 2;
        CreateCards(totalPairs);
    }

     public void GenerateLevel(int difficulty)
    {
        SetDifficulty(difficulty);
        InitializeGame();
    }

    public void SetDifficulty(int level)
    {
        switch (level)
        {
            case 1:
                gridHeight = 2;
                gridWidth = 2;
                break;

            case 2:
                gridHeight = 2;
                gridWidth = 3;
                break;

            case 3:
                gridHeight = 2;
                gridWidth = 4;
                break;

            default:
                gridHeight = 2;
                gridWidth = 2;
                break;

        }
    }

    private void CreateCards(int totalPairs)
    {
        foreach (Transform child in cardParent)
            Destroy(child.gameObject);
        allCards.Clear();
        matchedPairs = 0;

        List<int> cardIDs = new List<int>();
        for (int i = 0; i < totalPairs; i++)
        {
            cardIDs.Add(i);
            cardIDs.Add(i);
        }

        for (int i = cardIDs.Count - 1; i > 0; i--)
        {
            int rand = Random.Range(0, i + 1);
            int temp = cardIDs[i];
            cardIDs[i] = cardIDs[rand];
            cardIDs[rand] = temp;
        }

        for (int i = 0; i < cardIDs.Count; i++)
        {
            Card newCard = Instantiate(cardPrefab, cardParent);

            int id = cardIDs[i];

            newCard.Initialize(cardTypes[id]);
            allCards.Add(newCard);
        }
    }

    public void OnCardClicked(Card selectedCard)
    {
        if (isCheckingMatch || selectedCard.IsRevealed())
            return;

        revealedCards.Add(selectedCard);

        if (revealedCards.Count == 2)
        {
            isCheckingMatch = true;

            DOVirtual.DelayedCall(cardRevealTime, () =>
            {
                CheckForMatch();
                isCheckingMatch = false;

            });
        }
    }

    private void CheckForMatch()
    {
        if (revealedCards.Count < 2) return;

        Card c1 = revealedCards[0];
        Card c2 = revealedCards[1];

        if (c1.GetCardID() == c2.GetCardID())
        {
            c1.SetMatched();
            c2.SetMatched();
            matchedPairs++;

            if (matchedPairs >= totalPairs)
            {
                SoundManager.Instance.PlayVictory();
            }
            else
            {
                SoundManager.Instance.PlayMatch();
            }
        }
        else
        {
            c1.Hide();
            c2.Hide();
            SoundManager.Instance.PlayMismatch();
        }

        revealedCards.Clear();
    }

    public bool CanPlayerSelectCard()
    {
        // Eğer bir eşleşme kontrolü beklemiyorsak VE henüz 2 kart seçilmediyse, oyuncu seçebilir.
        return isCheckingMatch == false && revealedCards.Count < 2;
    }

}
