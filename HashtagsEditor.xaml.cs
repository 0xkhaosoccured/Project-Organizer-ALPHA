using System;
using System.Windows;
using Pjf = Folders.ProjectFolder;

namespace Project_Organizer_ALPHA
{
  /// <summary>
  /// Interaction logic for HashtagsEditor.xaml
  /// </summary>
  public partial class HashtagsEditor : Window
  {
    public Pjf _FolderToEdit { get; set; }

    public HashtagsEditor()
    {
      InitializeComponent();
      this.Loaded += HashtagsEditor_Loaded;
    }

    private void HashtagsEditor_Loaded(object sender, RoutedEventArgs e)
    {
      this.DataContext = this._FolderToEdit;

      if (this._FolderToEdit != null) {
        this.Title = $"Хештеги для: {System.IO.Path.GetFileName(this._FolderToEdit.Path)}";
      }
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
      if (this._FolderToEdit != null) 
      {
        if (!string.IsNullOrWhiteSpace(hashtag_box.Text) || hashtag_box.Text.Length < 8) {
          bool success = _FolderToEdit.AddHashtag(hashtag_box.Text);
          if (success) {
            this.Close();
          }
        }
      }
      else 
      {
        MessageBox.Show("Ошибка: папка не была передана.", "Internal error", MessageBoxButton.OK, MessageBoxImage.Error);
      }
    }
  }
}
