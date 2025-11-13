using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class FadeScreen : MonoBehaviour
{
    public bool fadeOnStart = true;
    public float fadeDuration = 5.0f;
    public Color fadeColor;
    private Renderer rend;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        rend = GetComponent<Renderer>();
    }
    void Start()
    {
        //rend = GetComponent<Renderer>();
        if (fadeOnStart) FadeOut();
    }

    public void FadeIn()
    {
        Debug.Log("Fading in");
        Fade(0, 1);
    }

    public void FadeOut()
    {
        Fade(1, 0);
    }
    public void Fade(float alphaIn, float alphaOut)
    {
        StartCoroutine(FadeRoutine(alphaIn, alphaOut));
    }
    public IEnumerator FadeRoutine(float alphaIn, float alphaOut)
    {
        float timer = 0;
        while (timer <= fadeDuration)
        {
            Color newColor = fadeColor;
            newColor.a = Mathf.Lerp(alphaIn, alphaOut, timer / fadeDuration);
            rend.material.SetColor("_Color", newColor);
            timer += Time.deltaTime;
            yield return null;
        }
        Color newColor2 = fadeColor;
        newColor2.a = alphaOut;
        rend.material.SetColor("_Color", newColor2);
        this.gameObject.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
