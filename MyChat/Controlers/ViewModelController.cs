using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MindLink.Recruitment.MyChat.Core.ViewModels;

namespace MindLink.Recruitment.MyChat.Controlers
{
    public class ViewModelController<TViewModel> where TViewModel : ViewModel, new()
    {
        //Get the exceptions here

        protected TViewModel ViewModel { get; private set; }

        public ViewModelController()
        {
            ViewModel = new TViewModel();
        }

        public void Start(String[] args)
        {
            ViewModel.ExportConversation(args);            
        }
    }
}
