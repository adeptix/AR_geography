using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using Random = Unity.Mathematics.Random;

public class ResourcesLoader : MonoBehaviour
{
    private QuizData QuizDataStorage;
    private InfoData InfoDataStorage;

    private const string JSON_PATH = "Json/";
    private const string QUIZ_PATH = "Quiz/quiz";
    private const string INFO_PATH = "Info/info";

    private const string IMAGES_INFO = "Images/Info/";
    private const string IMAGES_QUIZ = "Images/Quiz/";
    
    private const string VIDEOS_INFO = "Videos/Info/";

    private Random random;

    void Start()
    {
       LoadQuizStorage(); 
       LoadInfoStorage();

       random = new Random((uint)System.DateTime.Now.Millisecond);
    }

    public QuizCountry GetRandomizedQuizList(int countryID, int min = 4, int max = 8) {
        if (QuizDataStorage == null || QuizDataStorage.quizCountry == null) {
            return null;
        }

        var qc = QuizDataStorage.quizCountry.FirstOrDefault(qc => qc.countryID == countryID);
        if (qc == null || qc.quizes == null) {
            return null;
        }
        
        var length = qc.quizes.Length;
        
        min = Mathf.Min(length, min);
        max = Mathf.Min(length, max);
        
        var chosen = ShuffleAndTake(qc.quizes, random.NextInt(min, max+1));

        for (int i = 0; i < chosen.Length; i++) {
            chosen[i].answers = ShuffleAndTake(chosen[i].answers);
        }

        var result = new QuizCountry
        {
            countryID = qc.countryID,
            countryName = qc.countryName,
            quizes = chosen,
        }; // copy of quiz with changes

        return result;
    }

    private T[] ShuffleAndTake<T>(T[] array, int takeCount = 0) {
        var shuffled = array.OrderBy(c => random.NextInt());

        if (takeCount == 0) {
            return shuffled.ToArray();
        }

        return shuffled.Take(takeCount).ToArray();
    }

    public Info GetInfoByID(int countryID) {
        if (InfoDataStorage == null || InfoDataStorage.infoRaws == null) {
            return null;
        }

        return InfoDataStorage.infoRaws.FirstOrDefault(inf => inf.id == countryID);
    }

    private void LoadQuizStorage() {
        var quizFile = Resources.Load<TextAsset>(JSON_PATH + QUIZ_PATH);
        if (quizFile == null) {
            return;
        }

        QuizDataStorage = JsonUtility.FromJson<QuizData>(quizFile.text);
    }

    private void LoadInfoStorage() {
        var infoFile = Resources.Load<TextAsset>(JSON_PATH + INFO_PATH);
        if (infoFile == null) {
            return;
        }

        InfoDataStorage = JsonUtility.FromJson<InfoData>(infoFile.text);
    }

    public List<Sprite> GetImages(string[] images)
    {
        if (images == null)
        {
            return null;
        }

        List<Sprite> list = new List<Sprite>();
        
        foreach (var imageName in images)
        {
            list.Add(Resources.Load<Sprite>(IMAGES_INFO + imageName));
        }

        return list;
    }
    
    public List<VideoClip> GetVideos(string[] videos)
    {
        if (videos == null)
        {
            return null;
        }

        List<VideoClip> list = new List<VideoClip>();
        
        foreach (var videoName in videos)
        {
            list.Add(Resources.Load<VideoClip>(VIDEOS_INFO + videoName));
        }

        return list;
    }
}
