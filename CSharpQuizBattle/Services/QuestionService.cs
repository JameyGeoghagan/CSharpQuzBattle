using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

namespace CSharpQuizBattle.Services
{
    public class QuestionService
    {
        private readonly IWebHostEnvironment _environment;
        private readonly string _filePath;
        private List<Question> _askedQuestions;  // Track already asked questions

        public QuestionService(IWebHostEnvironment environment)
        {
            _environment = environment;
            _filePath = Path.Combine(_environment.ContentRootPath, "questions.json");
            _askedQuestions = new List<Question>();  // Initialize the list
        }

        public async Task<Question> GetRandomQuestionAsync(string category = null, string difficulty = null, Question excludeQuestion = null)
        {
            var json = await File.ReadAllTextAsync(_filePath);
            var categories = JsonSerializer.Deserialize<List<QuestionCategory>>(json);

            List<Question> questions;

            if (string.IsNullOrEmpty(category))
            {
                // Select from all categories if no specific category is given
                questions = categories.SelectMany(c => c.Questions).ToList();
            }
            else
            {
                // Select from the specified category
                var selectedCategory = categories.FirstOrDefault(c => c.Category.Equals(category, StringComparison.OrdinalIgnoreCase));
                if (selectedCategory == null)
                {
                    return null;
                }
                questions = selectedCategory.Questions;
            }

            if (!string.IsNullOrEmpty(difficulty))
            {
                // Filter questions by difficulty
                questions = questions.Where(q => q.Difficulty.ToString() == difficulty).ToList();
            }

            // Exclude already asked questions
            questions = questions.Where(q => !_askedQuestions.Contains(q)).ToList();

            // Exclude the last asked question if provided
            if (excludeQuestion != null)
            {
                questions = questions.Where(q => !q.Equals(excludeQuestion)).ToList();
            }

            if (questions.Count == 0)
            {
                // If all questions have been asked, reset the list
                _askedQuestions.Clear();
                questions = categories.SelectMany(c => c.Questions).ToList();

                if (!string.IsNullOrEmpty(difficulty))
                {
                    questions = questions.Where(q => q.Difficulty.ToString() == difficulty).ToList();
                }

                // Exclude already asked questions again after reset
                questions = questions.Where(q => !_askedQuestions.Contains(q)).ToList();
            }

            if (questions.Count == 0)
            {
                return null; // No questions left to ask
            }

            var random = new Random();
            var selectedQuestion = questions[random.Next(questions.Count)];

            // Add the selected question to the list of asked questions
            _askedQuestions.Add(selectedQuestion);

            return selectedQuestion;
        }
    }

    public class QuestionCategory
    {
        public string Category { get; set; }
        public List<Question> Questions { get; set; }
    }

    public class Question
    {
        public string Text { get; set; }
        public string[] Choices { get; set; }
        public int CorrectAnswerIndex { get; set; }
        public int Difficulty { get; set; }

        // Override Equals and GetHashCode to compare questions by their text
        public override bool Equals(object obj)
        {
            if (obj is Question otherQuestion)
            {
                return Text.Equals(otherQuestion.Text, StringComparison.OrdinalIgnoreCase);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Text.ToLowerInvariant().GetHashCode();
        }
    }
}
