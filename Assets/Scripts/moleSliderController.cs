using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class moleSliderController : MonoBehaviour
{

    public Text moleCurrentHealthValue;
    public Slider moleSlider;

    // Start is called before the first frame update
    void Start()
    {
        float value;
        if (float.TryParse(moleCurrentHealthValue.text, out value))
        {
            moleSlider.value = value;
        }

    }


    // Update is called once per frame
    void Update()
    {
        float value;
        if (float.TryParse(moleCurrentHealthValue.text, out value))
        {
            moleSlider.value = value;
        }

    }
}
