using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class QuestionTests
    {
        private QuestionManager questionManager;

        [SetUp]
        public void SetUp()
        {
            // Create a GameObject and add QuestionManager component
            GameObject go = new GameObject("QuestionManagerGO");
            questionManager = go.AddComponent<QuestionManager>();

            // Populate the question list for testing
            questionManager.questions = new System.Collections.Generic.List<Question>
            {
                new Question
                {
                    question = "Test question?",
                    answers = new string[] { "A", "B", "C" }, // use string[] instead of List<string>
                    correctAnswerIndex = 0
                }
            };
        }

        [Test]
        public void AllQuestionsHaveAnswers()
        {
            foreach (var q in questionManager.questions)
            {
                Assert.NotNull(q, "A question in the list is null!");
                Assert.NotNull(q.answers, "Question has a null answers array!");
                Assert.IsTrue(q.answers.Length > 0, "Question has no answers!"); // use Length instead of Count
            }
        }
    }
}