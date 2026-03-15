using System;
using System.Windows.Forms;

namespace ExpertSystem
{
    public partial class Form1 : Form
    {
        private int currentQuestion = 0;
        private int score = 0;

        // Вопросы (можно расширять)
        private readonly string[] questions = new string[]
        {
            "Программа стабильно работает более 8 часов без перезапусков?",
            "Есть ли автоматические тесты (unit / integration) с покрытием ≥ 60%?",
            "Код покрыт документацией (XML-комментарии, Swagger и т.п.) ≥ 70%?",
            "Используется система контроля версий (Git) с осмысленными коммитами?",
            "Есть ли code review перед слиянием в основную ветку?",
            "Применяются современные практики (чистый код, SOLID, DRY)?",
            "Есть ли CI/CD пайплайн (автоматическая сборка + деплой)?",
            "Логирование настроено адекватно (уровни, структура, ротация)?",
            "Обработка ошибок и исключений выполнена на хорошем уровне?",
            "Используется статический анализ кода (SonarQube, Roslyn analyzers и т.п.)?",
            "Приложение имеет защиту от основных уязвимостей OWASP Top 10?",
            "Есть ли мониторинг работоспособности в продакшене (Prometheus, ELK, Sentry…)?",
            "Проект активно поддерживается (коммиты за последние 3 месяца)?",
            "Пользовательский интерфейс интуитивно понятен большинству пользователей?"
        };

        // Вес каждого вопроса (можно менять)
        private readonly int[] weights = new int[]
        {
            8, 10, 7, 6, 8, 9, 10, 7, 8, 9, 10, 9, 6, 7
        };

        private readonly int maxPossibleScore;

        public Form1()
        {
            InitializeComponent();
            maxPossibleScore = CalculateMaxScore();

            // Начальные настройки формы
            this.Text = "Экспертная система оценки качества ПО";
            this.Size = new System.Drawing.Size(720, 520);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            lblQuestion = new Label
            {
                Location = new System.Drawing.Point(40, 60),
                Size = new System.Drawing.Size(640, 80),
                Font = new System.Drawing.Font("Segoe UI", 12F),
                TextAlign = System.Drawing.ContentAlignment.MiddleLeft,
                AutoSize = false
            };

            btnYes = new Button
            {
                Text = "Да",
                Location = new System.Drawing.Point(180, 180),
                Size = new System.Drawing.Size(140, 60),
                Font = new System.Drawing.Font("Segoe UI", 14F),
                BackColor = System.Drawing.Color.LightGreen
            };

            btnNo = new Button
            {
                Text = "Нет",
                Location = new System.Drawing.Point(380, 180),
                Size = new System.Drawing.Size(140, 60),
                Font = new System.Drawing.Font("Segoe UI", 14F),
                BackColor = System.Drawing.Color.LightCoral
            };

            lblProgress = new Label
            {
                Location = new System.Drawing.Point(40, 20),
                Size = new System.Drawing.Size(640, 30),
                Font = new System.Drawing.Font("Segoe UI", 11F),
                TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            };

            btnRestart = new Button
            {
                Text = "Начать заново",
                Location = new System.Drawing.Point(280, 380),
                Size = new System.Drawing.Size(160, 50),
                Font = new System.Drawing.Font("Segoe UI", 11F),
                Visible = false
            };

            this.Controls.AddRange(new Control[] { lblQuestion, btnYes, btnNo, lblProgress, btnRestart });

            btnYes.Click += BtnAnswer_Click;
            btnNo.Click += BtnAnswer_Click;
            btnRestart.Click += BtnRestart_Click;

            ShowQuestion();
        }

        private int CalculateMaxScore()
        {
            int sum = 0;
            foreach (int w in weights) sum += w;
            return sum;
        }

        private void ShowQuestion()
        {
            if (currentQuestion >= questions.Length)
            {
                ShowResult();
                return;
            }

            lblQuestion.Text = questions[currentQuestion];
            lblProgress.Text = $"Вопрос {currentQuestion + 1} из {questions.Length}";

            btnYes.Visible = true;
            btnNo.Visible = true;
            btnRestart.Visible = false;
        }

        private void BtnAnswer_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;

            if (btn.Text == "Да")
            {
                score += weights[currentQuestion];
            }
            // "Нет" → +0 баллов

            currentQuestion++;

            if (currentQuestion < questions.Length)
            {
                ShowQuestion();
            }
            else
            {
                ShowResult();
            }
        }

        private void ShowResult()
        {
            btnYes.Visible = false;
            btnNo.Visible = false;
            btnRestart.Visible = true;

            double percent = (double)score / maxPossibleScore * 100;

            string verdict;
            Color verdictColor;

            if (percent >= 90)
            {
                verdict = "Отличное качество (A+)";
                verdictColor = Color.DarkGreen;
            }
            else if (percent >= 80)
            {
                verdict = "Очень хорошее (A)";
                verdictColor = Color.ForestGreen;
            }
            else if (percent >= 70)
            {
                verdict = "Хорошее (B+)";
                verdictColor = Color.DarkGoldenrod;
            }
            else if (percent >= 60)
            {
                verdict = "Среднее (B)";
                verdictColor = Color.DarkOrange;
            }
            else if (percent >= 45)
            {
                verdict = "Ниже среднего (C)";
                verdictColor = Color.DarkRed;
            }
            else
            {
                verdict = "Критически низкое (переписывать)";
                verdictColor = Color.Maroon;
            }

            lblQuestion.ForeColor = verdictColor;
            lblQuestion.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);

            lblQuestion.Text =
                $"Результат: {score}/{maxPossibleScore} баллов\n" +
                $"({percent:F1}%)\n" +
                "──────────────────────────────\n" +
                $"Оценка:  {verdict}\n" +
                "──────────────────────────────\n\n" +
                (percent < 70 ? "Срочно обратите внимание на:\n• Отсутствие тестов / CI/CD\n• Слабая обработка ошибок\n• Отсутствие мониторинга" : "Хороший уровень зрелости проекта!");

            lblProgress.Text = "Оценка завершена";
        }

        private void BtnRestart_Click(object sender, EventArgs e)
        {
            score = 0;
            currentQuestion = 0;
            ShowQuestion();
        }

        // Объявляем элементы управления (чтобы не было ошибок компиляции)
        private Label lblQuestion;
        private Button btnYes;
        private Button btnNo;
        private Label lblProgress;
        private Button btnRestart;
    }
}