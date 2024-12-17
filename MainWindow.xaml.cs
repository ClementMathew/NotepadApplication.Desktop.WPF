using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using System.IO;
using Notepad.ViewModels;

namespace Notepad
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainWindowViewModel _model;

        public MainWindow()
        {
            InitializeComponent();
            _model = new MainWindowViewModel();
            DataContext = _model;
        }

        private void NewCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e) => e.CanExecute = true;

        private void NewCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            _model.Content = string.Empty;
            _model.FilePath = string.Empty;
            Title = "Notepad - Untitled";
        }

        private void OpenCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e) => e.CanExecute = true;

        private void OpenCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                Filter = "Text Files (*.txt)|*.txt|All files (*.*)|*.*",
                Multiselect = true,
                Title = "Select a file",
                ValidateNames = true,
                CheckFileExists = true
            };
            if (openFileDialog.ShowDialog() == true)
            {
                _model.FilePath = openFileDialog.FileName;
                _model.Content = File.ReadAllText(_model.FilePath);
                Title = $"Notepad - {System.IO.Path.GetFileName(_model.FilePath)}";
            }
        }

        private void SaveCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = _model.Content != "" && _model.Content != null;
        }

        private void SaveCommand_Executed(object sender, ExecutedRoutedEventArgs e) => SaveFile();

        private bool SaveFile()
        {
            if (string.IsNullOrEmpty(_model.FilePath))
            {
                return SaveFileAs();
            }
            try
            {
                File.WriteAllText(_model.FilePath, _model.Content);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error Saving File: {ex.Message}");
                return false;
            }
        }

        private void SaveAsCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = _model.Content != "" && _model.Content != null;
        }

        private void SaveAsCommand_Executed(object sender, ExecutedRoutedEventArgs e) => SaveFileAs();

        private bool SaveFileAs()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog()
            {
                Filter = "Text Files (*.txt)|*.txt|All files (*.*)|*.*"
            };
            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    File.WriteAllText(saveFileDialog.FileName, _model.Content);
                    _model.FilePath = saveFileDialog.FileName;
                    Title = $"Notepad - {System.IO.Path.GetFileName(_model.FilePath)}";
                    return true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error Saving File: {ex.Message}");
                }
            }
            return false;
        }

        private void CloseCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e) => e.CanExecute = true;

        private void CloseCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (_model.Content.Length > 0)
            {
                var result = MessageBox.Show("Do you want to save changes?",
                    "Untitled",
                    MessageBoxButton.YesNoCancel,
                    MessageBoxImage.Question);

                switch (result)
                {
                    case MessageBoxResult.Yes:
                        SaveFile();
                        break;

                    case MessageBoxResult.No:
                        Close();
                        break;
                }
            }
            else
            {
                Close();
            }
        }

        private void CutCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e) => e.CanExecute = true;

        private void CutCommand_Executed(object sender, ExecutedRoutedEventArgs e) { }

        private void CopyCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e) => e.CanExecute = true;

        private void CopyCommand_Executed(object sender, ExecutedRoutedEventArgs e) { }

        private void PasteCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e) => e.CanExecute = true;

        private void PasteCommand_Executed(object sender, ExecutedRoutedEventArgs e) { }

        private void FullScreen_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Maximized;
        }
    }
}
