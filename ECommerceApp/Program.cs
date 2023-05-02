using ECommerceApp.Models;
using ECommerceApp.Service;

namespace ECommerceApp {
    internal class Program {
        static void Main(string[] args) {
            var serv = new Services();
            string line = "";
            while (line != "stop") {
                line = Console.ReadLine(); ;
                var input = line.Split(" ");
                var command = input[0];
                switch (command) {

                    case "save_product":
                        if (checkForValidity(input.Count(), 4)) break;
                        serv.SaveProduct(new Product() {
                            Id = input[1],
                            Name = input[2],
                            Price = int.Parse(input[3])
                        });
                        break;

                    case "purchase_product":
                        if (checkForValidity(input.Count(), 4)) break;
                        serv.PurchaseProduct(new Product() {
                            Id = input[1],
                            Quantity = int.Parse(input[2]),
                            Price = int.Parse(input[3])
                        });
                        break;

                    case "order_product":
                        if (checkForValidity(input.Count(), 3)) break;
                        serv.OrderProduct(new Product() {
                            Id = input[1],
                            Quantity = int.Parse(input[2])
                        });
                        break;
                    
                    case "get_quantity_of_product":
                        if (checkForValidity(input.Count(), 2)) break;
                        Console.WriteLine(serv.GetQuantityOfProduct(new Product() { Id = input[1] }));
                        break;
                    
                    case "get_average_price":
                        if (checkForValidity(input.Count(), 2)) break;
                        Console.WriteLine(serv.GetAveragePrice(new Product() { Id = input[1] }));
                        break;
                    
                    case "get_product_profit":
                        if (checkForValidity(input.Count(), 2)) break;
                        serv.GetProductProfit(new Product() { Id = input[1] });
                        break;
                    
                    case "get_fewest_product":
                        Console.WriteLine(serv.GetFewestProduct());
                        break;
                    
                    case "get_most_popular_product":
                        Console.WriteLine(serv.GetMostPopularProduct());
                        break;
                    
                    case "exit":
                        serv.Exit();
                        break;

                    default:
                        Console.WriteLine("Invalid option !");
                        break;
                }
            }
        }
        static bool checkForValidity(int size, int validSize) {
            if (size != validSize) {
                Console.WriteLine("Invalid Command");
                return true;
            }
            return false;
        }
    }
}