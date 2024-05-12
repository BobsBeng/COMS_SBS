using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using WebApplication1.Models;
using System.Web.Mvc;

namespace WebApplication1.Utility
{
    public static class Helper
    {
        public static bool IsAccountExpired(string userName)
        {
            using (Entities db = new Entities())
            {
                
                var userNameCheck = db.SBS_COMS_USER.Where(x => x.UserName == userName).FirstOrDefault();
                TimeSpan? expirationDifference = userNameCheck.EXPIRATION_DT - DateTime.Now;
                if (expirationDifference.HasValue)
                {

                    int aging = (int)expirationDifference.Value.TotalDays; 
                    // If account aging is expired, return true
                    return aging <= 0;
                }
                // Handle the case where expirationDate is null
                return false; // Or you can throw an exception or handle it differently based on your requirements
            }
        }

        public static bool checkPasswordHistory( string PasswordHash,string username)
        {
            string curVal = string.Empty;
            string query = string.Empty;
            using (Entities db = new Entities())
            {

                query = "SELECT \"PasswordHash\" FROM SBS_COMS_USER_R WHERE \"UserName\" = '" + username + "' AND \"PasswordHash\" = '" + PasswordHash + "'";
                curVal = db.Database.SqlQuery<string>(query).FirstOrDefault();
                if (!string.IsNullOrEmpty(curVal))
            {
                return true;
            }
            return false;
            }
        }

    }
}