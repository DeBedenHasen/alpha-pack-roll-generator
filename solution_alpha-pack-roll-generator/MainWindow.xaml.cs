using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace solution_alpha_pack_roll_generator
{
    public partial class MainWindow : Window
    {
        static readonly double[] rarities_valuesdefault = { 0.33, 0.285, 0.224, 0.125, 0.036 };
        static double[] rarities_valuescustom = { 0, 0, 0, 0, 0 };

        static bool useCustom = false;

        static TextBox[] rarities_boxeschances;

        static TextBlock[] amounts;
        static Button[] amounts_calls;
        static int[] amounts_counters = { 0, 0, 0, 0, 0 };

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

            amounts_calls = new Button[]
            {
                Rolls_Results_Amount_1_Label,
                Rolls_Results_Amount_2_Label,
                Rolls_Results_Amount_3_Label,
                Rolls_Results_Amount_4_Label,
                Rolls_Results_Amount_5_Label
            };

            amounts = new TextBlock[]
            {
                Rolls_Results_Amount_1,
                Rolls_Results_Amount_2,
                Rolls_Results_Amount_3,
                Rolls_Results_Amount_4,
                Rolls_Results_Amount_5
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

            try
            {
                for (int i = 0; i < rarities_boxeschances.Length; i++)
                {
                    rarities_valuescustom[i] = Convert.ToDouble(rarities_boxeschances[i].Text);
                }
            }
            catch (Exception)
            {
                useCustom = false;

                for (int i = 0; i < rarities_valuescustom.Length; i++)
                {
                    rarities_valuescustom[i] = 0;
                    rarities_boxeschances[i].Text = rarities_valuesdefault[i].ToString();
                }

                Error("Incorrect input format of at least one custom rarity value.\nResetting to defaults.", "Error");

                return;
            }

            //Check if the values do not add up to 1
            //If yes, show error, reset rarities_valuescustom[] to all 0s and change useCustom flag back
            //If not, proceed
            if (Sum(5, rarities_valuescustom) != 1)
            {
                for (int i = 0; i < rarities_valuescustom.Length; i++)
                {
                    rarities_valuescustom[i] = 0;
                    rarities_boxeschances[i].Text = rarities_valuesdefault[i].ToString();
                }

                useCustom = false;

                Error("The values entered do not add up to 1 and therefore cannot produce valid output!\nResetting to defaults.","Error");

                return;
            }

            //Check if all of the custom values are equal to the defaults
            //If yes, reset rarities_valuescustom[] to all 0s and change useCustom flag back
            //If not, do nothing
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

        private double Sum(int howManyElements, double[] fromThisArray)
        {
            double sum = 0.0;

            for (int i = 0; i < howManyElements; i++)
            {
                sum += fromThisArray[i];
            }

            return sum;
        }

        private void RollControls_RollOnce_Click(object sender, RoutedEventArgs e)
        {
            RollControls_RollOnce_Click();
        }

        private void RollControls_RollOnce_Click()
        {
            //Generate new random double
            Random ran = new Random();
            double rollChance = Math.Round(ran.NextDouble(), 4);

            //Decide which set of values to use, then perform the checks on rollChance to determine the roll rarity,
            //then update the connected counter
            if (useCustom == true)
            {
                if (rollChance <= Sum(1, rarities_valuescustom))
                {
                    Rolls_RollOnce_Result.Text = "Common";
                    amounts[0].Text = Convert.ToString(Convert.ToInt32(amounts[0].Text) + 1);
                }
                else if (rollChance <= Sum(2, rarities_valuescustom))
                {
                    Rolls_RollOnce_Result.Text = "Uncommon";
                    amounts[1].Text = Convert.ToString(Convert.ToInt32(amounts[1].Text) + 1);
                }
                else if (rollChance <= Sum(3, rarities_valuescustom))
                {
                    Rolls_RollOnce_Result.Text = "Rare";
                    amounts[2].Text = Convert.ToString(Convert.ToInt32(amounts[2].Text) + 1);
                }
                else if (rollChance <= Sum(4, rarities_valuescustom))
                {
                    Rolls_RollOnce_Result.Text = "Epic";
                    amounts[3].Text = Convert.ToString(Convert.ToInt32(amounts[3].Text) + 1);
                }
                else if (rollChance > Sum(4, rarities_valuescustom))
                {
                    Rolls_RollOnce_Result.Text = "Legendary";
                    amounts[4].Text = Convert.ToString(Convert.ToInt32(amounts[4].Text) + 1);
                }
                else
                {
                    Error("Out-of-range value for rollChance encountered: < " + rollChance + " >", "Error!");
                }
            }
            else
            {
                if (rollChance <= Sum(1, rarities_valuesdefault))
                {
                    Rolls_RollOnce_Result.Text = "Common";
                    amounts[0].Text = Convert.ToString(Convert.ToInt32(amounts[0].Text) + 1);
                }
                else if (rollChance <= Sum(2, rarities_valuesdefault))
                {
                    Rolls_RollOnce_Result.Text = "Uncommon";
                    amounts[1].Text = Convert.ToString(Convert.ToInt32(amounts[1].Text) + 1);
                }
                else if (rollChance <= Sum(3, rarities_valuesdefault))
                {
                    Rolls_RollOnce_Result.Text = "Rare";
                    amounts[2].Text = Convert.ToString(Convert.ToInt32(amounts[2].Text) + 1);
                }
                else if (rollChance <= Sum(4, rarities_valuesdefault))
                {
                    Rolls_RollOnce_Result.Text = "Epic";
                    amounts[3].Text = Convert.ToString(Convert.ToInt32(amounts[3].Text) + 1);
                }
                else if (rollChance > Sum(4, rarities_valuesdefault))
                {
                    Rolls_RollOnce_Result.Text = "Legendary";
                    amounts[4].Text = Convert.ToString(Convert.ToInt32(amounts[4].Text) + 1);
                }
                else
                {
                    Error("Out-of-range value for rollChance encountered: < " + rollChance + " >", "Error!");
                }
            }

            //Update last rollChance display
            Rolls_LastRollChance.Text = rollChance.ToString();

            //Get new total and update the display
            int newTotal = 0;

            for (int i = 0; i < amounts.Length; i++)
            {
                newTotal += Convert.ToInt32(amounts[i].Text);
            }

            Rolls_TotalRolls.Text = newTotal.ToString();
        }

        private void RollControls_RollUntil_Click(object sender, RoutedEventArgs e)
        {
            RollControls_ResetAmounts_Click();

            TextBlock target = null;

            for (int i = 0; i < amounts_calls.Length; i++)
            {
                if (sender == amounts_calls[i])
                {
                    target = amounts[i];
                }
            }

            if (target == null)
            {
                Error("Determining < target > returned < null >! It is likely the comparison to < sender > failed!\n< sender > == " + sender.ToString(), "Error");
                return;
            }

            while (true)
            {
                RollControls_RollOnce_Click();

                if (Convert.ToInt32(target.Text) != 0)
                {
                    break;
                }

                //Sleep(100);
            }
        }

        private void Sleep(int ms)
        {
            
        }

        private MessageBoxResult YesNo(string msg, string title)
        {
            return MessageBox.Show(msg, title, MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
        }
        
        private void Error(string msg, string title)
        {
            MessageBox.Show(msg, title, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void OK(string msg, string title)
        {
            MessageBox.Show(msg, title, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void RollControls_ResetAmounts_Click(object sender, RoutedEventArgs e)
        {
            RollControls_ResetAmounts_Click();
        }

        private void RollControls_ResetAmounts_Click()
        {
            for (int i = 0; i < amounts_counters.Length; i++)
            {
                Rolls_RollOnce_Result.Text = "—";

                amounts_counters[i] = 0;
                amounts[i].Text = amounts_counters[i].ToString();
                Rolls_TotalRolls.Text = 0.ToString();
            }
        }
    }
}