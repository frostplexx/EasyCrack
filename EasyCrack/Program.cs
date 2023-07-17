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
using System.Windows.Forms;
using EasyCrack;
using MessageBox = System.Windows.MessageBox;
//How to sign: signtool sign /f "frostplexx.pfx" /fd SHA256 /p frostplexx "D:\EasyCrack.exe"
//more info: https://docs.google.com/document/d/1e5hbWLSDe71jfEtzUiVqKbv5LO8KUBlD37oitckn3z0/edit#

namespace EasyCrack
{

    static class Program
    {
        
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
    private Messages messages = new Messages();
    public SteamParser steamParser = new SteamParser();
    public Crack cracker = new Crack();
    public Game(EasyCrack.EasyCrack form)
    {
        this.form = form;
    }
    public async void crack()
    {
        //disable the button
        form.button1.Enabled = false;
        //set cursor to wait
        form.Cursor = Cursors.WaitCursor;
        //check if none of the inputs is wrong
        if (gamePath.Equals(""))
        {
            messages.errorPopup("Path to Game Folder cannot be empty!");
            form.button1.Enabled = true;
            form.Cursor = Cursors.Default;
            return;
        }
        if (appID.Equals(""))
        {
            messages.errorPopup("App ID cannot be empty!");
            form.button1.Enabled = true;
            form.Cursor = Cursors.Default;
            return;
        }
        if (playerName.Equals(""))
        {
            messages.errorPopup("Player Name cannot be empty!");
            form.button1.Enabled = true;
            form.Cursor = Cursors.Default;
            return;
        }
        if (language.Equals(""))
        {
            messages.errorPopup("Language cannot be empty!");
            form.button1.Enabled = true;
            form.Cursor = Cursors.Default;
            return;
        }

        //removes the last element of the path because we dont need the file
        List<String> pathArr = new List<String>(this.gamePath.Split(new string[] { "\\" }, StringSplitOptions.None));
        pathArr.RemoveAt(pathArr.Count - 1);
        gamePath = String.Join("\\", pathArr);

        cracker.setPlayerName(playerName);
        cracker.setLanguage(form.comboBox1.Text);
        cracker.setAppID(appID);
        cracker.setGamePath(gamePath);
        cracker.setCreateModsFolder(createModsFolder);

        //download the emulator files
        string folderPath = await cracker.downloadEmu(prog: form.progressBar1);


        steamParser.setAppID(appID);
        //check for drm by querying steam again
        steamParser.checkForDRM();

        generateInterfaces(gamePath + "\\steam_api.dll");

        //create the files with the options
        cracker.createFiles(form.progressBar1);

        //delete folder
        Directory.Delete(folderPath, true);

        //set progress to 100%
        form.progressBar1.Value = 100;

        messages.successMessageBox();

        form.progressBar1.Value = 0;
        //enable button again
        form.button1.Enabled = true;
        form.Cursor = Cursors.Default;
    }

    public void updateLanguage(string lang)
    {
        cracker.setLanguage(language: lang);
    }

    public void updateAppID(string newItem, Dictionary<string, string> searchedGames)
    {
        if (searchedGames.TryGetValue(newItem, out var game))
        {
            form.appid.Text = game;
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
    private uint findInInterface(StreamWriter outFile, string fileContents, string @interface)
    {
        Regex interfaceRegex = new Regex(@interface);
        uint matches = 0;
        foreach (var interf in interfaceRegex.Matches(fileContents))
        {

            string matchStr = interf.ToString();
            outFile.WriteLine(matchStr);
            ++matches;
        }
        return matches;
    }



    public void detectSettings()
    {
        //remove the *.exe from the path and the last \
        string gameFolder = "";
        //check if its a valid path
        if(gamePath.Contains("\\"))
        {
            gameFolder = gamePath.Substring(0, gamePath.LastIndexOf("\\"));
        }
        //check if the the file steam_appid.txt exists at gamePath
        if (File.Exists(gameFolder + "\\steam_appid.txt"))
        {
            //read the file
            string appid = File.ReadAllText(gameFolder + "\\steam_appid.txt");
            //check if the file is empty
            if (appid != "")
            {
                //set the appid box to the appid
                form.appid.Text = appid;
                appID = appid;
            }
        }
            
        //ceck if the file steam_interfaces.txt exists at gamePath
        if (File.Exists(gameFolder + "\\steam_interfaces.txt"))
        {
            //read the file
            string interfaces = File.ReadAllText(gameFolder + "\\steam_interfaces.txt");
            //check if the file is empty
            if (interfaces == "")
            {
                messages.errorPopup("It seems like the steam_interfaces.txt file is empty, but it will be fixed if you click \"Patch\" again.");
            }
        }

        //check if the file force_language.txt exists at gamePath\steam_settings
        if (File.Exists(gameFolder + "\\steam_settings\\force_language.txt"))
        {
            //read the file
            string language = File.ReadAllText(gameFolder + "\\steam_settings\\force_language.txt");
            //check if the file is empty
            if (language != "")
            {
                form.comboBox1.Text = language;
            }
        }

        //check if force_account_name.txt exists at gamePath\steam_settings
        if (File.Exists(gameFolder + "\\steam_settings\\force_account_name.txt"))
        {
            //read the file
            string accountName = File.ReadAllText(gameFolder + "\\steam_settings\\force_account_name.txt");
            //check if the file is empty
            if (accountName != "")
            {
                //set the account name box to the account name
                EasyCrack.Properties.Settings.Default.playername = accountName;
            }
        }

    }
}
