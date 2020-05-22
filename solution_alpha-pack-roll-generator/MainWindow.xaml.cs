using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace solution_alpha_pack_roll_generator
{
    public partial class MainWindow : Window
    {
        /*
         * To-do
         * =====
         * - add counters for the rolled packs → RollUntil button
         * - make RollUntil button... work
         * - create different routines depnding on useCustom variable !!
         */

        static readonly double[] rarities_valuesdefault = { 0.33, 0.285, 0.224, 0.125, 0.036 };
        static double[] rarities_valuescustom = { 0, 0, 0, 0, 0 };

        static bool useCustom = false;

        static ComboBoxItem stopOnSelection;

        static TextBox[] rarities_boxeschances;

        public MainWindow()
        {
            InitializeComponent();

            rarities_boxeschances = new TextBox[]
            {
                Rarities_1_Chance,
                Rarities_2_Chance,
                Rarities_3_Chance,
                Rarities_4_Chance,
                Rarities_5_Chance
            };

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
            for (int i = 0; i < rarities_valuesdefault.Length; i++)
            {
                rarities_boxeschances[i].Text = rarities_valuesdefault[i].ToString();

                rarities_valuescustom[i] = 0;
            }
        }
        
        private void Rarities_UseCustoms_Click(object sender, RoutedEventArgs e)
        {
            useCustom = true;
            
            for (int i = 0; i < rarities_boxeschances.Length; i++)
            {
                rarities_valuescustom[i] = Convert.ToDouble(rarities_boxeschances[i].Text);
            }

            // Check if all of the custom values are equal to the defaults
            // If yes, reset rarities_valuescustom[] to all 0s and change useCustom flag
            // If not, do nothing
            bool[] vs = { false, false, false, false, false };

            for (int i = 0; i < rarities_valuescustom.Length; i++)
            {
                if (rarities_valuescustom[i] == rarities_valuesdefault[i])
                {
                    vs[i] = true;
                }
            }

            if (vs[0] == true && vs[1] == true && vs[2] == true && vs[3] == true && vs[4] == true)
            {
                for (int i = 0; i < rarities_valuescustom.Length; i++)
                {
                    rarities_valuescustom[i] = 0;
                }

                useCustom = false;
            }
        }

        private void RollControls_RollOnce_Click(object sender, RoutedEventArgs e)
        {
            Random ran = new Random();
            double rollChance = Math.Round(ran.NextDouble(), 3);

            if (rollChance > rarities_valuesdefault[0])
            {
                Rolls_RollOnce_Result.Text = "Common";
            }
            else if (rollChance <= rarities_valuesdefault[0] && rollChance > rarities_valuesdefault[1])
            {
                Rolls_RollOnce_Result.Text = "Uncommon";
            }
            else if (rollChance <= rarities_valuesdefault[1] && rollChance > rarities_valuesdefault[2])
            {
                Rolls_RollOnce_Result.Text = "Rare";
            }
            else if (rollChance <= rarities_valuesdefault[2] && rollChance > rarities_valuesdefault[3])
            {
                Rolls_RollOnce_Result.Text = "Epic";
            }
            else if (rollChance < rarities_valuesdefault[3])
            {
                Rolls_RollOnce_Result.Text = "Legendary";
            }
        }



        private void RollControls_RollUntil_Click(object sender, RoutedEventArgs e)
        {

        }

        private void RollControls_StopOn_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            stopOnSelection = (ComboBoxItem)RollControls_StopOn.SelectedItem;
        }

        private MessageBoxResult YesNo(string msg, string title)
        {
            return MessageBox.Show(msg, title, MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
        }
    }
}