using System;
using System.Collections.Generic;
using System.Text;

namespace EnigmaMachine
{

    class Keyboard
    {
        public char letter { get; set; }
        private char[] characters = new char[26];
        public char[] Characters
        {
            get { return characters; }
            set { characters = value; }
        }
        public Keyboard()
        {
            CharactersOnKeyboard();
        }
        public void CharactersOnKeyboard()
        {
            
            // All letters between  97 - 122 
            int pos = 97;
            for (int i = 0; i < characters.Length; i++)
            {
                int j = i + pos;
                characters[i] = ((char)j);
             
            }
                


        }
        public char getLetter() => letter;

        // User can press any letter
        // Search for that letter in array and if match store letter
        public void PickCharacter(char _letter )
        {
            
            //Check if _letter is empty
            string text = _letter.ToString();
            if (String.IsNullOrEmpty(text))
                throw new ArgumentNullException("Your input can not be empty");
            // zet de juiste key om naar een cHAR en vind dat in de array terug
            foreach (var le in characters)
            {
                
                if (le.Equals(_letter))
                {
                    letter = _letter;
                    

                }
                    

            }
                

        
        }

        //Send letter to rotor => convert letter to number 
        public int FromKeyBoardToFastRotor() =>  (int)letter - 97;

        //Receive num van Fast rotor ==> Convert to letter

        public char FromFastRotorToKeyBoard(int index) => (char)((char)index + 97);

    }
}
