using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BOCOM.IVX.ViewModel
{
    public class ImportVMBase : ViewModelBase
    {
        public CreateTaskViewModel CreateTaskVM { get; set; }

        public virtual void Commit() { }

        public virtual bool Validate() { return false; }
    }
}
