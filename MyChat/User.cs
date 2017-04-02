using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace MindLink.Recruitment.MyChat
{
    /// <summary>
    /// A class tha represents a user involved in the conversation
    /// </summary>
    public class User
    {
        /// <summary>
        /// the user's id
        /// </summary>
        string name;

        /// <summary>
        /// the number of messages
        /// </summary>
        int numberOfmessages;

        /// <summary>
        /// a string used to obscure user's id
        /// </summary>
        string nameHash;

        /// <summary>
        /// an instance of the MD5 class to encrypt user's id
        /// </summary>
        MD5 md5Hash = MD5.Create();    

        /// <summary>
        /// Property that sets or gets the number of messages of the user
        /// </summary>
        public int NumberOfMessages
        {
            get { return numberOfmessages; }
            set { numberOfmessages = value; }
        }
        
        /// <summary>
        /// Property that sets or gets the user id
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// property that holds the user's encrypted id
        /// </summary>
        public string idHash
        {
            get { return nameHash; }
        }

        /// <summary>
        /// Property that returns the instance of the 
        /// MD5 class
        /// </summary>
        public MD5 MD5hash
        {
            get { return md5Hash; }
        }

        /// <summary>
        /// A constructor for user initialization
        /// </summary>
        public User(string name)
        {
            this.name = name;
            numberOfmessages = 0;
            if(name != null)
                nameHash = GetMd5Hash(md5Hash, name);
        }


        public string GetMd5Hash(MD5 md5Hash, string input)
        {
            try
            {

                // Convert the input string to a byte array and compute the hash.
                byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

                // Create a new Stringbuilder to collect the bytes
                // and create a string.
                StringBuilder sBuilder = new StringBuilder();

                // Loop through each byte of the hashed data 
                // and format each one as a hexadecimal string.
                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }

                // Return the hexadecimal string.
                return sBuilder.ToString();
            }
            catch(ArgumentNullException)
            {
                throw new ArgumentNullException("There was an error encrypting user id's");    
            }
        }

        // Verify a hash against a string.
        public  bool VerifyMd5Hash(MD5 md5Hash, string input, string hash)
        {
            // Hash the input.
            string hashOfInput = GetMd5Hash(md5Hash, input);

            // Create a StringComparer an compare the hashes.
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            if (0 == comparer.Compare(hashOfInput, hash))
            {
                return true;
            }
            else
            {
                return false;
            }
        }



    }
}
