namespace MindLink.Recruitment.MyChat
{
    using System;
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;

    /// <summary>
    /// Represents a security class to assist with encryption and decryption of strings using AES algorithm.
    /// </summary>
    public static class Encryption
    {
        private static byte[] salt = Encoding.UTF8.GetBytes("thisisusedforsalting");

        // needs to be 16 characters.
        private static byte[] initVector = Encoding.UTF8.GetBytes("theinitialvector");

        private const string password = "Athos";

        /// <summary>
        /// Encrypts a string with AES algorithm.
        /// </summary>
        /// <param name="plaintext">string to be encrypted.</param>
        /// <param name="password">Password to encrypt with.</param>   
        /// <returns>An encrypted string.</returns>        
        public static string Encrypt(string plaintext)
        {
            return Convert.ToBase64String(EncryptToBytes(plaintext));
        }

        /// <summary>
        /// Encrypts a string with AES.
        /// </summary>
        /// <param name="plaintext">Text to be encrypted.</param>
        /// <param name="password">Password to encrypt with.</param>   
        /// <returns>An encrypted string.</returns>        
        public static byte[] EncryptToBytes(string plaintext)
        {
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plaintext);
            return EncryptToBytes(plainTextBytes);
        }

        /// <summary>
        /// Encrypts a string with AES.
        /// </summary>
        /// <param name="value">Bytes to be encrypted.</param>
        /// <param name="password">Password to encrypt with.</param>   
        /// <returns>An encrypted string.</returns>        
        public static byte[] EncryptToBytes(byte[] value)
        {
            int keySize = 256;

            byte[] initialVectorBytes = initVector;
            byte[] saltValueBytes = salt;
            byte[] keyBytes = new Rfc2898DeriveBytes(password, saltValueBytes).GetBytes(keySize / 8);

            using (RijndaelManaged symmetricKey = new RijndaelManaged())
            {
                symmetricKey.Mode = CipherMode.CBC;

                using (ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes, initialVectorBytes))
                {
                    MemoryStream memStream = new MemoryStream();

                    using (CryptoStream cryptoStream = new CryptoStream(memStream, encryptor, CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(value, 0, value.Length);
                        cryptoStream.FlushFinalBlock();
                    
                        return memStream.ToArray();
                    }
                }
            }
        }

        /// <summary>  
        /// Decrypts an AES-encrypted string.
        /// </summary>  
        /// <param name="cipherText">Text to be decrypted.</param> 
        /// <param name="password">Password to decrypt with.</param> 
        /// <returns>A decrypted string.</returns>
        public static string Decrypt(string cipherText)
        {
            byte[] cipherTextBytes = Convert.FromBase64String(cipherText.Replace(' ', '+'));
            return Decrypt(cipherTextBytes).TrimEnd('\0');
        }

        /// <summary>  
        /// Decrypts an AES-encrypted string. 
        /// </summary>  
        /// <param name="cipherText">Text to be decrypted.</param> 
        /// <param name="password">Password to decrypt with.</param> 
        /// <returns>A decrypted string.</returns>
        public static string Decrypt(byte[] value)
        {
            int keySize = 256;

            byte[] initialVectorBytes = initVector;
            byte[] saltValueBytes = salt;
            byte[] keyBytes = new Rfc2898DeriveBytes(password, saltValueBytes).GetBytes(keySize / 8);
            byte[] plainTextBytes = new byte[value.Length];

            using (RijndaelManaged symmetricKey = new RijndaelManaged())
            {
                symmetricKey.Mode = CipherMode.CBC;

                using (ICryptoTransform decryptor = symmetricKey.CreateDecryptor(keyBytes, initialVectorBytes))
                {
                    MemoryStream memStream = new MemoryStream(value);

                    using (CryptoStream cryptoStream = new CryptoStream(memStream, decryptor, CryptoStreamMode.Read))
                    {
                        int byteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);

                        return Encoding.UTF8.GetString(plainTextBytes, 0, byteCount);
                    }
                }
            }
        }
    }
}
