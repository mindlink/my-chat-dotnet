using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindLink.Recruitment.MyChat.Elements
{
   public class Sender
    {
        private String ID;
        private String HidenID;

        /// <summary>
        /// Constractor
        /// </summary>
        /// <param name="id"></param>
        public Sender(String id) {

            //if id is empty we throw an exception
            if (String.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentNullException("id", String.Format("Exception in {0}, Error message : {1}",
                        this.GetType().Name + "." +System.Reflection.MethodBase.GetCurrentMethod().Name, "Sender id can not be empty when creating new sender"));
            }
            this.ID = id;
            HidenID = GenerateId();

        }

        /// <summary>
        /// generate a unique id which will be used to hide the user id
        /// </summary>
        /// <returns></returns>
        private string GenerateId()
        {
            long i = 1;
            foreach (byte b in Guid.NewGuid().ToByteArray())
            {
                i *= ((int)b + 1);
            }
            return string.Format("{0:x}", i - DateTime.Now.Ticks);
        }


        /// <summary>
        /// Return the senderID
        /// </summary>
        public String senderID
        {
            get
            {
                return ID;
            }
        }

        /// <summary>
        /// Return the senderHidenID
        /// </summary>
        public String senderHidenID
        {
            get
            {
                return HidenID;
            }
        }
    }
}
