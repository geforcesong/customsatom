using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.IO;
using Telerik.Windows.Controls;
using System.Collections;

namespace ProTemplate.Utility
{
    public class CommonUIFunction
    {
        public static void ShowMessageBox(string msg)
        {
            MessageBox.Show(msg, SystemConfiguration.Instance.ApplicationName, MessageBoxButton.OK);
        }

        public static MessageBoxResult ShowConfirm(string msg)
        {
            return MessageBox.Show(msg, SystemConfiguration.Instance.ApplicationName, MessageBoxButton.OKCancel);
        }

        public static void ShowConfirmYesNo(string msg, EventHandler<Telerik.Windows.Controls.WindowClosedEventArgs> callback)
        {
            DialogParameters dp = new DialogParameters();
            dp.Header = "CustomAtom";
            dp.Content = msg;
            dp.Closed = new EventHandler<WindowClosedEventArgs>(callback);
            dp.OkButtonContent = "是";
            dp.CancelButtonContent = "否";
            RadWindow.Confirm(dp);
        }

        public static string GetExceptionUserMessage(Exception  exp)
        {
            if (exp == null || string.IsNullOrEmpty(exp.Message))
                return "";
            else
            {
                int i = exp.Message.IndexOf("@@");
                if (i >= 0)
                    return exp.Message.Substring(i+2);
                else
                    return "";
            }
        }

        public static bool VerifyTextBox(TextBox tb)
        {
            System.Windows.Data.BindingExpression tbProperty = tb.GetBindingExpression(TextBox.TextProperty);
            tbProperty.UpdateSource();

            if (Validation.GetHasError(tb))
                return false;
            return true;
        }

        public static bool VerifyPasswordBox(PasswordBox tb)
        {
            System.Windows.Data.BindingExpression tbProperty = tb.GetBindingExpression(PasswordBox.PasswordProperty);
            tbProperty.UpdateSource();

            if (Validation.GetHasError(tb))
                return false;
            return true;
        }

        public static void SetApplcationBusyIndicator(bool isBusy, string busyText="")
        {
            App app = (App)App.Current;
            if (app.RootProjectContentFrame != null)
            {
                app.RootProjectContentFrame.IsApplicationBusy = isBusy;
                if (isBusy)
                    app.RootProjectContentFrame.ApplicationBusyText = busyText;
            }
        }

        public static void ShowMessageText(Border parent, string text, bool isError = false)
        {
            if (parent == null || string.IsNullOrEmpty(text))
                return;
            TextBlock tb = new TextBlock();
            tb.Text = text;
            if (isError)
                tb.Style = App.Current.Resources["MessageErrorTextBlock"] as Style;
            else
                tb.Style = App.Current.Resources["MessageTextBlock"] as Style;
            parent.Child = tb;
        }

        public static void HideMessageText(Border parent)
        {
            if (parent != null)
                parent.Child = null;
        }

        public static void ExportToFile(ExportFileType extType, Telerik.Windows.Controls.RadGridView grid, bool IsExportAll = false)
        {
            if (IsExportAll)
            {
                if (grid == null)
                {
                    ShowMessageBox(MessageTexts.MSG014);
                    return;
                }
            }
            else
            {
                if (grid == null || grid.SelectedItems == null || grid.SelectedItems.Count == 0)
                {
                    ShowMessageBox(MessageTexts.MSG014);
                    return;
                }
            }
            string extension = "";
            Telerik.Windows.Controls.ExportFormat format = Telerik.Windows.Controls.ExportFormat.Html;
            if (extType == ExportFileType.Excel)
                extension = "xls";
            else
                extension = "doc";
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.DefaultExt = extension;
            dialog.Filter = String.Format("{1} files (*.{0})|*.{0}|All files (*.*)|*.*", extension, extType.ToString());
            dialog.FilterIndex = 1;

            if (dialog.ShowDialog() == true)
            {
                using (Stream stream = dialog.OpenFile())
                {
                    Telerik.Windows.Controls.GridViewExportOptions exportOptions = new Telerik.Windows.Controls.GridViewExportOptions();
                    exportOptions.Format = format;
                    exportOptions.ShowColumnFooters = true;
                    exportOptions.ShowColumnHeaders = true;
                    exportOptions.ShowGroupFooters = true;
                    exportOptions.Encoding = System.Text.Encoding.Unicode;
                    if (IsExportAll)
                        exportOptions.Items = (IEnumerable)grid.ItemsSource;
                    else
                        exportOptions.Items = grid.SelectedItems;

                    grid.Export(stream, exportOptions);
                }
            }
        }
    }
}
