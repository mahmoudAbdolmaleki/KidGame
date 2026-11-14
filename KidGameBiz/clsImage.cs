using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using KidGameData;
using System.Windows.Forms;
using System.Drawing;

//using System.Windows.Media.Imaging;




namespace KidGameBiz
{
  public  class clsImage
    {
      
        clsData objClsData = new clsData();
        string pathPic;
        int newWidth =80;
        int newHeight = 80;
        const string MESSEGE = "اشکال سیستمی";

       private  Bitmap picResize;


        private string namePic;

        public string NamePic
        {
            get { return namePic; }
            set { namePic = value; }
        }


        private int refCode;

        public int RefCode
        {
            get { return refCode; }
            set { refCode = value; }
        }

        private int picGroup;

        public int PicGroup
        {
            get { return picGroup; }
            set { picGroup = value; }
        }


        public MemoryStream PicChoice()
        {
           
            OpenFileDialog openDialog = new OpenFileDialog();
            try
            {
                if (openDialog.ShowDialog() == DialogResult.OK)
                {
                    pathPic = openDialog.FileName;

                    byte[] buffer = System.IO.File.ReadAllBytes(pathPic);

                    MemoryStream ms = new MemoryStream(buffer);
                    //  memory.Write(buffer, 0, buffer.Length);

                    picResize = new Bitmap(80, 80);
                    picResize = (Bitmap)Image.FromFile(pathPic);

                    return ms;
                }
                else return null;
            }
            catch (Exception)
            {
                
                throw;
            }
            

        }


        public void PicResize()
        {
               //picResize = new Bitmap(80, 80);
               //picResize = (Bitmap)Image.FromStream(memory);
            try
            {
                Bitmap temp = new Bitmap(newWidth, newHeight);//,picResize.PixelFormat
                Graphics g = Graphics.FromImage(temp);

                Rectangle srcRect = new Rectangle(0, 0, picResize.Width, picResize.Height);
                Rectangle destRect = new Rectangle(0, 0, newWidth, newHeight);

                g.DrawImage(picResize, destRect, srcRect, GraphicsUnit.Pixel);

                picResize = (Bitmap)temp.Clone();

                g.Dispose();
                temp.Dispose();
            }
            catch (Exception)
            {
                
                throw;
            }
                
             
        }



        public int PicSave()
        {
            //Random random = new Random();
            //int newName=  random.Next(1,1000);

            //picResize.Save(@"C:\" + "test" + newName + "." + System.Drawing.Imaging.ImageFormat.Jpeg);
            MemoryStream ms = new MemoryStream();

            try
            {
               
                picResize.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);

                int rowEffect;
                return rowEffect = objClsData.SavePic(namePic, ms, refCode, picGroup);
       
            }
            catch (Exception)
            {
                System.Windows.Forms.MessageBox.Show(MESSEGE);
                return -1;  
                //throw;
            }
            }


        public int MaxRefCode()
        {
          return  objClsData.MaxRefCode();
        }




    }
}












/*

              // Image d = (Image)picResize;

               //picResize = (Bitmap)Image.FromStream(memory);









*/