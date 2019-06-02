using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Numerics;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Controls
{
    /// <summary>
    /// Interaction logic for DvTextBox.xaml
    /// </summary>
    [ProvideProperty("DvTextBox", typeof(TextBox))]
    public partial class DvTextBox : TextBox
    {  
        public DvTextBox()
        {
            this.InitializeComponent();

            this.PreviewTextInput += DvTextBox_PreviewTextInput;

            this.TextForeColor = Brushes.Black;
            this.DefaultValueColor = Brushes.Gray;
            this.DefaultValue = "Default Value";
            this.Text = this.DefaultValue;

            beepSound = System.Media.SystemSounds.Beep;
        }

        #region Members
        
        // just a little sound effect for wrong key pressed
        System.Media.SystemSound beepSound;

        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Description("Default text, when text box is empty this value be superseded."), Category("Appearance")]
        [DefaultValue("Default Value")]
        public string DefaultValue
        {
            set
            {
                if (this.Text == this.defaultValue || this.Text == value || this.Text == string.Empty)
                {
                    this.Text = value;
                    this.Foreground = this.DefaultValueColor;
                }
                this.defaultValue = value;
            }
            get { return defaultValue; }
        }
        private string defaultValue;

        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Description("Default value text's fore color."), Category("Appearance")]
        public SolidColorBrush DefaultValueColor
        {
            set
            {
                if (this.Foreground == this.defaultValueColor || this.Text == string.Empty || this.Text == this.DefaultValue)
                {
                    this.Foreground = value;
                }
                defaultValueColor = value;
            }
            get { return this.defaultValueColor; }
        }
        private SolidColorBrush defaultValueColor;


        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Description("Main text's fore color."), Category("Appearance")]
        [DefaultValue(typeof(SolidColorBrush), "Brushes.Gray")]
        public SolidColorBrush TextForeColor { get; set; }


        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Description("Is Numerical TextBox? if value is true then just typed numbers, and if false then typed any chars."), Category("Behavior")]
        [DefaultValue(false)]
        public bool IsNumerical
        {
            get { return isNumerical; }
            set
            {
                isNumerical = value;
                if (!value)
                {
                    this.ThousandsSplitter = false;
                }
            }
        }
        private bool isNumerical;

        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Description("Show Thousands Splitter in TextBox? if value is true then split any 3 numerical digits by char ',' .\nNote: IsNumerical must be 'true' for runes this behavior."), Category("Behavior")]
        [DisplayName("Thousands Splitter"), DefaultValue(false)]
        public bool ThousandsSplitter
        {
            get { return thousandsSplitter; }
            set
            {
                thousandsSplitter = value;
                if (value)
                {
                    IsNumerical = true;
                }
            }
        }
        private bool thousandsSplitter;        

        #endregion

        #region PreInitialized Events

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            //
            if (this.Text == this.DefaultValue)
            {
                // Clean default text
                this.Text = string.Empty;
                this.Foreground = this.TextForeColor;
            }
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);
            //
            if (this.Text == string.Empty)
            {
                // Set to default text
                this.Foreground = this.DefaultValueColor;
                this.Text = this.DefaultValue;
            }
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            base.OnPreviewKeyDown(e);
            //
            if (this.Text.Equals(this.DefaultValue, StringComparison.OrdinalIgnoreCase))
            {
                // Clean default text
                this.Text = string.Empty;
                this.Foreground = this.TextForeColor;
            }

        }

        protected override void OnPreviewKeyUp(KeyEventArgs e)
        {
            base.OnPreviewKeyUp(e);

            //
            if (this.Text == string.Empty)
            {
                // Set to default text
                this.Foreground = this.DefaultValueColor;
                this.Text = this.DefaultValue;
            }

            if (ThousandsSplitter)
            {
                int indexSelectionBuffer = this.SelectionStart;
                if (!string.IsNullOrEmpty(this.Text) && e.Key != Key.Left && e.Key != Key.Right)
                {
                    BigInteger valueBefore;
                    // Parse currency value using en-GB culture. 
                    // value = "�1,097.63";
                    // Displays:  
                    //       Converted '�1,097.63' to 1097.63
                    var style = NumberStyles.Number | NumberStyles.AllowCurrencySymbol;
                    var culture = CultureInfo.CreateSpecificCulture("en-US");
                    if (BigInteger.TryParse(this.Text, style, culture, out valueBefore))
                    {
                        this.Text = String.Format(culture, "{0:N0}", valueBefore);
                        if (e.Key != Key.Delete && e.Key != Key.Back) this.Select(this.Text.Length, 0);
                        else this.Select(indexSelectionBuffer, 0);
                    }
                }
            }
        }

        private void DvTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsNumber(e.Text, e.Text.Length - 1) && this.IsNumerical)
            {
                e.Handled = true;
                beepSound.Play();
            }
            if (((e.Text).ToCharArray()[e.Text.Length - 1] == '.') ||
                ((e.Text).ToCharArray()[e.Text.Length - 1] == ','))
            {
                e.Handled = true;
                beepSound.Play();
                if (!(((TextBox)sender).Text.Contains(".")))
                {
                    if (((TextBox)sender).Text.Length == 0)
                    {
                        ((TextBox)sender).Text = "0.";
                        ((TextBox)sender).CaretIndex = ((TextBox)sender).Text.Length;
                    }
                    else
                    {
                        ((TextBox)sender).Text += ".";
                        ((TextBox)sender).CaretIndex = ((TextBox)sender).Text.Length;
                    }
                }
            }

            if ((e.Text).ToCharArray()[e.Text.Length - 1] == '-' &
                !((TextBox)sender).Text.Contains("-"))
            {
                e.Handled = true;

                beepSound.Play();

                ((TextBox)sender).Text = "-" + ((TextBox)sender).Text;
                ((TextBox)sender).CaretIndex = ((TextBox)sender).Text.Length;
            }

            if ((e.Text).ToCharArray()[e.Text.Length - 1] == '+' &
                ((TextBox)sender).Text.Contains("-"))
            {
                e.Handled = true;

                beepSound.Play();

                ((TextBox)sender).Text = ((TextBox)sender).Text.Substring(1);
                ((TextBox)sender).CaretIndex = ((TextBox)sender).Text.Length;
            }
        }

        #endregion
    }
}