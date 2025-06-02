using System.Collections.Generic;
using UnityEngine;

// Represents a single (commonly asked in interviews) SWE multiple-choice question
[System.Serializable]
public class SoftDevQuestion
{
    public string question; 
    public List<string> answers; 
    public int correctAnswerIndex; 
}

// Manages the list of SoftDevQuestion objects and provides functionality to retrieve them
public class SoftDevQuestionManager : MonoBehaviour
{
    public List<SoftDevQuestion> questions; // List of soft dev questions

    // Method to get a random question from the list
    public SoftDevQuestion GetRandomQuestion()
    {
        int randomIndex = Random.Range(0, questions.Count);
        return questions[randomIndex];
    }
}