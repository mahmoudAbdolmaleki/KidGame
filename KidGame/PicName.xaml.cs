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


namespace KidGame
{
    /// <summary>
    /// Interaction logic for PicName.xaml
    /// </summary>
    public partial class PicName : Window
    {
        clSPicName objPicName=new clSPicName();
        List<TextBlock> listTxtBl;
        List<Image> listImg;
        clslevel objLevel = new clslevel();
        int numberPicForSimilar;
        DispatcherTimer Timer;

        int pointerChangeNmae = 0;
        int timeRemain;
        int timeIncrement;
        int totaltime;

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


        public PicName()
        {
            InitializeComponent();
        }

        private void WinGame()
        {
            Timer.Stop();

            MessageBox.Show("یک دور تمام شد برای دور بعد برروی دکمه شروع کلیک کنید");

            //objLevel.Time -= 20;
            objLevel.Time = objLevel.Time - (int)(objLevel.Time * 0.2f);
            objLevel.Round += 1;
            if (radibtnEasy.IsChecked == true)
                objLevel.Score += 200 + (timeRemain + (objLevel.Round * 10 ));
            else if (radibtnNormal.IsChecked == true)
                objLevel.Score += 200 + (timeRemain + (objLevel.Round * 10 ));


            int tempGift = objLevel.Score - objLevel.ScoreForGift;
            if (tempGift >= 500)
            {
                objLevel.Gift += 2;
                objLevel.ScoreForGift += 500;
            }
            RadioBtnCheck();
            RefreshScore();

        }




        // این متد نمایش تصویر جدید یا اتمام یک دور از بازی را مشخص میکند 
        private void ShowNextName(bool choicImg)
        {
            if (pointerChangeNmae >= (Int32)(listImg.Count / 2))
            {
                WinGame();

            }
            else

                // اگر بازیکن تصویر درست را انتخاب کرد تصویر بعدی نمایش داده می شود
                if (choicImg == true)
                {
                    TxtBlChangeName.Text = listTxtBl[pointerChangeNmae].Text; ;
                    TxtBlChangeName.Tag = listTxtBl[pointerChangeNmae].Tag;
                    
                }


        }


        void CancelGame()
        {
            RadioBtnCheck();
            RefreshScore();
            btnStartGame.IsEnabled = true;
            btnGift.IsEnabled = true;
        }

        private void FillImg()
        {
            listTxtBl = new List<TextBlock>();
                 
            CreateListImg();
           listTxtBl= objPicName.GetPic(ref listImg);
        }



        private bool ChoseImage(int CodeImg)
        {
            if (int.Parse(TxtBlChangeName.Tag.ToString()) == CodeImg)
            {
                numberPicForSimilar--;
                txtNumberPicForSimilar.Text = numberPicForSimilar.ToString();

                pointerChangeNmae++;
                Console.Beep(300, 100);
                return true;
            }
            else
            {
                Console.Beep(2000, 500);
                return false;
            }

        }


        


        private void CreateTimer()
        {
            Timer = new DispatcherTimer();
            Timer.Tick += new EventHandler(Timer_Tick);
            //Timer.Interval = new TimeSpan(0, 0, 1);

            Timer.Interval = TimeSpan.FromMilliseconds(1000);
        }



        //این متد امتیاز بازیکن و دور بازی را نمایش میدهد 
        private void RefreshScore()
        {
            Timer.Stop();
            // RadioBtnCheck();
            timeIncrement = 0;
            pointerChangeNmae = 0;

            txtRound.Text = objLevel.Round.ToString();
            txtScore.Text = objLevel.Score.ToString();
            txtGift.Text = objLevel.Gift.ToString();
            txtNumberPicForSimilar.Text = numberPicForSimilar.ToString();

            totaltime = objLevel.Time;
            txtTime.Text = totaltime.ToString();

            btnGift.IsEnabled = true;
            btnStartGame.IsEnabled = true;
            gridImage.Visibility = Visibility.Hidden;
            TxtBlChangeName.Text = null;
           // imgChange.Source = null;
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
                RadioBtnCheck();
                RefreshScore();

            }

        }
       


        private void CreateListImg()
        {
            listImg = new List<Image>();

            listImg.Add(image1);
            listImg.Add(image2);
            listImg.Add(image3);
            listImg.Add(image4);
            listImg.Add(image5);
            listImg.Add(image6);
            listImg.Add(image7);
            listImg.Add(image8);
            listImg.Add(image9);
            listImg.Add(image10);
            listImg.Add(image11);
            listImg.Add(image12);
            if (radibtnNormal.IsChecked == true)
            {
                listImg.Add(image13);
                listImg.Add(image14);
                listImg.Add(image15);
                listImg.Add(image16);
                listImg.Add(image17);
                listImg.Add(image18);
                objLevel.CodeLevel = 2;
            }
            else
            {
                image13.Source = null;
                image14.Source = null;
                image15.Source = null;
                image16.Source = null;
                image17.Source = null;
                image18.Source = null;
                objLevel.CodeLevel = 1;

            }



        }




        private void RadioBtnCheck()
        {
            if (objLevel.Round > 1)
            {
                radibtnEasy.IsEnabled = false;
                radibtnNormal.IsEnabled = false;

                if (objLevel.CodeLevel ==1)
                {
                    radibtnEasy.IsChecked = true;
                    numberPicForSimilar = 6;
                }
                else
                {
                    radibtnNormal.IsChecked = true;
                    numberPicForSimilar = 9;
                }


            }
            else
            {
                radibtnEasy.IsChecked = true;
                radibtnNormal.IsEnabled = true;
                radibtnEasy.IsEnabled = true;
                numberPicForSimilar = 6;
            }

        }

        void LoadForm()
        {
            FillImg();
            txtbNamePlayer.Text = namePlayer;

            objLevel.CodePlayer = codePlayer;
            objLevel.CodeGame = codeGame;

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
            RadioBtnCheck();
            RefreshScore();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadForm();
           
        }




        void StartGame()
        {
            gridImage.Visibility = Visibility.Visible;
            FillImg();


            //int effect = FillImg();
            //if (effect != -1)
            {
                ShowNextName(true);
                Timer.Start();
            }
            btnStartGame.IsEnabled = false;
            btnGift.IsEnabled = false;

            if (radibtnEasy.IsChecked == true)
                radibtnNormal.IsEnabled = false;
            else
                radibtnEasy.IsEnabled = false;
        }

        private void btnStartGame_Click(object sender, RoutedEventArgs e)
        {
           StartGame();
          
            
        }



        private void btnGift_Click(object sender, RoutedEventArgs e)
        {
            if (objLevel.Gift > 0)
            {
                totaltime++;
                objLevel.Gift--;
                txtGift.Text = objLevel.Gift.ToString();
                txtTime.Text = totaltime.ToString();
            }
        }



        private void radibtnEasy_Checked(object sender, RoutedEventArgs e)
        {

            numberPicForSimilar = 6;
            txtNumberPicForSimilar.Text = numberPicForSimilar.ToString();
   
        }
        private void radibtnNormal_Checked(object sender, RoutedEventArgs e)
        {

            numberPicForSimilar = 9;
            txtNumberPicForSimilar.Text = numberPicForSimilar.ToString();
              
        }

    
        private void lblCancel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            CancelGame();
           
        }

        void GridClick(MouseButtonEventArgs e)
        {
            FrameworkElement fe = e.OriginalSource as FrameworkElement;


            if (ChoseImage(int.Parse(fe.Tag.ToString())))
                ShowNextName(true);
            else
                ShowNextName(false);
        }


        private void gridImage_MouseDown(object sender, MouseButtonEventArgs e)
        {

            GridClick(e);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Timer.Stop();
            if (codePlayer > 0)
                objLevel.SaveLevelPlayer();

        }

        private void lblCancel_MouseWheel(object sender, MouseWheelEventArgs e)
        {
           
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
