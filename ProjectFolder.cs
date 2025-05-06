using System.Collections.ObjectModel;
using System.Windows;

/// <summary>
/// This .cs file contains class about folder
/// </summary>

namespace Folders
{
  public class ProjectFolder
  {
    public required string Path { get; set; }
    public ObservableCollection<string> Hashtags { get; set; }
    public ProjectFolder()
    {
      Hashtags = new ObservableCollection<string>();
    }

    public bool AddHashtag(string tag)
    {
      const int MAX_HASHTAGS = 5;

      if (!string.IsNullOrEmpty(tag)) {
        string cleanedHashtag = tag.Trim().ToLower();
        if (cleanedHashtag.StartsWith("#")) {
          cleanedHashtag = cleanedHashtag.Substring(1);
        }
        if (!string.IsNullOrWhiteSpace(cleanedHashtag)) {
          if (Hashtags.Count >= MAX_HASHTAGS) {
            MessageBox.Show($"Для папки '{System.IO.Path.GetFileName(this.Path)}' already max hashtags count -- ({MAX_HASHTAGS}).",
              "Too much hashtags", MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
          }
          else {
            if (Hashtags.Contains(cleanedHashtag)) {
              MessageBox.Show($"Tag '{cleanedHashtag}' already here.");
              return false;
            }
            else {
              Hashtags.Add(cleanedHashtag);
              return true;
            }
          }
        }
      }
      return false;
    }
    public bool RemoveHashtag(string tag)
    {
      if (string.IsNullOrWhiteSpace(tag)) return false;

      string cleaned_tag = tag.Trim().ToLower();

      if (cleaned_tag.StartsWith("#")) {
        cleaned_tag = cleaned_tag.Substring(1);
      }
      bool removed = Hashtags.Remove(cleaned_tag);

      return removed;
    }
  }
}
