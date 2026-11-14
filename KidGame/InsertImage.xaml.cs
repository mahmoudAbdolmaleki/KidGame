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
using System.IO;
using KidGameBiz;

//using System.Windows.Forms;

namespace KidGame
{
    /// <summary>
    /// Interaction logic for ins_Image.xaml
    /// </summary>
    public partial class InsertImage : Window
    {

        private  clsImage objClsImage1 = new clsImage();
        private  clsImage objClsImage2 = new clsImage();

        private const string MESSAGE = "تراکنش  انجام شد";
        private const string MESSAGE1 = "لطفا تصاویر را انتخاب کنید";
        private const string MESSAGE2 = "اشکال سیستمی";
        private const string MESSAGE3 = "لطفا نام تصاویر را وارد کنید";



        private BitmapImage selectPic(ref clsImage objClsImage)
        {
            try
            {
                MemoryStream ms = objClsImage.PicChoice();


                if (ms != null)
                {
                    BitmapImage bitImage = new BitmapImage();

                    bitImage.BeginInit();
                    bitImage.StreamSource = ms;
                    // bitImage.CacheOption = BitmapCacheOption.OnLoad;
                    bitImage.EndInit();
                    bitImage.Freeze();
                    //ms.Close();
                    return bitImage;

                }
            }
            catch (Exception)
            {
                System.Windows.Forms.MessageBox.Show(MESSAGE2);
               
                
            }

            return null;



            //bitmapImage.StreamSource.Seek(0, System.IO.SeekOrigin.Begin);//very important, it should be set to the start of the stream

            //bitmapImage.StreamSource.Read(imageData, 0, imageData.Length);

 
        }


        public InsertImage()
        {
            InitializeComponent();
        }



        private void saveWithResizePic()
        {

            int maxRefCode = objClsImage1.MaxRefCode();
            int newRefCode = maxRefCode + 1; ;

            objClsImage1.NamePic = txtNamePic1.Text;
            objClsImage1.RefCode = newRefCode;
            objClsImage1.PicGroup = 1;
            objClsImage1.PicResize();
            objClsImage1.PicSave();

            objClsImage2.NamePic = txtNamePic2.Text;
            objClsImage2.RefCode = newRefCode;
            objClsImage2.PicGroup = 2;
            objClsImage2.PicResize();
            objClsImage2.PicSave();

            System.Windows.Forms.MessageBox.Show(MESSAGE);
            image1.Source = null;
            image2.Source = null;
            txtNamePic1.Text = null;
            txtNamePic2.Text = null;
        }



        private void btn_SelectImag1_Click(object sender, RoutedEventArgs e)
        {
            
                image1.Source = selectPic(ref objClsImage1);
                  
        }



        private void btn_SelectImag2_Click(object sender, RoutedEventArgs e)
        {

            image2.Source = selectPic(ref objClsImage2);
           
         }



        private void btnSavePics_Click(object sender, RoutedEventArgs e)
        {

            if (image1.Source != null && image2.Source != null)
            {
                if (string.IsNullOrWhiteSpace(txtNamePic1.Text) || string.IsNullOrWhiteSpace(txtNamePic2.Text))
                    MessageBox.Show(MESSAGE3);
                else
                {
                    saveWithResizePic();
                   
                }
            }
            else
               MessageBox.Show(MESSAGE1);

        }






        private void image1_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {
            
        }

        private void image1_MouseEnter(object sender, MouseEventArgs e)
        {
          
        }


        private void image1_MouseDown(object sender, MouseButtonEventArgs e)
        {
          //  System.Windows.Forms.MessageBox.Show("Test");
        }

    
    
    
    
    
    
    }
}






/*
 
           
 * 
            //MemoryStream streamPic = obj_ClsImage1.selectPic();
            //if (streamPic != null)
            //{
            //    BitmapImage bitImage = new BitmapImage();

            //    bitImage.BeginInit();
            //    bitImage.StreamSource = streamPic;
            //    // bitImage.CacheOption = BitmapCacheOption.OnLoad;
            //    bitImage.EndInit();
            //    bitImage.Freeze();
            //    image1.Source = bitImage;
            // }
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * //image1.Tag = 6;
            //MessageBox.Show(image1.Tag.ToString());
 * 
            // BitmapImage bitImage1 = new BitmapImage();
            //bitImage1 = (BitmapImage)image1.Source;

            //image2.Source = bitImage1;

 
 * 
 * * 
 * string filename="jlk";
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
           // dlg.FileName = "Document"; // Default file name
          //  dlg.DefaultExt = ".txt"; // Default file extension
           // dlg.Filter = "Text documents (.txt)|*.txt"; // Filter files by extension

            // Show open file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process open file dialog box results
          //  dlg.ShowDialog().Value;
            if (result == true)
            {
                // Open document
                 filename = dlg.FileName;
                MessageBox.Show(filename);
               // image1.Source = filename;
            }
            byte[] buffer = System.IO.File.ReadAllBytes(filename);
            MemoryStream ms = new MemoryStream(buffer);



*/