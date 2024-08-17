using Microsoft.AspNetCore.Mvc;
using CSharpQuizBattle.Services;
using System.Threading.Tasks;
using CSharpQuizBattle.Models;

namespace CSharpQuizBattle.Controllers
{
    public class GameController : Controller
    {
        private readonly QuestionService _questionService;
        private static Player _player;
        private static Enemy _enemy;
        private static Question _currentQuestion;
        private static Question _lastQuestion;  // Store the last asked question

        public GameController(QuestionService questionService)
        {
            _questionService = questionService;
        }

        public async Task<IActionResult> Index()
        {
            if (_player == null)
            {
                return RedirectToAction("Setup");
            }

            if (!_player.IsAlive)
            {
                ViewBag.Message = "You have been defeated!";
                return View("GameOver");
            }

            if (!_enemy.IsAlive)
            {
                ViewBag.Message = "You defeated the enemy!";
                return View("Victory");
            }

            // Fetch a random question, ensuring it's not the same as the last one
            _currentQuestion = await _questionService.GetRandomQuestionAsync(null, null, _lastQuestion);
            _lastQuestion = _currentQuestion; // Store the current question as the last question

            var viewModel = new GameViewModel
            {
                Player = _player,
                Enemy = _enemy,
                CurrentQuestion = _currentQuestion
            };

            if (TempData.ContainsKey("AnswerFeedback"))
            {
                ViewBag.AnswerFeedback = TempData["AnswerFeedback"];
            }

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Answer(int selectedAnswer)
        {
            bool isCorrect = selectedAnswer == _currentQuestion.CorrectAnswerIndex;
            string feedbackClass = isCorrect ? "correct-answer" : "incorrect-answer";

            ViewBag.Message = isCorrect
                ? $"Correct! You dealt {_currentQuestion.Difficulty * 10} damage."
                : $"Incorrect! You took {_currentQuestion.Difficulty * 10} damage.";

            TempData["AnswerFeedback"] = feedbackClass; // Store feedback in TempData for the next request

            if (isCorrect)
            {
                int damage = _currentQuestion.Difficulty * 5;
                _enemy.TakeDamage(damage);
            }
            else
            {
                int damage = _currentQuestion.Difficulty * 10;
                _player.TakeDamage(damage);
            }

            return RedirectToAction("Index");
        }


        public IActionResult Setup()
        {
            var model = new CharacterSetupViewModel
            {
                AvailableAvatars = new List<string>
                {
                    "avataaars.png",
                    "avataaars (1).png",
                    "avataaars (2).png",
                    "avataaars (3).png",
                    "avataaars (5).png"
                }
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Setup(CharacterSetupViewModel model)
        {
            if (ModelState.IsValid)
            {
                _player = new Player(model.PlayerName, model.SelectedAvatar);
                _enemy = new Enemy("BOB In IT", 100, 8, 3);

                return RedirectToAction("Index");
            }

            model.AvailableAvatars = new List<string>
            {
                "avataaars.png",
                "avataaars (1).png",
                "avataaars (2).png",
                "avataaars (3).png",
                "avataaars (5).png"
            };

            return View(model);
        }

        public IActionResult GameOver()
        {
            return View();
        }

        public IActionResult Victory()
        {
            return View();
        }
    }
}
