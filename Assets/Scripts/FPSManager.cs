using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPSManager : MonoBehaviour
{
    public Text switchText;

    [SerializeField]
    RectTransform handler;

    [SerializeField]
    Color backgroundActiveColor;

    [SerializeField]
    Color handleActiveColor;

    [SerializeField]
    Color switchTextActiveColor;

    Image backgroundImage, handleImage;
    Color backgroundDefaultColor, handleDefaultColor;

    Toggle toggle;

    Vector2 handlePosition;

    private int gameFPS;

    private void Awake()
    {
        if (!PlayerPrefs.HasKey("gameFrame"))
        {
            PlayerPrefs.SetInt("gameFrame", 75);
        }
        toggle = GetComponent<Toggle>();

        startToggleState();

        handlePosition = handler.anchoredPosition;

        backgroundImage = handler.parent.GetComponent<Image>();
        handleImage = handler.GetComponent<Image>();

        backgroundDefaultColor = backgroundImage.color;
        handleDefaultColor = handleImage.color;

        toggle.onValueChanged.AddListener(OnSwitch);

    }

    private void Start()
    {
        gameFPS = PlayerPrefs.GetInt("gameFrame");
        startToggleState();
    }

    void OnSwitch (bool on)
    {
        Debug.Log("Toggle value changed: " + on);
        handler.anchoredPosition = on ? handlePosition * -1 : handlePosition;
        backgroundImage.color = on ? backgroundActiveColor : backgroundDefaultColor;
        handleImage.color = on ? handleActiveColor : handleDefaultColor;
        switchText.color = on ? switchTextActiveColor : Color.white;

        if (on)
        {
            PlayerPrefs.SetInt("gameFrame", 30);
        }
        else
        {
            PlayerPrefs.SetInt("gameFrame", 75);
        }

    }

    private void OnDestroy()
    {
        toggle.onValueChanged.RemoveListener(OnSwitch);
    }

    private void startToggleState()
    {

        if (!toggle.isOn)
        {
            if (gameFPS == 30)
            {
                OnSwitch(true);
                toggle.isOn = true;
            }

        if (toggle.isOn)
            {
                if(gameFPS == 75)
                {
                    OnSwitch(false);
                    toggle.isOn = false;
                }
            }

        }

    }

}
