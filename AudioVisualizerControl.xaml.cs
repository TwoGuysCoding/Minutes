using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.VisualStyles;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Minutes
{
    /// <summary>
    /// Interaction logic for AudioVisualizerControl.xaml
    /// </summary>
    public partial class AudioVisualizerControl : UserControl
    {
        public AudioVisualizerControl()
        {
            InitializeComponent();
            //DataContext = this;
        }


        public double[] AudioLevelsProperty
        {
            get => (double[])GetValue(AudioLevelsPropertyProperty);
            set => SetValue(AudioLevelsPropertyProperty, value);
        }

        public static readonly DependencyProperty AudioLevelsPropertyProperty =
            DependencyProperty.Register(nameof(AudioLevelsProperty), typeof(double[]), typeof(AudioVisualizerControl), new PropertyMetadata(null, OnAudioLevelsChanged));

        public double MaxHeightValue
        {
            get => (double)GetValue(MaxHeightValueProperty);
            set => SetValue(MaxHeightValueProperty, value);
        }

        public static readonly DependencyProperty MaxHeightValueProperty =
            DependencyProperty.Register(nameof(MaxHeightValue), typeof(double), typeof(AudioVisualizerControl), new PropertyMetadata(100.0));

        public double BarWidth
        {
            get => (double)GetValue(BarWidthProperty);
            set => SetValue(BarWidthProperty, value);
        }

        public static readonly DependencyProperty BarWidthProperty =
            DependencyProperty.Register(nameof(BarWidth), typeof(double), typeof(AudioVisualizerControl), new PropertyMetadata(3.0));

        public double BarMargin
        {
            get => (double)GetValue(BarMarginProperty);
            set => SetValue(BarMarginProperty, value);
        }

        public static readonly DependencyProperty BarMarginProperty =
            DependencyProperty.Register(nameof(BarMargin), typeof(double), typeof(AudioVisualizerControl), new PropertyMetadata(1.0));

        public double BarCornerRadius
        {
            get => (double)GetValue(BarCornerRadiusProperty);
            set => SetValue(BarCornerRadiusProperty, value);
        }

        public static readonly DependencyProperty BarCornerRadiusProperty =
            DependencyProperty.Register(nameof(BarCornerRadius), typeof(double), typeof(AudioVisualizerControl), new PropertyMetadata(0.0));


        private static void OnAudioLevelsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (AudioVisualizerControl)d;
            var audioLevels = (double[])e.NewValue;
            var scaledAudioLevels = audioLevels.Select(level => level * control.MaxHeightValue).ToArray();
            control.AudioLevels.ItemsSource = scaledAudioLevels;
        }
    }
}
