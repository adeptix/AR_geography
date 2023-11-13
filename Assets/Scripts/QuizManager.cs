using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class QuizManager : MonoBehaviour
{
    private JsonLoader jsonLoaderScript;

    [SerializeField] private Button startButton;
    [SerializeField] private Button endButton;

    [SerializeField] private GameObject resultHeader;
    [SerializeField] private TMP_Text resultText;

    [SerializeField] private GameObject scrollView;
    [SerializeField] private TMP_Text counterText;
    [SerializeField] private TMP_Text descriptionText;

    [SerializeField] private Button[] answerButtons;

    

    private QuizRaw[] quizRaws;
    private int count;
    private int correctCount;

    void Start()
    {
        jsonLoaderScript = FindObjectOfType<JsonLoader>();

        startButton.onClick.AddListener(StartQuizAction);
        endButton.onClick.AddListener(QuitAction);
        scrollView.SetActive(false);
    }

    private void StartQuizAction() {
        quizRaws = jsonLoaderScript.GetRandomizedQuizList();
        count = -1;
        correctCount = 0;

        NextQuestion();
        scrollView.SetActive(true);
        startButton.gameObject.SetActive(false);
        endButton.gameObject.SetActive(true);
    }

    private void NextQuestion() {
        count++;
        if (count > quizRaws.Length - 1) {
            ShowResult();
            return;
        }

        var oneQuiz = quizRaws[count];
        
        counterText.text = string.Format("{0} / {1}", count + 1, quizRaws.Length);
        descriptionText.text = oneQuiz.text;

        HideElemets();

        if (oneQuiz.quizType == "text") {
            RenderTextQuestion(oneQuiz);
        }
        //todo: add more types
    }

    private void RenderTextQuestion(QuizRaw oneQuiz) {
        for (int i = 0; i < oneQuiz.answers.Length; i++) {
            if (i > answerButtons.Length - 1) {
                break;
            }

            answerButtons[i].onClick.RemoveAllListeners();

            var answerCopy = oneQuiz.answers[i];

            answerButtons[i].onClick.AddListener(() => {AnswerClicked(answerCopy);});
            answerButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = oneQuiz.answers[i].text;

            answerButtons[i].gameObject.SetActive(true);
        }
    }

    private void HideElemets() {
        foreach (var b in answerButtons) {
            b.gameObject.SetActive(false);
        }

        //todo: add hide images / video / etc..
    }

    private void AnswerClicked(Answer answer) {
        if (answer.correct) {
            correctCount++;
        }

        NextQuestion();
    }

    private void ShowResult() {
        resultText.text = FormatResult();
        resultHeader.SetActive(true);

        quizRaws = null;
        scrollView.SetActive(false);
    }

    private string FormatResult() {
        var allCount = quizRaws.Length;

        var percent = Mathf.RoundToInt((float)correctCount / (float) allCount * 100);

        var reaction = percent switch
        {
            <= 20 => "Попробуйте еще раз",
            > 20 and <= 45 => "Есть знания, но их мало",
            > 45 and <= 60 => "Средний уровень, на троечку",
            > 60 and <= 85 => "Неплохо",
            > 85 and < 100 => "Отлично, почти идеально",
            100 => "Идеально",
            _ => "Если вы видите это - то в программе баг",
        };

        return string.Format("{0}% - {1}", percent, reaction);
    }

    private void QuitAction() {
        quizRaws = null;

        scrollView.SetActive(false);

        startButton.gameObject.SetActive(true);
        endButton.gameObject.SetActive(false);

        resultHeader.SetActive(false);
    }
}
