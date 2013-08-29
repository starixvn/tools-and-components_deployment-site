using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using ICSharpCode.SharpZipLib.Zip;
using System.IO;
using System.Text.RegularExpressions;


namespace Deployment.Helper
{
    class Zip
    {
        public static List<string> IgnoreFilePatternList;

        static Zip()
        {
            IgnoreFilePatternList = System.Configuration.ConfigurationManager.AppSettings["IgnorePattern"]
                 .Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries)
                 .ToList();

        }

        public static bool IsIgnore(string filePath)
        {
            if (filePath.EndsWith("Thumbs.db", StringComparison.CurrentCultureIgnoreCase))
            {
                return true;
            }

            foreach (var ignorePattern in IgnoreFilePatternList)
            {
                if (Regex.IsMatch(filePath, ignorePattern, RegexOptions.IgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }

        public static void ZipFiles(string inputFolderPath, string outputPathAndFile)
        {
            if (File.Exists(outputPathAndFile)) IOUtil.DeleteFile(outputPathAndFile);

            ArrayList ar = GenerateFileList(inputFolderPath); // generate file list
            int TrimLength = (Directory.GetParent(inputFolderPath)).ToString().Length;
            // find number of chars to remove     // from orginal file path
            TrimLength += 1; //remove '\'
            byte[] obuffer;

            ZipOutputStream oZipStream = new ZipOutputStream(File.Create(outputPathAndFile)); // create zip stream
            oZipStream.SetLevel(9); // maximum compression
            ZipEntry oZipEntry;
            foreach (string Fil in ar) // for each file, generate a zipentry
            {
                if (IsIgnore(Fil))
                {
                    continue;
                }
                oZipEntry = new ZipEntry(Fil.Remove(0, TrimLength));
                oZipStream.PutNextEntry(oZipEntry);

                if (!Fil.EndsWith(@"/")) // if a file ends with '/' its a directory
                {
                    using (FileStream ostream = File.OpenRead(Fil))
                    {
                        obuffer = new byte[ostream.Length];
                        ostream.Read(obuffer, 0, obuffer.Length);
                        oZipStream.Write(obuffer, 0, obuffer.Length);
                    }
                }
            }
            oZipStream.Finish();
            oZipStream.Close();
        }

        public static void ZipFiles(string inputFolderPath, string[] fileList, string outputPathAndFile)
        {
            if (File.Exists(outputPathAndFile)) IOUtil.DeleteFile(outputPathAndFile);

      
            int TrimLength = (Directory.GetParent(inputFolderPath)).ToString().Length;
            // find number of chars to remove     // from orginal file path
            TrimLength += 1; //remove '\'
            byte[] obuffer;

            ZipOutputStream oZipStream = new ZipOutputStream(File.Create(outputPathAndFile)); // create zip stream
            oZipStream.SetLevel(9); // maximum compression
            ZipEntry oZipEntry;
            foreach (string Fil in fileList) // for each file, generate a zipentry
            {
                if (IsIgnore(Fil))
                {
                    continue;
                }
                oZipEntry = new ZipEntry(Fil.Remove(0, TrimLength));
                oZipStream.PutNextEntry(oZipEntry);

                if (!Fil.EndsWith(@"/")) // if a file ends with '/' its a directory
                {
                    using (FileStream ostream = File.OpenRead(Fil))
                    {
                        obuffer = new byte[ostream.Length];
                        ostream.Read(obuffer, 0, obuffer.Length);
                        oZipStream.Write(obuffer, 0, obuffer.Length);
                    }
                }
            }
            oZipStream.Finish();
            oZipStream.Close();
        }

        private static ArrayList GenerateFileList(string Dir)
        {
            ArrayList fils = new ArrayList();
            bool Empty = true;
            foreach (string file in Directory.GetFiles(Dir)) // add each file in directory
            {
                fils.Add(file);
                Empty = false;
            }

            if (Empty)
            {
                if (Directory.GetDirectories(Dir).Length == 0)
                // if directory is completely empty, add it
                {
                    fils.Add(Dir + @"/");
                }
            }

            foreach (string dirs in Directory.GetDirectories(Dir)) // recursive
            {
                foreach (object obj in GenerateFileList(dirs))
                {
                    fils.Add(obj);
                }
            }
            return fils; // return file list
        }

        public static void UnZipFiles(string zipPathAndFile, string outputFolder, bool unzipChildFolderOnly)
        {
            using (var s = new ZipInputStream(File.OpenRead(zipPathAndFile)))
            {
                if (!Directory.Exists(outputFolder)) Directory.CreateDirectory(outputFolder);

                ICSharpCode.SharpZipLib.Zip.ZipEntry theEntry;
                string tmpEntry = String.Empty;
                while ((theEntry = s.GetNextEntry()) != null)
                {
                    string fileName = Path.GetFileName(theEntry.Name);

                    if (fileName != String.Empty && !fileName.EndsWith("Thumbs.db", StringComparison.CurrentCultureIgnoreCase))
                    {
                        string fullPath = outputFolder + "\\";
                        var entryName = theEntry.Name;
                        entryName = entryName.Replace("/", "\\");

                        entryName = unzipChildFolderOnly
                            ? entryName.Substring(entryName.IndexOf("\\") + 1)
                            : entryName;
                        fullPath += entryName;
                        fullPath = fullPath.Replace("\\ ", "\\");
                        fullPath = fullPath.Replace("/", "\\");
                        string fullDirPath = Path.GetDirectoryName(fullPath);
                        if (!Directory.Exists(fullDirPath)) Directory.CreateDirectory(fullDirPath);

                        UnzipZipEntry(s, fullPath);
                    }
                }

            }
        }

        private static void UnzipZipEntry(ZipInputStream stream, string outputPath)
        {
            if (File.Exists(outputPath))
            {
                IOUtil.DeleteFile(outputPath);
            }

            using (FileStream streamWriter = File.Create(outputPath))
            {
                int size = 2048;
                byte[] data = new byte[2048];
                while (true)
                {
                    size = stream.Read(data, 0, data.Length);
                    if (size > 0)
                    {
                        streamWriter.Write(data, 0, size);
                    }
                    else
                    {
                        break;
                    }
                }

            }
        }
    }
}
