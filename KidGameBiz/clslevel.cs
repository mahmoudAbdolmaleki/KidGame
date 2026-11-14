using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KidGameData;

namespace KidGameBiz
{
    public class clslevel
    {
        clsData objData = new clsData();
        const string MESSEGE = "اشکال سیستمی";

        private string namePlayer;

        public string NamePlayer
        {
          get { return namePlayer; }
         set { namePlayer = value; }
        }


        private int codePlayer;

        public int CodePlayer
        {
            get { return codePlayer; }
            set { codePlayer = value; }
        }

        private int codeGame;

        public int CodeGame
        {
            get { return codeGame; }
            set { codeGame = value; }
        }


        private int round;

        public int Round
        {
            get { return round; }
            set { round = value; }
        }

        private int score;

        public int Score
        {
            get { return score; }
            set { score = value; }
        }

        private int codeLevel;

        public int CodeLevel
        {
            get { return codeLevel; }
            set { codeLevel = value; }
        }

        private int gift;

        public int Gift
        {
            get { return gift; }
            set { gift = value; }
        }

        private int scoreForGift;

        public int ScoreForGift
        {
            get { return scoreForGift; }
            set { scoreForGift = value; }
        }

        private int time=100;

        public int Time
        {
            get {
                //if (codeLevel == 1 && time < 6)
                //    time = 6;
                if (codeGame == 1 && time < 9)
                    time = 9;
                if (codeGame == 2 && time < 11)
                {
                //    if (codeLevel == 1 && time < 7)
                //        time = 8;
                    //if (codeLevel == 2 && time < 11)
                        time = 11;
                }
                //if (codeGame == 3 && codeLevel==2)
                //{
                //    time = time / 5;
                //    if (time < 1)
                //        time = 1;
                //}
                return time; }
            set { time = value; }
        }


        public void GetLevel()
        {
            List<int> levelPlayer = new List<int>();
            try
            {
                levelPlayer = objData.GetLevelGame(codePlayer, codeGame);
                round = levelPlayer[0];
                score = levelPlayer[1];
                codeLevel = levelPlayer[2];
                gift = levelPlayer[3];
                scoreForGift = score / 500;
                scoreForGift = scoreForGift * 500;
                TimeRegulation();
            }
            catch (Exception)
            {
                System.Windows.Forms.MessageBox.Show(MESSEGE);
               // throw;
            }
           
        }


       private void TimeRegulation()
        {
            try
            {
                if (codeGame == 3 && codeLevel == 1)
                    time = 100;
                else

                    for (int i = 1; i < round; i++)
                    {
                        //double a =100- (100 * ((double)(20 / 100)));
                        time = time - (int)(time * 0.2f);
                    }
            }
            catch (Exception)
            {
                System.Windows.Forms.MessageBox.Show(MESSEGE);
               // throw;
            }
        
           
        }

       public void SaveLevelPlayer()
       {
           objData.SaveLevelPlayer(codePlayer,codeGame,round,score,codeLevel,gift);
       }




    }
}
