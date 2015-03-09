using System;
using System.Collections.Generic;
using System.Windows.Forms;
using DevExpress.Skins;
using DevExpress.XtraEditors.Registrator;
using DevExpress.XtraEditors.Repository;
using System.Reflection;
using System.Drawing;
using DevExpress.XtraEditors.ViewInfo;

using DevExpress.XtraEditors.Drawing;

namespace BOCOM.IVX.Controls {
    [UserRepositoryItem("RegisterCustomEdit")]
    public class RepositoryItemCustomRadioGroup : RepositoryItemRadioGroup
    {

        static RepositoryItemCustomRadioGroup() { RegisterCustomEdit(); }

        public RepositoryItemCustomRadioGroup() { }

        public const string CustomEditName = "CustomRadioGroup";

        public override string EditorTypeName { get { return CustomEditName; } }

        public static void RegisterCustomEdit()
        {
            Image img = null;
            try
            {
                img = (Bitmap)Bitmap.FromStream(Assembly.GetExecutingAssembly().
                  GetManifestResourceStream("DevExpress.CustomEditors.CustomEdit.bmp"));
            }
            catch
            {
            }
            EditorRegistrationInfo.Default.Editors.Add(new EditorClassInfo(CustomEditName,
              typeof(CustomRadioGroup), typeof(RepositoryItemCustomRadioGroup),
              typeof(MyRadioGroupViewInfo), new RadioGroupPainter(), true, img));
        }

    }

}