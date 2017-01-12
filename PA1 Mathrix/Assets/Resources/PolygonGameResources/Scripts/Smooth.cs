using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public static class Smooth {

    static public IEnumerator Fade(Image image, float targetAlpha, float duration)
    {
        Color targetColor = image.color;
        targetColor.a = targetAlpha;
        float smoothness = 0.02f;
        float progress = 0; //This float will serve as the 3rd parameter of the lerp function.
        float increment = smoothness / duration; //The amount of change to apply.
        while (progress < 1)
        {
            image.color = Color.Lerp(image.color, targetColor, progress);
            progress += increment;
            yield return new WaitForSeconds(smoothness);
        }
    }


    static public IEnumerator Fade(Material mat, float targetAlpha, float duration)
    {
        Color targetColor = mat.color;
        targetColor.a = targetAlpha;
        float smoothness = 0.02f;
        float progress = 0; //This float will serve as the 3rd parameter of the lerp function.
        float increment = smoothness / duration; //The amount of change to apply.
        while (progress < 1)
        {
            mat.color = Color.Lerp(mat.color, targetColor, progress);
            progress += increment;
            yield return new WaitForSeconds(smoothness);
        }
    }

    static public IEnumerator Fade(Text text, float targetAlpha, float duration)
    {
        Color targetColor = text.color;
        targetColor.a = targetAlpha;
        float smoothness = 0.02f;
        float progress = 0; //This float will serve as the 3rd parameter of the lerp function.
        float increment = smoothness / duration; //The amount of change to apply.
        while (progress < 1)
        {
            text.color = Color.Lerp(text.color, targetColor, progress);
            progress += increment;
            yield return new WaitForSeconds(smoothness);
        }
    }

    static public IEnumerator Fade(SpriteRenderer sprite, float targetAlpha, float duration)
    {
        Color targetColor = sprite.color;
        targetColor.a = targetAlpha;
        float smoothness = 0.02f;
        float progress = 0; //This float will serve as the 3rd parameter of the lerp function.
        float increment = smoothness / duration; //The amount of change to apply.
        while (progress < 1)
        {
            sprite.color = Color.Lerp(sprite.color, targetColor, progress);
            progress += increment;
            yield return new WaitForSeconds(smoothness);
        }
    }
}
