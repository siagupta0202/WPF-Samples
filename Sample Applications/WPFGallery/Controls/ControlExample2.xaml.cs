using System;
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
using System.Windows.Automation;
using System.Windows.Automation.Provider;


namespace WPFGallery.Controls
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    [ContentProperty(nameof(ExampleContent))]
    public partial class ControlExample2 : UserControl
    {
        static ControlExample2()
        {
            CommandManager.RegisterClassCommandBinding(typeof(ControlExample2), new CommandBinding(ApplicationCommands.Copy, Copy_SourceCode));
        }

        public static readonly DependencyProperty HeaderTextProperty = DependencyProperty.Register(
            nameof(HeaderText),
            typeof(string),
            typeof(ControlExample2),
            new PropertyMetadata(null)
        );

        //public static readonly DependencyProperty NarratorTextProperty = DependencyProperty.Register(
        //    nameof(NarratorText),
        //    typeof(string),
        //    typeof(ControlExample2),
        //    new PropertyMetadata(null)
        //);

        public static readonly DependencyProperty ExampleContentProperty = DependencyProperty.Register(
            nameof(ExampleContent),
            typeof(object),
            typeof(ControlExample2),
            new PropertyMetadata(null)
        );

        public static readonly DependencyProperty XamlCodeProperty = DependencyProperty.Register(
            nameof(XamlCode),
            typeof(string),
            typeof(ControlExample2),
            new PropertyMetadata(null)
        );

        public static readonly DependencyProperty XamlCodeSourceProperty = DependencyProperty.Register(
            nameof(XamlCodeSource),
            typeof(Uri),
            typeof(ControlExample2),
            new PropertyMetadata(
                null,
                static (o, args) => ((ControlExample2)o).OnXamlCodeSourceChanged((Uri)args.NewValue)
            )
        );

        public static readonly DependencyProperty CsharpCodeProperty = DependencyProperty.Register(
            nameof(CsharpCode),
            typeof(string),
            typeof(ControlExample2),
            new PropertyMetadata(null)
        );

        public static readonly DependencyProperty CsharpCodeSourceProperty = DependencyProperty.Register(
            nameof(CsharpCodeSource),
            typeof(Uri),
            typeof(ControlExample2),
            new PropertyMetadata(
                null,
                static (o, args) => ((ControlExample2)o).OnCsharpCodeSourceChanged((Uri)args.NewValue)
            )
        );

        public string? HeaderText
        {
            get => (string)GetValue(HeaderTextProperty);
            set => SetValue(HeaderTextProperty, value);
        }

        //public string? NarratorText
        //{
        //    get => (string)GetValue(NarratorTextProperty);
        //    set => SetValue(NarratorTextProperty, value);
        //}

        public object? ExampleContent
        {
            get => GetValue(ExampleContentProperty);
            set => SetValue(ExampleContentProperty, value);
        }

        public string? XamlCode
        {
            get => (string)GetValue(XamlCodeProperty);
            set => SetValue(XamlCodeProperty, value);
        }

        public Uri? XamlCodeSource
        {
            get => (Uri)GetValue(XamlCodeSourceProperty);
            set => SetValue(XamlCodeSourceProperty, value);
        }

        public string? CsharpCode
        {
            get => (string)GetValue(CsharpCodeProperty);
            set => SetValue(CsharpCodeProperty, value);
        }

        public Uri? CsharpCodeSource
        {
            get => (Uri)GetValue(CsharpCodeSourceProperty);
            set => SetValue(CsharpCodeSourceProperty, value);
        }

        private void OnXamlCodeSourceChanged(Uri uri)
        {
            XamlCode = LoadResource(uri);
        }

        private void OnCsharpCodeSourceChanged(Uri uri)
        {
            CsharpCode = LoadResource(uri);
        }

        private static void Copy_SourceCode(object sender, RoutedEventArgs e)
        {
            if (sender is ControlExample2 ControlExample2)
            {
                if (!string.IsNullOrEmpty(ControlExample2.XamlCode))
                {
                    try
                    {
                        switch (((ExecutedRoutedEventArgs)e).Parameter.ToString())
                        {
                            case "Copy_XamlCode":
                                Clipboard.SetText(ControlExample2.XamlCode);
                                //NarratorText = "XAML code copied to clipboard";
                                break;
                            case "Copy_CsharpCode":
                                Clipboard.SetText(ControlExample2.CsharpCode);
                                break;
                            default:
                                throw new InvalidOperationException();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error copying to clipboard: " + ex.Message);
                    }
                }
            }
        }

        private static string LoadResource(Uri uri)
        {
            try
            {
                if (Application.GetResourceStream(uri) is not { } steamInfo)
                {
                    return String.Empty;
                }

                using StreamReader streamReader = new(steamInfo.Stream, Encoding.UTF8);

                return streamReader.ReadToEnd();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return e.ToString();
            }
        }

        private void CopyButton_Click(object sender, RoutedEventArgs e)
        {
            string textToNarrate = "Hello, this is a sample text to be narrated.";
            AnnounceText(textToNarrate);
        }

        //private void AnnounceText(string text)
        //{
        //    // Create an AutomationElement for the main window
        //    AutomationElement element = AutomationElement.FromHandle(new System.Windows.Interop.WindowInteropHelper(this).Handle);

        //    // Create an AutomationEventArgs with the required Message
        //    AutomationEventArgs args = new AutomationEventArgs(AutomationElement.AutomationFocusChangedEvent);
        //    AutomationInteropProvider.RaiseAutomationEvent(AutomationElement.AutomationFocusChangedEvent, element, args);

        //    // Set the live setting to assertive (or polite, depending on your preference)
        //    AutomationLiveSetting liveSetting = AutomationLiveSetting.Assertive;
        //    AutomationProperties.SetLiveSetting(element, liveSetting);

        //    // Set the announcement text
        //    AutomationProperties.SetName(element, text);
        //}

        //private void CopyButton_Click(object sender, RoutedEventArgs e)
        //{
        //    string textToNarrate = "Hello, this is a sample text to be narrated.";
        //    AnnounceText(textToNarrate);
        //}

        private void AnnounceText(string text)
        {
            Window parentWindow = Window.GetWindow(this);
            // Create an AutomationElement for the main window
            //AutomationElement element = AutomationElement.FromHandle(new System.Windows.Interop.WindowInteropHelper(parentWindow).Handle);
            var provider = AutomationInteropProvider.HostProviderFromHandle(new System.Windows.Interop.WindowInteropHelper(parentWindow).Handle);
            // Create an AutomationEventArgs with the required Message
            AutomationEventArgs args = new AutomationEventArgs(AutomationElement.AutomationFocusChangedEvent);
            AutomationInteropProvider.RaiseAutomationEvent(InvokePatternIdentifiers.InvokedEvent, provider, args);

            // Set the live setting to assertive (or polite, depending on your preference)
            AutomationLiveSetting liveSetting = AutomationLiveSetting.Assertive;
            AutomationProperties.SetLiveSetting(parentWindow, liveSetting);

            // Set the announcement text
            AutomationProperties.SetName(parentWindow, text);
        }

    }
}
