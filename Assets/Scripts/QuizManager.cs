using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class QuizManager : MonoBehaviour
{
    [SerializeField] private TMP_Text countryText;

    [SerializeField] private GameObject startPanel;
    [SerializeField] private Button startButton;

    [SerializeField] private GameObject questionPanel;
    [SerializeField] private Button cancelButton; // cancel running test
    [SerializeField] private TMP_Text counterText;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private Image quizImage;
    [SerializeField] private Button[] answerButtons;

    [SerializeField] private GameObject resultPanel;
    [SerializeField] private TMP_Text resultText;
    [SerializeField] private Button restartButton;

    private ResourcesLoader _resourcesLoaderScript;

    private int countryID;
    private QuizCountry quizCountry;


    private int count;
    private int correctCount;

    void Start()
    {
        _resourcesLoaderScript = FindObjectOfType<ResourcesLoader>();

        startButton.onClick.AddListener(StartQuizAction);
        restartButton.onClick.AddListener(StartQuizAction);
        cancelButton.onClick.AddListener(CancelQuizAction);

        startPanel.SetActive(false);
        questionPanel.SetActive(false);
        resultPanel.SetActive(false);
    }

    public void CountrySelected(int countryID)
    {
        this.countryID = countryID;
        startPanel.SetActive(true);
        questionPanel.SetActive(false);
        resultPanel.SetActive(false);

        var info = _resourcesLoaderScript.GetInfoByID(countryID);
        countryText.text = "Страна: " + info.name;
    }

    public void CountryDeselected()
    {
        // now not possible
    }

    private void StartQuizAction()
    {
        quizCountry = _resourcesLoaderScript.GetRandomizedQuizList(countryID);
        count = -1;
        correctCount = 0;

        startPanel.SetActive(false);
        questionPanel.SetActive(true);
        resultPanel.SetActive(false);

        NextQuestion();
    }


    private void NextQuestion()
    {
        count++;
        if (count > quizCountry.quizes.Length - 1)
        {
            ShowResult();
            return;
        }

        var oneQuiz = quizCountry.quizes[count];

        counterText.text = string.Format("{0} / {1}", count + 1, quizCountry.quizes.Length);
        descriptionText.text = oneQuiz.text;

        LayoutRebuilder.ForceRebuildLayoutImmediate(descriptionText.transform as RectTransform);

        HideElements();

        if (oneQuiz.quizType == "text")
        {
            RenderTextQuestion(oneQuiz);
            return;
        }

        if (oneQuiz.quizType == "image")
        {
            RenderImageQuestion(oneQuiz);
            return;
        }

        //todo: add more types
    }

    private void RenderTextQuestion(Quiz oneQuiz)
    {
        for (int i = 0; i < oneQuiz.answers.Length; i++)
        {
            if (i > answerButtons.Length - 1)
            {
                break;
            }

            answerButtons[i].onClick.RemoveAllListeners();

            var answerCopy = oneQuiz.answers[i];

            answerButtons[i].onClick.AddListener(() => { AnswerClicked(answerCopy); });
            answerButtons[i].GetComponentInChildren<Text>().text = oneQuiz.answers[i].text;

            answerButtons[i].gameObject.SetActive(true);
        }
    }

    private void RenderImageQuestion(Quiz oneQuiz)
    {
        var sprites = _resourcesLoaderScript.GetQuizImages(oneQuiz.additional);
        if (sprites.Count > 0)
        {
            quizImage.sprite = sprites[0];
            quizImage.gameObject.SetActive(true);
        }

        for (int i = 0; i < oneQuiz.answers.Length; i++)
        {
            if (i > answerButtons.Length - 1)
            {
                break;
            }

            answerButtons[i].onClick.RemoveAllListeners();

            var answerCopy = oneQuiz.answers[i];

            answerButtons[i].onClick.AddListener(() => { AnswerClicked(answerCopy); });
            answerButtons[i].GetComponentInChildren<Text>().text = oneQuiz.answers[i].text;

            answerButtons[i].gameObject.SetActive(true);
        }
    }

    private void HideElements()
    {
        quizImage.gameObject.SetActive(false);
        
        foreach (var b in answerButtons)
        {
            b.gameObject.SetActive(false);
        }

        //todo: add hide images / video / etc..
    }

    private void AnswerClicked(Answer answer)
    {
        if (answer.correct)
        {
            correctCount++;
        }

        //todo: right/wrong click animation should be here
        NextQuestion();
    }

    private void ShowResult()
    {
        startPanel.SetActive(false);
        questionPanel.SetActive(false);
        resultPanel.SetActive(true);

        resultText.text = FormatResult();
    }

    private string FormatResult()
    {
        var allCount = quizCountry.quizes.Length;

        var percent = Mathf.RoundToInt((float)correctCount / (float)allCount * 100);

        return string.Format("Ваш результат: {0}%", percent);
    }

    private void CancelQuizAction()
    {
        startPanel.SetActive(true);
        questionPanel.SetActive(false);
        resultPanel.SetActive(false);
    }
}