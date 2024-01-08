using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace VendingMachineProgram;

/// <summary>
/// This class simulates the behavior of a coin box
///
/// <para>Author - Maria Esteban</para>
/// <para>Version - 1.6 (11-30-23)</para>
/// <para>Since - 10-30-23</para>
/// </summary>
public class CoinBox
{
    private List<Coin> box;



    /// <summary>
    /// Constructs a CoinBox object.
    /// </summary>
    public CoinBox()
    {
        box = new List<Coin>();
    }
    
    /// <summary>
    /// Adds a coin.
    /// </summary>
    /// <param name="c">the coin to add</param>
    public void AddCoin(Coin c)
    {
        box.Add(c);
    }

    /// <summary>
    /// Adds coins from one coinbox to another.
    /// </summary>
    /// <param name="other">the box of coins</param>
    public void AddCoins(CoinBox other)
    {
        box.AddRange(other.box);
    }

    /// <summary>
    /// Gets the value of all the coins.
    /// </summary>
    /// <returns>the total value of all the coins</returns>
    public double GetValue()
    {
        double total = 0;
        foreach (Coin c in box)
        {
            total += c.Value;
        }

        return total;
    }

    /// <summary>
    /// Enumaretes the coins in the coinbox
    /// </summary>
    /// <returns><an enumarator for the coinbox/returns>
    public MyEnumerator GetEnumerator()
    {
        return new MyEnumerator(this);
    }

    // Declare the enumerator class:  
    public class MyEnumerator
    {
        int nIndex;
        CoinBox collection;
        public MyEnumerator(CoinBox coll)
        {
            collection = coll;
            nIndex = -1;
        }

        public bool MoveNext()
        {
            nIndex++;
            return (nIndex < collection.box.Count);
        }

        public Coin Current => collection.box[nIndex];
    }
    /// <summary>
    /// Removes a single coin from the coinbox
    /// </summary>
    /// <param name="c">the coin to be removed</param>
    public void RemoveCoin(Coin c)
    {
        box.Remove(c);
    }


    /// <summary>
    /// Removes all the coins.
    /// </summary>
    public void RemoveAllCoins()
    {
        box.Clear();
    }
}