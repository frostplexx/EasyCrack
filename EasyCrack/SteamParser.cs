using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Forms;
using HtmlAgilityPack;
using ScrapySharp.Extensions;
using EasyCrack;
using MessageBox = System.Windows.Forms.MessageBox;

namespace EasyCrack
{
    public class SteamParser
    {
        private Messages messages = new Messages();
        private string appID = "";
        /**
         * Search for a game by scraping the steam website
         */
        public Dictionary<string, string> searchGame(string game, EasyCrack form)
    {
        //set cursor to loading
        form.Cursor = Cursors.WaitCursor;
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
            new Messages().errorPopup("Could not connect to Steam! Make sure you are connected to the Internet.\n\n\n\n" + e);
            return null;
        }
        var games = doc.DocumentNode.CssSelect("#search_resultsRows");
        //select first game
        //clear combobox
        form.comboBox2.Items.Clear();
        Dictionary<string, string> searchedGames = new Dictionary<string, string>();
        foreach (var item in games)
        {


            //add all appIDs to the appIDS array
            var gamesA = item.CssSelect("a");
            string[] appIDs = new string[gamesA.Count()];
            int ind = 0;
            foreach (var gameA in gamesA)
            {
                foreach (var attribute in gameA.Attributes)
                {
                    if (attribute.Name.Equals("data-ds-appid"))
                    {
                        
                        appIDs[ind] = attribute.Value;
                        break;
                    }
                }
                ind++;
            }

            //get all the titles of the searched games an put them into an array
            var gameSpans = item.CssSelect("span.title");
            string[] gameTitles = new string[gameSpans.Count()];
            int inde = 0;
            foreach (var title in gameSpans)
            {
                gameTitles[inde] = title.InnerText;
                inde++;
            }

            foreach (var id in appIDs)
            {

                if (!String.IsNullOrEmpty(id)) //filter out bundles as they do not have a appID
                {
                    form.comboBox2.Items.Add(gameTitles[Array.IndexOf(appIDs, id)]);
                    searchedGames.Add(gameTitles[Array.IndexOf(appIDs, id)], id);
                }

            }
        }

        //fill the combobox and appid box with the first search result
        if (searchedGames.Count() != 0)
        {
            form.appid.Text = searchedGames.First().Value;
            form.comboBox2.Text = searchedGames.First().Key;
        }
        else
        {
            new Messages().errorPopup("Could not find Game! Are you sure you wrote it correctly?");
        }
        //set cursor to normal
        form.Cursor = Cursors.Default;
        return searchedGames;
    }

        public bool checkForDRM()
    {
        if (this.appID == "")
        {
            Console.WriteLine("No AppID given, make sure you set it with setAppID()");
            return false;
        }
        string url = "https://store.steampowered.com/app/" + appID;

        HtmlWeb web;
        HtmlAgilityPack.HtmlDocument doc;
        try
        {
            CookieContainer cookieContainer = new CookieContainer();
            web = new HtmlWeb();
            //handle cookies
            web.UseCookies = true;
            web.PreRequest = new HtmlWeb.PreRequestHandler(OnPreRequest2);
            web.PostResponse = new HtmlWeb.PostResponseHandler(OnAfterResponse2);
            //load url
            doc = web.Load(url);

            //more cookie stuff
            bool OnPreRequest2(HttpWebRequest request)
            {
                cookieContainer.Add(new Cookie("birthtime", "568022401") { Domain = new Uri(url).Host});
                request.CookieContainer = cookieContainer;
                return true;
            }
            void OnAfterResponse2(HttpWebRequest request, HttpWebResponse response)
            {
                //do nothing
            }
        }
        catch (Exception e)
        {
            messages.errorPopup("Could not connect to Steam! Make sure you are connected to the Internet.\n\n\n\n" + e);
            return false;
        }
        var hasDRM = doc.DocumentNode.CssSelect(".DRM_notice");
        if(hasDRM.Count() > 0)
        {
            messages.hasDRMPopup(hasDRM.First().InnerText.Trim().Replace("&nbsp;", " "));
            return true;
        } else
        {
            return false;
        }
    }

        public void setAppID(string appID)
        {
            this.appID = appID;
        }
    }
}