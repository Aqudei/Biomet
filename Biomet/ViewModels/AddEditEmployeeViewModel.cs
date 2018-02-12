using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biomet.ViewModels
{
    class AddEditEmployeeViewModel : Screen
    {
        private string _photo;

        public string Photo
        {
            get { return _photo; }
            set { Set(ref _photo, value); }
        }

        public AddEditEmployeeViewModel()
        {

        }

        public void Done()
        {
            TryClose();
        }

        public void Save()
        {
            TryClose();
        }
    }
}
