using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using MouseEventArgs = System.Windows.Input.MouseEventArgs;

namespace Controls
{
    /// <summary>
    /// Interaction logic for CloseButton.xaml
    /// </summary>
    public partial class CloseButton : System.Windows.Controls.UserControl
    {
        private TimeSpan AnimationDuration;
        private Storyboard storyMouseEnter;
        private Storyboard storyMouseLeave;

        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public EventHandler<RoutedEventArgs> Click = delegate(object sender, RoutedEventArgs args) { };

        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Bindable(true)]
        public Color BaseColor = Color.FromRgb(0, 173, 238);

        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Bindable(true)]
        public Color TargetColor = Color.FromRgb(67, 80, 162);

        public CloseButton()
        {
            InitializeComponent();

            multipeCanvas.RenderTransform = new RotateTransform(0);
            AnimationDuration = TimeSpan.FromSeconds(0.4);

            InitializeStoryMouseEnter();

            InitializeStoryMouseLeave();

            btnClose.Click += (source, e) => this.Click(source, e);
            btnClose.Click += (source, e) => OnClick(e);
        }

        private void InitializeStoryMouseEnter()
        {
            storyMouseEnter = new Storyboard();

            ColorAnimation ellipseColorAnimation = new ColorAnimation(BaseColor, TargetColor, AnimationDuration);
            ColorAnimation lineRTColorAnimation = ellipseColorAnimation.Clone();
            ColorAnimation lineLTColorAnimation = ellipseColorAnimation.Clone();


            this.ellipse.RegisterName("ellipseBrush", this.ellipse.Stroke);
            Storyboard.SetTargetName(ellipseColorAnimation, "ellipseBrush");
            Storyboard.SetTargetProperty(ellipseColorAnimation, new PropertyPath(SolidColorBrush.ColorProperty));

            this.rectLineLT.RegisterName("lineLTBrush", this.rectLineLT.Stroke);
            Storyboard.SetTargetName(lineLTColorAnimation, "lineLTBrush");
            Storyboard.SetTargetProperty(lineLTColorAnimation, new PropertyPath(SolidColorBrush.ColorProperty));

            this.rectLineRT.RegisterName("lineRTBrush", this.rectLineRT.Stroke);
            Storyboard.SetTargetName(lineRTColorAnimation, "lineRTBrush");
            Storyboard.SetTargetProperty(lineRTColorAnimation, new PropertyPath(SolidColorBrush.ColorProperty));


            storyMouseEnter.Children.Add(ellipseColorAnimation);
            storyMouseEnter.Children.Add(lineLTColorAnimation);
            storyMouseEnter.Children.Add(lineRTColorAnimation);
        }

        private void InitializeStoryMouseLeave()
        {
            storyMouseLeave = new Storyboard();

            ColorAnimation ellipseColorAnimation = new ColorAnimation(BaseColor, AnimationDuration);
            ColorAnimation lineRTColorAnimation = ellipseColorAnimation.Clone();
            ColorAnimation lineLTColorAnimation = ellipseColorAnimation.Clone();


            this.ellipse.RegisterName("ellipseBrush", this.ellipse.Stroke);
            Storyboard.SetTargetName(ellipseColorAnimation, "ellipseBrush");
            Storyboard.SetTargetProperty(ellipseColorAnimation, new PropertyPath(SolidColorBrush.ColorProperty));

            this.rectLineLT.RegisterName("lineLTBrush", this.rectLineLT.Stroke);
            Storyboard.SetTargetName(lineLTColorAnimation, "lineLTBrush");
            Storyboard.SetTargetProperty(lineLTColorAnimation, new PropertyPath(SolidColorBrush.ColorProperty));

            this.rectLineRT.RegisterName("lineRTBrush", this.rectLineRT.Stroke);
            Storyboard.SetTargetName(lineRTColorAnimation, "lineRTBrush");
            Storyboard.SetTargetProperty(lineRTColorAnimation, new PropertyPath(SolidColorBrush.ColorProperty));


            storyMouseLeave.Children.Add(ellipseColorAnimation);
            storyMouseLeave.Children.Add(lineLTColorAnimation);
            storyMouseLeave.Children.Add(lineRTColorAnimation);
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);


            if (sizeInfo.WidthChanged) this.Height = sizeInfo.NewSize.Width;
            else if (sizeInfo.HeightChanged) this.Width = sizeInfo.NewSize.Height;
        }

        protected override void OnMouseEnter(MouseEventArgs e)
        {
            base.OnMouseEnter(e);

            DoubleAnimation dbAscending = new DoubleAnimation(180, AnimationDuration);

            (this.multipeCanvas.RenderTransform as RotateTransform).BeginAnimation(RotateTransform.AngleProperty, dbAscending);

            storyMouseEnter.Begin(this);
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);


            DoubleAnimation dbAscending = new DoubleAnimation(0, AnimationDuration);

            (this.multipeCanvas.RenderTransform as RotateTransform).BeginAnimation(RotateTransform.AngleProperty, dbAscending);


            storyMouseLeave.Begin(this);
        }

        protected virtual void OnClick(RoutedEventArgs e)
        {
            var parentWindow = Window.GetWindow(this);
            if (parentWindow != null) parentWindow.Close();
        }

    }
}
