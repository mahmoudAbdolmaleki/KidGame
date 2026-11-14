using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;






namespace KidGameBiz
{
  public  class clsRandom
    {

    
      private   Random random = new Random();
      private List<int> randomList;
     
      int pointer;

      public List<int> rand(int maxRandom)
         {
             randomList = new List<int>();
          
            do
             {
                 //برای تولید اعداد از 0 تا عدد ارسال شده
            int numberNewRandom = random.Next(0, maxRandom);

            if (check(numberNewRandom) == false)
                     continue;
                 else
                 {
                     randomList.Add(numberNewRandom);
                   
                 }
                 pointer++;
             }
            while (randomList.Count < maxRandom);

            return randomList;

         }
    
      

        // برای بررسی تکراری نبودن
      private bool check(int newRandom)
        {
            foreach (int tempRand in randomList)
                if (tempRand == newRandom)
                {
                    return false;
                }

            return true;
        }




       // public int numberNewRandom { get; set; }
    }
}





//نمایش اطلاعات
//foreach (int i in numbers)
//{
//    MessageBox.Show(i.ToString());
//}
