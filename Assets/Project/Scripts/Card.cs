using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Card : MonoBehaviour
{

    [Header("Card Settings")]
    [SerializeField] private int cardID;
    [SerializeField] private Image frontImage;
    [SerializeField] private Image backImage;
    [SerializeField] private Button cardButton;
    [SerializeField] private float flipDuration = 0.4f;
    [SerializeField] private Ease flipEase = Ease.OutQuad;

    //Card State
    private bool isRevealed = false;
    private bool isMatched = false;
    private bool isFlipping = false;

    public void Initialize(CardData data)
    {
        cardID = data.cardID;
        frontImage.sprite = data.frontSprite;
        backImage.sprite = data.backSprite;

        transform.localEulerAngles = Vector3.zero;
        frontImage.gameObject.SetActive(false);
        backImage.gameObject.SetActive(true);

        isRevealed = false;
        isMatched = false;
        isFlipping = false;
    }

    public void OnClicked()
    {
        if (GameManager.Instance.CanPlayerSelectCard() == false)
            return;
            
        if (isFlipping || isRevealed)
            return;

        SoundManager.Instance.PlayClick();

        Reveal();

        GameManager.Instance.OnCardClicked(this);
    }


    public void Reveal()
    {
        //buradaki if koşulunu ( if (isFlipping || isRevealed)) tamamen kaldırdım çünkü onclicked metodunda
        //bunun kontrolünü yapıyor ve ona göre reveal metodunu çağırıyorum.

        isFlipping = true;

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
            transform.localEulerAngles = new Vector3(0, 180, 0);
            isRevealed = true;
            isFlipping = false;
        });

    }

    public void Hide()
    {
        if (isFlipping || !isRevealed || isMatched)
            return;

        isFlipping = true;
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
            isFlipping = false;
        });
    }
    //!!!!! SOR: card flipi buraya taşıdıktan sonra celeb için kullandığım 
    //dotween animasyonları çalışmıyor
    public void SetMatched()
    {
        isMatched = true;
        isRevealed = true;
        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOScale(1.15f, 0.2f).SetEase(Ease.OutQuad));
        sequence.Append(transform.DOScale(1f, 0.2f).SetEase(Ease.InQuad));
    }


    public int GetCardID() => cardID;
    public bool IsRevealed() => isRevealed;
    public bool IsMatched() => isMatched;
}
