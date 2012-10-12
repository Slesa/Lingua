/* Copyright (c) 2009 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System;
using System.Diagnostics;
using System.Windows;

using Lingua;

namespace LinguaDemo
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            LinguaTrace.TraceSource.Listeners.Add(new LinguaTraceListener(Dispatcher, WriteTraceLine));

            App.CreateParser();
        }

        public App App
        {
            get { return (App)App.Current; }
        }

        public void WriteTraceLine(string text)
        {
            txtTrace.Text += text + Environment.NewLine;
        }

        void mnuFileExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        void btnEvaluate_Click(object sender, RoutedEventArgs e)
        {
            App.TerminalReader.Open(txtExpression.Text);
            Prolog.Clause result = (Prolog.Clause)App.Parser.Parse(App.TerminalReader);
            if (result != null)
            {
                //txtResult.Text = result.Value.ToString();
            }

            LinguaTrace.TraceSource.Flush();
        }

        void btnClear_Click(object sender, RoutedEventArgs e)
        {
            txtTrace.Text = null;
        }
    }
}
