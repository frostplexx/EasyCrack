using HtmlAgilityPack;
using ScrapySharp.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
namespace EasyCrack
{

    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>

        [STAThread]
        static void Main()
        {
            System.Windows.Forms.Application.EnableVisualStyles();
            System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);
            System.Windows.Forms.Application.Run(new EasyCrack());
        }
    }
}

public class Game
{
    EasyCrack.EasyCrack form;
    public string gamePath = ""; //path to game.exe
    public string appID = ""; //app id
    public string playerName = EasyCrack.Properties.Settings.Default.playername; //player name
    public bool createModsFolder = true; //if mods folder should be created
    public string language = "english"; //language, default english

    public Game(EasyCrack.EasyCrack form)
    {
        this.form = form;
    }
    public async void crack()
    {
        //check if none of the inputs is wrong
        if (this.gamePath.Equals(""))
        {
            errorPopup("Path to Game Folder cannot be empty!");
            return;
        }
        if (this.appID.Equals(""))
        {
            errorPopup("App ID cannot be empty!");
            return;
        }
        if (this.playerName.Equals(""))
        {
            errorPopup("Player Name cannot be empty!");
            return;
        }
        if (this.language.Equals(""))
        {
            errorPopup("Language cannot be empty!");
            return;
        }

        //removes the last element of the path because we dont need the file
        List<String> pathArr = new List<String>(this.gamePath.Split(new string[] { "\\" }, StringSplitOptions.None));
        pathArr.RemoveAt(pathArr.Count - 1);
        this.gamePath = String.Join("\\", pathArr);

        //list of files we want to copy from the emu folder
        string[] filesToCopy = new string[] { "steam_api64.dll", "steam_api.dll", "lobby_connect\\lobby_connect.exe" };

        //download file 
        string zipPath = this.gamePath + "\\emu.zip";
        var folderPath = await downloadCrack(zipPath);
        //copy the relevant files to the root of the game
        foreach (var file in filesToCopy)
        {
            var filePath = folderPath + "\\" + file;
            var rootFilePath = this.gamePath + "\\" + file;
            if (file.Contains("\\")) rootFilePath = this.gamePath + "\\" + file.Split(new string[] { "\\" }, StringSplitOptions.None)[1]; //if the file contains a path only use the filename, so it copies to the root of the game folder
            if (File.Exists(rootFilePath)) File.Delete(rootFilePath); //delete original file if its exits; 
            File.Copy(filePath, rootFilePath); //copy new file
        }
        generateInterfaces(this.gamePath + "\\steam_api.dll");

        //generate appID
        var appIdFile = File.CreateText(this.gamePath + "\\steam_appid.txt");
        appIdFile.WriteLine(this.appID);
        appIdFile.Close();

        //generate other folders
        Directory.CreateDirectory(this.gamePath + "\\steam_settings");

        //create mods folder if ticked
        if (this.createModsFolder) Directory.CreateDirectory(this.gamePath + "\\steam_settings\\mods");

        //set progress to 50%
        form.progressBar1.Value = 50;

        //create language file
        var langFile = File.CreateText(this.gamePath + "\\steam_settings\\force_language.txt");
        langFile.WriteLine(this.language);
        langFile.Close();

        //create account name file
        var playerFile = File.CreateText(this.gamePath + "\\steam_settings\\force_account_name.txt");
        playerFile.WriteLine(this.playerName);
        playerFile.Close();

        //delete folder
        Directory.Delete(folderPath, true);

        //set progress to 100%
        form.progressBar1.Value = 100;

        //Print success Message
        string messageBoxText = "Successfully installed the Steam Emulator!";
        string caption = "Success";
        MessageBoxButton button = MessageBoxButton.OK;
        MessageBoxImage icon = MessageBoxImage.Information;
        MessageBoxResult result;

        result = System.Windows.MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.Yes);
        form.progressBar1.Value = 0;
    }

    // Event to track the progress
    private void wc_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
    {
        Console.WriteLine(e.ProgressPercentage);
        form.progressBar1.Value = e.ProgressPercentage;
    }

    //download and extract the emu
    private async Task<string> downloadCrack(string zipPath)
    {
        var folderPath = this.gamePath + "\\emu_tmp";
        if (Directory.Exists(folderPath)) return folderPath; //if the directory already exists, return it
        if (File.Exists(zipPath)) File.Delete(zipPath);
        using (WebClient wc = new WebClient())
        {
            wc.DownloadProgressChanged += wc_DownloadProgressChanged;
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

    public void SearchGame(string game)
    {
        string searchURL = "https://store.steampowered.com/search/?term=" + game.Replace(" ", "+");
        HtmlWeb web;
        HtmlAgilityPack.HtmlDocument doc;
        try
        {
            web = new HtmlWeb();
            doc = web.Load(searchURL);
        }
        catch (Exception e)
        {
            errorPopup("Could not connect to Steam! Make sure you are connected to the Internet.\n\n\n\n" + e);
            return;
        }
        var games = doc.DocumentNode.CssSelect("#search_resultsRows");
        string gameTitle = String.Empty;
        string gameID = String.Empty;
        //select first game 
        foreach (var item in games)
        {
            gameTitle = item.CssSelect("span.title").First().InnerText;


            var gameAttributes = item.CssSelect("a").First().Attributes;
            foreach (var attribute in gameAttributes)
            {
                if (attribute.Name.Equals("data-ds-appid"))
                {
                    gameID = attribute.Value;
                    break;
                }
            }
        }
        if (gameTitle.Equals(String.Empty) || gameID.Equals(String.Empty))
        {
            string noGameMsg = "Could not find the Game. Make sure you wrote it's name correctly!";
            errorPopup(noGameMsg);

        }
        else
        {
            form.appid.Text = gameID;
            form.textBox3.Text = gameTitle;
        }


    }

    //generates the interfaces file 
    //ported from https://gitlab.com/Mr_Goldberg/goldberg_emulator/-/blob/master/generate_interfaces_file.cpp
    private bool generateInterfaces(string dllPath)
    {
        var outFile = File.CreateText(this.gamePath + "\\steam_interfaces.txt");
        var steamApiContents = File.ReadAllText(dllPath);
        string[] interfaceNames = new string[] {"SteamClient",
                                                "SteamGameServer",
                                                "SteamGameServerStats",
                                                "SteamUser",
                                                "SteamFriends",
                                                "SteamUtils",
                                                "SteamMatchMaking",
                                                "SteamMatchMakingServers",
                                                "STEAMUSERSTATS_INTERFACE_VERSION",
                                                "STEAMAPPS_INTERFACE_VERSION",
                                                "SteamNetworking",
                                                "STEAMREMOTESTORAGE_INTERFACE_VERSION",
                                                "STEAMSCREENSHOTS_INTERFACE_VERSION",
                                                "STEAMHTTP_INTERFACE_VERSION",
                                                "STEAMUNIFIEDMESSAGES_INTERFACE_VERSION",
                                                "STEAMUGC_INTERFACE_VERSION",
                                                "STEAMAPPLIST_INTERFACE_VERSION",
                                                "STEAMMUSIC_INTERFACE_VERSION",
                                                "STEAMMUSICREMOTE_INTERFACE_VERSION",
                                                "STEAMHTMLSURFACE_INTERFACE_VERSION_",
                                                "STEAMINVENTORY_INTERFACE_V",
                                                "SteamController",
                                                "SteamMasterServerUpdater",
                                                "STEAMVIDEO_INTERFACE_V"};
        foreach (var name in interfaceNames)
        {
            findInInterface(outFile, steamApiContents, name + "\\d{3}");
        }

        if (findInInterface(outFile, steamApiContents, "STEAMCONTROLLER_INTERFACE_VERSION\\d{3}") == 0)
        {
            findInInterface(outFile, steamApiContents, "STEAMCONTROLLER_INTERFACE_VERSION");
        }
        outFile.Close();
        return true;
    }

    //part of generate interfaces
    private uint findInInterface(StreamWriter outFile, string fileContents, string intrface)
    {
        Regex interface_regex = new Regex(intrface);
        uint matches = 0;
        foreach (var interf in interface_regex.Matches(fileContents))
        {

            string match_str = interf.ToString();
            outFile.WriteLine(match_str);
            ++matches;
        }
        return matches;
    }

    //creates an error alert with a given message
    private void errorPopup(string message)
    {
        string messageBoxText = message;
        string caption = "Error!";
        MessageBoxButton button = MessageBoxButton.OK;
        MessageBoxImage icon = MessageBoxImage.Error;
        MessageBoxResult result;

        result = System.Windows.MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.Yes);
    }


}
