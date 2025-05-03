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
using System.Windows.Shapes;
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
