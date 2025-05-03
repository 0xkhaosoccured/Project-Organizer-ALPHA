using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Folders
{
  public class ProjectFolder
  {
    public required string Path { get; set; }
    public List<string> Hashtags { get; set; }
    public ProjectFolder()
    {
      Hashtags = new List<string>();
    }

    public bool AddHashtag(string tag)
    {
      const int MAX_HASHTAGS = 5;

      if(!string.IsNullOrEmpty(tag)) {
        string cleanedHashtag = tag.Trim().ToLower(); // Приводим к норм состоянию, без пробелов и прочего кала
        if (cleanedHashtag.StartsWith("#")) {
          cleanedHashtag = cleanedHashtag.Substring(1);
        }
        if (!string.IsNullOrWhiteSpace(cleanedHashtag)) {
          if (Hashtags.Count >= MAX_HASHTAGS) {
            MessageBox.Show($"Для папки '{System.IO.Path.GetFileName(this.Path)}' достигнут максимум хештегов ({MAX_HASHTAGS}).",
              "Превышен лимит хештегов", MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
          }
          else {
            if (Hashtags.Contains(cleanedHashtag)) {
              MessageBox.Show($"Хештег {cleanedHashtag} уже имеется.");
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
