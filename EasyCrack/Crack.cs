using System;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EasyCrack
{
    public class Crack
    {
        private string gamePath = "";
        private string appID = "";
        private bool createModsFolder = true;
        private string language = "";
        private string playerName = "";

        /**
         * Downloads the Emulator Files and returns the path to the folder
         */
        public async Task<string> downloadEmu(ProgressBar prog)
        {
            //list of files we want to copy from the emu folder
            string[] filesToCopy = { "steam_api64.dll", "steam_api.dll", "lobby_connect\\lobby_connect.exe" };

            //download file
            string zipPath = gamePath + "\\emu.zip";
            var folderPath = await downloadCrack(zipPath, prog);

            //copy the relevant files to the root of the game
            foreach (var file in filesToCopy)
            {
                var filePath = folderPath + "\\" + file;
                var rootFilePath = gamePath + "\\" + file;
                if (file.Contains("\\")) rootFilePath = gamePath + "\\" + file.Split(new string[] { "\\" }, StringSplitOptions.None)[1]; //if the file contains a path only use the filename, so it copies to the root of the game folder
                if (File.Exists(rootFilePath)) File.Delete(rootFilePath); //delete original file if its exits;
                File.Copy(filePath, rootFilePath); //copy new file
            }

            return folderPath;
        }


        public void createFiles(ProgressBar prog)
        {
            //generate appID
        var appIdFile = File.CreateText(this.gamePath + "\\steam_appid.txt");
        appIdFile.WriteLine(appID);
        appIdFile.Close();

        //generate other folders
        Directory.CreateDirectory(this.gamePath + "\\steam_settings");

        //create mods folder if ticked
        if (createModsFolder) Directory.CreateDirectory(this.gamePath + "\\steam_settings\\mods");

        //set progress to 50%
        prog.Value = 50;

        //create language file
        var langFile = File.CreateText(this.gamePath + "\\steam_settings\\force_language.txt");
        langFile.WriteLine(language);
        langFile.Close();

        //create account name file
        var playerFile = File.CreateText(this.gamePath + "\\steam_settings\\force_account_name.txt");
        playerFile.WriteLine(playerName);
        playerFile.Close();
        }


        /**
         * This method downloads and extracts the emulator zip
         */
        private async Task<string> downloadCrack(string zipPath, ProgressBar prog)
        {
            var folderPath = gamePath + "\\emu_tmp";
            if (Directory.Exists(folderPath)) return folderPath; //if the directory already exists, return it
            if (File.Exists(zipPath)) File.Delete(zipPath);
            using (WebClient wc = new WebClient())
            {
                //handle progress bar
                wc.DownloadProgressChanged += (s, e) =>
                {
                    prog.Value = e.ProgressPercentage;
                };
                
                //TODO Rework this to scrape the download website and always get the newest version
                await wc.DownloadFileTaskAsync(
                    // Param1 = Link of file
                    new System.Uri("https://gitlab.com/Mr_Goldberg/goldberg_emulator/-/jobs/2426823199/artifacts/download"),
                    // Param2 = Path to save
                    zipPath
                );
            }
            ZipFile.ExtractToDirectory(zipPath, folderPath);
            File.Delete(zipPath);
            return folderPath;
        }


        public string getGamePath()
        {
            return gamePath;
        }

        public string getAppID()
        {
            return appID;
        }

        public bool getCreateModsFolder()
        {
            return createModsFolder;
        }

        public string getLanguage()
        {
            return language;
        }

        public string getPlayerName()
        {
            return playerName;
        }

        public void setGamePath(string gamePath)
        {
            this.gamePath = gamePath;
        }

        public void setAppID(string appID)
        {
            this.appID = appID;
        }

        public void setCreateModsFolder(bool createModsFolder)
        {
            this.createModsFolder = createModsFolder;
        }

        public void setLanguage(string language)
        {
            this.language = language;
        }

        public void setPlayerName(string playerName)
        {
            this.playerName = playerName;
        }

    }
}