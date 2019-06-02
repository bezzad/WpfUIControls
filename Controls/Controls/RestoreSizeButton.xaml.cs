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
    /// Interaction logic for RestoreSizeButton.xaml
    /// </summary>
    public partial class RestoreSizeButton : UserControl
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

        /// <summary>
        /// Width of rectangle is 40% of this UserControl Width
        /// </summary>
        private double RectangleWidth
        {
            get { return this.Width * 0.40; }
            set
            {
                this.Height = this.Width = (value * 2.5);
                this.rectangle.Width = value;
            }
        }

        /// <summary>
        /// Height of rectangle is 30% of this UserControl Height
        /// </summary>
        private double RectangleHeight
        {
            get { return this.Height * 0.30; }
            set
            {
                this.Height = this.Width = value * 100 / 30;
                this.rectangle.Height = value;
            }
        }


        public RestoreSizeButton()
        {
            this.InitializeComponent();

            AnimationDuration = TimeSpan.FromSeconds(0.4);

            InitializeStoryMouseEnter();

            InitializeStoryMouseLeave();

            btnRestoreSize.Click += (source, e) => this.Click(source, e);
            btnRestoreSize.Click += (source, e) => OnClick(e);
        }

        private void InitializeStoryMouseLeave()
        {
            storyMouseLeave = new Storyboard();

            DoubleAnimation dbWidth = new DoubleAnimation()
            {
                To = RectangleWidth,
                Duration = AnimationDuration
            };

            DoubleAnimation dbHeight = new DoubleAnimation()
            {
                To = RectangleHeight,
                Duration = AnimationDuration
            };

            ColorAnimation c1Animation = new ColorAnimation()
            {
                To = BaseColor,
                Duration = AnimationDuration,
                AutoReverse = false
            };

            var c2Animation = c1Animation.Clone();

            storyMouseLeave.Children.Add(dbWidth);
            Storyboard.SetTarget(dbWidth, this.rectangle);
            Storyboard.SetTargetProperty(dbWidth, new PropertyPath(MediaElement.WidthProperty));

            storyMouseLeave.Children.Add(dbHeight);
            Storyboard.SetTarget(dbHeight, this.rectangle);
            Storyboard.SetTargetProperty(dbHeight, new PropertyPath(MediaElement.HeightProperty));
            Storyboard.SetTargetName(c1Animation, "rectBrush");

            storyMouseLeave.Children.Add(c1Animation);
            this.rectangle.RegisterName("rectBrush", this.rectangle.Stroke);
            Storyboard.SetTargetName(c2Animation, "rectBrush");
            Storyboard.SetTargetProperty(c1Animation, new PropertyPath(SolidColorBrush.ColorProperty));

            storyMouseLeave.Children.Add(c2Animation);
            this.ellipse.RegisterName("resizeBrush", this.ellipse.Stroke);
            Storyboard.SetTargetName(c2Animation, "resizeBrush");
            Storyboard.SetTargetProperty(c2Animation, new PropertyPath(SolidColorBrush.ColorProperty));
        }

        private void InitializeStoryMouseEnter()
        {
            this.rectangle.Margin = new Thickness(7, 8.25, 7, 8.25);

            //  This Control is a Square Rectangle with a Oval Circle in the Center of this shape
            //   by Radius equal this Square Diagonal.
            //
            //  In animate do not rectangle overlapping by oval circle, 
            //  so must be have limit a for rectangle
            //
            //  rectangle (a, b) ---(Animate)---> rectangle(W, H)
            //  
            //  a -> W    &    b -> H
            //
            //  H = ?
            //  W = ?
            //
            //       ____Width:W_____
            //      |\              /| 
            //      | \            / |  
            //      |  \          /  |
            //      |   \R       /   |
            //      |    \______/    | 
            //      |    |\    /|    | H
            //      |    | \r / |    | e
            //      |____|__\/  |    | i : H
            //      |    |  /\  |b   | g 
            //      |    | /  \ |    | h
            //      |    |/____\|    | t
            //      |    /   a  \    |
            //      |   /        \   |
            //      |  /          \  |
            //      | /            \ |
            //      |/______________\|           
            //
            //  a = RectangleWidth
            //  b = RectangleHeight
            //
            //  r/R = (a/2)/(W/2) = (b/2)/(H/2)
            // 
            //  r = √(a^2 + b^2)  
            //  
            //  R  = √(H^2 + W^2) = Diagonal of this Square = this.Height/2 = this.Width/2
            //                             __________________________
            //                            |                          |
            //  ==> W = 2*R*a / r    ==>  | W = 2*R*a / √(a^2 + b^2) |
            //                            |__________________________|
            //  &
            //                             __________________________
            //                            |                          |
            //  ==> H = 2*R*b / r    ==>  | H = 2*R*b / √(a^2 + b^2) |
            //                            |__________________________|
            //
            int a = (int)RectangleWidth;
            int b = (int)RectangleHeight;
            int R = (int)this.Height / 2;
            double W = (int)(2 * R * a / Math.Sqrt(a * a + b * b));
            double H = (int)(2 * R * b / Math.Sqrt(a * a + b * b));
            W -= W * 0.2;
            H -= H * 0.2;

            storyMouseEnter = new Storyboard();

            DoubleAnimation dbWidth = new DoubleAnimation()
            {                
                To = W,
                Duration = AnimationDuration
            };

            DoubleAnimation dbHeight = new DoubleAnimation()
            {                
                To = H,
                Duration = AnimationDuration
            };

            ColorAnimation c1Animation = new ColorAnimation()
            {               
                To = TargetColor,
                Duration = AnimationDuration,
                AutoReverse = false
            };

            var c2Animation = c1Animation.Clone();

            storyMouseEnter.Children.Add(dbWidth);
            Storyboard.SetTarget(dbWidth, this.rectangle);
            Storyboard.SetTargetProperty(dbWidth, new PropertyPath(MediaElement.WidthProperty));

            storyMouseEnter.Children.Add(dbHeight);
            Storyboard.SetTarget(dbHeight, this.rectangle);
            Storyboard.SetTargetProperty(dbHeight, new PropertyPath(MediaElement.HeightProperty));
            Storyboard.SetTargetName(c1Animation, "rectBrush");

            storyMouseEnter.Children.Add(c1Animation);
            this.rectangle.RegisterName("rectBrush", this.rectangle.Stroke);
            Storyboard.SetTargetName(c2Animation, "rectBrush");
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

            this.rectangle.Width = RectangleWidth;
            this.rectangle.Height = RectangleHeight;
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
                if (parentWindow.WindowState == System.Windows.WindowState.Normal)
                {
                    parentWindow.WindowState = System.Windows.WindowState.Maximized;
                }
                else
                {
                    parentWindow.WindowState = System.Windows.WindowState.Normal;
                }
            }
        }
    }


}
