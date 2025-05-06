using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using fs = Filesystem.Filemanipulation;
using Pjf = Folders.ProjectFolder;

namespace Project_Organizer_ALPHA
{
  public partial class MainWindow : Window
  {
    fs filesystem = new fs();
    public ObservableCollection<Pjf> project_folders { get; set; }
    public MainWindow()
    {
      InitializeComponent();
      project_folders = new ObservableCollection<Pjf>();
      this.DataContext = this;
    }

    private void AllocateFolder(object sender, RoutedEventArgs e)
    {
      OpenFolderDialog openFolderDialog = new OpenFolderDialog();
      openFolderDialog.Title = "Choose folder";
      bool? result = openFolderDialog.ShowDialog();
      if (result == true) {
        Pjf new_folder = new Pjf { Path = openFolderDialog.FolderName };
        project_folders.Add(new_folder);
        PathInfo.Text = "";
      }
      else {
        MessageBox.Show("Failed to get path to your folder!");
      }
    }

    private void RemoveFunction(object sender, RoutedEventArgs e)
    {
      if (foldersListView.SelectedItems != null) {
        if (foldersListView.SelectedItems.Count > 0) {
          List<Pjf> items_to_remove = foldersListView.SelectedItems.Cast<Pjf>().ToList();

          MessageBoxButton btn = MessageBoxButton.YesNo;
          MessageBoxImage icon = MessageBoxImage.Question;
          MessageBoxResult result = MessageBox.Show("Delete Folder", "Do you want to delete this folders?", btn, icon);
          if (result == MessageBoxResult.Yes) {
            foreach (Pjf item in items_to_remove) {
              project_folders.Remove(item);
            }
          }
          else {
            return;
          }

        }
      }
      else {
        MessageBox.Show("Please, choose folder", "Nothing is selected.");
      }
    }

    private void Finalize(object sender, RoutedEventArgs e)
    {
      if (foldersListView.SelectedItems != null) {
        List<Pjf> items_to_pack = foldersListView.SelectedItems.Cast<Pjf>().ToList();
        foreach (Pjf item in items_to_pack) {
          filesystem.Bundle(item.Path);
        }
      }
    }

    private void HashtagsEditing(object sender, MouseButtonEventArgs e)
    {
      if (foldersListView.SelectedItem != null) {
        Pjf? selected_folder = foldersListView.SelectedItem as Pjf;
        if (selected_folder != null) {
          HashtagsEditor hse = new HashtagsEditor();
          hse._FolderToEdit = selected_folder;
          hse.ShowDialog();
        }
      }
    }

    ///<summary>
    /// Смотрим, куда был сделан клик, после чего от меньшего к большему идём и проверяем 
    /// Является ли элемент строчкой таблицы, если не является, то 
    /// Обнуляем наведение 
    ///</summary>

    private void ResetOutline(object sender, MouseButtonEventArgs e)
    {
      DependencyObject? clickedElement = e.OriginalSource as DependencyObject;

      bool clickedInsideListViewItem = false;
      while (clickedElement != null && clickedElement != foldersListView) {
        if (clickedElement is System.Windows.Controls.ListViewItem) {
          clickedInsideListViewItem = true;
          break;
        }
        clickedElement = VisualTreeHelper.GetParent(clickedElement);
      }

      if (!clickedInsideListViewItem && foldersListView.SelectedItem != null) {
        foldersListView.SelectedItem = null;
      }
    }
  }
}