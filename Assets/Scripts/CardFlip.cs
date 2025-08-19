using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CardFlip : MonoBehaviour
{
    [Header("Flip Settings")]
    [SerializeField] private float flipDuration = 0.3f;
    [SerializeField] private AnimationCurve flipCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [Header("Images")]
    [SerializeField] private Image frontImage;   // Prefabda atayın (Front)
    [SerializeField] private Image backImage;    // Prefabda atayın (Back)

    private bool isFlipping = false;
    private Coroutine flipCoroutine;

    // === Celebration FX ===
    [Header("Match Celebration")]
    [SerializeField] private float celebrateDuration = 0.35f;
    [SerializeField] private AnimationCurve celebrateCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    [SerializeField] private float bounceScale = 1.15f;

    [Tooltip("UI > Effects > Outline (Front image'a eklemen önerilir)")]
    [SerializeField] private Outline glowOutline;   // opsiyonel
    [SerializeField] private float maxGlowDistance = 6f;

    [Tooltip("Shine için küçük bir Image + CanvasGroup (Front altında olabilir)")]
    [SerializeField] private CanvasGroup shineGroup; // opsiyonel

    private void Awake()
    {
        // Referanslar inspector'da boşsa isimden bulmayı dener
        if (frontImage == null)
            frontImage = transform.Find("Front")?.GetComponent<Image>();
        if (backImage == null)
            backImage = transform.Find("Back")?.GetComponent<Image>();

        // Başlangıç güvenliği
        if (frontImage != null && backImage != null)
        {
            // Başlangıçta back açık kalsın, front kapalı
            frontImage.gameObject.SetActive(false);
            backImage.gameObject.SetActive(true);
        }

        if (shineGroup != null)
        {
            shineGroup.alpha = 0f;
            shineGroup.gameObject.SetActive(false);
        }
        if (glowOutline != null)
        {
            glowOutline.enabled = false;
            glowOutline.effectDistance = Vector2.zero;
        }
    }

    /// <summary>
    /// Kartı animasyonla çevirir. İsteğe bağlı sprite günceller.
    /// </summary>
    public void FlipCard(bool showFront, Sprite frontSprite = null, Sprite backSprite = null)
    {
        if (isFlipping) return;

        // Spriteları güncelle
        if (frontSprite != null && frontImage != null) frontImage.sprite = frontSprite;
        if (backSprite != null && backImage != null) backImage.sprite = backSprite;

        if (flipCoroutine != null) StopCoroutine(flipCoroutine);
        flipCoroutine = StartCoroutine(FlipAnimation(showFront));
    }

    private IEnumerator FlipAnimation(bool showFront)
    {
        isFlipping = true;

        Vector3 baseScale = transform.localScale;
        float half = flipDuration * 0.5f;

        // 1) 1 -> 0 (X ekseninde)
        float t = 0f;
        while (t < half)
        {
            t += Time.deltaTime;
            float p = flipCurve.Evaluate(t / half);
            float sx = Mathf.Lerp(1f, 0f, p);
            transform.localScale = new Vector3(sx, baseScale.y, baseScale.z);
            yield return null;
        }

        // Yüzleri değiştir
        if (frontImage != null) frontImage.gameObject.SetActive(showFront);
        if (backImage != null)  backImage.gameObject.SetActive(!showFront);

        // 2) 0 -> 1 (X ekseninde)
        t = 0f;
        while (t < half)
        {
            t += Time.deltaTime;
            float p = flipCurve.Evaluate(t / half);
            float sx = Mathf.Lerp(0f, 1f, p);
            transform.localScale = new Vector3(sx, baseScale.y, baseScale.z);
            yield return null;
        }

        transform.localScale = baseScale;
        isFlipping = false;
    }

    /// <summary>
    /// Animasyonsuz anında ön/arka gösterimini ayarlar. (ilk kurulumda ideal)
    /// </summary>
    public void SetCardImmediate(bool showFront, Sprite frontSprite = null, Sprite backSprite = null)
    {
        if (frontSprite != null && frontImage != null) frontImage.sprite = frontSprite;
        if (backSprite != null && backImage != null) backImage.sprite = backSprite;

        if (frontImage != null) frontImage.gameObject.SetActive(showFront);
        if (backImage != null)  backImage.gameObject.SetActive(!showFront);

        // olası flip kalıntısı için
        transform.localScale = Vector3.one;
        isFlipping = false;
    }

    /// <summary>
    /// Doğru eşleşmede süslü kutlama (bounce + glow + shine)
    /// Card.SetMatched() tarafından çağrılır.
    /// </summary>
    public IEnumerator PlayMatchCelebrate()
    {
        Vector3 baseScale = transform.localScale;

        // Outline başlangıcını sıfırla
        Color outlineColor = Color.white;
        if (glowOutline != null)
        {
            outlineColor = glowOutline.effectColor;
            glowOutline.enabled = true;
            glowOutline.effectColor = new Color(outlineColor.r, outlineColor.g, outlineColor.b, 0f);
            glowOutline.effectDistance = Vector2.zero;
        }

        if (shineGroup != null)
        {
            shineGroup.alpha = 0f;
            shineGroup.gameObject.SetActive(true);
        }

        float t = 0f;
        while (t < celebrateDuration)
        {
            t += Time.deltaTime;
            float p = celebrateCurve.Evaluate(t / celebrateDuration);

            // 1) Bounce scale
            float s = Mathf.Lerp(1f, bounceScale, p);
            transform.localScale = new Vector3(s, s, s);

            // 2) Glow (Outline) — ortada pik yapıp geri insin
            if (glowOutline != null)
            {
                float half = celebrateDuration * 0.5f;
                float upDown = (t <= half)
                    ? Mathf.InverseLerp(0f, half, t)
                    : 1f - Mathf.InverseLerp(half, celebrateDuration, t);

                glowOutline.effectColor = new Color(outlineColor.r, outlineColor.g, outlineColor.b, upDown);
                float dist = Mathf.Lerp(0f, maxGlowDistance, upDown);
                glowOutline.effectDistance = new Vector2(dist, dist);
            }

            // 3) Shine — hafif dön + fade in/out
            if (shineGroup != null)
            {
                shineGroup.alpha = Mathf.SmoothStep(0f, 1f, p);
                shineGroup.transform.Rotate(0f, 0f, 180f * Time.deltaTime);
            }

            yield return null;
        }

        // toparla
        transform.localScale = baseScale;
        if (glowOutline != null)
        {
            glowOutline.effectColor = new Color(outlineColor.r, outlineColor.g, outlineColor.b, 0f);
            glowOutline.effectDistance = Vector2.zero;
            glowOutline.enabled = false;
        }
        if (shineGroup != null)
        {
            shineGroup.alpha = 0f;
            shineGroup.gameObject.SetActive(false);
            shineGroup.transform.localRotation = Quaternion.identity;
        }
    }

    /// <summary>
    /// (Opsiyonel) Yanlış eşleşmede ufak wiggle efekti.
    /// </summary>
    public IEnumerator PlayWrongWiggle(float amplitude = 6f, float duration = 0.2f, int oscillations = 2)
    {
        float t = 0f;
        Quaternion baseRot = transform.rotation;
        while (t < duration)
        {
            t += Time.deltaTime;
            float s = Mathf.Sin(t / duration * Mathf.PI * oscillations);
            transform.rotation = baseRot * Quaternion.Euler(0f, 0f, s * amplitude);
            yield return null;
        }
        transform.rotation = baseRot;
    }
}
