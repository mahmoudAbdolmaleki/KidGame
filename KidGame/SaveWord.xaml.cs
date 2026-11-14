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
using System.Data;
using KidGameBiz;

namespace KidGame
{
    /// <summary>
    /// Interaction logic for SaveWord.xaml
    /// </summary>
    public partial class SaveWord : Window
    {
        DataTable dt;
        bool update = false;
        string word;
        string wordBeforUpdate;
        int codeWord;
        clsAlphaWord objAlphaword = new clsAlphaWord();
        const string MESSAGE1 = "تراکنش با موفقیت انجام شد";
        const string MESSAGE2 = "مشکل صیستمی";

      private void  Update()
        {   global::System.Windows.Forms.DialogResult re = System.Windows.Forms.MessageBox.Show("  آیا "+" | "+txtWord.Text+" | "+"  را بجای "+" | "+wordBeforUpdate+" | "+" ثبت می کنید  ", "ثبت تغییرات", System.Windows.Forms.MessageBoxButtons.YesNo);
                    if (re == System.Windows.Forms.DialogResult.Yes)
                    {
                         int effect= objAlphaword.Update(codeWord,txtWord.Text);

                         if (effect == -1)
                             MessageBox.Show(MESSAGE2);
                         else
                         {
                             MessageBox.Show(MESSAGE1);
                             CmbBinding();
                            
                         }
                    }
           }

      private void Save()
      {
          global::System.Windows.Forms.DialogResult re = System.Windows.Forms.MessageBox.Show("  آیا " + " | " + txtWord.Text + " | " + "  را ثبت می کند  ", "ثبت تغییرات", System.Windows.Forms.MessageBoxButtons.YesNo,System.Windows.Forms.MessageBoxIcon.None,System.Windows.Forms.MessageBoxDefaultButton.Button1,System.Windows.Forms.MessageBoxOptions.RightAlign);
          if (re == System.Windows.Forms.DialogResult.Yes)
          {
              objAlphaword.SaveWord(txtWord.Text.TrimEnd());
              CmbBinding();
             
          }
      }


        private void CmbBinding()
        {
           dt = new DataTable();
            dt = objAlphaword.GetWords();

            cmbWord.DataContext = dt;
            cmbWord.SelectedValuePath = dt.Columns[0].ToString();
            cmbWord.DisplayMemberPath = dt.Columns[1].ToString();

            cmbWord.SelectedIndex = 0;
          

        }


        public SaveWord()
        {
            InitializeComponent();
        }


        private void DeleteChar()
        {
         
            if(!string.IsNullOrEmpty(word))
                  word = word.Substring(0,txtWord.Text.Length-1);
           
           
        }


        private void ChoseChar(RoutedEventArgs e)
        {
            
            try
            {

                Button tempButton = (Button)e.Source;
                string temp = tempButton.Content.ToString();
                
                switch (temp)
                {
                    case "الف": temp = "ا";
                        break;

                    case "فاصله": temp=" ";
                        break;

                    case "حذف":DeleteChar();
                            temp="";
                        break;
                }

                        word += temp;

                        if (string.IsNullOrWhiteSpace(word))
                        {
                            word = null;
                            txtWord.Text = word;
                        }
                        else
                        {
                            if (word.Length < 11)
                            {

                                if (temp != " ")
                                    txtWord.Text = word;
                            }
                            else MessageBox.Show("حداکثر طول کلمه باید کمتر از 10 حرف باشد");
                        }
              
               
            }


            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }



        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CmbBinding();
        }





        void WordSave()
        {
            if (!string.IsNullOrWhiteSpace(txtWord.Text))
            {

                if (update)
                {
                    Update();

                }
                else
                {
                    Save();
                }

            }
            update = false;
            txtWord.Text = null;
            word = null;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {

            WordSave();

        }




        void EditWord()
        {
            if (cmbWord.SelectedIndex > -1)
            {
                update = true;
                txtWord.Text = word = wordBeforUpdate = cmbWord.Text;
                codeWord = (int)cmbWord.SelectedValue;

            }
            else
            {
                txtWord.Text = null;
                update = false;
            }
        }

        private void btnWordEdit_Click(object sender, RoutedEventArgs e)
        {

            EditWord();
           
        }

       

        private void gridWord_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            ChoseChar(e);
        }

        private void btnNewWord_Click(object sender, RoutedEventArgs e)
        {
            txtWord.Text = null;
            update = false;
        }

       
       
    }
}
