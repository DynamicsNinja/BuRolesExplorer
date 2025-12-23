namespace BuRolesExplorer.Proxy
{
    public class ComboBoxItem<T>
    {
        public string Text { get; set; }
        public T Value { get; set; }

        public ComboBoxItem(string text, T value)
        {
            Text = text;
            Value = value;
        }

        public override string ToString()
        {
            return Text; // what ComboBox displays
        }
    }

}
