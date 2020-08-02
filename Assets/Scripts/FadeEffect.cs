using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeEffect : MonoBehaviour
{
    public Image image;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            StartCoroutine(PerformFadding(2f));
        }
    }

    IEnumerator PerformFadding(float time)
    {
        float i = 0;
        while (i < 1.01f)
        {
            image.color = new Color(0.0f, 0.0f, 0.0f, i);
            i += .10f;
            yield return new WaitForSeconds(time / 4);
        }
        image.color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
    }
}
