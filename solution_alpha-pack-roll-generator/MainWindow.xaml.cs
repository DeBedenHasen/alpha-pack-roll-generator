using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace solution_alpha_pack_roll_generator
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            LB_Rarities.ItemsSource = LoadCollectionData();
            // resize columns
        }

        private List<AlphaPack> LoadCollectionData()
        {
            List<AlphaPack> packs = new List<AlphaPack>
            {
                new AlphaPack()
                {
                    Rarity = "Common",
                    Chance = 0.33
                },

                new AlphaPack()
                {
                    Rarity = "Uncommon",
                    Chance = 0.285
                },

                new AlphaPack()
                {
                    Rarity = "Rare",
                    Chance = 0.224
                },

                new AlphaPack()
                {
                    Rarity = "Epic",
                    Chance = 0.125
                },

                new AlphaPack()
                {
                    Rarity = "Legendary",
                    Chance = 0.036
                }
            };

            return packs;
        }
    }

    public class AlphaPack
    {
        public string Rarity { get; set; }
        public double Chance { get; set; }
    }
}