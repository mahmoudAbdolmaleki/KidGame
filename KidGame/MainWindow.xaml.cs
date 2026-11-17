using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using KidGameBiz;
using System.Threading.Tasks;


namespace KidGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

             private void ProfileShow()
        {
            Profile objprofile = new Profile();


            objprofile.CodePlayer = int.Parse(cmbPlayer.SelectedValue.ToString());
            objprofile.NamePlayer = cmbPlayer.Text;
            objprofile.Show();
        }

              private void CmbBinding()
        {
            clsProfile objProfile = new clsProfile();
            //DataTable dt = new DataTable();

            DataTable dt = objProfile.GetPlayer();
         

            cmbPlayer.DataContext = dt;
            cmbPlayer.SelectedValuePath = dt.Columns[0].ToString();
            cmbPlayer.DisplayMemberPath = dt.Columns[1].ToString();

            cmbPlayer.SelectedValue = 0;
            
        }


              private void PicSimilarShow()
        {
            PictureSimilar objPicSimilar = new PictureSimilar();

            objPicSimilar.CodePlayer = int.Parse(cmbPlayer.SelectedValue.ToString());
            objPicSimilar.NamePlayer = cmbPlayer.Text;
            objPicSimilar.CodeGame = 1;

            objPicSimilar.Show();
        }


             private void PicNameShow()
        {
            PicName objPicName = new PicName();

            objPicName.CodePlayer = int.Parse(cmbPlayer.SelectedValue.ToString());
            objPicName.NamePlayer = cmbPlayer.Text;
            objPicName.CodeGame = 2;

            objPicName.Show();
        }


             private void AlphaWordShow()
        {
            AlphabetWord objAlphaWord = new AlphabetWord();

            objAlphaWord.CodePlayer = int.Parse(cmbPlayer.SelectedValue.ToString());
            objAlphaWord.NamePlayer = cmbPlayer.Text;
            objAlphaWord.CodeGame = 3;
            objAlphaWord.Show();
        }


             public MainWindow()
        {
            InitializeComponent();
        }

       

      




        private void btn_PicSimilar_Click(object sender, RoutedEventArgs e)
        {
            PicSimilarShow();
        }





        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            App.Log("Main window displayed; loading players in background.");
            try
            {
                DataTable dt = await Task.Run(() => new clsProfile().GetPlayer());
                if (dt == null)
                    throw new InvalidOperationException("دریافت فهرست بازیکنان انجام نشد.");
                cmbPlayer.DataContext = dt;
                cmbPlayer.SelectedValuePath = dt.Columns[0].ToString();
                cmbPlayer.DisplayMemberPath = dt.Columns[1].ToString();
                cmbPlayer.SelectedValue = 0;
                App.Log("Player list loaded.");
            }
            catch (Exception ex)
            {
                App.Log("Player list load failed: " + ex);
                MessageBox.Show("خواندن دیتابیس انجام نشد:\n" + ex.Message, "KidGame",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

      

       

        private void btnPicName_Click(object sender, RoutedEventArgs e)
        {
            PicNameShow();
        }

        private void btnAlphaWord_Click(object sender, RoutedEventArgs e)
        {
            AlphaWordShow();
        }

       

        private void menuItemSaveWord_Click(object sender, RoutedEventArgs e)
        {
            SaveWord objSveWord = new SaveWord();
            objSveWord.Show();
        }

        private void menuItemProfile_Click(object sender, RoutedEventArgs e)
        {
            ProfileShow();
            this.Close();
        }

        private void menuItemSavePic_Click(object sender, RoutedEventArgs e)
        {
            InsertImage obj_Ins_Image = new InsertImage();
            obj_Ins_Image.Show();
        }



        private void exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

      
       
       
        
        
       
        
    }
}
