using System.Text.RegularExpressions;

namespace Notepad.ViewModels
{
    internal class MainWindowViewModel : ViewModelBase
    {
        private string _content;
        private string _filePath;
        private int _lineCount;
        private int _wordCount;

        /// <summary>
        /// Notepad Content Property
        /// </summary>
        public string Content
        {
            get => _content;
            set
            {
                _content = value;
                OnPropertyChanged();
                UpdateCounts();
            }
        }

        /// <summary>
        /// Notepad Filepath Property
        /// </summary>
        public string FilePath
        {
            get => _filePath;
            set
            {
                _filePath = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Notepad LineCount Property
        /// </summary>
        public int LineCount
        {
            get => _lineCount;
            private set
            {
                _lineCount = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Notepad WordCount Property
        /// </summary>
        public int WordCount
        {
            get => _wordCount;
            set
            {
                _wordCount = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// UpdateCounts Function
        /// ---------------------
        /// 1. LineCount updated by Split method.
        /// 2. WordCount updated by Regex method.
        /// </summary>
        private void UpdateCounts()
        {
            LineCount = string.IsNullOrEmpty(Content) ? 0 : Content.Split('\n').Length;
            WordCount = string.IsNullOrEmpty(Content) ? 0 : Regex.Matches(Content, @"\b\S+\b").Count;
        }
    }
}
