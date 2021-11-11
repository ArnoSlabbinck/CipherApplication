using System;
using System.Collections.Generic;
using System.Text;

namespace EnigmaMachine
{
    public enum Rotors { Rotor1, Rotor2, Rotor3, Rotor4, Rotor5 }
    public enum SpeedRotor { fast, medium, slow };
    public interface IRotor
    {
        void TypeOfRotor(Rotors rotor);
        void SpeedOfRotor(SpeedRotor speed);
        int[] StateRotor { get;  set; }

        int[,] Wires { get;  set; }

       
    }
}
