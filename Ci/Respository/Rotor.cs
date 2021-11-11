using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EnigmaMachine
{
    public class Rotor : IRotor
    {
        // Check the 
        static Dictionary<SpeedRotor, int> RotorCounter = new Dictionary<SpeedRotor, int>
        {
            {SpeedRotor.fast, 0},
            {SpeedRotor.medium, 0},
            {SpeedRotor.slow, 0}

        };
        SpeedRotor speed;
        Rotors rotor;

        private Stack myStack = new Stack();

        public static int[] ChangedPreviousRotorState = new int[26];

        private int[] initalState;

        public int[] InitialState
        {
            get { return initalState; }
            set { initalState = value; }
        }


        private int[] previousRotor;
        public int[] PreviousRotor
        {
            get { return previousRotor; }
            set { previousRotor = value; }
        }
        public int[] StateRotor = new int[26];
        int[] IRotor.StateRotor {
            get { return StateRotor; }
            set { StateRotor = value; }
        }
        int[,] IRotor.Wires {
            get { return Wires; }
            set { Wires = value; }
        }

        private int[,] Wires = new int[2, 26];

        private char letter;

        public char Letter
        {
            get { return letter; }
            set { letter = value; }
        }

        private int[] num = new int[26];

        private int target = 0;



        // Setting rotor 1 ==> from keyboard to rotor1 and back
        public Rotor(char[] PreviousRotor, char _letter)
        {
            letter = _letter;
            previousRotor = CharToIntArray(PreviousRotor);


            //Setting the state of the first rotor from the keyboard
            DetermineInitialStateRotor(letter.ToString());
        }

        private void DetermineInitialStateRotor(string startletter)
        {
            //
            char[] az = Enumerable.Range('a', 'z' - 'a' + 1).Select(i => (Char)i).ToArray();
            char[] temp = new char[2];
            int total = az.Length;
            int index = Array.IndexOf(az, Convert.ToChar(startletter));


            do
            {
                index = Array.IndexOf(az, Convert.ToChar(startletter));

                for (int i = 0; i < az.Length; i++)
                {
                    if (i == 0)
                    {
                        temp[0] = az[i];

                    }

                    else if (i == total)
                    {
                        temp[0] = az[i];
                        az[i] = temp[1];

                    }
                    else
                    {
                        // swap
                        (temp[0], temp[1]) = (temp[1], temp[0]);
                        temp[0] = az[i];
                        az[i] = temp[1];
                        temp[1] = ' ';

                    }
                }
                az[0] = temp[0];



            } while (index != 0);


            int[] newArray = new int[26];
            for (int i = 0; i < az.Length; i++)
            {
                newArray[i] = (int)az[i] - 97;

            }
            initalState = newArray;// Determine start position of rotor

            //Randomly connect the wires between current and previous rotor
            ConnectWiresRotor(newArray);

            StateRotor = initalState;


            initalState = new int[26];

            for (int i = 0; i < az.Length; i++)
            {
                initalState[i] = (int)az[i] - 97;

            }



        }
        // Setting initial state rotor ==> Rotor 2 and 3
        public Rotor(int[] PreviousRotor, int index)
        {
            previousRotor = PreviousRotor;
            initalState = new int[26];

            target = index;
            for (int i = 0; i < 26; i++)
                num[i] = i + 1;

            int N = num.Length;
            int[] B = FindElementRec(num, N);
            if (B.Length > 1)
            {

                int[] firstItemsInArray = B.Distinct().ToArray();
                RemoveLastItem(ref firstItemsInArray, firstItemsInArray.Length - 1);


                int lastItemIndex = firstItemsInArray[firstItemsInArray.Length - 1];
                initalState[0] = lastItemIndex;
                int i = 1;
                foreach (var item in myStack)
                {
                    initalState[i] = (int)item;
                    i++;
                }
                AddElementToArray(ref initalState, firstItemsInArray);

                var randomState = initalState;

                ConnectWiresRotor(randomState);

                StateRotor = initalState;

            }

        }


        

        // Send Message back from reflector  to Keyboard through rotors
        public int SendMessageforth(int index)
        {
            // Every number from previous is matched and replaced with the current rotor
            // And send to the next rotor
            for (int i = 0; i < Wires.GetLength(1); i++)
            {
                if (index == Wires[1, i])
                {
                    SetTypeMoveRotor();
                    return Wires[0, i];
                }

            }

            return 0;
         
            //  search for the number  
            // Get the index ==> Find the number in the other  
            // Sent number back

        }

     

        public int SendMessageBack(int index) 
        {

            //Every letter/Number form the previous rotor  is matched with every other letter from the current rotor
            // Look up th index from the previous rotor and sent the next number from the current rotor
            for (int i = 0; i < Wires.GetLength(1); i++)
            {
                if (index == Wires[0, i])
                {

                    return Wires[1, i];
                }

            }

            return 0;

        }
        private void MoveRotor()
        {
            //Move the current state of the  rotor up with one position
            for (int i = 0; i < 1; i++)
            {

                // take out the last element
                int temp = StateRotor[StateRotor.Length - 1];
                for (int j = StateRotor.Length - 1; j > 0; j--)
                {

                    // shift array elements towards right by one place
                    StateRotor[j] = StateRotor[j - 1];
                }
                StateRotor[0] = temp;
            }
           

        }
        public void UpdateState(int[] Staterotor)
        {
            
            for(int i  = 0; i < Wires.GetLength(0); i++)
            {
                for(int j = 0;  j  < Wires.GetLength(1); j++)
                {
                    if(i == 1)
                    {
                        Wires[i, j] = Staterotor[j];
                    }
                }
                
            }

        }

        public void SetTypeMoveRotor()
        {
            // Check what the tyepe of the rotor is.  add + 1 when message back
            // If the rotor has a full turn reset rotor
            switch (speed)
            {
                case SpeedRotor.fast:
                        MoveRotor();
                        RotorCounter[SpeedRotor.fast] += 1;
                        
                    break;
                case SpeedRotor.medium:
                    if(RotorCounter[SpeedRotor.fast] == 26)
                    {
                        RotorCounter[SpeedRotor.fast] = 0;
                        MoveRotor();
                        RotorCounter[SpeedRotor.medium] += 1;
                        
                    }
                    break;
                case SpeedRotor.slow:
                    if (RotorCounter[SpeedRotor.medium] == 26)
                    {
                        RotorCounter[SpeedRotor.medium] = 0;
                        MoveRotor();
                        RotorCounter[SpeedRotor.slow] += 1;
                       
                    }
                    break;
                default:
                    throw new ArgumentException("Invalid rotor");
                    
            }
        
            
        }

        // Display the initial state of the rotor
        public int ResetRotor()
        {
           
            // Set the current rotor state back to the start setting
            StateRotor = initalState;
            // Reset the current pos
            RotorCounter[speed] = 0;


            if(speed.Equals(SpeedRotor.fast))
                return Array.IndexOf(StateRotor, Math.Abs((int)letter) - 97);
            else
                return target;


        }


  
        //Connect the wires from previous and current rotor
        private void ConnectWiresRotor(int[] StateRotor)
        {
            this.StateRotor = ShuffleArray(StateRotor);
            
            // Set both the rotors in 2D array.
            for (int i = 0; i < this.StateRotor.Length; i++)
            {
                Wires[0, i] = this.StateRotor[i];
                Wires[1, i] = previousRotor[i];


            }
            
        }




        public int DisplayRotorState()
        {
            
            if (speed.ToString().Equals("fast"))
            {
               
                int posLetter = Math.Abs((int)letter) - 97;

                

                int displayNum = 0;
                for (int i = 0; i < StateRotor.Length; i++)
                {
                    Console.WriteLine(StateRotor[i]);

                    if (posLetter.Equals(StateRotor[i]))
                    {
                        displayNum = Array.IndexOf(StateRotor, posLetter);


                    }


                }
               
                return displayNum;
            }
            return StateRotor[0];                
            


        }



        private int[] FindElementRec(int[] A, int N)
        {
            if (N == 1)
                return A;
            else if (A[N - 1] == target)
                return A;
            
            myStack.Push(A[N - 1]);
            A[N - 1] = 0;
            return FindElementRec(A, N - 1);



        }

        // Determine random positions to match current rotor with previous 
        private int[] ShuffleArray(int[] StateRotor)
        {
            Random rnd = new Random();
            return StateRotor.OrderBy(x => rnd.Next()).ToArray();


        }

      

        private void RemoveLastItem(ref int[] lastItems, int index)
        {
            
           
            for (int a = index; a < lastItems.Length - 1; a++)
            {
                // moving elements downwards, to fill the gap at [index]
                lastItems[a] = lastItems[a + 1];
            }
            // finally, let's decrement Array's size by one
            Array.Resize(ref lastItems, lastItems.Length - 1);


            
        }

        private void AddElementToArray(ref int[] initialState, int[] firstItems)
        {
            int length = myStack.Count + 1;
            for (int i = length; i < initalState.Length; i++)
                initalState[i] = firstItems[i- length];
            
            
        
        }

        public void displayWires()
        {
            //Shows the current state of the rotor
            for (int i = 0; i < StateRotor.Length; i++)
            {

                Console.WriteLine(StateRotor[i]);

            }
            //Console.WriteLine();
            for (int i = 0; i < previousRotor.Length; i++)
            {

                Console.WriteLine(previousRotor[i]);

            }


        }
        private int[] CharToIntArray(char[] characters)
                => Enumerable.Range(0, characters.Length).Select(i => (int)i).ToArray();


       
        public void TypeOfRotor(Rotors rotor)
        {
            this.rotor = rotor;
        }

        public override string ToString()
        {
            return $"This is {rotor} " +
                $"and it's speed is {speed} " +
                $"and it's state is {RotorCounter[speed]}";

        }

        public void SpeedOfRotor(SpeedRotor _speed)
        {
            this.speed = _speed;

        }

        public int RotorState()
        {
            return StateRotor[0]; 
        }
    }

    
}
