using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace VendingMachineProgram;

/// <summary>
///  This class simulates the behavior of a coin
///  
/// <para>Author - Maria Esteban</para> 
/// <para>Version - 1.3 (11-30-23)</para> 
/// <para>Since - 10-30-23</para>  
/// </summary>

public class Coin
{
    private readonly double value;
    private readonly String name;
    public double Value { get { return value; } }
    public String Name { get { return name; } }

    public static readonly Coin NICKEL = new(.05, "nickel");
    public static readonly Coin DIME = new(.10, "dime");
    public static readonly Coin QUARTER = new(.25, "quarter");
    public static readonly Coin DOLLAR = new(1.0, "dollar");

    /// <summary>
    /// Constructs a coin.
    /// </summary>
    /// <param name="value">the monetary value of the coin.</param>
    /// <param name="name">the name of the coin</param>
    public Coin(double value, String name)
    {
        this.value = value;
        this.name = name;
    }

    /// <summary>
    /// Returns the object as a string
    /// </summary>
    /// <returns>a string representing the object</returns>
    public override String ToString()
    {
        return name + " @ " + $"{value:F2}";
    }

    /// <summary>
    /// Checks wether or not two objects are equal
    /// </summary>
    /// <param name="obj">the object being compared to the current object</param>
    /// <returns>wether or not the objects are equal</returns>
    public override bool Equals(object? obj)
    {
        if (obj == null)
            return false;

        Coin aCoin = (Coin)obj;

        if (this.value == aCoin.Value && this.name == aCoin.Name)
            return true;
        else
            return false;
    }
}
