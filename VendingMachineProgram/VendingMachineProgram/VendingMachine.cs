using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;
namespace VendingMachineProgram;

/// <summary>
/// This class simulates the behavior of a coin box
///
/// <para>Author - Maria Esteban</para>
/// <para>Version - 1.6 (11-30-23)</para>
/// <para>Since - 10-30-23</para>
/// </summary>
/// 
public class VendingMachine
{
    internal List<Product> products;
    public CoinBox coins = new();  // main coin box
    public CoinBox currentCoins; // temp for inserted coins
    public CoinBox changeBox; // change box

    private string password = "admin";
    public string Password { get => password; }

    /// <summary>
    /// Constructs a VendingMachine object.
    /// </summary>
    public VendingMachine()
    {
        products = new List<Product>();
        coins = new CoinBox();
        currentCoins = new CoinBox();
        changeBox = new CoinBox();
    }

    /// <summary>
    /// Gets the type of products in the vending machine.
    /// </summary>
    /// <returns>an array of products sold in this machine.</returns>
    public List<Product> GetProductTypes()
    {
        List<Product> types = new();

        for (int i = 0; i < products.Count; i++)
        {
            Product entry = products[i];
            types.Add(entry);
        }
        foreach (Product p in products)
        {
            if (! types.Contains(p))
            {
                types.Add(p);
            }
        }
        return types;
    }

    /// <summary>
    /// Adds a product to the vending machine.
    /// </summary>
    /// <param name="p">p the product to add</param>
    public void AddProduct(Product p)
    {
        products.Add(p);
    }

    /// <summary>
    /// Adds the coin to the vending machine.
    /// </summary>
    /// <param name="c">c the coin to add</param>
    /// <param name="start">Wether or not we are loading the bank</param>
    /// <returns>the value of the current transaction coinbox</returns>
    public double AddCoin(Coin c, bool start = false)
    {
        if (start)
        {
            coins.AddCoin(c);
            return coins.GetValue();
        }
        currentCoins.AddCoin(c);
        return currentCoins.GetValue();
    }

    /// <summary>
    /// Change the password
    /// </summary>
    /// <param name="password">the new password</param>
    public void ChangePassword(string password)
    {
        if (this.password == password)
        {
            WriteLine("Password already used");
            return;
        }
        this.password = password;
    }

    /// <summary>
    /// Buys a product from the vending machine.
    /// </summary>
    /// <param name="p">the product being purchased</param>
    /// <param name="change">the change</param>
    /// <returns>wether or not the transaction went successfully</returns>
    public String BuyProduct(Product p, out double change)
    {
        change = 0;
        for (int i = 0; i < products.Count; i++)
        {
            Product prod = products[i];
            if (prod.Equals(p))
            {
                double payment = currentCoins.GetValue();
                if (p.Price <= payment)
                {
                    p.Quantity--;

                    change = GetChange(currentCoins.GetValue() - p.Price).GetValue();

                    if (p.Quantity <= 0)
                        products.Remove(p);
                    coins.AddCoins(currentCoins);
                    currentCoins.RemoveAllCoins();
                    return "OK";
                }
                else
                {
                    return "Insufficient money";
                }
            }
        }
        return "No such product";
    }

    /// <summary>
    /// Calcules and returns the necessary amount of coins for the change
    /// </summary>
    /// <param name="value">the change</param>
    /// <returns>a coinbox contain the coins for the change</returns>
    public CoinBox GetChange(double value)
    {
        value = Math.Round(value, 2);
        CoinBox change = new();

        while (value >= 1)
        {
            change.AddCoin(new Coin(1, "dollar"));
            changeBox.RemoveCoin(Coin.DOLLAR);
            value -= 1;
        }

        while (value >= .25)
        {
            change.AddCoin(new Coin(.25, "quarter"));
            changeBox.RemoveCoin(new Coin(.25, "quarter"));
            value -= .25;
        }

        while (value >= .10)
        {
            change.AddCoin(new Coin(.10, "dime"));
            changeBox.RemoveCoin(Coin.DIME);
            value -= .10;
        }


        while (value >= .05)
        {
            change.AddCoin(new Coin(.05, "nickel"));
            changeBox.RemoveCoin(Coin.NICKEL);
            value -= .05;
        }

        while (value >= 0.01)
        {
            change.AddCoin(new Coin(.01, "penny"));
            changeBox.RemoveCoin(new Coin(0.01, "penny"));
            value -= .01;
        }

        return change;
    }

    /// <summary>
    /// Cancel the on-going transaction release the coins in the current transaction coinbox
    /// </summary>
    /// <returns>the value of the on-going transaction</returns>
    public double CancelTransaction()
    {
        double money = currentCoins.GetValue();
        currentCoins.RemoveAllCoins();

        return money;
    }

    /// <summary>
    /// Removes the money from the vending machine.
    /// </summary>
    /// <returns>the amount of money removed</returns>
    public double RemoveMoney()
    {
        double r = coins.GetValue();
        coins.RemoveAllCoins();
        return r;
    }

}