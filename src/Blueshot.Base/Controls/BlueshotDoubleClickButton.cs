using System.Windows.Forms;

namespace blueshot.Base.Controls
{
    public class blueshotDoubleClickButton : Button
    {
        public blueshotDoubleClickButton()
        {
            SetStyle(ControlStyles.StandardClick | ControlStyles.StandardDoubleClick, true);
        }
    }
}
