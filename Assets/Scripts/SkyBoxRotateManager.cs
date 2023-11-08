using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyBoxRotateManager : MonoBehaviour
{
    public Material EarthSkyBox;
    public Material MarsSkyBox;
    private Material CurrentSkyBox;
    private float rotationSpeed = 1.0f; // Skybox dönme hızı
    private float currentRotation = 45f;
    // Start is called before the first frame update
    void Start()
    {
        if(PlayerPrefs.GetInt("terrainType") == 1)
        {
            CurrentSkyBox = EarthSkyBox;
        }
        else if (PlayerPrefs.GetInt("terrainType") == 2)
        {
            CurrentSkyBox = MarsSkyBox;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (currentRotation <= 280f)
        {
            currentRotation += rotationSpeed * Time.deltaTime;

            // Yeni rotasyon değerini materyale uygula
            CurrentSkyBox.SetFloat("_Rotation", currentRotation);
        }
    }
}
