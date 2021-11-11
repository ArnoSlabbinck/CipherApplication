using System;
using System.Collections.Generic;
using System.Text;

namespace EnigmaMachine
{
    public enum Rotors { Rotor1, Rotor2, Rotor3, Rotor4, Rotor5 }
    public enum SpeedRotor { fast, medium, slow };
    public interface IRotor
    {
        public void TypeOfRotor(Rotors rotor);
        public void SpeedOfRotor(SpeedRotor speed);
        public int[] StateRotor { get;  set; }

        public int[,] Wires { get;  set; }

       
    }
}
