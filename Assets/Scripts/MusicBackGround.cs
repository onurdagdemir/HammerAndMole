using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicBackGround : MonoBehaviour
{

    public AudioSource audioSource;
    public Sprite musicOnSprite;
    public Sprite musicOffSprite;
    public Button musicButton;
    private int musicOn;

    // Start is called before the first frame update
    private void Start()
    {

        if (!PlayerPrefs.HasKey("MusicOn"))
        {
            PlayerPrefs.SetInt("MusicOn", 1);
        }

        musicOn = PlayerPrefs.GetInt("MusicOn");


        UpdateIcon(musicOn);


        if(musicOn == 1)
        {
            audioSource.Play();
            audioSource.loop = true;
        }


    }

    public void ToggleMusic()
    {
        if(musicOn == 1)
        {
            musicOn = 0;
        }
        else
        {
            musicOn = 1;
        }



        PlayerPrefs.SetInt("MusicOn", musicOn);


        UpdateIcon(musicOn);


        if (musicOn == 1)
        {
            audioSource.Play();
            audioSource.loop = true;
        } else
        {
            audioSource.Pause();
        }
    }

    private void UpdateIcon(int musicOn)
    {
        Image musicButtonImage = musicButton.GetComponent<Image>();
        if (musicOn == 1)
        {
            musicButtonImage.sprite = musicOnSprite;
        }
        else
        {
            musicButtonImage.sprite = musicOffSprite;
        }
    }

}
