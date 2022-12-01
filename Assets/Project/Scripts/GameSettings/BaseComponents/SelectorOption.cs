namespace GameSettings
{
    public class SelectorOption
    {
        private readonly string _text;

        public string TextValue => _text;

        public SelectorOption(string text)
        {
            _text = text;
        }
    }
}

