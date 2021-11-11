using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace EnigmaMachine
{
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    class Reflector 
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    {
        // Connection:  26 positions => letters =>  13 connections
        // Randomly assigned ==> Connections stay the same for all 3  rotors
        private readonly Dictionary<int, int> positions = new Dictionary<int, int>();
        private List<int> r = new List<int>();
        private List<int> compareNumbers = new List<int>();
        private int lastItem;
        
        // Compare the values in the dictonary with the values i

        public Reflector()
        {

            InitialSettingReflectorPostions();
        }

        //Setting which wires are connected
        private void InitialSettingReflectorPostions()
        {
            Random rnd = new Random();

            for (int i = 0; i <= 26; i++)
                r.Add(i);
                

            
                

            int total = r.Count;
            int pos2;
            

            
            while (total >= 0)
            {
                int index = rnd.Next(0, total);
                int pos = DeleteElementFromList(index, total);
                
                total--;
                // Verwijder eerst num1

                if (total > 1)
                {
                    int index2 = rnd.Next(0, total);
                    pos2 = DeleteElementFromList(index2, total);
                    total--;

                }
                else
                    break;

                positions.Add(pos, pos2);

            }
    

        }

        private int DeleteElementFromList(int index, int total)
        {

            int pos = r[index];
            r.RemoveAt(index);
            return pos;

        }


        //Reflector acts like a mirror. Reflector state doesn't change
        public int ReflectorInAndOutput(int index)
        {
            // Get number from rotor3 and sends back number that is matched with other number
           // Numbers are matched in key-value pair ==> dictionary
            var item = 0;

            foreach (var k in positions)
            {
                var key = k.Key;
                var value = k.Value;
                if (index == key)
                    item = value;
                else if (index == value)
                    item = key;

            }
            return item;
        }


       

        public override bool Equals(object obj)
        {
            // Check for the same type 
            return base.Equals(obj);
        }

    }
}
