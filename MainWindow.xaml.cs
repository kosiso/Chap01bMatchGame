﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Chap01bMatchGame
{
    using System.Windows.Threading;
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer timer = new DispatcherTimer();
        int tenthsOfSecondElapsed;
        int matchesFound;
        int bestOfSecondElapsed = 0;
        public MainWindow()
        {
            InitializeComponent();

            timer.Interval = TimeSpan.FromSeconds(.1);
            timer.Tick += Timer_Tick;
            SetUpGame();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            tenthsOfSecondElapsed++;
            timeTextBlock.Text = (tenthsOfSecondElapsed / 10F).ToString("0.0s");
            if (matchesFound == 8)
            {
                timer.Stop();
                if (tenthsOfSecondElapsed < bestOfSecondElapsed && bestOfSecondElapsed != 0)
                {
                    bestOfSecondElapsed = tenthsOfSecondElapsed;
                    timeTextBlock.Text = "Best time " + timeTextBlock.Text + "! - Play again?";
                }
                else
                {
                    if (bestOfSecondElapsed == 0)
                    {
                        bestOfSecondElapsed = tenthsOfSecondElapsed;
                    }
                    timeTextBlock.Text += " - Play again?";
                }
            }
        }

        private void SetUpGame()
        {
            List<string> animalEmoji = new List<string>()
            {
                "🐙", "🐙",
                "🐠", "🐠",
                "🐕", "🐕",
                "🐀", "🐀",
                "🐪", "🐪",
                "🦨", "🦨",
                "🦚", "🦚",
                "🦋", "🦋",
            };
            Random random = new Random();

            foreach (TextBlock textBlock in mainGrid.Children.OfType<TextBlock>())
            {
                if (textBlock.Text != timeTextBlock.Text)
                {
                    int index = random.Next(animalEmoji.Count);
                    string nextEmoji = animalEmoji[index];
                    textBlock.Text = nextEmoji;
                    animalEmoji.RemoveAt(index);
                }
            }

            timer.Start();
            tenthsOfSecondElapsed = 0;
            matchesFound = 0;
        }

        TextBlock lastTextBlockClicked;
        bool findingMatch = false;
        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            TextBlock textBlock = sender as TextBlock;
            if (findingMatch == false)
            {
                textBlock.Visibility = Visibility.Hidden;
                lastTextBlockClicked = textBlock;
                findingMatch = true;
            }
            else if (textBlock.Text == lastTextBlockClicked.Text)
            {
                matchesFound++;
                textBlock.Visibility = Visibility.Hidden;
                findingMatch = false;
            }
            else
            {
                lastTextBlockClicked.Visibility = Visibility.Visible;
                findingMatch = false;
            }
        }

        private void TimeTextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (matchesFound == 8)
            {                
                SetUpGame();
                foreach (TextBlock textBlock in mainGrid.Children.OfType<TextBlock>())
                {
                    if (textBlock.Text != timeTextBlock.Text)
                    {
                        textBlock.Visibility = Visibility.Visible;
                    }
                }
            }
        }
    }
}
