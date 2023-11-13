using UnityEngine;

// QUIZ MODEL
[System.Serializable]
public class QuizData {
    public QuizRaw[] quizRaws; 
} 

[System.Serializable]
public class QuizRaw {
    public int id;
    public string name;
    public string quizType;
    public string text;
    public Answer[] answers;
    public string[] additionalData;
}

[System.Serializable]
public class Answer {
    public int id;
    public string text;
    public string imageName;
    public bool correct;
}



// INFO MODEL
[System.Serializable]
public class InfoData {
    public InfoRaw[] infoRaws; 
}

[System.Serializable]
public class InfoRaw {
    public int id;
    public string name;
    public string population;
    public string text;

    public string videoName;
    public string flagName;
    public string anthemName;
    public string[] images;
}