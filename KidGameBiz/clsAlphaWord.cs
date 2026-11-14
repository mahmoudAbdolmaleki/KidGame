using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KidGameData;
using System.Data;

namespace KidGameBiz
{
  public  class clsAlphaWord
    {
        clsData objData = new clsData();
        List<string> listWord = new List<string>();
        const string MESSEGE = "اشکال سیستمی";
        const string MESSEGE1 = "تراکنش با موفقیت انجام شد";
        const string MESSEGE2 = "این کلمه قبلا ثبت شده است";
        DataTable dt;//; = new DataTable();
        //public void TestInsert()
        //{
        //    objData.TestInsert();
        //}

        public clsAlphaWord()
        {
           
        }

        public string GetWord()
        {
            Random rand = new Random();
            try
            {
                int pointer = rand.Next(0, dt.Rows.Count);
                return dt.Rows[pointer][1].ToString();

            }
            catch (Exception)
            {
                System.Windows.Forms.MessageBox.Show(MESSEGE);
                return null;
                //throw;
            }
            
        }


        public void SaveWord(string word)
        {
            bool temp= objData.SaveWord(word);
            if(temp)
                System.Windows.Forms.MessageBox.Show(MESSEGE1);
            else
                System.Windows.Forms.MessageBox.Show(MESSEGE2);
 
        }


        public DataTable GetWords()
        {
            dt = new DataTable();
            try
            {
                dt = objData.SelectWords();
            }
            catch (Exception)
            {

                System.Windows.Forms.MessageBox.Show(MESSEGE);
            }
            return dt;
        }


        public int Update(int code, string name)
        {
          return  objData.UpdateWord(code,name);
        }
          



    }
}
