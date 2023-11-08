using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicGameBackGround : MonoBehaviour
{
    public AudioSource audioSourceGame;
    public AudioSource audioSourcePauseMenu;
    public GameObject PauseScreen;
    private bool isPlayingGameMusicP = false;
    private bool isPlayingPauseMenuMusic = false;

    public Sprite musicOnSprite;
    public Sprite musicOffSprite;
    public Button musicButton;
    private int musicOn;

    // Start is called before the first frame update
    void Start()
    {

        musicOn = PlayerPrefs.GetInt("MusicOn");


        UpdateIcon(musicOn);


        if (musicOn == 1)
        {
            PlayGameMusic();
            PlayGameMusicLoop();
        }

    }

    // Update is called once per frame
    void Update()
    {

        if(musicOn == 1)
        {
            if (PauseScreen.activeSelf && isPlayingPauseMenuMusic == false)
            {
                audioSourceGame.Pause();
                PlayPauseMenuMusic();
                isPlayingGameMusicP = false;
                isPlayingPauseMenuMusic = true;
            }
            if (!PauseScreen.activeSelf && isPlayingGameMusicP == false)
            {
                PlayGameMusic();
                audioSourcePauseMenu.Pause();
                isPlayingGameMusicP = true;
                isPlayingPauseMenuMusic = false;
            }
        }


    }


    private void PlayGameMusic()
    {
        audioSourceGame.Play();
    }

    private void PlayGameMusicLoop()
    {
        audioSourceGame.loop = true;
    }

    private void PlayPauseMenuMusic()
    {
        audioSourcePauseMenu.Play();
        audioSourcePauseMenu.loop = true;
    }

    public void ToggleMusic()
    {
        if (musicOn == 1)
        {
            musicOn = 0;
        }
        else
        {
            musicOn = 1;
        }



        PlayerPrefs.SetInt("MusicOn", musicOn);


        UpdateIcon(musicOn);


        if (musicOn == 0)
        {
            audioSourceGame.Pause();
            audioSourcePauseMenu.Pause();
        }
        else
        {
            audioSourcePauseMenu.Play();
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
