using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using System.Data;
using System.Configuration;
using System.IO;
//using System.Drawing;

namespace KidGameData
{
    public  class clsData
    {
         private static OleDbConnection cn;
       private OleDbCommand cmd;
       private OleDbDataAdapter adapter;
       private static  int  rowInceriment;
       private static int picGroup;
      // int rowNumber = 18;
       Random random = new Random();

    


    static clsData()
       {
           try
           {

               cn = new OleDbConnection(ConfigurationManager.ConnectionStrings["connectionStrings"].ConnectionString);
               cn.Open();
               cn.Close();
               //global::System.Windows.Forms.MessageBox.Show("d");
           }
          
           catch 
           {
               try
               {
                   cn = new OleDbConnection();
                   cn.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|\db\dbGame.accdb;";
                   cn.Open();
                   cn.Close();
               }
               catch (Exception e)
               {
                   global::System.Windows.Forms.MessageBox.Show(e.ToString());
                   
                  
                 
               }
               
              // throw;
           } 
       }



       #region کدهای مربوط به پروفایل بازیکن

       public int SavePlayer(string namePlayer)
        {
            const string TOBE_PLAYER = "SELECT count(*) FROM profile where namePlayer=(@namePlayer)";

            cmd = cn.CreateCommand();
            cmd.Parameters.AddWithValue("@namePlayer", namePlayer);
            cmd.CommandText = TOBE_PLAYER;

            try
            {
            OpenCn();
                if (0 < (int)cmd.ExecuteScalar())
                {
                    CloseCn();
                    return 0;
                }
                else
                {
                    const string INS_PLAYER = "INSERT INTO profile (namePlayer) VALUES (@namePlayer)";
                    cmd.CommandText = INS_PLAYER;

                    int effect = cmd.ExecuteNonQuery();
                    CloseCn();

                    return effect;
                }

            }
            catch (Exception)
            {
                
                throw;
            }
           
          }



       public DataTable SelectPlayer()
       { 
             const string QUERY="SELECT profile.[codePlayer], profile.[namePlayer] FROM profile;";

             DataTable dt = new DataTable();

              cmd = cn.CreateCommand();
              cmd.CommandText = QUERY;

              adapter = new OleDbDataAdapter();
              adapter.SelectCommand = cmd;


              try
              {
                  OpenCn();
                  adapter.Fill(dt);
                  CloseCn();
              }
              catch (Exception)
              {

                  throw;
              }
             

           return dt;
       }

       //delete * from profile where codePlayer=[@codePlayer]

       public int DeletePlayer(int codePlayer)
       {
           const string QUERY= "delete * from profile where codePlayer=(@codePlayer);";
           cmd = cn.CreateCommand();
           cmd.CommandText = QUERY;
           cmd.Parameters.AddWithValue("@codePlayer", codePlayer);


           try
           {
               OpenCn();
               int effect = cmd.ExecuteNonQuery();
               CloseCn();
               cmd.Parameters.Clear();
               return effect;
           }
           catch (Exception)
           {
               
               throw;
           }
          

           
 
       }


       #endregion


       #region کدهای مربوط به بازی کلمات

       public DataTable SelectWords()
       {
           const string QUERY = "select code,name from word order by name";
           //List<string> tempList = new List<string>();
           DataTable dt=new DataTable();
           cmd = cn.CreateCommand();
           cmd.CommandText = QUERY;
           adapter = new OleDbDataAdapter();
           adapter.SelectCommand = cmd;
           try
           {
               OpenCn();
              // OleDbDataReader reader = cmd.ExecuteReader();
               adapter.Fill(dt);

               CloseCn();
               adapter = null;
               cmd = null;
           }
           catch (Exception)
           {

              // throw;
           }

           
           return dt;
       }



      public int UpdateWord(int code,string name)
       {
           const string QUERY = "update word set name=@name where code=@code";
           cmd = cn.CreateCommand();
           cmd.CommandText = QUERY;
          
           cmd.Parameters.AddWithValue("@name", name);
           cmd.Parameters.AddWithValue("@code", code);
           try
           {
               OpenCn();
               int effect = cmd.ExecuteNonQuery();
               CloseCn();
               return effect;
             
           }
           catch (Exception)
           {
               return -1;
               //throw;
           }
         

       }








       //public List<string> SelectWords()
       //{
       //    const string QUERY = "select name from word";
       //    List<string> tempList = new List<string>();

       //    cmd = cn.CreateCommand();
       //    cmd.CommandText = QUERY;
       //    try
       //    {
       //        OpenCn();
       //        OleDbDataReader reader = cmd.ExecuteReader();


       //        while (reader.Read())

       //            tempList.Add(reader.GetValue(0).ToString());

       //        reader.Close();
       //    }
       //    catch (Exception)
       //    {
               
       //        throw;
       //    }
         
       //    CloseCn();
       //    return tempList;
       //}



       public bool SaveWord(string name)
       {
           const string TOBE_WORD = "SELECT count(*) FROM word where name=(@name)";

           cmd = cn.CreateCommand();
           cmd.Parameters.AddWithValue("@name", name);
           cmd.CommandText = TOBE_WORD;

           try
           {
               OpenCn();
               if (0 < (int)cmd.ExecuteScalar())
               {
                   CloseCn();
                   return false;
               }
               else
               {
                   const string INS_WORD = "INSERT INTO word(name) VALUES (@name)";
                   cmd.CommandText = INS_WORD;

                   int effect = cmd.ExecuteNonQuery();
                   CloseCn();

                   return true;
               }

           }
           catch (Exception)
           {

               throw;
           }

       }



      





#endregion




       #region کدهای مربوط یه بازی نام تصاویر


       public DataTable PicForPicName(int rowSelect)
       {
           //RandomPicGroup();
           rowInceriment++;

           string query = " SELECT TOP " + rowSelect + " code,namePic, pic.[pic] FROM (select top " +
               rowSelect * rowInceriment + " * from pic order by code) ORDER BY code DESC , code;";
           
           DataTable dt = new DataTable();

           cmd = cn.CreateCommand();
           cmd.CommandText = query;

           adapter = new OleDbDataAdapter();
           adapter.SelectCommand = cmd;
           int effect;
           try
           {
               OpenCn();
                effect = adapter.Fill(dt);
               CloseCn();
           }
           catch (Exception)
           {
              // global::System.Windows.Forms.MessageBox.Show("Test");
               throw;
           }
          

           if (effect < rowSelect)
           {
               return null;
           }

           if (RowCount("allPic") <= rowInceriment * rowSelect)
               rowInceriment = 0;

           return dt;

       }



       #endregion







       #region کدهای مربوط به بدست آوردن و ذخیره امتیازات بازیکن در هر 3 بازی

       private int TobePlayer(int codePlayer, int codeGame)
       {
           const string QUERY1 = "SELECT Count(*) FROM scorePlayer where codePlayer=(@codePlayer) and codeGame=(@codeGame);";
           cmd = cn.CreateCommand();
           cmd.CommandText = QUERY1;
           cmd.Parameters.AddWithValue("@codePlayer", codePlayer);
           cmd.Parameters.AddWithValue("@codeGame", codeGame);

           try
           {
               OpenCn();
               int effect = int.Parse(cmd.ExecuteScalar().ToString());
               CloseCn();

               cmd.Parameters.Clear();
               return effect;
           }
           catch (Exception)
           {
               
               throw;
           }
          
       }



       private void CreatePlayer(int codePlayer, int codeGame)
       {
           const string QUERY2 = "insert into scorePlayer(codePlayer,codeGame,round,score,codeLevel,gift) values(@codePlayer,@codeGame,@round,@score,@codeLevel,@gift)";
           cmd = cn.CreateCommand();
           cmd.CommandText = QUERY2;
           cmd.Parameters.AddWithValue("@codePlayer", codePlayer);
           cmd.Parameters.AddWithValue("@codeGame", codeGame);
           cmd.Parameters.AddWithValue("@round", 1);
           cmd.Parameters.AddWithValue("@score", 0);
           cmd.Parameters.AddWithValue("@codeLevel", 1);
           cmd.Parameters.AddWithValue("@gift", 0);

           try
           {
               OpenCn();
               int effect1 = cmd.ExecuteNonQuery();
               CloseCn();
               cmd.Parameters.Clear();

           }
           catch (Exception)
           {
               
               throw;
           }
         
          

       }

       public List<int> GetLevelGame(int codePlayer,int codeGame)
       {
         
           int effect = TobePlayer(codePlayer, codeGame);

           if (effect<1)
           {
               CreatePlayer(codePlayer, codeGame);

           }
           
               List<int> listLevelPlayer = new List<int>();

               const string QUERY3 = "SELECT round,score,codeLevel,gift  FROM scorePlayer where codePlayer=(@codePlayer) and codeGame=(@codeGame);";
               cmd = cn.CreateCommand();
               cmd.CommandText = QUERY3;
               cmd.Parameters.AddWithValue("@codePlayer", codePlayer);
               cmd.Parameters.AddWithValue("@codeGame", codeGame);

               try
               {
                   OpenCn();
                   OleDbDataReader reader = cmd.ExecuteReader();

                   while (reader.Read())
                   {
                       listLevelPlayer.Add(int.Parse((reader.GetValue(0).ToString())));
                       listLevelPlayer.Add(int.Parse((reader.GetValue(1).ToString())));
                       listLevelPlayer.Add(int.Parse((reader.GetValue(2).ToString())));
                       listLevelPlayer.Add(int.Parse((reader.GetValue(3).ToString())));
                   }
                   reader.Close();
                   CloseCn();

                   cmd.Parameters.Clear();

                   return listLevelPlayer;
       
               }
               catch (Exception)
               {
                   
                   throw;
               }
              
        }

     





       public void SaveLevelPlayer(int codePlayer,int codeGame,int round,int score,int codeLevel,int gift)
       {
           const string QUERY = "update scorePlayer set round=(@round),score=(@score),codeLevel=(@codeLevel),gift=(@gift) where codePlayer=(@codePlayer) and codeGame=(@codeGame)";
           cmd = cn.CreateCommand();
           cmd.CommandText = QUERY;

           cmd.Parameters.AddWithValue("@round", round);
           cmd.Parameters.AddWithValue("@score", score);
           cmd.Parameters.AddWithValue("@codeLevel", codeLevel);
           cmd.Parameters.AddWithValue("@gift", gift);
           cmd.Parameters.AddWithValue("@codePlayer", codePlayer);
           cmd.Parameters.AddWithValue("@codeGame", codeGame);

           try
           {
               
           OpenCn();
       int effect= cmd.ExecuteNonQuery();
           CloseCn();
       
           }
           catch (Exception)
           {
               
               throw;
           }
       }


   #endregion





       #region کدهای مربوط به انتخاب تصویر از دیتابیس

       private void RandomPicGroup()
       {
           Random rand = new Random();
           picGroup = rand.Next(1, 3);

       }


       private void CreatPicGroup2()
       {
           if (picGroup == 1)
               picGroup = 2;
           else
               picGroup = 1;


       }



       public DataTable PicForPicSimilar1(int rowSelect)
       {
           RandomPicGroup();
           rowInceriment++;

           string query = " SELECT TOP " + rowSelect + " pic.[pic],refCode FROM (select top " +
               rowSelect * rowInceriment + " * from pic where picGroup=" + picGroup + " order by code) ORDER BY code DESC , code;";

           DataTable dt = new DataTable();

           cmd = cn.CreateCommand();
           cmd.CommandText = query;

           adapter = new OleDbDataAdapter();
           adapter.SelectCommand = cmd;
           try
           {

           OpenCn();
           int effect = adapter.Fill(dt);
           CloseCn();
           if (effect < rowSelect)
           {
               return null;
           }

           if (RowCount() <= rowInceriment * rowSelect)
               rowInceriment = 0;

           return dt;
           }
           catch (Exception)
           {
               
               throw;
           }

       }



       public DataTable PicForPicSimilar2(List<int> refCode)
       {
           CreatPicGroup2();

           int NewRowNumber = refCode.Count;

           string query = " SELECT  pic.[pic],refCode FROM  pic  where picGroup=" + picGroup + " and" +
               " ((refCode=@refCode0) ";
           //or (refCode=@refCode2) or (refCode=@refCode3) or (refCode=@refCode4) or"+
           //" (refCode=@refCode5) or (refCode=@refCode6))  ORDER BY code DESC , code;";

           for (int i = 1; i < NewRowNumber; i++)
           {
               string creatQuery = "or (refCode=@refCode" + i + ")";
               query += creatQuery;
           }
           query += ")  ORDER BY code DESC , code;";


           DataTable dt = new DataTable();

           cmd = cn.CreateCommand();
           cmd.CommandText = query;

           for (int i = 0; i < NewRowNumber; i++)
           {
               string refCode1 = "@refCode";
               refCode1 += i;
               cmd.Parameters.AddWithValue(refCode1, refCode[i]);

           }


           adapter = new OleDbDataAdapter();
           adapter.SelectCommand = cmd;

           try
           {
               OpenCn();
               adapter.Fill(dt);
               CloseCn();
           }
           catch (Exception)
           {
               
               throw;
           }
          

           return dt;

       }




       private int RowCount(string selectQuery=null)
       {
              string QUERY_ROWCOUNT;
              if (string.IsNullOrEmpty(selectQuery))

                  QUERY_ROWCOUNT = "SELECT count(*) FROM pic where picGroup=2;";
              else
                  QUERY_ROWCOUNT = "SELECT count(*) FROM pic;"; 

           cmd = cn.CreateCommand();
           cmd.CommandText = QUERY_ROWCOUNT;

           try
           {
               OpenCn();
               string rowCount = cmd.ExecuteScalar().ToString();
               CloseCn();

               if (string.IsNullOrEmpty(rowCount))
                   return 0;
               else
                   return int.Parse(rowCount);
           }
           catch (Exception)
           {
               
               throw;
           }
         

       }

       #endregion






       #region کدهای مربوط به ذخیره سازی تصویر در دیتابیس

       public int SavePic(string namePic,MemoryStream ms, int refCode, int picGroup)
        {
              
           const string QUERY_INS_IMAGE = "INSERT INTO pic (namePic,pic,refCode,picGroup) Values (@namePic,@pic,@refCode,@picGroup)";
     
            cmd = cn.CreateCommand();
           
            cmd.Parameters.AddWithValue("@namePic", namePic);
           // Command1.Parameters.Add("@Pic", OleDbType.Binary).Value = Memory.GetBuffer();
            cmd.Parameters.Add("@pic", OleDbType.Binary).Value = ms.ToArray();
            cmd.Parameters.AddWithValue("@refCode", refCode);
            cmd.Parameters.AddWithValue("@picGroup", picGroup);

            cmd.CommandText = QUERY_INS_IMAGE;

            try
            {
                OpenCn();
                int effect = cmd.ExecuteNonQuery();
                CloseCn();
                ms.Close();
                return effect;
            }
            catch (Exception)
            {
                
                throw;
            }
           

        }



          public int MaxRefCode()
          {
            const string QUERY_MAX_REFCODE = "SELECT max(refCode) FROM pic";
            string  maxRefCode;

            cmd = cn.CreateCommand();
            cmd.CommandText = QUERY_MAX_REFCODE;
            try
            {
                OpenCn();
                maxRefCode = cmd.ExecuteScalar().ToString();
                CloseCn();

            }
            catch (Exception)
            {
                
                throw;
            }
            

            if (string.IsNullOrEmpty(maxRefCode) )
                return 0;
             else
                return int.Parse(maxRefCode);
            }

       #endregion















          private void OpenCn()
        {
            if (cn.State == ConnectionState.Closed)
                cn.Open();

        }


         private void CloseCn()
        {
            if (cn.State == ConnectionState.Open)
                cn.Close();
        }





    }
}










#region
/*

  public void TestInsert()
       {
           List<string> tempList=new List<string>();
           string query = "select namePic from pic";
           cmd = cn.CreateCommand();
           cmd.CommandText = query;
            OpenCn();
               OleDbDataReader reader = cmd.ExecuteReader();

               while (reader.Read())

                   tempList.Add(reader.GetValue(0).ToString());
               reader.Close();
               query = "insert into word(name) values(@name)";

               for (int i = 0; i < tempList.Count; i++)
               {
                   cmd.CommandText = query;
                   cmd.Parameters.AddWithValue("@name", tempList[i]);
                   cmd.ExecuteNonQuery();
                   cmd.Parameters.Clear();
               }
               CloseCn();

       }





*/
#endregion