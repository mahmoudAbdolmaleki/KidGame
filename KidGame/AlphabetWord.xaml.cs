using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using KidGameBiz;
using System.Windows.Threading;
//using System.Globalization;

namespace KidGame
{
    /// <summary>
    /// Interaction logic for AlphabetWord.xaml
    /// </summary>
    public partial class AlphabetWord : Window
    {
        clsAlphaWord objAlphaWord = new clsAlphaWord();
        clslevel objLevel = new clslevel();
        DispatcherTimer Timer;
        List<char> listColorChar;// = new List<char>();
       
        List<Button> listButton;

       char[] selectChar;
       char[] arrayChar;
       int  endWord=0;
       int counterWordShow = 0;
       bool choseChar = false;
       int timeRemain;
       int timeIncrement;
       int totaltime;
       int totaltimeRemain;
       int gift = 0;
       int LengthWord;
       const int NNMBERWORD=5;


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



       private void RadioBtnCheck()
       {
            if (objLevel.Round > 1)
            {
                radibtnEasy.IsEnabled = false;
                radibtnNormal.IsEnabled = false;

                if (objLevel.CodeLevel == 1)
                {
                    radibtnEasy.IsChecked = true;
                  
                }
                else
                {
                    radibtnNormal.IsChecked = true;
                
                }


            }
           

            if (radibtnNormal.IsChecked == true)
            {
               
                objLevel.CodeLevel = 2;
            }
            else
            {
                objLevel.CodeLevel = 1;

            }




       }





       private void CreateTimer()
       {
           Timer = new DispatcherTimer();
           Timer.Tick += new EventHandler(Timer_Tick);
           //Timer.Interval = new TimeSpan(0, 0, 1);

           Timer.Interval = TimeSpan.FromMilliseconds(1000);
       }




       private void Timer_Tick(object sender, EventArgs e)
       {

           timeIncrement++;

           timeRemain = totaltime - timeIncrement;

           txtTime.Text = timeRemain.ToString();

           if (timeIncrement >= totaltime)
           {
               Timer.Stop();

               MessageBox.Show("شما باختید");
              // counterWordShow = 0;
             RefreshScore();
            
           }

       }


       private void RefreshScore()
       {
            counterWordShow = 0;
            Timer.Stop();
            timeIncrement = 0;
            gridWord.IsEnabled = false;

            txtRound.Text = objLevel.Round.ToString();
            txtScore.Text = objLevel.Score.ToString();
            txtGift.Text = objLevel.Gift.ToString();
          
            btnGift.IsEnabled = true;
            btnStartGame.IsEnabled = true;
          
            TxtBlChangeName.Text = null;
            txtTime.Text = null;
            gift = 0;

            if(listButton!=null)
                foreach (Button tempBtn in listButton)
                    tempBtn.IsEnabled = true;
          
            txtNumberWord.Text = (NNMBERWORD - counterWordShow).ToString();
       }


       private void WinGame()
       {
           Timer.Stop();
           //counterWordShow = 0;
          // timeIncrement = 0;
           MessageBox.Show("یک دور تمام شد برای دور بعد برروی دکمه شروع کلیک کنید");
          
           //objLevel.Time -= 20;
           if (radibtnNormal.IsChecked == true)
               objLevel.Time = objLevel.Time - (int)(objLevel.Time * 0.2f);
           else
               objLevel.Time = 100;

           objLevel.Round += 1;


           objLevel.Score += (totaltimeRemain + (objLevel.Round * (2)));
           


           int tempGift = objLevel.Score - objLevel.ScoreForGift;
           if (tempGift >= 500)
           {
               objLevel.Gift += 2;
               objLevel.ScoreForGift += 500;
           }
           totaltimeRemain = 0;
          
           RefreshScore();
           RadioBtnCheck();

       }


        private bool BeforColorChar(char colorChar)
        {
            for (int i = 0; i < listColorChar.Count; i++)
                if (listColorChar[i] == colorChar)
                    return true;
               
                    return false;
        }

       

        private void FindChar(char selectChar)
        {
            TxtBlChangeName.Inlines.Clear();

            for (int i = 0; i <arrayChar.Length; i++)
            {
                if (arrayChar[i] == selectChar)
                {
                    TxtBlChangeName.Inlines.Add(new Run(arrayChar[i].ToString()) { Foreground = Brushes.Red});
                    choseChar = true;
                    listColorChar.Add(arrayChar[i]);
                    endWord++;
                   
                }

                else if (BeforColorChar(arrayChar[i]))
                {
                    TxtBlChangeName.Inlines.Add(new Run(arrayChar[i].ToString()) { Foreground = Brushes.Red });
                   
                }
                else
                {
                    TxtBlChangeName.Inlines.Add(new Run(arrayChar[i].ToString()));
                    
                }
             }
 
        }



        public AlphabetWord()
        {
            InitializeComponent();
        }



        void LoadForm()
        {
            txtbNamePlayer.Text = namePlayer;

            objLevel.CodeGame = codeGame;
            objLevel.CodePlayer = codePlayer;
            objAlphaWord.GetWords();

            if (codePlayer > 0)
            {

                objLevel.GetLevel();

            }
            else
            {
                objLevel.Round = 1;
                objLevel.Score = 0;

            }
            CreateTimer();
            RefreshScore();
            RadioBtnCheck();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadForm();
           
        }



        private void NewWord()
        {
           
            gridWord.IsEnabled = true;
            timeIncrement = 0;
            endWord = 0;
            LengthWord = 0;
            arrayChar = null;
           
            listColorChar = new List<char>();
            listButton = new List<Button>();

            string word = objAlphaWord.GetWord();
          
            TxtBlChangeName.Text = word;
            arrayChar = word.ToCharArray();
            //string tempWord = word.Trim();
            //MessageBox.Show(tempWord.Length.ToString());
            //if (arrayChar == null)
            //    LengthWord = -1;
            //else

            foreach (char temp in arrayChar)
            {
                if (temp != ' ')
                    LengthWord++;

                //if((int)temp!=32)
                //    LengthWord++;

            }
            totaltimeRemain += timeRemain;
            int timeForChar = objLevel.Time /5;
            if (timeForChar < 1)
                timeForChar = 1;
            totaltime = timeForChar * LengthWord;
            totaltime += gift;

            btnGift.IsEnabled = false;
            Timer.Start();
            txtTime.Text = totaltime.ToString();

            radibtnEasy.IsEnabled = false;
            radibtnNormal.IsEnabled = false;
            txtNumberWord.Text = (NNMBERWORD - counterWordShow).ToString();
            
        }

        private void PlayerChoseChar(RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty( TxtBlChangeName.Text ))
            {
                try
                {

                    choseChar = false;
                    Button tempButton = (Button)e.Source;

                    string temp = tempButton.Content.ToString();

                    selectChar = temp.ToCharArray();
                    if (temp == "الف")
                    {
                        selectChar[0] = 'ا';
                        selectChar[1] = 'آ';
                        FindChar(selectChar[1]);
                    }

                    FindChar(selectChar[0]);

                    if (choseChar)
                    {
                        Console.Beep(300, 100);
                        tempButton.IsEnabled = false;
                        listButton.Add(tempButton);
                    }
                    else
                        Console.Beep(2000, 500);

                    if (endWord == LengthWord)
                    {
                        counterWordShow++;
                        foreach (Button tempBtn in listButton)
                            tempBtn.IsEnabled = true;

                        if (counterWordShow >= 5)
                        {

                            WinGame();
                            RefreshScore();


                        }
                        else
                        {
                            Timer.Stop();
                            totaltimeRemain += timeRemain;
                            if (radibtnNormal.IsChecked == true)
                            {
                                NewWord();
                                RadioBtnCheck();

                            }
                            else
                            {
                                btnStartGame.IsEnabled = true;
                                TxtBlChangeName.Text = null;
                                gridWord.IsEnabled = false;
                                RadioBtnCheck();
                                txtNumberWord.Text = (NNMBERWORD - counterWordShow).ToString();
                            }

                        }


                    }

                }

                catch (Exception)
                {

                    //throw;
                }

            }

        }


        void StartGame()
        {
            if (radibtnEasy.IsChecked == true)
            {
                NewWord();
                btnStartGame.IsEnabled = false;
            }
            else
            {

                totaltimeRemain = 0;

                NewWord();
                btnStartGame.IsEnabled = false;
            }
            txtNumberWord.Text = (NNMBERWORD - counterWordShow).ToString();
        }

        private void btnStartGame_Click(object sender, RoutedEventArgs e)
        {
            StartGame();
        }




        void CancelGame()
        {
            timeRemain = 0;
            RefreshScore();
            RadioBtnCheck();
            if (objLevel.Round < 2)
            {
                radibtnEasy.IsEnabled = true;
                radibtnNormal.IsEnabled = true;
            }
            //txtNumberWord.Text = (NNMBERWORD - counterWordShow).ToString();
            //  btnStartGame.IsEnabled = true;
        }
        private void lblCancel_MouseDown(object sender, MouseButtonEventArgs e)
        {

            CancelGame();
        }



        void GiftClick()
        {
            if (objLevel.Gift > 0)
            {
                gift++;
                objLevel.Gift--;
                txtGift.Text = objLevel.Gift.ToString();
                txtTime.Text = gift.ToString();
            }
        }
        private void btnGift_Click(object sender, RoutedEventArgs e)
        {
            GiftClick();

        }



        private void gridWord_MouseDown(object sender, RoutedEventArgs e)
        {
           
        }
        // MouseButtonEventArgs e
       

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Timer.Stop();
            if (codePlayer > 0)
                objLevel.SaveLevelPlayer();
        }




        private void radibtnEasy_Checked(object sender, RoutedEventArgs e)
        {

            
        }
        private void radibtnNormal_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void gridWord_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
           
        }

        private void gridWord_MouseDown(object sender, MouseButtonEventArgs e)
        {
           
        }

        private void gridWord_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            
        }

        private void gridWord_MouseUp(object sender, MouseButtonEventArgs e)
        {
           
        }

        private void gridWord_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            PlayerChoseChar(e);
        }

        private void lblCancel_MouseEnter(object sender, MouseEventArgs e)
        {
            lblCancel.FontStyle = FontStyles.Italic;
            
        }

        private void lblCancel_MouseLeave(object sender, MouseEventArgs e)
        {
            lblCancel.FontStyle = FontStyles.Normal;
        }

        
        
      

        
    }
}



//FrameworkElement fe = e.OriginalSource as FrameworkElement;
//string temp = fe.DataContext.ToString();
//selectChar = temp.ToCharArray();