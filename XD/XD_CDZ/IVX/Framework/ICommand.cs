using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BOCOM.IVX.Framework
{
    public interface ICommand
    {
        void Execute(object sender);
    }
}
