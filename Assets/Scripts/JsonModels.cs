using UnityEngine;

// QUIZ MODEL
[System.Serializable]
public class QuizData {
    public QuizCountry[] quizCountry; 
}

[System.Serializable]
public class QuizCountry {
    public int countryID;
    public string countryName;

    public Quiz[] quizes;
}

[System.Serializable]
public class Quiz {
    public int id;
    public string name;
    public string quizType;
    public string text;
    public Answer[] answers;
    public string[] additional;
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
    public Info[] infoRaws; 
}

[System.Serializable]
public class Info {
    public int id;  // country_id
    public string name;
    public string population;
    public string text;
    
    public string[] images;
    public string[] videos;
}