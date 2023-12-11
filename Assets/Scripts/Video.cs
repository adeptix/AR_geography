using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class Video : MonoBehaviour
{

    [SerializeField] private Button playButton;
    [SerializeField] private Image btnImage;
    [SerializeField] private Image insideImage;
    
    [SerializeField] private Sprite pauseSprite;
    [SerializeField] private Sprite playSprite;
    
    [SerializeField] private VideoPlayer player;

    private RenderTexture texture;

    private bool isPlaying;

    void Start()
    {
        playButton.gameObject.SetActive(true);
        isPlaying = false;
        
        texture = new RenderTexture(1280, 720, 16);
        player.targetTexture = texture;
        gameObject.GetComponent<RawImage>().texture = texture;
    }

    public void ClickPlayAction()
    {
        if (isPlaying)
        {
            player.Pause();
            isPlaying = false;
            ChangeTransparent(btnImage, 1);
            insideImage.sprite = playSprite;
            insideImage.color = Color.black;
        }
        else
        {
            player.Play();
            isPlaying = true;
            ChangeTransparent(btnImage, 0);
            insideImage.sprite = pauseSprite;
            insideImage.color = Color.black;
        }
    }

    private void ChangeTransparent(Image image, float a)
    {
        var col = image.color;
        col.a = a;
        image.color = col;
    }
}
