using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = Unity.Mathematics.Random;

public class JsonLoader : MonoBehaviour
{
    private QuizData QuizDataStorage;
    private InfoData InfoDataStorage;

    private const string JSON_PATH = "Json/";
    private const string QUIZ_PATH = "Quiz/quiz";
    private const string INFO_PATH = "Info/info";

    private Random random;

    void Start()
    {
       LoadQuizStorage(); 
       LoadInfoStorage();

       random = new Random((uint)System.DateTime.Now.Millisecond);
    }

    public QuizRaw[] GetRandomizedQuizList(int min = 3, int max = 8) {
        if (QuizDataStorage == null || QuizDataStorage.quizRaws == null) {
            return null;
        }
        
        var length = QuizDataStorage.quizRaws.Length;
        
        min = Mathf.Min(length, min);
        max = Mathf.Min(length, max);
        
        var res = ShuffleAndTake(QuizDataStorage.quizRaws, random.NextInt(min, max+1));

        for (int i = 0; i < res.Length; i++) {
            res[i].answers = ShuffleAndTake(res[i].answers);
        }

        return res;
    }

    private T[] ShuffleAndTake<T>(T[] array, int takeCount = 0) {
        var shuffled = array.OrderBy(c => random.NextInt());

        if (takeCount == 0) {
            return shuffled.ToArray();
        }

        return shuffled.Take(takeCount).ToArray();
    }

    public InfoRaw GetInfoByID(int id) {
        if (InfoDataStorage == null || InfoDataStorage.infoRaws == null) {
            return null;
        }

        return InfoDataStorage.infoRaws.FirstOrDefault(inf => inf.id == id);
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

    
}
