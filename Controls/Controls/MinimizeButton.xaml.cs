using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Controls
{
    /// <summary>
    /// Interaction logic for MinimizeButton.xaml
    /// </summary>
    public partial class MinimizeButton : UserControl
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


        public MinimizeButton()
        {
            InitializeComponent();

            AnimationDuration = TimeSpan.FromSeconds(0.4);

            InitializeStoryMouseEnter();

            InitializeStoryMouseLeave();

            btnMinimize.Click += (source, e) => this.Click(source, e);
            btnMinimize.Click += (source, e) => OnClick(e);
        }

        private void InitializeStoryMouseLeave()
        {
            storyMouseLeave = new Storyboard();

            DoubleAnimation dbInnerRadius = new DoubleAnimation()
            {
                To = 1,
                Duration = AnimationDuration
            };

            ColorAnimation c1Animation = new ColorAnimation()
            {
                To = BaseColor,
                Duration = AnimationDuration,
                AutoReverse = false
            };

            var c2Animation = c1Animation.Clone();

            storyMouseLeave.Children.Add(dbInnerRadius);
            Storyboard.SetTarget(dbInnerRadius, this.regularPolygon);
            Storyboard.SetTargetProperty(dbInnerRadius, new PropertyPath("(ed:RegularPolygon.InnerRadius)"));

            storyMouseLeave.Children.Add(c1Animation);
            this.regularPolygon.RegisterName("regBrush", this.regularPolygon.Stroke);
            Storyboard.SetTargetName(c1Animation, "regBrush");
            Storyboard.SetTargetProperty(c1Animation, new PropertyPath(SolidColorBrush.ColorProperty));

            storyMouseLeave.Children.Add(c2Animation);
            this.ellipse.RegisterName("resizeBrush", this.ellipse.Stroke);
            Storyboard.SetTargetName(c2Animation, "resizeBrush");
            Storyboard.SetTargetProperty(c2Animation, new PropertyPath(SolidColorBrush.ColorProperty));
        }


        private void InitializeStoryMouseEnter()
        {
            storyMouseEnter = new Storyboard();

            DoubleAnimation dbInnerRadius = new DoubleAnimation()
            {                
                To = 0.30,
                Duration = AnimationDuration
            };

            ColorAnimation c1Animation = new ColorAnimation()
            {               
                To = TargetColor,
                Duration = AnimationDuration,
                AutoReverse = false
            };

            var c2Animation = c1Animation.Clone();

            storyMouseEnter.Children.Add(dbInnerRadius);
            Storyboard.SetTarget(dbInnerRadius, this.regularPolygon);
            Storyboard.SetTargetProperty(dbInnerRadius, new PropertyPath("(ed:RegularPolygon.InnerRadius)"));

            storyMouseEnter.Children.Add(c1Animation);
            this.regularPolygon.RegisterName("regBrush", this.regularPolygon.Stroke);
            Storyboard.SetTargetName(c1Animation, "regBrush");
            Storyboard.SetTargetProperty(c1Animation, new PropertyPath(SolidColorBrush.ColorProperty));

            storyMouseEnter.Children.Add(c2Animation);
            this.ellipse.RegisterName("resizeBrush", this.ellipse.Stroke);
            Storyboard.SetTargetName(c2Animation, "resizeBrush");
            Storyboard.SetTargetProperty(c2Animation, new PropertyPath(SolidColorBrush.ColorProperty));
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);


            if (sizeInfo.WidthChanged) this.Height = sizeInfo.NewSize.Width;
            else if (sizeInfo.HeightChanged) this.Width = sizeInfo.NewSize.Height;

            this.regularPolygon.Width = this.Width * 0.4;
            this.regularPolygon.Height = this.Height * 0.4;
        }

        protected override void OnMouseEnter(MouseEventArgs e)
        {
            base.OnMouseEnter(e);

            storyMouseEnter.Begin(this);
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseEnter(e);

            storyMouseLeave.Begin(this);
        }

        protected virtual void OnClick(RoutedEventArgs e)
        {
            var parentWindow = Window.GetWindow(this);
            if (parentWindow != null)
            {
                parentWindow.WindowState = System.Windows.WindowState.Minimized;
            }
        }
    }
}
