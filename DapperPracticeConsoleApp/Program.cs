using Dapper;
using Npgsql;
using System.Net.WebSockets;

namespace DapperPracticeConsoleApp
{
    public class Program
    {
        static void Main(string[] args)
        {
            var customerRepository = new CustomerRepository();
            var productRepository = new ProductRepository();
            var orderRepository = new OrdersRepository();

            while (true)
            {
                Console.WriteLine("Список команд для работы с приложением:\n" +
                                  "------- РАБОТА С ПОКУПАТЕЛЯМИ -------\n" +
                                  "1 - возвращает список покупателей\n" +
                                  "2 - возвращает покупателя по заданному ID\n" +
                                  "------- РАБОТА С ТОВАРАМИ -------\n" +
                                  "3 - возвращает список товаров на складе\n" +
                                  "4 - возвращает список товаров на складе, " +
                                  "чей минимальный остаток на складе равен введенному значению\n" +
                                  "------- РАБОТА С ЗАКАЗАМИ -------\n" +
                                  "5 - возвращает список заказов\n" +
                                  "6 - возвращает список заказов, которое содержит количество товаров из условия\n"  +
                                  "7 - возвращает список заказов, удовлетворяющих условию по возрасту покупателя и ID товара.\n");

                Console.WriteLine("Для выхода из приложения введите команду /exit.");

                var command =  Console.ReadLine();

                switch (command)
                {
                    case "1":
                        var customers = customerRepository.GetCustomers();
                        var text = "Список покупателей:";
                        Console.WriteLine(text);

                        foreach (var c in customers)
                        {
                            text = $"{c.Id}. {c.FirstName} {c.LastName}, возраст {c.Age}";
                            Console.WriteLine($"{text}");
                        }

                        Console.WriteLine("\n");
                        continue;

                    case "2":
                        Console.WriteLine("\nВведите идентификатор интересующего Вас покупателя:");
                        var inputResult = int.TryParse(Console.ReadLine(), out var id);

                        if (!inputResult)
                        {
                            text = $"Произошла ошибка при попытке обработать введенный идентификатор.\n" +
                                   "Убедитесь, что введено корректное значение и попробуйте снова.";
                            Console.WriteLine($"{text}\n");
                            continue;
                        }

                        var customer = customerRepository.GetCustomer(id);

                        if (customer == null)
                        {
                            Console.WriteLine("Покупателя с указанным идентификатором не найдено.\n");
                            continue;
                        }

                        text = $"Данные покупателя: {customer.FirstName} {customer.LastName}, возраст {customer.Age}";
                        Console.WriteLine($"{text}\n");
                        continue;

                    case "3":
                        var products = productRepository.GetProducts();
                        Console.WriteLine("Список товаров на складе:");

                        foreach (var p in products)
                        {
                            text = $"{p.Id}. {p.Name}\n" +
                                   $"Описание: {p.Description}\n" +
                                   $"Цена за ед.: {p.Price} руб.\n" +
                                   $"Кол-во на складе: {p.StockQuantity}";
                            Console.WriteLine($"{text}\n");
                        }
                        continue;

                    case "4":
                        Console.WriteLine("Укажите минимальный остаток для проверки наличия товаров на складе:");
                        inputResult = int.TryParse(Console.ReadLine(), out var productCount);

                        if (!inputResult)
                        {
                            text = "Произошла ошибка при попытке обработать введенное значение.\n" +
                                   "Убедитесь, что введено корректное значение и попробуйте снова.";
                            Console.WriteLine($"{text}\n");
                            continue;
                        }

                        var productsInStock = productRepository.GetProductsInStock(productCount);

                        if (productsInStock.Count == 0)
                        {
                            Console.WriteLine("Товаров с таким минимальным остатком на складе не найдено.\n");
                            continue;
                        }

                        Console.WriteLine("Найдены следующие товары на складе:");

                        foreach (var p in productsInStock)
                        {
                            text = $"{p.Id}. {p.Name}\n" +
                                   $"Описание: {p.Description}\n" +
                                   $"Цена за ед.: {p.Price} руб.\n" +
                                   $"Кол-во на складе: {p.StockQuantity}";
                            Console.WriteLine($"{text}\n");
                        }
                        continue;

                    case "5":
                        var orders = orderRepository.GetOrders();

                        Console.WriteLine("Список заказов:");

                        foreach (var o in orders)
                        {
                            text = $"{o.Id}. ID покупателя - {o.CustomerId}, ID товара - {o.ProductId}, кол-во товара в заказе - {o.Quantity}";
                            Console.WriteLine($"{text}\n");
                        }
                        continue;

                    case "6":
                        Console.WriteLine("\nВведите минимальное количество товаров в заказе:");
                        var inputMinQuantityResult = int.TryParse(Console.ReadLine(), out var minQuantity);

                        Console.WriteLine("Введите максимальное количество товаров в заказе:");
                        var inputMaxQuantityResult = int.TryParse(Console.ReadLine(), out var maxQuantity);

                        if (!inputMinQuantityResult || !inputMaxQuantityResult)
                        {
                            text = "Произошла ошибка при попытке обработать введенное значение.\n" +
                                   "Убедитесь, что введено корректное значение и попробуйте снова.";
                            Console.WriteLine($"{text}\n");
                            continue;
                        }
                        else if (minQuantity > maxQuantity)
                        {
                            text = "Минимальное значение не может быть выше максимального.\n" +
                                   "Убедитесь, что введено корректное значение и попробуйте снова.";
                            Console.WriteLine($"{text}\n");
                            continue;
                        }

                        var ordersByQuantityRange = orderRepository.GetOrdersByQuantityRange(minQuantity, maxQuantity);

                        if (ordersByQuantityRange.Count == 0)
                        {
                            Console.WriteLine("Заказов, соответствующих заданному условию, не найдено.\n");
                            continue;
                        }

                        Console.WriteLine($"Список заказов, которые содержат от {minQuantity} до {maxQuantity} товаров:");

                        foreach (var o in ordersByQuantityRange)
                        {
                            text = $"{o.Id}. ID покупателя - {o.CustomerId}, ID товара - {o.ProductId}, кол-во товара в заказе - {o.Quantity}";
                            Console.WriteLine($"{text}\n");
                        }
                        continue;

                    case "7":
                        // обработка запроса из задания "Кластерный индекс"
                        Console.WriteLine("\nВведите минимальный возраст покупателя:");
                        var inputMinAgeResult = int.TryParse(Console.ReadLine(), out var minAge);
                        Console.WriteLine("Введите идентификатор интересующего Вас товара:");

                        var inputProductIdResult = int.TryParse(Console.ReadLine(), out var productId);

                        if (!inputMinAgeResult || !inputProductIdResult)
                        {
                            text = "Произошла ошибка при попытке обработать введенное значение.\n" +
                                   "Убедитесь, что введено корректное значение и попробуйте снова.";
                            Console.WriteLine($"{text}\n");
                            continue;
                        }

                        var orderDetails = orderRepository.GetOrdersByAgeAndProduct(minAge, productId);

                        if (orderDetails.Count == 0)
                        {
                            Console.WriteLine("Заказов, соответствующих заданному условию, не найдено.\n");
                            continue;
                        }

                        Console.WriteLine("\nСписок пользователей старше 30 лет, у которых есть заказ на продукт с ID=1:");

                        foreach (var o in orderDetails)
                        {
                            text = $"{o.CustomerId}. {o.FirstName} {o.LastName}\n" +
                                   $"Заказ: ID продукта {o.ProductId}, цена за ед. - {o.Price} руб., кол-во товара в заказе - {o.Quantity}";
                            Console.WriteLine($"{text}\n");
                        }
                        continue;

                    case "/exit":
                        break;

                    default:
                        Console.WriteLine("Ошибка при обработке команды. Попробуйте ещё раз.\n");
                        continue;
                }
            }
        }
    }
}
