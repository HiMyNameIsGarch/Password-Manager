using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace PassManager.Models
{
    public class FieldsHelper
    {
        public static TaskStatus VerifyEmail(string email)
        {
            //verify length of the email
            if (email.Length < 6) return new TaskStatus(true, "Your email is too short");
            //verify if contains a "@"
            if (!email.Contains("@")) return new TaskStatus(true, "Your email does not contain a '@'");
            //check domain length
            if (email.Length - email.IndexOf("@") < 4) return new TaskStatus(true, "Your domain is too short!");
            //check for spaces
            if (email.Contains(" ")) return new TaskStatus(true, "Your email contains spaces!");
            //check for sequence of dots
            if (email.Contains("..")) return new TaskStatus(true, "Your email contains more than 1 dot in a row");
            //split the email in 2 parts
            string[] emailParts = email.Split('@');
            //check if exists another "@"
            if (emailParts.Length != 2) return new TaskStatus(true, "Your email does not contain just one '@'");
            //check if domain contains a dot
            if (!email.Contains('.')) return new TaskStatus(true, "Your domain does not contain a dot!");
            //split the domain in 2 parts
            string[] domainParts = emailParts[1].Split('.');
            //verify if the email contains more than 1 dot
            if (domainParts.Length != 2) return new TaskStatus(true, "Your domain contains more than 1 dot");
            foreach (char c in domainParts[0])
            {
                //verify if domain does not contain invalid characters
                if (!char.IsLetterOrDigit(c)) return new TaskStatus(true, "Your domain contains invalid characters");
            }
            foreach (char c in domainParts[1])
            {
                //verify if dns does not contain invalid characters
                if (!char.IsLetter(c)) return new TaskStatus(true, "Your dns contains invalid characters");
            }
            //check for dns length
            if (email.Length - email.IndexOf(".") < 2) return new TaskStatus(true, "Your dns is too short!");
            //check length of local part
            if (emailParts[0].Length < 2) return new TaskStatus(true, "Your username from email is too short!");
            if (emailParts[0].Length > 64) return new TaskStatus(true, "Your username from email is too long!");
            //verify if username contains a dot in front or in back
            if (emailParts[0].First().ToString() == "." || emailParts[0].Last().ToString() == ".") return new TaskStatus(true, "Your username starts or ends with a dot!"); 
            //if all is alright return nothing
            return new TaskStatus(false, string.Empty);

        } 
        public static TaskStatus VerifyPassword(string password)
        {
            if (password.Length < 10) return new TaskStatus(true, "Your password need to be minimum 9 characters long!");
            int specialChar = 0;
            int numOfNumbers = 0;
            int numOfUpper = 0;
            foreach (char c in password)
            {
                if (char.IsUpper(c)) numOfUpper++;
                else if (char.IsDigit(c)) numOfNumbers++;
                else if (!char.IsLetterOrDigit(c)) specialChar++;
            }
            if (numOfUpper == 0) return new TaskStatus(true, "Your password must contain at least one upper case letter!");
            if (numOfNumbers == 0) return new TaskStatus(true, "Your password must contain at least one digit!");
            if (specialChar == 0) return new TaskStatus(true, "Your password must contain at least one special character!");
            return new TaskStatus(false, string.Empty);
        }
        public static bool VerifyPhoneNumber(string phone)
        {
            return true;
        }
    }
}
