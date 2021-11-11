using EnigmaMachine;
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
        List<String> EncryptedMessage = new List<string>();
        List<Button> allButtons;
        List<Border> allLamps;

        private static Reflector reflector = new Reflector();
        private static EnigmaMachine.Keyboard keyBoard = new EnigmaMachine.Keyboard();
        private static Rotor rotor1;
        private static Rotor rotor2;
        private static Rotor rotor3;



        public Cipher()
        {
            InitializeComponent();
            //var key = (KeyBinding)Convert.ChangeType($"{A}", typeof(KeyBinding));
            SetRotorsState();
            allButtons = new List<Button> { Abutton, BButton, CButton, DButton,
                EButton, FButton, GButton, HButton,
                IButton, JButton,KButton, LButton, MButton, NButton,
                OButton, PButton, QButton, Rbutton,
                SButton, Tbutton, Ubutton, VButton,
                WButton, XButton, Ybutton, ZButton, Spacebutton};

            allLamps = new List<Border> { ALamp, BLamp, CLamp, DLamp,
                ELamp, FLamp, GLamp, HLamp,
                ILamp, JLamp,KLamp, LLamp, MLamp, NLamp,
                OLamp, PLamp, QLamp, RLamp,
                SLamp, TLamp, ULamp, VLamp,
                WLamp, XLamp, YLamp, ZLamp  };

            BindKeysWithButtons();
            

        }

   
        // Maak de lampen branden
        // Een Method lamp laten branden
        // 
        private  void LightUpTheLamp(char letter)
        {
            // Gets the letter and light up one of the lamps 
            // First needs to reset the past lamp
            ALamp.Background = Brushes.DarkOrange;
            foreach (var item in allLamps) 
            {
                if(item.Background != Brushes.Transparent)
                    item.Background = Brushes.Transparent;
            }
            
            foreach (var lamp in allLamps)
            {

                if (letter == lamp.Name[0])
                {
                    lamp.Background = Brushes.DarkOrange;
                    Message.Document.Blocks.Clear();
                    string encryptedLetter = letter.ToString();
                    EncryptedMessage.Add(encryptedLetter);
                    string message = string.Empty;
                    foreach(var le in EncryptedMessage)
                    {
                        message += le.ToLower();
                    }
                    Message.Document.Blocks.Add(new Paragraph(new Run(message)));
                }
                

            }


        }
        
      

        private async void  CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var keypressed = e.Parameter.ToString();
            var key = Convert.ToChar(keypressed);
            if(string.IsNullOrWhiteSpace(keypressed))
            {
                Spacebutton.Background = Brushes.YellowGreen;
                await Task.Delay(500);
                Spacebutton.Background = Brushes.Transparent;
                AddSpaceToMessage(keypressed);

            }
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
            var encryptedLetter = GetEncodedLetter(key);
            encryptedLetter = Convert.ToChar(encryptedLetter.ToString().ToUpper());
            LightUpTheLamp(encryptedLetter);


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
        private void AddSpaceToMessage(string space)
        {
            Message.Document.Blocks.Clear();
            EncryptedMessage.Add(space);
            string message = string.Empty;
            foreach (var le in EncryptedMessage)
            {
                message += le.ToLower();
            }
            Message.Document.Blocks.Add(new Paragraph(new Run(message)));
        }
        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void LockRotors_Checked(object sender, RoutedEventArgs e)
        {

        }

      
        public static char GetEncodedLetter(char letter)
        {
            letter = Convert.ToChar(letter.ToString().ToLower());

            keyBoard.PickCharacter(letter);
            var lett = (int)keyBoard.getLetter() - 97;
            //Convert back to number
            var number1 = rotor1.SendMessageforth(keyBoard.FromKeyBoardToFastRotor());
            var rotorState = rotor1.StateRotor;
            rotor2.UpdateState(rotorState);
            var number2 = rotor2.SendMessageforth(number1);
            rotorState = rotor2.StateRotor;
            rotor3.UpdateState(rotorState);


            var number3 = rotor3.SendMessageforth(number2);

            var refl = reflector.ReflectorInAndOutput(number3);

            number3 = rotor3.SendMessageBack(refl);
            number2 = rotor2.SendMessageBack(number3);

            number1 = rotor1.SendMessageBack(number2);


            char le = keyBoard.FromFastRotorToKeyBoard(number1);

            return le;



        }

        public static void SetRotorsState()
        {
            rotor1 = new Rotor(keyBoard.Characters, 'm');
            rotor1.SpeedOfRotor(SpeedRotor.fast);
            rotor2 = new Rotor(rotor1.InitialState, 20);
            rotor2.SpeedOfRotor(SpeedRotor.medium);
            rotor3 = new Rotor(rotor2.InitialState, 14);
            rotor3.SpeedOfRotor(SpeedRotor.slow);

        }
        public static void ResetEnigmaMachine()
        {
            rotor1.ResetRotor();
            rotor2.ResetRotor();
            rotor3.ResetRotor();

        }


        
    }
}
