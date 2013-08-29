using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Configuration;

namespace Deployment.Helper
{
    class IOUtil
    {
        public static List<string> IgnoreFilePatternList;

        static IOUtil()
        {
            IgnoreFilePatternList = ConfigurationManager.AppSettings["IgnorePattern"]
                 .Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries)
                 .ToList();
        }

        public static bool IsIgnore(string filePath)
        {
            foreach (var ignorePattern in IgnoreFilePatternList)
            {
                if (Regex.IsMatch(filePath, ignorePattern, RegexOptions.IgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }

        public static string GetDomainName(string uncPath)
        {
            var regex = new Regex(@"\\\\([^\\/]+)\\", RegexOptions.IgnoreCase);
            var match = regex.Match(uncPath);

            return match.Groups.Count > 1 ? match.Groups[1].Value : string.Empty;
        }

        public static void DeleteFile(string filePath)
        {
            if (IsIgnore(filePath))
            {
                return;
            }

            // delete/clear hidden attribute
            File.SetAttributes(filePath, File.GetAttributes(filePath) & ~FileAttributes.Hidden);

            // delete/clear archive and read only attributes
            File.SetAttributes(filePath, File.GetAttributes(filePath)
                & ~(FileAttributes.Archive | FileAttributes.ReadOnly));

            File.Delete(filePath);
        }



        public static void DeleteDirectory(string path, bool deleteChildOnly)
        {
            if (!Directory.Exists(path)) return;

            string[] files = Directory.GetFiles(path);

            foreach (string file in files)
            {
                DeleteFile(file);
            }

            string[] dirs = Directory.GetDirectories(path);

            foreach (string dir in dirs)
            {
                DeleteDirectory(dir, false);
            }

            if (!deleteChildOnly && Directory.GetFiles(path).Length == 0
                 && Directory.GetDirectories(path).Length == 0)
            {
                Directory.Delete(path);
            }
        }

        public static void CopyDirectory(string source, string destination)
        {
            source = source.EndsWith(@"\") ? source : source + @"\";
            destination = destination.EndsWith(@"\") ? destination : destination + @"\";

            if (Directory.Exists(source))
            {
                if (Directory.Exists(destination) == false)
                    Directory.CreateDirectory(destination);

                foreach (var fileName in Directory.GetFiles(source))
                {
                    if (IsIgnore(fileName))
                    {
                        continue;
                    }

                    var fileInfo = new FileInfo(fileName);
                    fileInfo.CopyTo(destination + fileInfo.Name, true);
                }

                foreach (string directory in Directory.GetDirectories(source))
                {
                    var dirInfo = new DirectoryInfo(directory);
                    CopyDirectory(dirInfo.FullName, destination + dirInfo.Name);
                }
            }
        }

    }
}
