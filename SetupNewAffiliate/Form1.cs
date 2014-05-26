using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SetupNewAffiliate.Properties;
using System.IO;

namespace SetupNewAffiliate
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Save_Click(object sender, EventArgs e)
        {
            try
            {
                // Setup for Web.
                SetupWebFolder();  
                // Setup for Service.
                SetupService(AffiliateName.Text);
                //Add Connection String for API Web Service.
                SetupWebService(AffiliateName.Text);

                Msg.Text = "Create System " + AffiliateName.Text + " Successfully";
            }
            catch (Exception ex) {
                Msg.Text = ex.Message;
            }
        }

        #region Setup structure for Service
        void SetupService(string affiliateName)
        {
            //1. Add SpName.xml in fanex.data folder
            AddSPNameFile(affiliateName);
            //2. Add Connection String  in connectionstring.config
            AddConnectionString(affiliateName);
            //3. Add trigger in quartjob.xml (only on service)
            AddTriggerInQuartJob(affiliateName);
            //4. Add new licensee in Licensees.xml
            AddLicensee(affiliateName);
        }

        void AddSPNameFile(string affiliateName)
        {
            var pathPatternSP = Settings.Default.AffServer2 + "\\E$\\2.Service\\0.Pattern\\SPName.xml";
            var spContent = File.ReadAllText(pathPatternSP);

            spContent = spContent.Replace("{affiliatename}", affiliateName.ToUpper());

            var pathWinForm = Settings.Default.AffServer2 + string.Format("\\E$\\2.Service\\6.Fanex.Affiliate.Services.WinForm\\Fanex.Data\\{0}.xml", affiliateName.ToUpper()) ;
            var pathService = Settings.Default.AffServer2 + string.Format("\\E$\\2.Service\\7.Fanex.Affiliate.Services.WinService\\Fanex.Data\\{0}.xml", affiliateName.ToUpper());

            //Create spname.xml file
            File.WriteAllText(pathWinForm, spContent);
            File.WriteAllText(pathService, spContent);
        }

        void AddConnectionString(string affiliateName) {
            var pathPatternConnection = Settings.Default.AffServer2 + "\\E$\\2.Service\\0.Pattern\\Connectionstring.config";
            var newConnectionContent = File.ReadAllText(pathPatternConnection);


            newConnectionContent = newConnectionContent.Replace("{affiliatename}", affiliateName.ToUpper());

            var pathConnectionService = Settings.Default.AffServer2 + "\\E$\\2.Service\\7.Fanex.Affiliate.Services.WinService\\ConnectionString.config";
            var pathConnectionWinform = Settings.Default.AffServer2 + "\\E$\\2.Service\\6.Fanex.Affiliate.Services.WinForm\\ConnectionString.config";

            var connectionContent = File.ReadAllText(pathConnectionService);
            if (connectionContent.IndexOf(affiliateName, StringComparison.OrdinalIgnoreCase) >= 0)
                return;

            connectionContent = connectionContent.Replace("</connectionStrings>", newConnectionContent + "\r\n" + "</connectionStrings>");

            File.WriteAllText(pathConnectionService, connectionContent);
            File.WriteAllText(pathConnectionWinform, connectionContent);
        }

        void AddTriggerInQuartJob(string affiliateName){
            var pathPatternQuartJob = Settings.Default.AffServer2 + "\\E$\\2.Service\\0.Pattern\\quartjob.xml";
            var newQuartJobContent = File.ReadAllText(pathPatternQuartJob);
            newQuartJobContent = newQuartJobContent.Replace("{affiliatename}", affiliateName.ToLower());

            var pathQuartJobService = Settings.Default.AffServer2 + "\\E$\\2.Service\\7.Fanex.Affiliate.Services.WinService\\QuartzJobs.xml";
            var QuartJobContent = File.ReadAllText(pathQuartJobService);
            
            if (QuartJobContent.IndexOf(affiliateName, StringComparison.OrdinalIgnoreCase) >= 0)
                return;

            QuartJobContent = QuartJobContent.Replace("</schedule>", newQuartJobContent + "\r\n" + "</schedule>");
            File.WriteAllText(pathQuartJobService, QuartJobContent);
        }

        void AddLicensee(string affiliateName){
            var pathLicensee = Settings.Default.AffServer2 + "\\E$\\2.Service\\0.Pattern\\Licensees.xml";
            var newLicenseeContent = File.ReadAllText(pathLicensee);

            newLicenseeContent = newLicenseeContent.Replace("{affiliatename}", affiliateName.ToLower());

            var pathLicenseeService = Settings.Default.AffServer2 + "\\E$\\2.Service\\7.Fanex.Affiliate.Services.WinService\\Licensees.xml";
            var pathLicenseeWinform = Settings.Default.AffServer2 + "\\E$\\2.Service\\6.Fanex.Affiliate.Services.WinForm\\Licensees.xml";

            var LicenseeContent = File.ReadAllText(pathLicenseeService);

            if (LicenseeContent.IndexOf(affiliateName, StringComparison.OrdinalIgnoreCase) >= 0)
                return;

            LicenseeContent = LicenseeContent.Replace("</Licensees>", newLicenseeContent + "\r\n" + "</Licensees>");

            File.WriteAllText(pathLicenseeService, LicenseeContent);
            File.WriteAllText(pathLicenseeWinform, LicenseeContent);
        }

        #endregion

        #region Setup folder structure for web
        void SetupWebFolder()
        {
            var pathWeb = "\\E$\\1.Web\\" + AffiliateId.Text + "." + AffiliateName.Text;
            var pathBackup = "\\E$\\4.Backup\\" + AffiliateId.Text + "." + AffiliateName.Text;
            var pathPackage = "\\E$\\5.Package\\" + AffiliateId.Text + "." + AffiliateName.Text;

            //1. Create Web Directory
            CreateFolderStructure(pathWeb);
            //2. Create Backup Directory
            CreateFolderStructure(pathBackup);
            //3. Create Folder to deploy package
            CreateFolderStructure(pathPackage);
            //4. Create IIS setup fie in server main only
            CreateIISFiles();
        }

        private void CreateIISFiles()
        {
            var sourceDir = Path.GetFullPath(Settings.Default.AffServer1 + "\\E$\\7.Script\\0.Pattern");
            var targetDir = Path.GetFullPath(Settings.Default.AffServer1 + "\\E$\\7.Script\\" + AffiliateId.Text + "." + AffiliateName.Text);
            CopyAllFiles(sourceDir, targetDir);
            UpdateIISFilesContent(targetDir);

            var targetDir2 = Path.GetFullPath(Settings.Default.AffServer2 + "\\E$\\7.Script\\" + AffiliateId.Text + "." + AffiliateName.Text);
            CopyAllFiles(sourceDir, targetDir2);
            UpdateIISFilesContent(targetDir2);
        }

        private void CreateFolderStructure(string path)
        {
            //Create 4 folder: 1.Admin, 2.User, 3.Banner, 4.Go
            var pathAdmin = path + "\\1.Admin";
            var pathUser = path + "\\2.User";
            var pathBanner = path + "\\3.Banner";
            var pathGo = path + "\\4.Go";

            CreateFolder(path, Settings.Default.AffServer1);
            CreateFolder(pathAdmin, Settings.Default.AffServer1);
            CreateFolder(pathUser, Settings.Default.AffServer1);
            CreateFolder(pathBanner, Settings.Default.AffServer1);
            CreateFolder(pathGo, Settings.Default.AffServer1);

            if (!string.IsNullOrEmpty(Settings.Default.AffServer2))
            {
                CreateFolder(path, Settings.Default.AffServer2);
                CreateFolder(pathAdmin, Settings.Default.AffServer2);
                CreateFolder(pathUser, Settings.Default.AffServer2);
                CreateFolder(pathBanner, Settings.Default.AffServer2);
                CreateFolder(pathGo, Settings.Default.AffServer2);
            }


        }

        void CreateFolder(string path, string server)
        {
            var fullPath = Path.GetFullPath(server + path);
            if (!Directory.Exists(fullPath))
            {
                Directory.CreateDirectory(fullPath);
            }
        }

        void CopyAllFiles(string sourceDir, string targetDir)
        {
            Directory.CreateDirectory(targetDir);

            foreach (var file in Directory.GetFiles(sourceDir))
                File.Copy(file, Path.Combine(targetDir, Path.GetFileName(file)), true);

            //foreach (var directory in Directory.GetDirectories(sourceDir))
            //    CopyAllFiles(directory, Path.Combine(targetDir, Path.GetFileName(directory)));
        }

        void UpdateIISFilesContent(string targetDir)
        {
            foreach (var file in Directory.GetFiles(targetDir))
            {
                string text = File.ReadAllText(file);
                text = text.Replace("{sitename}", AffiliateName.Text);
                text = text.Replace("{affiliateid}", AffiliateId.Text);
                                    
                text = text.Replace("{adminsiteid}", adminSiteId.Text);
                text = text.Replace("{usersiteid}", userSiteId.Text);
                text = text.Replace("{bannersiteid}", bannerSiteId.Text);
                text = text.Replace("{gositeid}", goSiteId.Text);
                                    
                text = text.Replace("{adminport}", adminPort.Text);
                text = text.Replace("{userport}", userPort.Text);
                text = text.Replace("{bannerport}", bannerPort.Text);
                text = text.Replace("{goport}", goPort.Text);
                
                File.WriteAllText(file, text);
            }
        }
        #endregion

        #region
        void SetupWebService(string affiliateName)
        {
            var pathPatternConnection = Settings.Default.AffServer1 + "\\E$\\0.Patern\\API_Config.txt";
            var newConnectionContent = File.ReadAllText(pathPatternConnection);


            newConnectionContent = newConnectionContent.Replace("{affiliatename}", affiliateName.ToUpper());

            var pathAPIService = Settings.Default.AffServer1 + "\\E$\\1.Web\\15.WebServiceAPI\\Web.ConnectionString.config";

            var connectionContent = File.ReadAllText(pathAPIService);

            if (connectionContent.IndexOf(affiliateName, StringComparison.OrdinalIgnoreCase) >= 0)
                return;

            connectionContent = connectionContent.Replace("</connectionStrings>", newConnectionContent + "\r\n" + "</connectionStrings>");

            File.WriteAllText(pathAPIService, connectionContent);
        }
        #endregion

    }
}
