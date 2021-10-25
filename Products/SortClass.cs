using System;
using System.Collections.Generic;
using System.Text;

namespace SigmaTask9.Products
{
    delegate int SortDelegate(Object obj1, Object obj2);
    static class SortClass
    {
        public static void Sort(Product[] prod_arr, SortDelegate deleg )
        {
            //алгоритм сортування бульбашки з прапорцем
            int flag = 0;
            for (int i = 0; i < prod_arr.Length; i++)
            {
                flag = 0;
                for (int k = 0; k + 1 < prod_arr.Length; k++)
                {
                    //у порядку зростання
                    if (deleg(prod_arr[k], prod_arr[k+1]) == 1)
                    {
                        //є зміни, міняємо прапорець
                        flag = 1;
                        Product temp = prod_arr[k];
                        prod_arr[k] = prod_arr[k + 1];
                        prod_arr[k + 1] = temp;

                    }
                }
                //якщо прапорець не змінювався - вихід
                if (flag == 0)
                {
                    break;
                }
            }

        }
    }
}
