using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Notepad.ViewModels
{
    internal class MainWindowViewModel : ViewModelBase
    {
        private string _content;
        private string _filePath;
        private int _lineCount;
        private int _wordCount;

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

        public string FilePath
        {
            get => _filePath;
            set
            {
                _filePath = value;
                OnPropertyChanged();
            }
        }

        public int LineCount
        {
            get => _lineCount;
            private set
            {
                _lineCount = value;
                OnPropertyChanged();
            }
        }

        public int WordCount
        {
            get => _wordCount;
            set
            {
                _wordCount = value;
                OnPropertyChanged();
            }
        }

        private void UpdateCounts()
        {
            LineCount = string.IsNullOrEmpty(Content) ? 0 : Content.Split('\n').Length;
            WordCount = string.IsNullOrEmpty(Content) ? 0 : Regex.Matches(Content,@"\b\S+\b").Count;
        }
    }
}
