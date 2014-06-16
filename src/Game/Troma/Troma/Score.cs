using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Troma
{
    [Serializable()]
    public class Score : ISerializable
    {
        private List<int> Precision;

        public int[] HighScore { get; private set; }
        public double TotalElapsedTime { get; private set; }

        public int AveragePrecision
        {
            get 
            {
                if (Precision.Count == 0)
                    return 0;
                else if (Precision.Count == 1)
                    return Precision[0];
                else
                    return (int)Precision.Average(); 
            }
        }

        public Score()
        {
            HighScore = new int[] 
            {
                0, 0, 0, 0, 0
            };

            Precision = new List<int>();
            TotalElapsedTime = 0;
        }

        public Score(SerializationInfo info, StreamingContext ctxt)
        {
            HighScore = (int[])info.GetValue("HighScore", typeof(int[]));
            Precision = new List<int>((int[])info.GetValue("Precision", typeof(int[])));
            TotalElapsedTime = (int)info.GetValue("TotalElapsedTime", typeof(int));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            info.AddValue("HighScore", HighScore);
            info.AddValue("Precision", Precision.ToArray());
            info.AddValue("TotalElapsedTime", TotalElapsedTime);
        }

        public int ComputeScore(TimeSpan elapsedTime, int nbTarget, int nbMunition)
        {
            double t = elapsedTime.TotalSeconds;
            int score = (int)(37 * (100 * nbTarget + (1 / t) * 1000) / (nbMunition + 1));

            AddScore(score);
            Precision.Add(100 * nbTarget / nbMunition);
            TotalElapsedTime += elapsedTime.TotalSeconds;

            return score;
        }

        private void AddScore(int score)
        {
            int i = 0;

            while (i < HighScore.Length && HighScore[i] > score)
                i++;

            if (i < HighScore.Length && score == HighScore[i])
                return;
            else if (i >= HighScore.Length)
                return;
            else
            {
                for (int j = HighScore.Length - 2; j >= i; j--)
                    HighScore[j + 1] = HighScore[j];

                HighScore[i] = score;
            }
        }
    }

    public class ScoreManager
    {
        public static Score Load()
        {
            Score score;

            try
            {
                Stream stream = File.Open("Score.bin", FileMode.Open);
                BinaryFormatter bFormatter = new BinaryFormatter();
                score = (Score)bFormatter.Deserialize(stream);
                stream.Close();
            }
            catch (FileNotFoundException)
            {
                score = new Score();
            }

            return score;
        }

        public static void Save(Score score)
        {
            Stream stream = File.Open("Score.bin", FileMode.Create);
            BinaryFormatter bFormatter = new BinaryFormatter();
            bFormatter.Serialize(stream, score);
            stream.Close();
        }
    }
}
