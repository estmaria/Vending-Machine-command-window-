using System;
using System.Collections.Generic;
using static System.Console;

namespace VendingMachineProgram;

/// <summary>
/// This class operates a vending machine object
///
/// <para>Author - Maria Esteban</para>
/// <para>Version - 1.8 (11-30-23)</para>
/// <para>Since - 10-30-23</para>
/// </summary>

public class VendingMachineMenu
{
    private static readonly string STORAGEFILE = "Products.txt";
    private static readonly string BANKFILE = "Bank.txt";
    private static readonly string CHANGEBOXFILE = "ChangeBox.txt";
    private static readonly string RESTOCKFILE = "ProductsRestock.txt";
    private static readonly string CHANGERESTOCKFILE = "ChangeBoxRestock.txt";

    private static readonly Coin[] COINS =
        {
            Coin.NICKEL, Coin.DIME, Coin.QUARTER, Coin.DOLLAR
        };

    /// <summary>
    /// Runs the vending machine system.
    /// </summary>
    /// <param name="machine">the vending machine instance</param>
    public void Run(VendingMachine machine)
    {
        LoadMachine(machine);
        LoadBank(machine);
        LoadChangeBox(machine);

        bool more = true;

        while (more)
        {
            WriteLine("S)how products B)uy product I)nsert C)ancel transaction M)aintenance mode Q)uit: ");
            String command = ReadLine()!.ToUpper();

            if (command.Equals("S"))
            {
                foreach (Product p in machine.GetProductTypes())
                {
                    WriteLine(p);
                }
            }
            else if (command.Equals("M"))
            {
                string userInput = "";
                bool correct;
                do
                {
                    correct = false;

                    Write("Enter the password (or -1 to return to vending mode): ");
                    userInput = ReadLine()!;

                    if (userInput == machine.Password)
                    {
                        correct = true;
                    }
                    else
                    {
                        WriteLine("Incorrect password!");
                    }

                } while (userInput != "-1" && !correct);

                if (correct)
                    MaintenanceMode(machine);
            }
            else if (command.Equals("C"))
            {
                double money = machine.CancelTransaction();
                if (money > 0)
                    WriteLine($"Transaction canceled: {money:C2} returned");
                else
                    WriteLine("There's no on-going transaction");
            }
            else if (command.Equals("A"))
            {
                Write("Description: ");
                String description = ReadLine()!;
                Write("Price: ");
                double price = Double.Parse(ReadLine()!);
                Write("Quantity: ");
                int quantity = int.Parse(ReadLine()!);
                machine.AddProduct(new Product(description, price, quantity));
            }
            else if (command.Equals("I"))
            {
                Coin chosen = PickCoin();
                WriteLine("Current $ " + machine.AddCoin(chosen));
            }
            else if (command.Equals("B"))
            {
                Product p = PickProduct(machine.GetProductTypes());
                String result = machine.BuyProduct(p, out double change);
                if (result == "OK")
                {
                    WriteLine("Purchased: " + p.Description + " @ " + p.Price);
                    WriteLine($"Change: {change:C2}");
                }
                else
                {
                    WriteLine("Sorry: " + result);
                }
            }
            else if (command.Equals("R"))
            {
                double totalInCoinBox = machine.RemoveMoney();
                WriteLine($"Removed: {totalInCoinBox:C2}");
            }
            else if (command.Equals("Q"))
            {
                more = false;
            }
        }
        OffLoadMachine(machine);
        SaveBank(machine);
        SaveChangeBox(machine);
    }

    /// <summary>
    /// Operates the maintenance mode of the vending machine
    /// </summary>
    /// <param name="machine">the vending machine instance</param>
    private static void MaintenanceMode(VendingMachine machine)
    {
        bool more = true;

        while (more)
        {
            WriteLine("S)how products A)dd product M)odify product price products E)mpty coinbox R)estock C)hange password Q)uit: ");
            String command = ReadLine()!.ToUpper();

            if (command.Equals("S"))
            {
                foreach (Product p in machine.GetProductTypes())
                {
                    WriteLine(p);
                }
            }
            else if (command.Equals("M"))
            {
                Write("Enter product name: ");
                string name = ReadLine()!;

                foreach (Product product in machine.products)
                {
                    if (product.Description == name)
                    {
                        Write($"Enter the new price for {product.Description}: ");
                        product.Price = Double.Parse(ReadLine()!);
                    }
                }
            }
            else if (command.Equals("C"))
            {
                Write("Enter the new password: ");
                string password = ReadLine()!;

                machine.ChangePassword(password);
            }
            else if (command.Equals("A"))
            {
                Write("Description: ");
                String description = ReadLine()!;
                Write("Price: ");
                double price = Double.Parse(ReadLine()!);
                Write("Quantity: ");
                int quantity = int.Parse(ReadLine()!);
                machine.AddProduct(new Product(description, price, quantity));
            }
            else if (command.Equals("I"))
            {
                Coin chosen = PickCoin();
                WriteLine("Current $ " + machine.AddCoin(chosen));
            }
            else if (command.Equals("B"))
            {
                Product p = PickProduct(machine.GetProductTypes());
                String result = machine.BuyProduct(p, out double change);
                if (result == "OK")
                {
                    WriteLine("Purchased: " + p.Description + " @ " + p.Price);
                    WriteLine($"Change: {change:C2}");
                }
                else
                {
                    WriteLine("Sorry: " + result);
                }
            }
            else if (command.Equals("R"))
            {
                machine.products.Clear();
                machine.changeBox.RemoveAllCoins();
                LoadChangeBox(machine, true);
                LoadMachine(machine, true);
            }
            else if (command.Equals("E"))
            {
                double totalInCoinBox = machine.RemoveMoney();
                WriteLine($"Removed: {totalInCoinBox:C2}");
            }
            else if (command.Equals("Q"))
            {
                more = false;
            }
        }
        return;
    }
    /// <summary>
    /// Loads the vending machine with the product stored in the products.txt file
    /// </summary>
    /// <param name="machine">the vending machine instance</param>
    /// <param name="restock">wether or not we are restocking</param>
    private static void LoadMachine(VendingMachine machine, bool restock = false)
    {
        try
        {
            string FileName;

            if (restock)
                FileName = RESTOCKFILE;
            else
                FileName = STORAGEFILE;

            using FileStream inFile = new(FileName, FileMode.Open, FileAccess.Read);
            using StreamReader reader = new(inFile);

            string line = reader.ReadLine()!;

            while (line != null)
            {
                string[] lines = line.Split(",");

                machine.AddProduct(new Product(lines[0], Double.Parse(lines[1]), int.Parse(lines[2])));

                line = reader.ReadLine()!;
            }
        }
        catch (FileNotFoundException)
        {
            WriteLine($"{STORAGEFILE} not found");
        }
        catch (Exception e)
        {
            WriteLine($"{e.Message} occured while reading the file {STORAGEFILE}");
        }
           
    }

    /// <summary>
    /// Saves every products in the vending machine in the products.txt file
    /// </summary>
    /// <param name="machine">the vending machine instance</param>
    private static void OffLoadMachine(VendingMachine machine)
    {
        try
        {
            using FileStream inFile = new(STORAGEFILE, FileMode.Create, FileAccess.Write);
            using StreamWriter writer = new(inFile);

            List<Product> products = machine.GetProductTypes();

            foreach (Product aProduct in products)
            {
                writer.WriteLine($"{aProduct.Description},{aProduct.Price},{aProduct.Quantity}");
            }
        }
        catch (FileNotFoundException)
        {
            WriteLine($"{STORAGEFILE} not found");
        }
        catch (Exception e)
        {
            WriteLine($"{e.Message} occured while writing on the file {STORAGEFILE}");
        }
    }

    /// <summary>
    /// Loads the storage coinbox of the vending machine with the coins in the Bank.txt file
    /// </summary>
    /// <param name="machine">the vending machine instance</param>
    private static void LoadBank(VendingMachine machine)
    {
        try
        {
            using FileStream inFile = new(BANKFILE, FileMode.OpenOrCreate, FileAccess.Read);
            using StreamReader reader = new(inFile);

            string line = reader.ReadLine()!;

            while (line != null)
            {
                string[] lines = line.Split(",");
                machine.AddCoin(new Coin(Double.Parse(lines[0]), lines[1]), true);

                line = reader.ReadLine()!;
            }

        }
        catch (FileNotFoundException)
        {
            WriteLine($"{BANKFILE} not found");
        }
        catch (Exception e)
        {
            WriteLine($"{e.Message} occured while reading the file {BANKFILE}");
        }
    }

    /// <summary>
    /// Save all the coins the storage coinbox in the Bank.txt file
    /// </summary>
    /// <param name="machine">the vending machine instance</param>
    private static void SaveBank(VendingMachine machine)
    {
        try
        {
            using FileStream inFile = new(BANKFILE, FileMode.Create, FileAccess.Write);
            using StreamWriter writer = new(inFile);

            foreach (Coin coin in machine.coins)
            {
                writer.WriteLine($"{coin.Value},{coin.Name}");
            }

        }
        catch (FileNotFoundException)
        {
            WriteLine($"{BANKFILE} not found");
        }
        catch (Exception e)
        {
            WriteLine($"{e.Message} occured while writing on the file {BANKFILE}");
        }
    }

    /// <summary>
    /// Loads the change coinbox with the coins in the ChangeBox.txt file
    /// </summary>
    /// <param name="machine">the vending machine instance</param>
    /// <param name="restock">wether or not we are restocking</param>
    private static void LoadChangeBox(VendingMachine machine, bool restock = false)
    {
        try
        {
            string fileName;

            if (restock)
                fileName = CHANGERESTOCKFILE;
            else
                fileName = CHANGEBOXFILE;

            using FileStream inFile = new(fileName, FileMode.Open, FileAccess.Read);
            using StreamReader reader = new(inFile);

            string line = reader.ReadLine()!;

            while (line != null)
            {
                string[] lines = line.Split(",");
                machine.changeBox.AddCoin(new Coin(Double.Parse(lines[0]), lines[1]));

                line = reader.ReadLine()!;
            }

        }
        catch (FileNotFoundException)
        {
            WriteLine($"{CHANGEBOXFILE} not found");
        }
        catch (Exception e)
        {
            WriteLine($"{e.Message} occured while reading the file {CHANGEBOXFILE}");
        }
    }

    /// <summary>
    /// Saves all the coins in the ChangeBox coinbox in the ChangeBox.txt file
    /// </summary>
    /// <param name="machine">the vending machine instance</param>
    private static void SaveChangeBox(VendingMachine machine)
    {
        try
        {
            using FileStream inFile = new(CHANGEBOXFILE, FileMode.Create, FileAccess.Write);
            using StreamWriter writer = new(inFile);

            foreach (Coin coin in machine.changeBox)
            {
                writer.WriteLine($"{coin.Value},{coin.Name}");
            }

        }
        catch (FileNotFoundException)
        {
            WriteLine($"{CHANGEBOXFILE} not found");
        }
        catch (Exception e)
        {
            WriteLine($"{e.Message} occured while writing on the file {CHANGEBOXFILE}");
        }
    }
        
    /// <summary>
    /// Pick a coin from the list of all coins
    /// </summary>
    /// <returns><the coin selected/returns>
    private static Coin PickCoin()
    {
        while (true)
        {
            char c = 'A';
            foreach (Coin choice in COINS)
            {
                WriteLine(c + ") " + choice);
                c++;
            }

            String input = ReadLine();
            int n = input.ToUpper()[0] - 'A';

            if (0 <= n && n < COINS.Length)
            {
                return COINS[n];
            }
        }
    }

    /// <summary>
    /// Pick a product from all products
    /// </summary>
    /// <param name="allProducts">every products in the vending machine</param>
    /// <returns>the product selected by the user</returns>
    private static Product PickProduct(List<Product> allProducts)
    {
        while (true)
        {
            char c = 'A';
            foreach (Product choice in allProducts)
            {
                WriteLine(c + ") " + choice);
                c++;
            }
            String input = ReadLine();
            int n = input.ToUpper()[0] - 'A';
            if (0 <= n && n < allProducts.Count)
            {
                return allProducts[n];
            }
        }
    }

    public static void Main()
    {
        new VendingMachineMenu()
            .Run(new VendingMachine());
    }
}