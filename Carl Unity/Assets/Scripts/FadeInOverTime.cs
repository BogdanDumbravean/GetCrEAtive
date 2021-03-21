using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FadeInOverTime : MonoBehaviour
{
    public float time;
    private TextMeshProUGUI text;
    private float timer;

    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        timer = 0;
    }

    void Update()
    {
        text.color = new Color(text.color.r, text.color.g, text.color.b, timer/time);
        if(timer < time)
            timer += Time.deltaTime;
        else if(timer > time)
            time = timer;
    }
}
