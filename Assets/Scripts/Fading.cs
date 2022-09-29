using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Fading
{
    public static IEnumerator FadeInText(float speed, TextMeshProUGUI text)
    {
        if (text.color.a != 1.0f)
        {
            DOTween
                .To(
                    () => text.color,
                    x => text.color = x,
                    new Color(text.color.r, text.color.g, text.color.b, 1.0f),
                    speed
                )
                .SetOptions(true);
            yield return null;
        }
    }

    public static IEnumerator FadeInText(float speed, TextMesh text)
    {
        if (text.color.a != 1.0f)
        {
            DOTween
                .To(
                    () => text.color,
                    x => text.color = x,
                    new Color(text.color.r, text.color.g, text.color.b, 1.0f),
                    speed
                )
                .SetOptions(true);
            yield return null;
        }
    }

    public static IEnumerator FadeOutText(float speed, TextMeshProUGUI text)
    {
        if (text.color.a != 0.0f)
        {
            DOTween
                .To(
                    () => text.color,
                    x => text.color = x,
                    new Color(text.color.r, text.color.g, text.color.b, 0.0f),
                    speed
                )
                .SetOptions(true);
            yield return null;
        }
    }

    public static IEnumerator FadeOutText(float speed, TextMesh text)
    {
        if (text.color.a != 0.0f)
        {
            DOTween
                .To(
                    () => text.color,
                    x => text.color = x,
                    new Color(text.color.r, text.color.g, text.color.b, 0.0f),
                    speed
                )
                .SetOptions(true);
            yield return null;
        }
    }

    public static IEnumerator FadeInImage(float speed, Image image)
    {
        while (image.color.a < 1.0f)
        {
            image.color = new Color(
                image.color.r,
                image.color.g,
                image.color.b,
                image.color.a + (Time.deltaTime * speed)
            );
            yield return null;
        }
    }

    public static IEnumerator FadeOutImage(float speed, Image image)
    {
        while (image.color.a > 0.0f)
        {
            image.color = new Color(
                image.color.r,
                image.color.g,
                image.color.b,
                image.color.a - (Time.deltaTime * speed)
            );
            yield return null;
        }
    }
}
