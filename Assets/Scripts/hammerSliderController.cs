using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class hammerSliderController : MonoBehaviour
{

    public Text hammerCurrentHealthValue;
    public Slider hammerSlider;

    void Start()
    {
        float value;
        if (float.TryParse(hammerCurrentHealthValue.text, out value))
        {
            hammerSlider.value = value;
        }

    }


    // Update is called once per frame
    void Update()
    {
        float value;
        if (float.TryParse(hammerCurrentHealthValue.text, out value))
        {
            hammerSlider.value = value;
        }

    }
}
