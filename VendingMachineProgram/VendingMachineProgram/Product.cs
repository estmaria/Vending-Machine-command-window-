using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace VendingMachineProgram;

/// <summary>
/// This class simulates the behavior of a product
///
/// <para>Author - Maria Esteban</para>
/// <para>Version - 1.4 (11-30-23)</para>
/// <para>Since - 10-30-23</para>
/// </summary>

public class Product
{
    private readonly String description;
    private double price;
    private int quantity;

    public string? Description { get { return description; } }
    public double Price { get { return price; } set { price = value; } }
    public int Quantity { get { return quantity; } set { quantity = value; } }

    /// <summary>
    /// Constructs a Product object
    /// </summary>
    /// <param name="description">the description of the product</param>
    /// <param name="price">the price of the product</param>
    /// <param name="quantity">the quantity of the product</param>
    public Product(String description, double price, int quantity)
    {
        this.description = description;
        this.price = price;
        this.quantity = quantity;
    }

    /// <summary>
    /// Determines of this product is the same as the other product.
    /// </summary>
    /// <param name="other">the other product</param>
    /// <returns>true if the products are equal, false otherwise</returns>
    public override Boolean Equals(Object? other)
    {
        if (other == null)
        {
            return false;
        }
        Product b = (Product)other;

        return this.description.Equals(b.description) && this.price == b.price;
    }

    /// <summary>
    /// Formats the product's description and price.
    /// </summary>
    /// <returns>a string representing the object</returns>
    public override String ToString()
    {
        return this.quantity + " x " + this.description + " @ " + $"{this.price:C2}";
    }
}

