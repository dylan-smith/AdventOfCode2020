using System.Windows.Controls;

namespace AdventOfCode
{
    public class ScrollingTextBox : TextBox
    {
        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            base.OnTextChanged(e);
            CaretIndex = Text.Length;
            ScrollToEnd();
        }

    }
}
