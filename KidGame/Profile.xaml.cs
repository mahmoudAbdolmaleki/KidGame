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

namespace KidGame
{
    /// <summary>
    /// Interaction logic for Profile.xaml
    /// </summary>
    public partial class Profile : Window
    {
        private const string MESSAGE = "لطفا یک نام وارد کنید";

        clsProfile objClsProfile = new clsProfile();
        private int codePlayer;

        public int CodePlayer
        {
            get { return codePlayer; }
            set { codePlayer = value; }
        }




        private string namePlayer;

        public string NamePlayer
        {
            get { return namePlayer; }
            set { namePlayer = value; }
        }

            
            public Profile()
        {
            InitializeComponent();
        }






            void CreatePlayer()
            {
                if (string.IsNullOrEmpty(txt_namePlayer.Text))
                {
                    MessageBox.Show(MESSAGE);
                }
                else
                {


                    objClsProfile.NamePlayer = txt_namePlayer.Text;
                    objClsProfile.CreatPlayer();


                }
            }

        private void btn_createPlayer_Click(object sender, RoutedEventArgs e)
        {
            CreatePlayer();
         
        }






        void DeletePlayer()
        {
            objClsProfile.DeletPlayer();
            txt_namePlayer.Text = null;
            btnDeletePlayer.IsEnabled = false;
        }

        private void btnDeletePlayer_Click(object sender, RoutedEventArgs e)
        {
            DeletePlayer();
            

        }





        void LoadForm()
        {
            if (codePlayer > 0)
            {
                txt_namePlayer.Text = namePlayer;
                objClsProfile.CodePlayer = codePlayer;
            }
            else
                btnDeletePlayer.IsEnabled = false;
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadForm();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MainWindow objMain = new MainWindow();
            objMain.Show();
        }
    }
}
