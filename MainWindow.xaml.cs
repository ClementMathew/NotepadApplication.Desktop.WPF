using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;
using Notepad.ViewModels;

namespace Notepad
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainWindowViewModel _model;

        /// <summary>
        /// Constructor of Main Window
        /// --------------------------
        /// 1. View Model initialized.
        /// 2. Data context set to view model. 
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            _model = new MainWindowViewModel();
            DataContext = _model;
        }

        /// <summary>
        /// New Command's Can Execute Function
        /// ----------------------------------
        /// 1. return true always.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e) => e.CanExecute = true;

        /// <summary>
        /// New Command's Execute Function 
        /// ------------------------------
        /// 1. Content and File path in view model is made empty.
        /// 2. Making title of notepad "Untitled".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            _model.Content = string.Empty;
            _model.FilePath = string.Empty;
            Title = "Notepad - Untitled";
        }

        /// <summary>
        /// Open Command's Can Execute Function
        /// -----------------------------------
        /// 1. returns true always.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e) => e.CanExecute = true;

        /// <summary>
        /// Open Command's Execute Function
        /// -------------------------------
        /// 1. Using OpenFileDialog to select the files from local disk.
        /// 2. Setting the filepath, content, and title of the opened file.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Save Command's Can Execute Function
        /// -----------------------------------
        /// 1. returns true if content is not empty or null.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = _model.Content != "" && _model.Content != null;
        }

        /// <summary>
        /// Save Command's Execute Function
        /// -------------------------------
        /// 1. Calling SaveFile Function.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveCommand_Executed(object sender, ExecutedRoutedEventArgs e) => SaveFile();

        /// <summary>
        /// SaveFile Function
        /// -----------------
        /// Saving notepad data to file in the local disk.
        /// </summary>
        /// <returns>
        ///     1. SaveFileAs Function if filepath is empty or null.
        ///     2. Write the existing file if filepath exists.
        /// </returns>
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

        /// <summary>
        /// SaveAs Command's Can Execute Function
        /// -------------------------------------
        /// 1. returns true if content is empty or null.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveAsCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = _model.Content != "" && _model.Content != null;
        }

        /// <summary>
        /// SaveAs Command's Execute Function
        /// ---------------------------------
        /// 1. Calling SaveFileAs Function.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveAsCommand_Executed(object sender, ExecutedRoutedEventArgs e) => SaveFileAs();

        /// <summary>
        /// SaveFileAs Function
        /// -------------------
        /// 1. Using SaveFileDialog to open file saving location.
        /// 2. Writes content to the filepath.
        /// </summary>
        /// <returns>
        ///     1. returns true if successfully written content.
        ///     2. returns false if exception occured.
        /// </returns>
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

        /// <summary>
        /// Close Command's Can Execute Function
        /// ------------------------------------
        /// 1. return true always.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e) => e.CanExecute = true;

        /// <summary>
        /// Close Command's Execute Function
        /// --------------------------------
        /// 1. Shows save alert using message box if content is not empty.
        /// 2. If not empty then SaveFile Function is called.
        /// 3. If empty then window closes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (_model.Content.Length > 0)
            {
                MessageBoxResult result = MessageBox.Show("Do you want to save changes?",
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

        /// <summary>
        /// Cut Command's Can Execute Function
        /// ----------------------------------
        /// 1. always return true.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CutCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e) => e.CanExecute = true;

        /// <summary>
        /// Cut Command's Execute Function
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CutCommand_Executed(object sender, ExecutedRoutedEventArgs e) { }

        /// <summary>
        /// Copy Command's Can Execute Function
        /// -----------------------------------
        /// 1. always return true.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CopyCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e) => e.CanExecute = true;

        /// <summary>
        /// Copy Command's Execute Function
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CopyCommand_Executed(object sender, ExecutedRoutedEventArgs e) { }

        /// <summary>
        /// Paste Command's Can Execute Function
        /// ------------------------------------
        /// 1. always return true.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PasteCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e) => e.CanExecute = true;

        /// <summary>
        /// Paste Command's Execute Function
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PasteCommand_Executed(object sender, ExecutedRoutedEventArgs e) { }

        /// <summary>
        /// Full Screen Click Event
        /// -----------------------
        /// 1. Made window state maximized.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FullScreen_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Maximized;
        }
    }
}
