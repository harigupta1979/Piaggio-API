using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace BusinessLogic.Healper
{
    public class SecurityEncy
    {
        static string[] saAllowedCharacters = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0" };
        static int iOTPLength = 4;
        static string key = "202277$#@$^@ACPL";
        public static byte[] Key = UTF8Encoding.UTF8.GetBytes(key);
        public static byte[] IV = UTF8Encoding.UTF8.GetBytes(key);
        public static string Encrypt(string plainText)
        {
            byte[] encrypted;
            //byte[] IV;

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;
                //aesAlg.GenerateIV();
                //IV = aesAlg.IV;

                aesAlg.Mode = CipherMode.CBC;
                aesAlg.Padding = PaddingMode.PKCS7;
                var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption. 
                using (var msEncrypt = new MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }
            // Return the encrypted bytes from the memory stream. 
            return Convert.ToBase64String(encrypted);
        }

        public static string Decrypt(string cipherTextCombined)
        {
            // Declare the string used to hold 
            // the decrypted text. 
            string plaintext = null;

            // Create an Aes object 
            // with the specified key and IV. 
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;

                //byte[] IV = new byte[aesAlg.BlockSize / 8];
                byte[] cipherText = Convert.FromBase64String(cipherTextCombined);// UTF8Encoding.UTF8.GetBytes(cipherTextCombined);
                aesAlg.IV = IV;
                aesAlg.Padding = PaddingMode.PKCS7;
                aesAlg.Mode = CipherMode.CBC;

                // Create a decrytor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                // Create the streams used for decryption. 
                using (var msDecrypt = new MemoryStream(cipherText))
                {
                    using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (var srDecrypt = new StreamReader(csDecrypt))
                        {

                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
            return plaintext;
        }
        public static string GenerateOTP()

        {

            string sOTP = String.Empty;

            string sTempChars = String.Empty;

            Random rand = new Random();

            for (int i = 0; i < iOTPLength; i++)

            {

                int p = rand.Next(0, saAllowedCharacters.Length);

                sTempChars = saAllowedCharacters[rand.Next(0, saAllowedCharacters.Length)];

                sOTP += sTempChars;

            }

            return sOTP;

        }
        public static string PwdEncryptDecrypt(string password, string action)
        {
            byte[] pass = Encoding.ASCII.GetBytes(password);
            byte offset = 17;
            for (int i = 0; i < pass.Length; i++)
            {
                if (action.Equals("E"))
                {
                    pass[i] = (byte)(pass[i] + offset);
                }
                else if (action.Equals("D"))
                {
                    pass[i] = (byte)(pass[i] - offset);
                }
            }
            return Encoding.ASCII.GetString(pass);
        }
    }
}
