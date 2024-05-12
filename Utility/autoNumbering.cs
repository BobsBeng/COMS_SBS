using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.Models;

namespace WebApplication1.Utility
{
    public class autoNumbering
    {
        public static int getLastNumber(string cnt_grp, DateTime period)
        {
            int lastNumb = 1;
            string curVal = string.Empty;
            string query = string.Empty;
            //Get lastest number from SBS_COMS_COUNTER_NUMBER_ALL
            using (Entities db = new Entities())
            {
                query = "SELECT CURR_VALUE VAL FROM SBS_COMS_COUNTER_NUMBER_ALL WHERE CNT_GRP = '" + cnt_grp + "' AND PERIOD = LAST_DAY(TO_DATE('" + period.ToString("dd/MM/yyyy") + "','DD/MM/YYYY'))";
                curVal = db.Database.SqlQuery<string>(query).FirstOrDefault();
            }

            if (!string.IsNullOrEmpty(curVal))
            {
                lastNumb = Convert.ToInt32(curVal)+1;
            }
            return lastNumb;
        }

        public static void updateLastNumber(string cnt_grp, DateTime period, int lastNumb)
        {
    
            //Check is cnt group and period is exist 
            // if exist then update the data
            //if no then insert the new row
            int count = 0;
            string co_cd = "SBS";
            string query = string.Empty;

            using (Entities db = new Entities())
            {
                count = db.Database.SqlQuery<Int32>("SELECT COUNT(*) CNT FROM SBS_COMS_COUNTER_NUMBER_ALL WHERE CNT_GRP = '" + cnt_grp + "' AND PERIOD = LAST_DAY(TO_DATE('" + period.ToString("dd/MM/yyyy") + "','DD/MM/YYYY'))").FirstOrDefault();
            }

            if (count > 0)
            {
                //update counter
                using (Entities db = new Entities())
                {
                    query = "UPDATE SBS_COMS_COUNTER_NUMBER_ALL SET CURR_VALUE = TO_CHAR(" + lastNumb + ",'000000') WHERE CNT_GRP = '" + cnt_grp + "' AND PERIOD = LAST_DAY(TO_DATE('" + period.ToString("dd/MM/yyyy") + "','DD/MM/YYYY'))";
                    db.Database.ExecuteSqlCommand(query);
                }
            }
            else
            {
                //insert counter
                using (Entities db = new Entities())
                {
                    query = "INSERT INTO SBS_COMS_COUNTER_NUMBER_ALL(CNT_GRP, CO_CD, PERIOD, CURR_VALUE) VALUES ('" + cnt_grp + "','" + co_cd + "', LAST_DAY(TO_DATE('" + period.ToString("dd/MM/yyyy") + "','DD/MM/YYYY')),  TO_CHAR(" + lastNumb + ",'000000'))";
                    db.Database.ExecuteSqlCommand(query);
                }
            }

        }
        public static  bool HasSpecialCharacter(string password)
        {
            string specialCharacters = "!@#$%^&*()_-+=<>{}[]|\\:;\"',.?/";
            return password.Any(char.IsSymbol) || password.Any(c => specialCharacters.Contains(c));
            //return password.Any(char.IsLetterOrDigit) && password.Any(char.IsSymbol);
        }

    }
}