using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFade : MonoBehaviour
{
    private Image fadeImage;

    public void Init()
    {
        fadeImage = GetComponentInChildren<Image>(true);
        DontDestroyOnLoad(gameObject);
        fadeImage.gameObject.SetActive(false);
    }

    IEnumerator IFade(Color start, Color ene, float speed)
    {
        fadeImage.gameObject.SetActive(true);
        float elapsed = 0;
        while(true)
        {
            elapsed += Time.deltaTime/speed;
            fadeImage.color = Color.Lerp(start, ene, elapsed);
            
            if (elapsed >= 1.0f) break;

            yield return null;
        }
        yield return null;
    }

    public void FadeIn(float speed =1)
    {
        StartCoroutine(IFade(Color.black, new Color(0,0,0,0), speed));
        Invoke("DeactiveImage", speed);
    }

    void DeactiveImage()
    {
        fadeImage.gameObject.SetActive(false);
    }

    public void FadeOut(float speed = 1)
    {
        StartCoroutine(IFade(new Color(0, 0, 0, 0), Color.black, speed));
        
    }
}
