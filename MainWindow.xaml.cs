using System.Collections.ObjectModel; 
using System.Windows;
using System.IO;
using Pjf = Folders.ProjectFolder;
using Path = System.IO.Path;
using Microsoft.Win32;
using System.Windows.Input;

namespace Project_Organizer_ALPHA
{
  public partial class MainWindow : Window
  {
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
          Repack(item.Path);
        }
      }
    }

    private void foldersListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {

    }

    private void Repack(string path)
    {
      (string sub, string footages) = CreateSubFold(path);
      if (!string.IsNullOrEmpty(sub)) {
        if (!string.IsNullOrEmpty(footages)) {
          try 
          {
            string[] filesInPath = Directory.GetFiles(path);
            foreach (string file in filesInPath) 
            {
              if (Path.GetExtension(file) == ".aep") {
                string filename = Path.GetFileName(file);
                string _dirname = Path.Combine(sub, filename);
                File.Move(file, _dirname);
              }
              else {
                string filename = Path.GetFileName(file);
                string _dirname = Path.Combine(footages, filename);
                File.Move(file, _dirname);
              }
            }
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo() {
              FileName = sub,
              UseShellExecute = true,
              Verb = "open" 
            });
          }
          catch (Exception ex) 
          {
            // TODO
            MessageBox.Show($"Error in repack function: {ex.Message}");
          }
        }  
      }
    }

    
    private (string subfolder, string footage_folder) CreateSubFold(string path)
    {
      string path_name = Path.GetFileName(path);
      string subfolder = Path.Combine(path, path_name);
      string footage_folder = Path.Combine(subfolder, "(Footage)");
      if (Directory.Exists(path)) {
        if (!string.IsNullOrEmpty(path_name)) {
          try {
            Directory.CreateDirectory(subfolder);
            Directory.CreateDirectory(footage_folder);
          }
          catch (Exception ex) {
            MessageBox.Show($"Не удалось создать папку: {ex.Message}");
          }
        } else {
          MessageBox.Show("Paths are empty");
        }
      }
      else {
        MessageBox.Show("Directory is not exist");
      }
      return (subfolder, footage_folder);
    }
  }
}