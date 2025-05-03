using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using fs = Filesystem.Filemanipulation;
using Path = System.IO.Path;
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

    private void Button_Click(object sender, RoutedEventArgs e)
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
        MessageBox.Show("Failed to get PATH to your folder");
      }
    }

    private void Button_Click_1(object sender, RoutedEventArgs e)
    {
      if(foldersListView.SelectedItems != null) {
        if(foldersListView.SelectedItems.Count > 0) {
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
        MessageBox.Show("Пожалуйста, выберите папки для удаления.", "Ничего не выбрано");
      }  
    }

    private void Button_Click_2(object sender, RoutedEventArgs e)
    {
      if(foldersListView.SelectedItems != null) {
        List<Pjf> items_to_pack = foldersListView.SelectedItems.Cast<Pjf>().ToList();
        foreach(Pjf item in items_to_pack) {
          filesystem.Repack(item.Path);
        }
      }
    }

    private void foldersListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
      if(foldersListView.SelectedItem != null) {
        Pjf selected_folder = foldersListView.SelectedItem as Pjf;
        if (selected_folder != null) {
          HashtagsEditor hse = new HashtagsEditor();
          hse._FolderToEdit = selected_folder;
          hse.ShowDialog();
        }
      }
    }

    private void Grid_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      Debug.WriteLine("Клик по Grid сработал!"); 
      DependencyObject clickedElement = e.OriginalSource as DependencyObject;

      bool clickedInsideListViewItem = false;
      while (clickedElement != null && clickedElement != foldersListView)
      {
        if (clickedElement is System.Windows.Controls.ListViewItem) {
          clickedInsideListViewItem = true;
          break; 
        }
        clickedElement = VisualTreeHelper.GetParent(clickedElement);
      }

      if (!clickedInsideListViewItem && foldersListView.SelectedItem != null) {
        Debug.WriteLine("Клик вне ячейки таблицы. Снятие выделения..."); 
        foldersListView.SelectedItem = null; 
      }
    }
  }
}