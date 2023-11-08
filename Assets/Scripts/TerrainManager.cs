using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TerrainManager : MonoBehaviour
{
    public Material PlanetEarthSkyBox;
    public Material SpaceMarsSkyBox;
    public Material MarsSkyBox;
    public GameObject SkyBoxRotateManager;
    public GameObject TerrainForest;
    public Text leftCurrentHealth;
    public Text rightCurrentHealth;
    public Text leftBarName;
    public Text rightBarName;
    public Text timeCounter;

    private int terrainType;
    //terrainType 0 Forest, 1 PlanetEarth, 2 Mars

    // Start is called before the first frame update
    void Awake()
    {
        if (!PlayerPrefs.HasKey("terrainType"))
        {
            terrainType = 0;
        }
        else
        {
            terrainType = PlayerPrefs.GetInt("terrainType");
        }

        if(terrainType == 0)
        {
            ForestSet();
        }
        else if (terrainType == 1)
        {
            PlanetEarthSet();
        }
        else if (terrainType == 2)
        {
            SpaceMarsSet();
        }
        else if (terrainType == 3)
        {
            MarsSet();
        }

        Destroy(gameObject);

    }


    private void ForestSet()
    {
        Destroy(SkyBoxRotateManager);
        TerrainForest.SetActive(true);
        leftCurrentHealth.color = Color.black;
        rightCurrentHealth.color = Color.black;
        leftBarName.color = Color.black;
        rightBarName.color = Color.black;
        timeCounter.color = Color.black;  
    }
    private void PlanetEarthSet()
    {
        RenderSettings.skybox = PlanetEarthSkyBox;
        SkyBoxRotateManager.SetActive(true);
        Destroy(TerrainForest);
        leftCurrentHealth.color = Color.white;
        rightCurrentHealth.color = Color.white;
        leftBarName.color = Color.white;
        rightBarName.color = Color.white;
        timeCounter.color = Color.white;
    }

    private void SpaceMarsSet()
    {
        RenderSettings.skybox = SpaceMarsSkyBox;
        SkyBoxRotateManager.SetActive(true);
        Destroy(TerrainForest);
        leftCurrentHealth.color = Color.white;
        rightCurrentHealth.color = Color.white;
        leftBarName.color = Color.white;
        rightBarName.color = Color.white;
        timeCounter.color = Color.white;
    }

    private void MarsSet()
    {
        RenderSettings.skybox = MarsSkyBox;
        Destroy(SkyBoxRotateManager);
        Destroy(TerrainForest);
        leftCurrentHealth.color = Color.black;
        rightCurrentHealth.color = Color.black;
        leftBarName.color = Color.black;
        rightBarName.color = Color.black;
        timeCounter.color = Color.black;
    }

}
