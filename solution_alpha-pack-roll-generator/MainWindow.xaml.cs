using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Windows;

namespace solution_alpha_pack_roll_generator
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            MessageBoxResult r = YesNo("This is all based on a Reddit post from July 19, 2017\nAt least *right now*, I really can't be arsed to count and find this data by myself. Maybe in the future, but... not now.\n\nWould you like to open the Reddit post?", "Reddit post");

            if (r == MessageBoxResult.Yes)
            {
                string browser = string.Empty;
                RegistryKey key = null;
                try
                {
                    key = Registry.ClassesRoot.OpenSubKey(@"HTTP\shell\open\command");
                    if (key != null)
                    {
                        browser = key.GetValue(null).ToString().ToLower().Trim(new[] { '"' });
                    }
                    if (!browser.EndsWith("exe"))
                    {
                        browser = browser.Substring(0, browser.LastIndexOf(".exe", StringComparison.InvariantCultureIgnoreCase) + 4);
                    }
                }
                finally
                {
                    if (key != null)
                    {
                        key.Close();
                    }
                }

                Process.Start(browser, "https://www.reddit.com/r/Rainbow6/comments/6o7o23/alpha_packs_statistics_and_info_1000_packs/");
            }
        }

        private void Rarities_Defaults_Click(object sender, RoutedEventArgs e)
        {
            
        }
        
        private void Rarities_UseCustoms_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private MessageBoxResult YesNo(string msg, string title)
        {
            return MessageBox.Show(msg, title, MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
        }
    }
}