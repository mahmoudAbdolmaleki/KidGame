using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KidGameData;
using System.IO;
using System.Data.OleDb;
//using System.Drawing;
using System.Collections;
using System.Data;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Xml;
using System.Windows.Controls;


namespace KidGameBiz
{
  public  class clSPicName
    {


        private const string MESSGE = "تعداد تصاویر پایگاه داده کافی نیست ";
        const string MASSEGE = "اشکال سیستمی";
        clsData objClsData;
        DataTable dt;
        List<int> randomList;
       List<TextBlock> ListTxtBl;

        //برای اینکه تصاویر بصورت تصادفی به کاربر نمایش داده شود واز دیتابیس نیز تصادفی انتخاب شود 
        clsRandom objRandom;

        //تعداد سطرها خوانده شده از جدول
        private int rowNumber;

        //public int RowNumber
        //{
        //    get { return rowNumber; }
        //  //  set { rowNumber = value; }
        //}
     //   private List<int> listRefCode;// = new List<int>();
        // برای گرفتن عدد کد مربوط به هر تصویر
        //private    int[] refCode;

        //public int[] RefCode
        //{
        //    get { return refCode; }
        //  //set { refCode = value; }
        //}


        private void CreatObject()
        {
        
            objClsData = new clsData();
            randomList = new List<int>();
            objRandom = new clsRandom();
            dt = new DataTable();
           ListTxtBl=new List<TextBlock>();
        }
        

        //تصاویر خوانده شده از دیتابیس را به فرم ارسال میکند
        public List<TextBlock> GetPic(ref List<Image> imgActiveWin)
        {
            CreatObject();

           // listRefCode = new List<int>();

            dt.Clear();

            try
            {

            //بدست آوردن تعداد کنترلهای فعال تصویر در فرم
            rowNumber = imgActiveWin.Count;

            //پر کردن یک دیتاتیبل از تصاویر
            dt = objClsData.PicForPicName(rowNumber);

            if (dt == null)
            {
                MessageBox.Show(MESSGE);
                return null;
            }

            randomList = objRandom.rand(rowNumber);

            for (int i = 0; i < rowNumber; i++)
            {
                TextBlock tempTxtBl = new TextBlock();
                BitmapImage tempBitImage = new BitmapImage();
                MemoryStream ms = new MemoryStream();

              int  tempCode = int.Parse(dt.Rows[randomList[i]][0].ToString());
              string tempNmaePic = dt.Rows[randomList[i]][1].ToString();
               byte[] data = (byte[])dt.Rows[randomList[i]][2];

              // ListTxtBl.Add(tempNmaePic);

               tempTxtBl.Text = tempNmaePic;
               tempTxtBl.Tag = tempCode;

                tempBitImage.BeginInit();

                ms.Write(data, 0, data.Length);

                ms.Position = 0;
                // img[i] = Image.FromStream(ms);
                tempBitImage.StreamSource = ms;

                tempBitImage.EndInit();
                tempBitImage.Freeze();
               

                ListTxtBl.Add(tempTxtBl);
                imgActiveWin[i].Source = tempBitImage;
                imgActiveWin[i].Tag = tempCode;

            }
            int newRowNumber = (Int32)(rowNumber / 2);
            List<int> newRandomList = new List<int>();
             newRandomList = objRandom.rand(rowNumber);

            List<TextBlock> randomListTxtBl = new List<TextBlock>();

            for (int i = 0; i < newRowNumber; i++)
            {
                randomListTxtBl.Add(ListTxtBl[newRandomList[i]]);
            }

            return randomListTxtBl;
            }
            catch (Exception)
            {
                System.Windows.Forms.MessageBox.Show(MASSEGE);
                return null; 
               // throw;
            }

        }


      
    }
}
