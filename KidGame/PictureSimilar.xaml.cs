using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Threading;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using KidGameBiz;
using System.IO;
using System.Collections;

namespace KidGame
{
    /// <summary>
    /// Interaction logic for PictureSimilar.xaml
    /// </summary>
    public partial class PictureSimilar : Window
    {
        clsPicSimilar objPicSimilar;
        clsRandom objRandom = new clsRandom();
        clslevel objLevel = new clslevel();
        List<Image> img2;
        List<Image> listImg;
        DispatcherTimer Timer;
        int pointerChangeImg = 0;
        int timeRemain;
        int timeIncrement;
        int totaltime;
        int numberPicForSimilar;
      
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

        //این متد امتیاز بازیکن و دور بازی را نمایش میدهد 
        private void RefreshScore()
        {
            Timer.Stop();
           // RadioBtnCheck();
            timeIncrement = 0;
            pointerChangeImg = 0;

            txtRound.Text = objLevel.Round.ToString();
            txtScore.Text = objLevel.Score.ToString();
            txtGift.Text = objLevel.Gift.ToString();
            txtNumberPicForSimilar.Text = numberPicForSimilar.ToString();

            totaltime = objLevel.Time;
            txtTime.Text = totaltime.ToString();

            btnGift.IsEnabled = true;
            btnStartGame.IsEnabled = true;
            gridImage.Visibility = Visibility.Hidden;
            imgChange.Source = null;
        }



        private void CreateTimer()
        {
            Timer = new DispatcherTimer();
            Timer.Tick += new EventHandler(Timer_Tick);
            //Timer.Interval = new TimeSpan(0, 0, 1);

            Timer.Interval = TimeSpan.FromMilliseconds(1000);
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

                if (objLevel.CodeLevel == 1)
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
                numberPicForSimilar = 6;
            }

        }



        //برای نمایش تصاویر در فرم
        private int FillImage()
        {
            gridImage.Visibility = Visibility.Visible;
            gridImgChange.Visibility = Visibility.Visible;

          
            objPicSimilar = new clsPicSimilar();

            CreateListImg();

           int effect= objPicSimilar.GetPic1(listImg);

           //اگر تعداد تصاویر دیتابیس کافی نبود ادامه داده نشود
           if (effect == -1)
               return -1;

            img2 = objPicSimilar.GetPic2();
            btnGift.IsEnabled = false;
           
             return 1;
                
        }



        //این متد درست یا غلط بودن انتخاب تصویر توسط بازیکن را مشخص میکند
        private bool ChoicImage(int refCodeImg)
        {
            if (int.Parse(imgChange.Tag.ToString()) == refCodeImg)
            {
                numberPicForSimilar--;
                txtNumberPicForSimilar.Text = numberPicForSimilar.ToString();

                pointerChangeImg++;
                Console.Beep(300, 100);
                return true;
            }
            else
            {
                Console.Beep(2000, 500);
                return false;
            }
 
        }



        public PictureSimilar()
        {
            InitializeComponent();
        }


        //این متد زمانی که  بازیکن یک دور را برنده می شود راه اندزی می شود
        private void WinGame()
        {
           Timer.Stop();

            MessageBox.Show("یک دور تمام شد برای دور بعد برروی دکمه شروع کلیک کنید");
               
            //objLevel.Time -= 20;
            objLevel.Time = objLevel.Time - (int)(objLevel.Time * 0.2f);
            objLevel.Round += 1;
            if(radibtnEasy.IsChecked==true)
            objLevel.Score += 200 + (timeRemain + (objLevel.Round * 10));
            else if(radibtnNormal.IsChecked==true)
                objLevel.Score += 200 + (timeRemain + (objLevel.Round * 10));


            int tempGift = objLevel.Score - objLevel.ScoreForGift;
            if (tempGift >= 500)
            {
                objLevel.Gift+=2;
                objLevel.ScoreForGift += 500;
            }
            RadioBtnCheck();
            RefreshScore();
           
        }


        // این متد نمایش تصویر جدید یا اتمام یک دور از بازی را مشخص میکند 
        private void ShowNextImg(bool choicImg)
        {
            if (pointerChangeImg >= (Int32)(listImg.Count/2))
            {
                WinGame();
               
             }
            else
            
        // اگر بازیکن تصویر درست را انتخاب کرد تصویر بعدی نمایش داده می شود
            if (choicImg == true)
            {               
                imgChange.Source = img2[pointerChangeImg].Source;
                imgChange.Tag = img2[pointerChangeImg].Tag;

            }
          
 
        }



        void loadForm()
        {
            CreateTimer();

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
            RadioBtnCheck();
            RefreshScore();

        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            loadForm();
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




        void GameStart()
        { //اگر تعداد تصاویر دیتابیس کافی نبود ادامه داده نشود
            int effect = FillImage();
            if (effect != -1)
            {
                ShowNextImg(true);
                Timer.Start();
            }
            btnStartGame.IsEnabled = false;
        }
        private void btnStartGame_Click(object sender, RoutedEventArgs e)
        {

            GameStart();
            if (radibtnEasy.IsChecked == true)
                radibtnNormal.IsEnabled = false;
            else
                radibtnEasy.IsEnabled = false;
        }




        void GridClick(MouseButtonEventArgs e)
        {
            FrameworkElement fe = e.OriginalSource as FrameworkElement;


            if (ChoicImage(int.Parse(fe.Tag.ToString())))
                ShowNextImg(true);
            else
                ShowNextImg(false);
        }

        private void gridImage_MouseDown(object sender, MouseButtonEventArgs e)
        {

            GridClick(e);
        }


     
        


        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Timer.Stop();
            if(codePlayer>0)
                objLevel.SaveLevelPlayer();
        }



        void GiftClick()
        {
            if (objLevel.Gift > 0)
            {
                totaltime++;
                objLevel.Gift--;
                txtGift.Text = objLevel.Gift.ToString();
                txtTime.Text = totaltime.ToString();
            }
        }
        private void btnGift_Click(object sender, RoutedEventArgs e)
        {
            GiftClick();
           
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


        void Cancel()
        {
            RadioBtnCheck();
            RefreshScore();
            btnStartGame.IsEnabled = true;
        }
        private void lblCancel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            radibtnEasy.IsEnabled = true;
            radibtnNormal.IsEnabled = true;
            Cancel();
           
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

















/*
 * 
 * 
 * 
          //  System.Drawing.Image[] img = new System.Drawing.Image[14];//.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
         
 * 
            for (int i=0; i < 14; i++)
            {
                bitImage = new BitmapImage();

                 bitImage.BeginInit();

                 byte[] bufer = (byte[])ob[i,0];
                 refCode[i] = int.Parse(ob[i,1].ToString());

                MemoryStream ms = new MemoryStream(bufer);
                //img[i] = System.Drawing.Image.FromStream(ms);
                
                //MemoryStream ms1 = new MemoryStream();
             
                //img[i].Save(ms1, System.Drawing.Imaging.ImageFormat.Bmp);
                //ms1.Seek(0, SeekOrigin.Begin);

                bitImage.StreamSource =ms;
                //bitImage.CacheOption = BitmapCacheOption.OnLoad;
                bitImage.EndInit();
                bitImage.Freeze();
                //object ob = image1.Source;
                //ob = bitImage;
                objImg[i].Source=bitImage;
                objImg[i].Tag=refCode[i];
            }
 * 
 * 
 * */
