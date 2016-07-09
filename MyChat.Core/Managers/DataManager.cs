using MyChat.Core.Abstract;
using MyChat.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyChat.Core.Managers
{
    public class DataManager
    {
        private static DataManager instance;
        private IIOHelperBase IOManager;

        public static DataManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new DataManager();

                return instance;
            }
        }

        private DataManager()
        {
            IOManager = IOCManager.Resolve<IIOHelperBase>();
        }

        private void readFile()
        {


        }

        public void writeFile()
        {

        }
    }
}
