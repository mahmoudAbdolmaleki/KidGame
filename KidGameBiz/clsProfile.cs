using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KidGameData;
using System.Data;

namespace KidGameBiz
{
 public  class clsProfile//:clsData
    {

        private const string MESSAGE = "تراکنش  انجام شد";
        private const string MESSAGE1 = " این نام قبلا استفاده شده است ";
        private const string MESSAGE2 = "اشکال سیستمی";

        clsData objData = new clsData();

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


        public int CreatPlayer()
        {
          
          int effect=objData.SavePlayer(namePlayer);

           if(effect>0)

              global::System.Windows.Forms.MessageBox.Show(MESSAGE);

            else

               global::System.Windows.Forms.MessageBox.Show(MESSAGE1);

          return effect;
        }


        public DataTable GetPlayer()
        {
            //DataTable dt = new DataTable();
            DataTable dt = objData.SelectPlayer();
            
            DataRow row = dt.NewRow();

            row[0] = 0;
            row[1] = "مهمان";
            dt.Rows.Add(row);

            return dt;
 
        }



        public void DeletPlayer()
        {

         global::System.Windows.Forms.DialogResult re = System.Windows.Forms.MessageBox.Show("    آیا قصد حذف بازیکن را دارید     ", "حذف بازیکن",System.Windows.Forms.MessageBoxButtons.YesNo);
            if (re == System.Windows.Forms.DialogResult.Yes)
            {
            int effect = objData.DeletePlayer(codePlayer);

            if (effect > 0)

                global::System.Windows.Forms.MessageBox.Show(MESSAGE);

            else

                global::System.Windows.Forms.MessageBox.Show(MESSAGE2);
            }

        }
      
       // throw new NotImplementedException();
        
    }
}
