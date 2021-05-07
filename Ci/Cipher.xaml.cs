using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Ci
{
    /// <summary>
    /// Interaction logic for Cipher.xaml
    /// </summary>
    public partial class Cipher : Window
    {
        Dictionary<Key, Button> Azerty = new Dictionary<Key, Button>();
        TypeConverter converter = TypeDescriptor.GetConverter(typeof(Key));
        List<Button> allButtons;
        List<TextBlock> allLamps;
        
       

        public Cipher()
        {
            InitializeComponent();
            //var key = (KeyBinding)Convert.ChangeType($"{A}", typeof(KeyBinding));

            allButtons = new List<Button> { Abutton, BButton, CButton, DButton,
                EButton, FButton, GButton, HButton,
                IButton, JButton,KButton, LButton, MButton, NButton,
                OButton, PButton, QButton, Rbutton,
                SButton, Tbutton, Ubutton, VButton,
                WButton, XButton, Ybutton, ZButton};

            allLamps = new List<TextBlock> { ALamp, BLamp, CLamp, DLamp,
                ELamp, FLamp, GLamp, HLamp,
                ILamp, JLamp,KLamp, LLamp, MLamp, NLamp,
                OLamp, PLamp, QLamp, RLamp,
                SLamp, TLamp, ULamp, VLamp,
                WLamp, XLamp, YLamp, ZLamp  };

            BindKeysWithButtons();
            MessageBox.Show(Abutton.GetType().ToString());

        }

        // Maak de lampen branden
        // Een Method lamp laten branden
        // 
        private  void LightUpTheLamp(char letter)
        {
            // Gets the letter and light up one of the lamps 
            // First needs to reset the past lamp
            foreach (var item in allLamps) 
            {
                if(item.Background != Brushes.Transparent)
                    item.Background = Brushes.Transparent;
            }

            foreach (var lamp in allLamps)
            {
                if (letter.ToString().ToUpper() == lamp.Text)
                {
                    lamp.Background = Brushes.DarkOrange;
                }

            }


        }
        
      

        private async void  CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var keypressed = e.Parameter.ToString();
            foreach (var item in Azerty.Keys)
            {
                if (item.ToString() == keypressed)
                {
                    var button = Azerty[item];
                    button.Background = Brushes.YellowGreen;
                    await Task.Delay(500);
                    button.Background = Brushes.Transparent;
                
                }

            }

            LightUpTheLamp('p');
            LightUpTheLamp('o');

        }
        private void BindKeysWithButtons()
        {
            for(int i = 65; i < 91; i++)
            {
                Key key = (Key)converter.ConvertFromString($"{((char)i)}");
                var button = allButtons[i - 65];
                Azerty[key] = button;
            }

        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void LockRotors_Checked(object sender, RoutedEventArgs e)
        {

        }
    }
}
