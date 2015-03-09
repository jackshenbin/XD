using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BOCOM.IVX.Interfaces;
using System.Windows.Forms;
using System.ComponentModel.Composition;

namespace BOCOM.IVX.Service
{
    [Export(typeof(IInteractionService))]
    public class InteractionService : IInteractionService
    {
        public System.Windows.Forms.DialogResult ShowMessageBox(IWin32Window owner, string text, string caption, MessageBoxButtons buttons = MessageBoxButtons.OK , MessageBoxIcon icon = MessageBoxIcon.Asterisk)
        {
            return MessageBox.Show(owner, text, caption, buttons, icon);
        }

        public System.Windows.Forms.DialogResult ShowMessageBox(string text, string caption, System.Windows.Forms.MessageBoxButtons buttons = MessageBoxButtons.OK, MessageBoxIcon icon = MessageBoxIcon.Asterisk)
        {
            return MessageBox.Show(text, caption, buttons, icon);
        }

    }
}
