using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Forms;
using Application = System.Windows.Forms.Application;
namespace EasyCrack
{
    public class Messages
    {
        public void helpMessageBox()
        {
            string messageBoxText = "1. Select the folder where the game .exe file is located\n2.Search your game to automatically find the App ID or go to http://steamdb.com and search your game.\n3. Enter the App ID from steamdb into Easy Crack if you did not choose to let EasyCrack search the game for you.\n4. Enter player name, language and if you want to generate a mods folder.\n5. Click on \"Patch\" and watch the magic happen!\n\nFor more info please see: https://gitlab.com/Mr_Goldberg/goldberg_emulator.";
            string caption = "Help";
            MessageBoxButton button = MessageBoxButton.OK;
            MessageBoxImage icon = MessageBoxImage.Question;
            System.Windows.MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.Yes);
        }

        public void successMessageBox()
        {
            string messageBoxText = "Successfully installed the Steam Emulator!";
            string caption = "Success";
            MessageBoxButton button = MessageBoxButton.OK;
            MessageBoxImage icon = MessageBoxImage.Information;
            System.Windows.MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.Yes);
        }

        //creates an error alert with a given message
        public void errorPopup(string message)
        {
            string messageBoxText = message;
            string caption = "Error!";
            MessageBoxButton button = MessageBoxButton.OK;
            MessageBoxImage icon = MessageBoxImage.Error;
            System.Windows.MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.Yes);
        }

        public void hasDRMPopup(string drm)
        {
            string messageBoxText = "DRM Detected!\n\n" + drm + "\n\nThe game will still be patched, but may not work without additional cracks!";
            string caption = "DRM Found";
            MessageBoxButton button = MessageBoxButton.OK;
            MessageBoxImage icon = MessageBoxImage.Warning;
            System.Windows.MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.Yes);
        }

    }
}