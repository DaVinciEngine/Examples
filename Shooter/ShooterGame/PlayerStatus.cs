using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace ShooterGame
{
    static class PlayerStatus
    {
        private const float multiplierExpiryTime = 0.8f;
        private const int maxMultiplier = 20;
        public static int Lives { get; private set; }
        public static int Score { get; private set; }
        public static int Multiplier { get; private set; }
        public static bool IsGameOver { get { return Lives == 0; } }
        public static int HighScore;
        private static float multiplierTimeLeft;
        private static int scoreForExtraLife;
        private const string highScoreFileName = "highscore.txt";

        static PlayerStatus()
        {
            HighScore = LoadHighScore();
            Reset();
        }

        public static void Reset()
        {
            if (Score > HighScore)
                SaveHighScore(HighScore = Score);
            Score = 0;
            Multiplier = 1;
            Lives = 4;
            scoreForExtraLife = 2000;
            multiplierTimeLeft = 0;

        }

        public static void Update(GameTime GameTime)
        {
            if (Multiplier > 1)
            {
                if ((multiplierTimeLeft -= (float)GameTime.ElapsedGameTime.TotalSeconds) <= 0)
                {
                    multiplierTimeLeft = multiplierExpiryTime;
                    ResetMultiplier();
                }
            }
        }

        public static void AddPoints(int basePoints)
        {
            if (PlayerShip.Instance.IsDead)
            {
                return;
            }

            Score += basePoints * Multiplier;
            while (Score >= scoreForExtraLife)
            {
                scoreForExtraLife += 2000;
                Lives++;
            }
        }

        public static void IncreaseMultiplier()
        {
            if (PlayerShip.Instance.IsDead)
            {
                return;
            }

            multiplierTimeLeft = multiplierExpiryTime;
            if (Multiplier < maxMultiplier)
                Multiplier++;
        }

        public static void ResetMultiplier()
        {
            Multiplier = 1;
        }

        public static void RemoveLive()
        {
            Lives--;
        }

        private static int LoadHighScore()
        {
            int score;
            return File.Exists(highScoreFileName) && int.TryParse(File.ReadAllText(highScoreFileName), out score)
                ? score
                : 0;

        }

        private static void SaveHighScore(int score)
        {
            File.WriteAllText(highScoreFileName, score.ToString());
        }   
    }
    }

