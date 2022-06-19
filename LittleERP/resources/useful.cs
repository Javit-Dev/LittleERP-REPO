using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace LittleERP.resources
{
    public class useful
    {
        public static string GetMD5(string str)
        {
            MD5 md5 = MD5CryptoServiceProvider.Create();
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] stream = null;
            StringBuilder sb = new StringBuilder();
            stream = md5.ComputeHash(encoding.GetBytes(str));
            for (int i = 0; i < stream.Length; i++) sb.AppendFormat("{0:x2}", stream[i]);
            return sb.ToString();
        }

        public static string GetSHA256(string str)
        {
            SHA256 sha256 = SHA256Managed.Create();
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] stream = null;
            StringBuilder sb = new StringBuilder();
            stream = sha256.ComputeHash(encoding.GetBytes(str));
            for (int i = 0; i < stream.Length; i++) sb.AppendFormat("{0:x2}", stream[i]);
            return sb.ToString();
        }

        internal static string GetSHA256(object password)
        {
            throw new NotImplementedException();
        }

        /**
        * Valida la contraseña, es valida si:
        * Tiene al menos un numero
        * Tiene al menos una letra minuscula
        * Tiene al menos una letra mayuscula
        * Tiene al menos un simbolo
        * La longitud minima es de 8 caracteres
        */
        public static Boolean validatePassword(String passsword)
        {
            Boolean isValid = false;

            Regex hasNumber = new Regex(@"[0-9]+");
            Regex hasUpperChar = new Regex(@"[A-Z]+");
            Regex hasLowerChar = new Regex(@"[a-z]+");
            Regex hasSymbols = new Regex(@"[!@#$%^&*()_+=\[{\]};:<>|./?,-]");
            //despues de la coma se puede definir tambien un maximo
            Regex hasMinimum8Chars = new Regex(@".{8,}");

            if (hasNumber.IsMatch(passsword) && hasUpperChar.IsMatch(passsword)
                && hasMinimum8Chars.IsMatch(passsword) && hasSymbols.IsMatch(passsword))
                isValid = true;



            return isValid;
        }


        public enum operations {
            add,mod,chpass
        }
        public static DateTime IntegerToFecha(int fechanum)
        {
            DateTime fecha;

            int dia;
            int mes;
            int año;

            año = fechanum / 10000;
            fechanum = fechanum % 10000;
            mes = fechanum / 100;
            dia = fechanum % 100;

            fecha = new DateTime(año, mes, dia);

            return fecha;
        }
        public static int FechaToInteger(DateTime fecha)
        {
            int dia = fecha.Day;
            int mes = fecha.Month;
            int year = fecha.Year;

            int fechanum;

            fechanum = year * 100;
            fechanum = fechanum + mes;
            fechanum = fechanum * 100;
            fechanum = fechanum + dia;

            return fechanum;
        }


    }
}
