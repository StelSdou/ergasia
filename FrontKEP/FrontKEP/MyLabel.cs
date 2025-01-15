using System.Drawing;
using System.Security.RightsManagement;
using System.Windows.Forms;
using TextBox = System.Windows.Forms.TextBox;

namespace FrontKEP
{
    internal class MyLabel : TextBox
    {
        private string restoreText;
        public MyLabel(string text)
        {
            ShortcutsEnabled = false;
            Text = text;
            restoreText = Text;
            BorderStyle = BorderStyle.None;
            ReadOnly = true;
            Multiline = false;
            //Height = 10;
            AutoSize = true;
            Dock = DockStyle.Fill;
            Font = new Font("Microsoft Sans Serif", 10.3F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(161)));
            ForeColor = Color.Black;
            Location = new Point(664, 0);
            Margin = new Padding(0);
            TextAlign = HorizontalAlignment.Center;
        }
        public void Edit(bool enable)
        {
            ReadOnly = !enable;
        }
        public void restore()
        {
            Text = restoreText;
        }
        public void setChange()
        {
            restoreText = Text;
        }
        public string getText()
        {
            return Text;
        }
    }
}
