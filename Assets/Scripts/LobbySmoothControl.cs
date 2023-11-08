using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbySmoothControl : MonoBehaviour
{
    public Material targetMaterial;
    public Material targetMaterial2;
    public Material targetMaterial3;

    [Range(0, 1)]
    public float smoothnessSpeed = 0.5f; // Smoothness değişiminin hızı

    private float smoothness = 0.5f; // Başlangıçta 1 olarak ayarlanmış smoothness değeri
    private bool increasing = false; // Değer artırma durumu

    private void Update()
    {
        if (targetMaterial != null)
        {
            // Değeri 1 saniye içinde 0'dan 1'e veya 1'den 0'a döndürün
            if (increasing)
            {
                smoothness += smoothnessSpeed * Time.deltaTime;
                if (smoothness >= 0.5f)
                {
                    smoothness = 0.5f;
                    increasing = false;
                }
            }
            else
            {
                smoothness -= smoothnessSpeed * Time.deltaTime;
                if (smoothness <= 0f)
                {
                    smoothness = 0f;
                    increasing = true;
                }
            }

            // Materyalin smoothness değerini değiştirin
            targetMaterial.SetFloat("_Glossiness", smoothness);
            targetMaterial2.SetFloat("_Glossiness", smoothness);
            targetMaterial3.SetFloat("_Glossiness", smoothness);
        }
        else
        {
            Debug.LogWarning("Target Material is missing.");
        }
    }
}
