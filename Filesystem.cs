using System.IO;
using System.Windows.Forms;

/// <summary>
/// This .cs file contains all useful methods for working with files
/// </summary>

namespace Filesystem
{
  class Filemanipulation
  {
    public void Bundle(string path)
    {
      (string sub, string footages) = CreateSubFold(path);
      if (!string.IsNullOrEmpty(sub)) {
        if (!string.IsNullOrEmpty(footages)) {
          try {
            string[] filesInPath = Directory.GetFiles(path);
            foreach (string file in filesInPath) {
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
          catch (Exception ex) {
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
            MessageBox.Show($"Error while creating a folder: {ex.Message}");
          }
        }
        else {
          MessageBox.Show("Paths are empty.");
        }
      }
      else {
        MessageBox.Show("Directory is not exist.");
      }
      return (subfolder, footage_folder);
    }
  }
}
