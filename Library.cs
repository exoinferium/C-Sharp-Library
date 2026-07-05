// library app project
using System;
using System.Collections;
using System.IO;
// item class
abstract class Item
{
    // constructor
    public string ID
    {
        get;
        set;
    }
    public string Name
    {
        get;
        set;
    }
    public string Date
    {
        get;
        set;
    }
    public bool Loan
    {
        get;
        set;
    }

    public Item(string id, string name, string date, bool loan)
    {
        ID = id;
        Name = name;
        Date = date;
        Loan = loan;
    }
    // overdue
    public abstract double CalculateOverdueCharge(int days);
    // output
    public override string ToString()
    {
        return ID + " " + Name + " Loan: " + Loan;
    }
}
// book class extend item class
class Book : Item
{
    //author
    public string Author;

    public Book(string id, string name, string date, bool loan, string author): base(id, name, date, loan)
    {
        Author = author;
    }

    // book overdue
    public override double CalculateOverdueCharge(int days)
    {
        // charge 10 cents per day after 2 weeks
        if (days <= 14)
        {
            return 0;
        }
        return (days - 14) * 0.10;
    }
    // output
    public override string ToString()
    {
        return "Book: " + base.ToString() + " Author: " + Author;
    }
}
// magazine class extend item class
class Magazine : Item
{
    // publisher
    public string Publisher;

    public Magazine(string id, string name, string date, bool loan, string publisher): base(id, name, date, loan)
    {
        Publisher = publisher;
    }
    // magazine overdue
    public override double CalculateOverdueCharge(int days)
    {
        // charge 5 cents per day after a week
        if (days <= 7)
        {
            return 0;
        }
        return (days - 7) * 0.05;
    }
    // output
    public override string ToString()
    {
        return "Magazine: " + base.ToString() + " Publisher: " + Publisher;
    }
}
// newspaper class extend item class
class Newspaper : Item
{

    // format
    public string Format;

    public Newspaper(string id, string name, string date, bool loan, string format): base(id, name, date, loan)
    {
        Format = format;
    }
    // newspaper overdue
    public override double CalculateOverdueCharge(int days)
    {
        // charge 2 cents per day after 3 days
        if (days <= 3)
        {
            return 0;
        }
        return (days - 3) * 0.02;
    }
    // output
    public override string ToString()
    {
        return "Newspaper: " + base.ToString() + " Format: " + Format;
    }
}
// library class
class Library
{
    // arraylist of items
    ArrayList items = new ArrayList();
    // random
    Random rgen = new Random();
    // file of items
    public void LoadFile()
    {
        if (!File.Exists("library.txt"))
        {
            // add items
            items.Add(new Book("1", "Harry Potter", "2026-07-05", false, "J.K. Rowling"));
            items.Add(new Book("2", "Hungry Caterpillar", "2025-07-05", false, "Eric Carle"));
            items.Add(new Magazine("3", "Vogue", "2025-07-05", false, "Vogue"));
            items.Add(new Newspaper("4", "The Globe", "2025-07-05", false, "Paper"));
            return;
        }
        // file lines
        string[] lines = File.ReadAllLines("library.txt");
        // format
        foreach (string line in lines)
        {
            string[] p = line.Split(',');
            if (p[0] == "Book")
            {
                items.Add(new Book(p[1], p[2], p[3], bool.Parse(p[4]), p[5]));
            }
            else if (p[0] == "Magazine")
            {
                items.Add(new Magazine(p[1], p[2], p[3], bool.Parse(p[4]), p[5]));
            }
            else if (p[0] == "Newspaper")
            {
                items.Add(new Newspaper(p[1], p[2], p[3], bool.Parse(p[4]), p[5]));
            }
        }
    }
    // save file after exit
    public void SaveFile()
    {
        StreamWriter outFile = new StreamWriter("library.txt");
        foreach (Item item in items)
        {
            if (item is Book)
            {
                Book b = (Book)item;
                outFile.WriteLine("Book," + b.ID + "," + b.Name + "," + b.Date + "," + b.Loan + "," + b.Author);
            }
            else if (item is Magazine)
            {
                Magazine m = (Magazine)item;
                outFile.WriteLine("Magazine," + m.ID + "," + m.Name + "," + m.Date + "," + m.Loan + "," + m.Publisher);
            }
            else if (item is Newspaper)
            {
                Newspaper n = (Newspaper)item;
                outFile.WriteLine("Newspaper," + n.ID + "," + n.Name + "," + n.Date + "," + n.Loan + "," + n.Format);
            }
        }
        outFile.Close();
    }
    // display available items
    public void ShowAvailable()
    {
        // loop through items
        foreach (Item item in items)
        {
            if (!item.Loan)
            {
                //output available items
                Console.WriteLine(item);
            }
        }
    }
    // dispaly loans
    public void ShowLoan()
    {
        // loop through items
        foreach (Item item in items)
        {
            if (item.Loan)
            {
                // output loan items
                Console.WriteLine(item);
            }
        }
    }
    // borrow
    public void Borrow()
    {
        // input item id
        Console.Write("Enter ID: ");
        string id = Console.ReadLine();
        // seach through items
        foreach (Item item in items)
        {
            if (item.ID == id)
            {
                // found item
                if (!item.Loan)
                {
                    item.Loan = true;
                    Console.WriteLine("Item Borrowed");
                }
                else
                {
                    //item loaned
                    Console.WriteLine("Item Loaned");
                }
                return;
            }
        }
        // unavailable
        Console.WriteLine("Item Unavailable");
    }
    // return item
    public void Return()
    {
        // input item id
        Console.Write("Enter ID: ");
        string id = Console.ReadLine();
        // loop through items to find loan
        foreach (Item item in items)
        {
            if (item.ID == id)
            {
                if (item.Loan)
                {
                    // clear loan
                    item.Loan = false;
                    // generate random number 0-25 days borrowed
                    int days = rgen.Next(0, 26);
                    Console.WriteLine("Borrowed for " + days + " days.");
                    // cost
                    double fee = item.CalculateOverdueCharge(days);
                    Console.WriteLine("Overdue charge: $" + fee.ToString("F2"));
                }
                else
                {
                    // now borrowed items
                    Console.WriteLine("No items borrowed");
                }
                return;
            }
        }
        Console.WriteLine("Item unavailable");
    }

    // menu
    public int Menu()
    {
        // output options
        Console.WriteLine("1. Display");
        Console.WriteLine("2. Borrow");
        Console.WriteLine("3. Return");
        Console.WriteLine("4. Loan");
        Console.WriteLine("5. Exit");
        // return user intput
        Console.Write("Enter: ");
        return Convert.ToInt32(Console.ReadLine());
    }
}
// projetc class
class Project
{
    // main
    static void Main(string[] args)
    {
        // create library
        Library library = new Library();
        // create file of items
        library.LoadFile();
        // exit boolean
        bool exit = false;
        // loop until user exits
        while (!exit)
        {
            // option cases
            switch (library.Menu())
            {
                // available
                case 1:
                    library.ShowAvailable();
                    break;
                // borrow
                case 2:
                    library.Borrow();
                    break;
                // return
                case 3:
                    library.Return();
                    break;
                // loan
                case 4:
                    library.ShowLoan();
                    break;
                // exit
                case 5:
                    library.SaveFile();
                    exit = true;
                    break;
                // invalid input
                default:
                    Console.WriteLine("Invalid");
                    break;
            }
        }
    }
}