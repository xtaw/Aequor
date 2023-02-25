using Aequor.Main;
using Aequor.Model;
using Aequor.Util;
using CefSharp;
using CefSharp.Wpf;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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

namespace Aequor.UI
{

    public partial class MainWindow : Window
    {

        private Sentence? currentSentence;

        private double sentenceBackgroundX, sentenceBackgroundY;

        public MainWindow()
        {
            InitializeComponent();
            MainBorder.MouseLeftButtonDown += (o, e) => { DragMove(); };
            MainBorder.MouseMove += (o, e) =>
            {
                Point point = Mouse.GetPosition(MainBorder);
                double validWidth = SentenceBackgroundImage.Width - 375D;
                double validHeight = SentenceBackgroundImage.Height - 135D;
                sentenceBackgroundX = -Math.Min(1D, Math.Max(0D, point.X / MainBorder.Width)) * validWidth;
                sentenceBackgroundY = -Math.Min(1D, Math.Max(0D, point.Y / MainBorder.Height)) * validHeight;
            };
            CompositionTarget.Rendering += (o, e) =>
            {
                MathUtil.MoveUD(SentenceBackgroundImage.Margin.Left, sentenceBackgroundX, 0.05D, 0.0025D, out double x);
                MathUtil.MoveUD(SentenceBackgroundImage.Margin.Top, sentenceBackgroundY, 0.05D, 0.0025D, out double y);
                SetSentenceBackgroundPosition(x, y);
            };
            SetSentence(AequorClient.INSTANCE.sentenceManager.GetRandomSentence(AequorClient.INSTANCE.configManager.sentenceCategory));
            SentenceCategory.SelectedIndex = Math.Min(Math.Max(AequorClient.INSTANCE.configManager.sentenceCategory, 0), SentenceCategory.Items.Count - 1);
            if (StartupUtil.IsStartup())
            {
                Startup.IsChecked = true;
            }
            if (AequorClient.INSTANCE.configManager.forceStudy)
            {
                ForceStudy.IsChecked = true;
            }
            if (AequorClient.INSTANCE.configManager.restReminder)
            {
                RestReminder.IsChecked = true;
            }
        }

        private void SetSentence(Sentence? sentence)
        {
            currentSentence = sentence;
            if (sentence == null)
            {
                SentenceText.Text = "该句子ID不存在";
                SentenceSource.Text = "—— 无 「无」";
                SentenceCategoryText.Text = "分类：无";
                return;
            }
            SentenceText.Text = sentence.text;
            SentenceSource.Text = "—— " + sentence.sourceCharacter + " 「" + sentence.source + "」";
            SentenceId.Text = sentence.id.ToString();
            SentenceCategoryText.Text = "分类：" + sentence.categoryText;
        }

        private void SetSentenceBackgroundPosition(double x, double y)
        {
            SentenceBackgroundImage.Margin = new Thickness(x, y, 0, 0);
            SentenceBackgroundClip.Transform = new TranslateTransform(-x, -y);
        }

        private void Close(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void Min(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void SentenceIdEntered(object sender, TextChangedEventArgs e)
        {
            string oldText = SentenceId.Text;
            int oldCaretIndex = SentenceId.CaretIndex;
            SentenceId.Text = string.Concat(SentenceId.Text.Where((c) => c >= '0' && c <= '9'));
            if (oldText != SentenceId.Text)
            {
                SentenceId.CaretIndex = oldCaretIndex - oldText.Length + SentenceId.Text.Length;
            }
            if (SentenceId.Text.Length <= 0)
            {
                SentenceId.Text = "0";
            }
            if (SentenceId.Text.Length >= 5)
            {
                SentenceId.Text = "9999";
            }
            Sentence? sentence = AequorClient.INSTANCE.sentenceManager.GetSentenceById(int.Parse(SentenceId.Text));
            SetSentence(sentence);
        }

        private void SentenceIdMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (currentSentence == null)
            {
                return;
            }
            int index = AequorClient.INSTANCE.sentenceManager.GetSentenceIndex(AequorClient.INSTANCE.configManager.sentenceCategory, currentSentence) - (e.Delta > 0 ? 1 : -1);
            if (index < 0)
            {
                index = 0;
            }
            if (index >= AequorClient.INSTANCE.sentenceManager.GetSentenceCount(AequorClient.INSTANCE.configManager.sentenceCategory))
            {
                index = AequorClient.INSTANCE.sentenceManager.GetSentenceCount(AequorClient.INSTANCE.configManager.sentenceCategory) - 1;
            }
            SetSentence(AequorClient.INSTANCE.sentenceManager.GetSentenceByIndex(AequorClient.INSTANCE.configManager.sentenceCategory, index));
        }

        private void RefreshSentence(object sender, RoutedEventArgs e)
        {
            SetSentence(AequorClient.INSTANCE.sentenceManager.GetRandomSentence(AequorClient.INSTANCE.configManager.sentenceCategory));
        }

        private void ShowShanBay(object sender, RoutedEventArgs e)
        {
            AequorClient.INSTANCE.shanBay.Show();
        }

        private void SentenceCategoryChanged(object sender, SelectionChangedEventArgs e)
        {
            AequorClient.INSTANCE.configManager.sentenceCategory = SentenceCategory.SelectedIndex;
            AequorClient.INSTANCE.configManager.Save();
            SetSentence(AequorClient.INSTANCE.sentenceManager.GetRandomSentence(AequorClient.INSTANCE.configManager.sentenceCategory));
        }

        private void StartupChanged(object sender, RoutedEventArgs e)
        {
            if (Startup.IsChecked ?? false)
            {
                StartupUtil.EnableStartup();
            }
            else
            {
                StartupUtil.DisableStartup();
            }
        }

        private void ForceStudyChanged(object sender, RoutedEventArgs e)
        {
            AequorClient.INSTANCE.configManager.forceStudy = ForceStudy.IsChecked ?? false;
            AequorClient.INSTANCE.configManager.Save();
        }

        private void RestReminderChanged(object sender, RoutedEventArgs e)
        {
            AequorClient.INSTANCE.configManager.restReminder = RestReminder.IsChecked ?? false;
            AequorClient.INSTANCE.configManager.Save();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (AequorClient.INSTANCE.shanBay.Locked)
            {
                e.Cancel = true;
            }
            else
            {
                Environment.Exit(0);
            }
        }

    }

}
