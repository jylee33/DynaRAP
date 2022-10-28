using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DynaRAP.UTIL
{
    public static class Prompt
    {
        public static string ShowDialog(string text, string caption, bool bOnlyDigit = false)
        {
            XtraForm prompt = new XtraForm()
            {
                Width = 500,
                Height = 150,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = caption,
                StartPosition = FormStartPosition.CenterScreen,
            };
            LabelControl textLabel = new LabelControl() { Left = 50, Top = 20, Width = 400, Text = text };
            TextEdit textEdit = new TextEdit() { Left = 50, Top = 50, Width = 400 };
            if(bOnlyDigit)
            {
                prompt.Text = "숫자만 입력 가능합니다.";
                textEdit.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
                textEdit.Properties.Mask.EditMask = "d";
            }
            SimpleButton confirmation = new SimpleButton() { Text = "OK", Left = 241, Width = 100, Top = 75, DialogResult = DialogResult.OK };
            SimpleButton cancel = new SimpleButton() { Text = "Cancel", Left = 351, Width = 100, Top = 75, DialogResult = DialogResult.Cancel };
            confirmation.Click += (sender, e) => { prompt.Close(); };
            prompt.Controls.Add(textEdit);
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(cancel);
            prompt.Controls.Add(textLabel);
            prompt.AcceptButton = confirmation;
            prompt.CancelButton = cancel;

            return prompt.ShowDialog() == DialogResult.OK ? textEdit.Text : string.Empty;
        }
        public static string ShowDialogCancel(string text, string caption,string insideValue = "" , bool bOnlyDigit = false)
        {
            XtraForm prompt = new XtraForm()
            {
                Width = 500,
                Height = 150,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = caption,
                StartPosition = FormStartPosition.CenterScreen,
            };
            LabelControl textLabel = new LabelControl() { Left = 50, Top = 20, Width = 400, Text = text };
            TextEdit textEdit = new TextEdit() { Left = 50, Top = 50, Width = 400 };
            textEdit.Text = insideValue;
            if (bOnlyDigit)
            {
                prompt.Text = "숫자만 입력 가능합니다.";
                textEdit.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
                textEdit.Properties.Mask.EditMask = "d";
            }
            SimpleButton confirmation = new SimpleButton() { Text = "OK", Left = 241, Width = 100, Top = 75, DialogResult = DialogResult.OK };
            SimpleButton cancel = new SimpleButton() { Text = "Cancel", Left = 351, Width = 100, Top = 75, DialogResult = DialogResult.Cancel };
            confirmation.Click += (sender, e) => { prompt.Close(); };
            prompt.Controls.Add(textEdit);
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(cancel);
            prompt.Controls.Add(textLabel);
            prompt.AcceptButton = confirmation;
            prompt.CancelButton = cancel;

            return prompt.ShowDialog() == DialogResult.OK ? textEdit.Text : "Cancel123";
        }
    }
}
