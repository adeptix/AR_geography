using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class InfoManager : MonoBehaviour
{
    [SerializeField] private TMP_Text countryText;

    [SerializeField] private GameObject infoScrollPanel;
    [SerializeField] private TMP_Text countryInnerText;
    
    [SerializeField] private GameObject imageWrapper;
    [SerializeField] private GameObject imageWrapperContent;

    [SerializeField] private GameObject dynamicImagePrefab;
    [SerializeField] private GameObject dynamicVideoPlayerPrefab;

    //todo: add video
    [SerializeField] private TMP_Text description;

    private ResourcesLoader _resourcesLoaderScript;

    void Start()
    {
        _resourcesLoaderScript = FindObjectOfType<ResourcesLoader>();

        infoScrollPanel.SetActive(false);
    }

    public void CountrySelected(int countryID)
    {
        var info = _resourcesLoaderScript.GetInfoByID(countryID);
        countryText.text = "Страна: " + info.name;

        infoScrollPanel.SetActive(true);

        countryInnerText.text = info.name;
        description.text = info.text;
        LoadImagesAndVideos(info);

        RebuildFitters(); // fix ui issues
    }

    private void LoadImagesAndVideos(Info info)
    {
        bool hasContent = false;
        
        ClearChildren(imageWrapperContent);
        imageWrapper.SetActive(false);
        
        var sprites = _resourcesLoaderScript.GetImages(info.images);
        
        sprites?.ForEach(s =>
        {
            hasContent = true;
            var image = Instantiate(dynamicImagePrefab, imageWrapperContent.transform);
            image.GetComponent<Image>().sprite = s;
        });
        
       
        var videoClips = _resourcesLoaderScript.GetVideos(info.videos);
        videoClips?.ForEach(clip =>
        {
            hasContent = true;
            var videoPlayer = Instantiate(dynamicVideoPlayerPrefab, imageWrapperContent.transform);
            videoPlayer.GetComponent<VideoPlayer>().clip = clip;
        });

        if (hasContent)
        {
            imageWrapper.SetActive(true);
        }
    }

    private void RebuildFitters()
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(description.transform as RectTransform);
        LayoutRebuilder.ForceRebuildLayoutImmediate(imageWrapperContent.transform as RectTransform);
    }

    private void ClearChildren(GameObject parent)
    {
        foreach (Transform tr in parent.transform)
        {
            Destroy(tr.gameObject);
        }
    }

}
