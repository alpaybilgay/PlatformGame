using System;
using TMPro;
using UnityEngine;

public class HealtText : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public Vector3 moveSpeed = new Vector3(0,75,0);
    public float timeToFade = 1f;

    RectTransform textTransform;
    TextMeshProUGUI textMeshPro;


    private float timeElapsed = 0f;
    private Color startColor;


    public void Awake()
    {
        textTransform = GetComponent<RectTransform>();
        textMeshPro = GetComponent<TextMeshProUGUI>();
        startColor = textMeshPro.color;
    }




    private void Update()
    {
        textTransform.position += moveSpeed * Time.deltaTime;
        
        timeElapsed += Time.deltaTime;

        if(timeElapsed < timeToFade)
        {
            float fadeAlpha= startColor.a * (1 - (timeElapsed / timeToFade));
            textMeshPro.color = new Color(startColor.r,startColor.g,startColor.b , fadeAlpha);
        } else
        {
            Destroy(gameObject);
        }
    }




}
