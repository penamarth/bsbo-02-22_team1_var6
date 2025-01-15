using System;
using System.Collections.Generic;

// Интерфейс StorageComponent
public interface StorageComponent
{
    void Add(StorageComponent component);
    void Remove(StorageComponent component);
    void Move(string location);
    void DisplayContent();
    DateTime CalculateEndDate();
}

// Класс Composite
public abstract class Composite : StorageComponent
{
    protected internal List<StorageComponent> components = new List<StorageComponent>();

    public virtual void Add(StorageComponent component)
    {
        components.Add(component);
    }

    public virtual void Remove(StorageComponent component)
    {
        components.Remove(component);
    }

    public virtual void Move(string location)
    {
        foreach (var component in components)
        {
            component.Move(location);
        }
    }

    public virtual void DisplayContent()
    {
        foreach (var component in components)
        {
            component.DisplayContent();
        }
    }

    public abstract DateTime CalculateEndDate();
}

// Класс Warehouse
public class Warehouse : Composite
{
    public override DateTime CalculateEndDate()
    {
        Console.WriteLine("Warehose: Расчёт последнего дня аренды...");
        return DateTime.Now.AddDays(30); // Пример
    }
    public void DisplayContent()
    {
        Console.WriteLine("Warehouse:");
        foreach (var wardrobe in components)
        {
            wardrobe.DisplayContent(); // Отображаем содержимое каждого шкафа
        }
    }
    public void Sort()
    {
        Console.WriteLine("Запрос на сортировку");
        Console.WriteLine("Передача команды на сортировку");

        foreach (var wardrobe in components)
        {
            Console.WriteLine("Warehouse -> Wardrobe: Запрос на сортировку содержимого шкафа");
            wardrobe.DisplayContent();

            foreach (var shelf in ((Composite)wardrobe).components)
            {
                Console.WriteLine("Wardrobe -> Shelf: Запрос на сортировку содержимого полки");
                shelf.DisplayContent();

                foreach (var cell in ((Composite)shelf).components)
                {
                    Console.WriteLine("Shelf -> Cell: Запрос на сортировку содержимого ячеек");
                    if (cell is Cell currentCell)
                    {
                        Console.WriteLine("Cell -> Shelf: Подтверждение сортировки ячеек");
                    }
                }

                Console.WriteLine("Shelf -> Wardrobe: Подтверждение сортировки полок");
            }

            Console.WriteLine("Wardrobe -> Warehouse: Подтверждение сортировки шкафа");
        }

        Console.WriteLine("Подтверждение завершения сортировки");
    }

    public void AddRental(int size)
    {
        Console.WriteLine("Запрос на аренду");
        Console.WriteLine("Передача данных аренды");

        foreach (var wardrobe in components)
        {
            Console.WriteLine("Warehouse -> Wardrobe: Запрос на проверку наличия ресурсов");

            foreach (var shelf in ((Composite)wardrobe).components)
            {
                Console.WriteLine("Wardrobe -> Shelf: Запрос на проверку наличия ресурсов");

                foreach (var cell in ((Composite)shelf).components)
                {
                    Console.WriteLine("Shelf -> Cell: Запрос на проверку наличия ресурсов");
                    if (cell is Cell currentCell && currentCell.IsEmpty())
                    {
                        Console.WriteLine("Cell -> Warehouse: Информация о доступности ресурсов");
                        Rental rental = new Rental("Арендатор", size);
                        rental.Space = currentCell;
                        components.Add(rental);
                        Console.WriteLine("Warehouse -> Rental: Создать объект аренды");
                        Console.WriteLine("Warehouse -> Warehouse: Сохранить данные аренды");
                        Console.WriteLine("Передача данных");
                        Console.WriteLine("Подтверждение аренды");
                        return;
                    }
                }
            }
        }

        Console.WriteLine("Уведомление: недостаточно ресурсов для аренды");
    }

    public void AddItemToWarehouse(string name, int quantity)
    {
        Console.WriteLine("Запрос на добавление товара");

        Item newItem = new Item { ItemID = Guid.NewGuid().ToString(), ItemName = name, Quantity = quantity };
        Console.WriteLine($"Warehouse -> Item: Создание объекта товара (имя: {name}, количество: {quantity})");

        foreach (var wardrobe in components)
        {
            Console.WriteLine("Warehouse -> Wardrobe: Проверка доступного места");
            wardrobe.DisplayContent();

            foreach (var shelf in ((Composite)wardrobe).components)
            {
                Console.WriteLine("Wardrobe -> Shelf: Проверка доступного места");
                shelf.DisplayContent();

                foreach (var cell in ((Composite)shelf).components)
                {
                    Console.WriteLine("Shelf -> Cell: Проверка доступного места");
                    if (cell is Cell currentCell && currentCell.IsEmpty())
                    {
                        Console.WriteLine("Cell -> Warehouse: Информация о доступности места");
                        currentCell.AddItem(newItem);
                        Console.WriteLine("Warehouse -> Warehouse: Вычисление объема и габаритов");
                        Console.WriteLine("Warehouse -> Warehouse: Обновление данных склада");
                        Console.WriteLine("Уведомление: товар добавлен");
                        return;
                    }
                }
            }
        }

        Console.WriteLine("Уведомление: недостаточно места для добавления товара");
    }

    public void RemoveItemFromWarehouse(string itemName)
    {
        Console.WriteLine("Запрос на удаление конкретного товара");

        foreach (var wardrobe in components)
        {
            Console.WriteLine("Warehouse -> Wardrobe: Проверка совпадений по шкафам");

            foreach (var shelf in ((Composite)wardrobe).components)
            {
                Console.WriteLine("Wardrobe -> Shelf: Проверка совпадений по полкам");

                foreach (var cell in ((Composite)shelf).components)
                {
                    Console.WriteLine("Shelf -> Cell: Проверка совпадений по ячейкам");
                    if (cell is Cell currentCell)
                    {
                        Item itemToRemove = currentCell.Items.Find(item => item.ItemName == itemName);
                        if (itemToRemove != null)
                        {
                            currentCell.RemoveItem(itemToRemove);
                            Console.WriteLine("Cell -> Warehouse: содержимое удалено");
                            Console.WriteLine("Warehouse -> Warehouse: Обновление данных склада");
                            Console.WriteLine("Уведомление об успешном удалении");
                            return;
                        }
                    }
                }
            }
        }

        Console.WriteLine("Уведомление: товар не найден");
    }
    public void GenerateReport()
    {
        Report report = new Report();

        Console.WriteLine("Warehouse -> Report: Сбор данных для отчёта");

        foreach (var wardrobe in components)
        {
            foreach (var shelf in ((Composite)wardrobe).components)
            {
                foreach (var cell in ((Composite)shelf).components)
                {
                    if (cell is Cell currentCell)
                    {
                        foreach (var item in currentCell.Items)
                        {
                            string itemInfo = $"Item: {item.ItemName}, Quantity: {item.Quantity}, Location: Warehouse -> Wardrobe -> Shelf -> Cell";
                            report.AddData(itemInfo);
                        }
                    }
                }
            }
        }

        Console.WriteLine("Warehouse -> Report: Формирование отчёта");
        report.DisplayReport();
    }
}

// Класс Rental
public class Rental : Composite
{
    public string RenterName { get; set; }
    public StorageComponent Space { get; set; }
    public DateTime StartDate { get; set; }
    public int DurationDays { get; set; }

    public Rental(string renterName, int durationDays)
    {
        RenterName = renterName;
        DurationDays = durationDays;
        StartDate = DateTime.Now;
    }

    public override DateTime CalculateEndDate()
    {
        return StartDate.AddDays(DurationDays);
    }

    public void CreateRental(int sizeRental)
    {
        Console.WriteLine($"Rental: Creating rental of size {sizeRental}");
    }
}

// Класс Wardrobe
public class Wardrobe : Composite
{
    public override DateTime CalculateEndDate()
    {
        Console.WriteLine("Wardrobe: Calculating end date...");
        return DateTime.Now.AddDays(15); // Пример
    }
    public override void DisplayContent()
    {
 
        Console.WriteLine("  Wardrobe: ");
        foreach (var shelf in components)
        {
            shelf.DisplayContent(); // Отображаем содержимое каждой полки
        }
    }
}

// Класс Shelf
public class Shelf : Composite
{
    public override DateTime CalculateEndDate()
    {
        Console.WriteLine("Shelf: ");
        return DateTime.Now.AddDays(10); // Пример
    }

    public override void DisplayContent()
    {
        Console.WriteLine("      Shelf: ");
        foreach (var cell in components)
        {
            cell.DisplayContent(); // Отображаем содержимое каждой ячейки
        }
    }
}

// Класс Cell
public class Cell : StorageComponent
{
    public List<Item> Items { get; } = new List<Item>();

    public void AddItem(Item item)
    {
        Items.Add(item);
        Console.WriteLine($"Cell: Item {item.ItemName} added.");
    }

    public bool IsEmpty()
    {
        return Items.Count == 0;
    }

    public void RemoveItem(Item item)
    {
        Items.Remove(item);
        Console.WriteLine($"Cell: Item {item.ItemName} removed.");
    }

    public void Add(StorageComponent component)
    {
        throw new NotImplementedException("Cell cannot contain other components.");
    }

    public void Remove(StorageComponent component)
    {
        throw new NotImplementedException("Cell cannot contain other components.");
    }

    public void Move(string location)
    {
        Console.WriteLine($"Cell: Moving items to {location}.");
    }

    public void DisplayContent()
    {
        Console.WriteLine("          Cell: ");
        if (Items.Count == 0)
        {
            Console.WriteLine("            No items in this cell.");
        }
        else
        {
            foreach (var item in Items)
            {
                Console.WriteLine($"            --Item: {item.ItemName}, Quantity: {item.Quantity}");
            }
        }
    }

    public DateTime CalculateEndDate()
    {
        return DateTime.Now.AddDays(5); // Пример
    }
}

// Класс Item
public class Item
{
    public string ItemID { get; set; }
    public string ItemName { get; set; }
    public int Quantity { get; set; }

    public void Move(string location)
    {
        Console.WriteLine($"Item {ItemName} moved to {location}.");
    }
}

public class Report
{
    private List<string> reportData = new List<string>();

    public void AddData(string data)
    {
        reportData.Add(data);
    }

    public void DisplayReport()
    {
        Console.WriteLine("--- Warehouse Report ---");
        foreach (var data in reportData)
        {
            Console.WriteLine(data);
        }
        Console.WriteLine("-------------------------");
    }
}

// Главный класс программы
public class Program
{
    public static void Main(string[] args)
    {
        Warehouse warehouse = new Warehouse();

        Wardrobe wardrobe = new Wardrobe();
        Shelf shelf = new Shelf();
        Shelf shelf1 = new Shelf();
        Cell cell1 = new Cell();
        Cell cell2 = new Cell();
        Cell cell3 = new Cell();

        wardrobe.Add(shelf);
        wardrobe.Add(shelf1);
        shelf.Add(cell1);
        shelf.Add(cell2);
        shelf1.Add(cell3);
        warehouse.Add(wardrobe);

        while (true)
        {
            Console.WriteLine("\nChoose an action:");
            Console.WriteLine("1 - Add item");
            Console.WriteLine("2 - Remove item");
            Console.WriteLine("3 - Sort items");
            Console.WriteLine("4 - Rent space");
            Console.WriteLine("5 - Generate Report");
            Console.WriteLine("6 - Display content");
            Console.WriteLine("0 - Exit");

            int choice;
            bool isNumber = int.TryParse(Console.ReadLine(), out choice);

            if (!isNumber)
            {
                Console.WriteLine("Invalid input. Please enter a number.");
                continue;
            }

            switch (choice)
            {
                case 1:
                    Console.WriteLine("Enter item name:");
                    string itemName = Console.ReadLine();
                    Console.WriteLine("Enter item quantity:");
                    if (int.TryParse(Console.ReadLine(), out int quantity))
                    {
                        warehouse.AddItemToWarehouse(itemName, quantity);
                    }
                    else
                    {
                        Console.WriteLine("Invalid quantity.");
                    }
                    break;

                case 2:
                    Console.WriteLine("Enter the name of the item to remove:");
                    string itemToRemove = Console.ReadLine();
                    warehouse.RemoveItemFromWarehouse(itemToRemove);
                    break;

                case 3:
                    Console.WriteLine("Sorting items in Warehouse...");
                    warehouse.Sort();
                    break;

                case 4:
                    Console.WriteLine("Renting space in Warehouse...");
                    warehouse.AddRental(50);
                    break;

                case 5:
                    Console.WriteLine("Generating report...");
                    warehouse.GenerateReport();
                    break;

                case 6:
                    Console.WriteLine("Displaying content of Warehouse...");
                    warehouse.DisplayContent();
                    break;

                case 0:
                    Console.WriteLine("Exiting the program. Goodbye!");
                    return;

                default:
                    Console.WriteLine("Invalid choice. Please select a valid option.");
                    break;
            }
        }
    }
}
