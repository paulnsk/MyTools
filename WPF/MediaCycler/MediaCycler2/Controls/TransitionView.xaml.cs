using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace MediaCycler2.Controls
{

    /// <summary>
    /// Control capable of displaying one piece of content at a time. Use <see cref="CurrentFrame"/> property to set the content.
    /// When current content is not empty, a transition effect occurs while loading the new content.
    /// </summary>
    public partial class TransitionView : Canvas
    {
        //todo to settings
        private readonly Duration _animationDuration = new Duration(TimeSpan.FromSeconds(1));



        public TransitionView()
        {
            InitializeComponent();
            SizeChanged += TransitionControl_SizeChanged;
        }

        private bool _isBusyAnimating;

        private void FitChild(FrameworkElement child)
        {
        //    if (_isBusyAnimating)
        //    {
        //        //todo schedule resize to after animation ends
        //        return;
        //    }
        //    if (Children.Count < 1) return;

        //    //todo думать как ресайзить картинки. Хотя хуле думать. Засунуть картинку в некий ПикчаКонтролъ и пусть он ее ресайзит правильно. А мы его просто растянем

        //    var child = (FrameworkElement)Children[0];

            child.Width = ActualWidth;
            child.Height = ActualHeight;           

        }

        private void TransitionControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            foreach (FrameworkElement child in Children)
            {
                FitChild(child);
            }            
        }

        private DoubleAnimation CreateDoubleAnimation(double from, double to, EventHandler completedEventHandler)
        {
            DoubleAnimation doubleAnimation = new DoubleAnimation(from, to, _animationDuration);
            if (completedEventHandler != null)
            {
                doubleAnimation.Completed += completedEventHandler;
            }
            return doubleAnimation;
        }

        private void AddChild(FrameworkElement child)
        {
            Children.Add(child);
            FitChild(child);
        }

        private void AnimateReplaceFrame(FrameworkElement newFrame)
        {
            _isBusyAnimating = true;

            if (Children.Count == 0)
            {
                //Adding fake old frame if there is none present
                AddChild(new Grid());
            }

            AddChild(newFrame);

            var oldFrame = (FrameworkElement)Children[0];

            double leftStart = GetLeft(oldFrame);
            SetLeft(newFrame, leftStart - ActualWidth);            
            if (double.IsNaN(leftStart))
            {
                leftStart = 0;
            }

            IsHitTestVisible = false;
                        
            var easing = new QuadraticEase { EasingMode = EasingMode.EaseInOut };

            var oldFrameOutAnimation = new DoubleAnimation(leftStart, leftStart + ActualWidth, _animationDuration)
            {
                EasingFunction = easing
            };
            //CreateDoubleAnimation(leftStart, leftStart + Width, null);
            var newFrameInAnimation = new DoubleAnimation(leftStart - ActualWidth, leftStart, _animationDuration)
            {
                EasingFunction = easing
            };
            newFrameInAnimation.Completed += (s, e) =>
            {
                IsHitTestVisible = true;
                Children.Remove(oldFrame);
                _isBusyAnimating = false;
            };

            oldFrame.BeginAnimation(LeftProperty, oldFrameOutAnimation);
            newFrame.BeginAnimation(LeftProperty, newFrameInAnimation);
        }


        public FrameworkElement CurrentFrame
        {
            get { return (FrameworkElement)GetValue(CurrentFrameProperty); }
            set 
            {
                if (Children.Contains(value)) { return; }
                if (_isBusyAnimating) return;
                AnimateReplaceFrame(value);
                //Children.Clear();
                //Children.Add(value);
                SetValue(CurrentFrameProperty, value);
                //FitChild();
            }
        }

        
        public static readonly DependencyProperty CurrentFrameProperty =
            DependencyProperty.Register(nameof(CurrentFrameProperty), typeof(FrameworkElement), typeof(TransitionView), new PropertyMetadata(null));


    }
}
