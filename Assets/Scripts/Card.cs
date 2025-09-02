using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Card : MonoBehaviour
{

    [Header("Card Settings")]
    private CardData cardData;
    [SerializeField] private Image frontImage;
    [SerializeField] private Image backImage;
    [SerializeField] private Button cardButton;
    [SerializeField] private float flipDuration = 0.4f;
    [SerializeField] private Ease flipEase = Ease.OutQuad;

    //Card State
    private bool isRevealed = false;
    private bool isMatched = false;

    public void Initialize(CardData data)
    {
        this.cardData = data;
        frontImage.sprite = data.frontSprite;
        backImage.sprite = data.backSprite;

        transform.localEulerAngles = Vector3.zero;
        frontImage.gameObject.SetActive(false);
        backImage.gameObject.SetActive(true);

        isRevealed = false;
        isMatched = false;
    }

    public void OnClicked()
    {
        if (GameManager.Instance.CanPlayerSelectCard() == false)
            return;

        if (isRevealed)
            return;

        SoundManager.Instance.PlayClick();

        Reveal();

        GameManager.Instance.OnCardClicked(this);
    }


    public void Reveal()
    {
        //buradaki if koşulunu ( if (isFlipping || isRevealed)) tamamen kaldırdım çünkü onclicked metodunda
        //bunun kontrolünü yapıyor ve ona göre reveal metodunu çağırıyorum.

        cardButton.interactable = false;

        Sequence sequence = DOTween.Sequence();

        sequence.Append(transform.DORotate(new Vector3(0, 90, 0), flipDuration / 2).SetEase(flipEase));
        sequence.AppendCallback(() =>
        {
            backImage.gameObject.SetActive(false);
            frontImage.gameObject.SetActive(true);
        });
        sequence.Append(transform.DORotate(new Vector3(0, 180, 0), flipDuration / 2).SetEase(flipEase));
        sequence.OnComplete(() =>
        {
            isRevealed = true;
        });

    }

    public void Hide()
    {
        if (!isRevealed || isMatched)
            return;

        //cardButton.interactable = false;
        isRevealed = false;

        Sequence sequence = DOTween.Sequence();

        sequence.Append(transform.DORotate(new Vector3(0, 90, 0), flipDuration / 2).SetEase(flipEase));
        sequence.AppendCallback(() =>
        {
            frontImage.gameObject.SetActive(false);
            backImage.gameObject.SetActive(true);
        });
        sequence.Append(transform.DORotate(new Vector3(0, 0, 0), flipDuration / 2).SetEase(flipEase));
        sequence.OnComplete(() =>
        {
            transform.localEulerAngles = Vector3.zero;
            cardButton.interactable = true;

        });
    }
    //!!!!! SOR: card flipi buraya taşıdıktan sonra celeb için kullandığım 
    //dotween animasyonları çalışmıyor
    public void SetMatched()
    {
        isMatched = true;
        isRevealed = true;
        cardButton.interactable = false;
        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOScale(1.15f, 0.2f).SetEase(Ease.OutQuad));
        sequence.Append(transform.DOScale(1f, 0.2f).SetEase(Ease.InQuad));
    }


    public int GetCardID()
    {
        return cardData.cardID;
    }
    public bool IsRevealed() => isRevealed;
    public bool IsMatched() => isMatched;
}
