using Pastel;

namespace MenuMan
{
    /// <summary>
    /// Represents an ANSI color code wrapped string with all string data available (original text, color, colored text, etcetera).
    /// </summary>
    internal class DelayedPastel
    {
        private bool hasColor = true;

        private bool isForeground = true;
        public bool IsForeground
        {
            get
            {
                return isForeground;
            }
            set
            {
                isForeground = value;
                Recalculate();
            }
        }
        private string color;
        public string Color
        {
            get
            {
                return color;
            }
            set
            {
                color = value;
                hasColor = true;
                Recalculate();
            }
        }
        public string rawText;
        public string RawText
        {
            get
            {
                return rawText;
            }
            set
            {
                rawText = value;
                Recalculate();
            }
        }
        public int Length => RawText.Length;
        public string ColoredText { get; private set; }

        private void Recalculate()
        {
            if (hasColor)
            {
                if (isForeground) ColoredText = rawText.Pastel(color);
                else ColoredText = rawText.PastelBg(color);
            }
        }

        public DelayedPastel(string input, string color, bool foreground = true)
        {
            rawText = input;
            this.color = color;
            isForeground = foreground;

            Recalculate();
        }

        public DelayedPastel(string input, bool foreground = true)
        {
            rawText = input;
            hasColor = false;
            isForeground = foreground;
        }

        public override string ToString() => ColoredText;
    }
}
