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
  public  class clsPicSimilar
    {
      const string MESSAGE = "اشکال سیستمی";
      private const string MESSGE = "تعداد تصاویر پایگاه داده کافی نیست ";
        clsData objClsData;
        DataTable dt;
       List<int> randomList;
     
      //برای اینکه تصاویر بصورت تصادفی به کاربر نمایش داده شود واز دیتابیس نیز تصادفی انتخاب شود 
        clsRandom objRandom;

      //تعداد سطرها خوانده شده از جدول
        private int rowNumber;

        //public int RowNumber
        //{
        //    get { return rowNumber; }
        //  //  set { rowNumber = value; }
        //}
    private  List<int> listRefCode;// = new List<int>();
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
        }


      //تصاویر خوانده شده از دیتابیس را به فرم ارسال میکند
        public int GetPic1(List<Image> imgActiveWin)
        {
            CreatObject();

            listRefCode = new List<int>();

            dt.Clear();

            try
            {

            //بدست آوردن تعداد کنترلهای فعال تصویر در فرم
            rowNumber = imgActiveWin.Count;

            //پر کردن یک دیتاتیبل از تصاویر
            dt = objClsData.PicForPicSimilar1(rowNumber);

            if (dt == null)
            {
                MessageBox.Show(MESSGE);
                return -1;
             }


            randomList = objRandom.rand(rowNumber);

            for (int i = 0; i < rowNumber; i++)
            {
                BitmapImage tempBitImage = new BitmapImage();
                MemoryStream ms = new MemoryStream();

                byte[] data = (byte[])dt.Rows[randomList[i]][0];
                listRefCode.Add(int.Parse(dt.Rows[randomList[i]][1].ToString()));

               
                tempBitImage.BeginInit();

                ms.Write(data, 0, data.Length);

                ms.Position = 0;
               // img[i] = Image.FromStream(ms);
                tempBitImage.StreamSource = ms;

                tempBitImage.EndInit();
                tempBitImage.Freeze();
               // bitImage.Freeze();
                
                imgActiveWin[i].Source = tempBitImage;
                imgActiveWin[i].Tag = listRefCode[i];

            }

            return 1;
            }
            catch (Exception)
            {
                
                throw;
            }

        }



        //این متد یک دیتاتیبل از تصاویری که کدهای ارجاع آنها برابر با تصاویر متد قبلی است میسازد 
        private DataTable SelectPic2()
        {
            CreatObject();
            dt.Clear();

            try
            {
            //تعداد تصاویر برای مقایسه باید کمتر از کل تصاویر باشد
            int newRowNumber = (Int32)(rowNumber/ 2);

            //برای ذخیره کدهای تصاویر
             List<int> newRefCode = new List<int>();

            //یک لیست تصادفی از صفر تا عدد ارسال شده میسازد
            randomList = objRandom.rand(rowNumber);

            for (int i = 0; i < newRowNumber; i++)
            {
                //ازبین کدهای کل تصاویر چند کد بصورت تصادفی برای انتخاب از دیتابیس انتخاب میشود
                newRefCode.Add(listRefCode[randomList[i]]);

            }
            //پر کردن یک دیتاتیبل از تصاویر برای مقایسه با تصاویر قبلی
            dt = objClsData.PicForPicSimilar2(newRefCode);
          
            return dt;

            }
            catch (Exception)
            {
                System.Windows.Forms.MessageBox.Show(MESSAGE);
                throw;
            }
        }

       

      //این متد تصاویر و کدهای مربوط به آنها را به فرم ارسال میکند
       public List<Image> GetPic2()
        {
            try
            {

            int newRowNumber = (Int32)(rowNumber / 2);
           
             List<Image> img2 = new List<Image>();

            SelectPic2();


            //یک لیست تصادفی از صفر تا عدد ارسال شده میسازد
            randomList = objRandom.rand(newRowNumber);

            // newRefCode=0;

            //گرفتن تصاویر وکدهای آنها از دیتاتیبل
            for (int i = 0; i < newRowNumber; i++)
            {
                Image tempImg = new Image();
                BitmapImage tempBitImage = new BitmapImage();

                byte[] data = (byte[])dt.Rows[randomList[i]][0];
                int newRefCode = int.Parse((dt.Rows[randomList[i]][1]).ToString());
                
                MemoryStream ms = new MemoryStream();

                 tempBitImage.BeginInit();

                ms.Write(data, 0, data.Length);

                ms.Position = 0;
               // img[i] = Image.FromStream(ms);
                tempBitImage.StreamSource = ms;
                tempBitImage.EndInit();

                tempBitImage.Freeze();
                

                tempImg.Tag = newRefCode;
                tempImg.Source = tempBitImage;
                //img2[0].BeginInit();
                img2.Add(tempImg);
                
              }

            return img2;
            }
            catch (Exception)
            {
                
                throw;
            }
 
        }




       

    }
}



























        //public object[,] GetPics()
        //{
        //    object[,] ob = new object[14, 2];
        //  //    object[,] ob=new object[14,2];
        //  //    Image[,] bm = new Image[14,2];
        //  //    ArrayList[,] arraList=new ArrayList[14,2];
        //    return ob = obj_ClsData.SelectPic();

        //  //  for(int i=0;i<14;i++)
        //  //  {

        //  //    byte[] bufer = (byte[])ob[i,0];
        //  //   //byte[] bufer1 = (byte[])ob[i,1];
        //  //    MemoryStream ms = new MemoryStream(bufer);
        //  //    bm[i,0] = Image.FromStream(ms);

        //  //    arraList[i, 0].Add(bufer);
        //  //    arraList[i, 1].Add(ob[i, 1]);
        //  //    bufer = null;
        //  //    ms = null;
          
        //  //   }
        //  //    return arraList;
        //}

        // System.Drawing.Image[] img = System.Drawing.Image[rowNumber];//.FromStream(ms); 










