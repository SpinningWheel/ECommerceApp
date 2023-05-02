using ECommerceApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceApp.Service {
    class Services {
        private List<Product> _catalogDb = new List<Product>();
        private List<Product> _purchasedDb = new List<Product>();
        private List<Product> _orderedDb = new List<Product>();

        public Services() {

        }
        public void SaveProduct(Product product) {
            var resultProduct = _catalogDb.Where(c => c.Id.Equals(product.Id)).FirstOrDefault();
            if (resultProduct == null) {
                _catalogDb.Add(new Product {
                    Id = product.Id,
                    Name = product.Name,
                    Price = product.Price
                });
                return;
            }
            resultProduct.Name = product.Name;
            resultProduct.Price = product.Price;
        }
        public void PurchaseProduct(Product product) {
            var resultProduct = _catalogDb.Where(c => c.Id.Equals(product.Id)).FirstOrDefault();
            if (resultProduct == null) {
                Console.WriteLine("You have to register product beforehand in order to purchase it !");
                return;
            }
            _purchasedDb.Add(product);
            resultProduct.Quantity += product.Quantity;
        }
        public void OrderProduct(Product product) {
            var orderedProduct = _catalogDb.Where(c => c.Id.Equals(product.Id)).FirstOrDefault();
            if (orderedProduct == null) {
                Console.WriteLine("No such product to sell ! ");
                return;
            }
            product.Price = orderedProduct.Price;
            product.Name = orderedProduct.Name;
            _orderedDb.Add(product);
            orderedProduct.Quantity -= product.Quantity;
        }
        public int GetQuantityOfProduct(Product product) {
            var resultProd = _catalogDb.Where(c => c.Id.Equals(product.Id)).FirstOrDefault();
            if (resultProd == null) {
                Console.WriteLine("No such product ! ");
                return 0;
            }
            return resultProd.Quantity;
        }
        public double GetAveragePrice(Product product) {
            var purchasedQuantity = 0;
            var purchasedBalance = 0;
            var purchasedList = _purchasedDb.Where(c => c.Id.Equals(product.Id)).ToList();
            if (purchasedList.Count == 0) {
                return 0;
            }
            foreach (var p in purchasedList) {
                purchasedQuantity += p.Quantity;
                purchasedBalance += p.Price * p.Quantity;
            }
            return (double)purchasedBalance / (double)purchasedQuantity;
        }
        public void GetProductProfit(Product product) {
            var avgPurchasedPrice = GetAveragePrice(product);
            if (avgPurchasedPrice == 0) {
                Console.WriteLine(avgPurchasedPrice);
                return;
            }
            var orderedQuantity = 0;
            var orderedBalance = 0;
            var orderedList = _orderedDb.Where(c => c.Id.Equals(product.Id)).ToList();
            if (orderedList.Count == 0) {
                Console.WriteLine(0);
                return;
            }
            foreach (var o in orderedList) {
                orderedQuantity += o.Quantity;
                orderedBalance += o.Price * o.Quantity;
            }
            var avgOrderedPrice = (double)orderedBalance / (double)orderedQuantity;
            var avgProfitPU = avgOrderedPrice - avgPurchasedPrice;
            var totalProfit = avgProfitPU * orderedQuantity;
            Console.WriteLine(totalProfit);
        }
        public string GetFewestProduct() {
            if (_catalogDb.Count == 0) return "There are no products on the catalog ! ";
            var lowestQuantity = _catalogDb[0].Quantity;
            var worstPName = _catalogDb[0].Name;
            foreach (var p in _catalogDb) {
                if (p.Quantity < lowestQuantity) {
                    lowestQuantity = p.Quantity;
                    worstPName = p.Name;
                } 
            }
            return worstPName;
        }
        public string GetMostPopularProduct() {
            int popQuantity = 0;
            string popName = "";
            if (_orderedDb.Count == 0) return "There are no products sold ! ";
            foreach (var o in _orderedDb) {
                int save = 0;
                foreach (var p in _orderedDb) {
                    if (p.Id.Equals(o.Id)) {
                        save += p.Quantity;
                    }
                }
                if (save > popQuantity) {
                    popQuantity = save;
                    popName = o.Name;
                }
            }
            return popName;
        }
        public void Exit() {
            Environment.Exit(0);
        }

        public void GetOrdersReport() {
            var cogs = CalculateCOGS();
            foreach (var o in _orderedDb) {
                Console.WriteLine("Ordered  : " + o.Id + o.Name + o.Quantity + o.Price + ", COGS : " + cogs[o.Id]);
            }
        }
        public void ExportOrdersReport(string path) {
            string fileName = path + "\\OrdersReport.csv";
            var cogs = CalculateCOGS();
            try {
                if (File.Exists(fileName)) File.Delete(fileName);
                using (StreamWriter sw = File.CreateText(fileName)) {
                    sw.WriteLine("Id," + "Name," + "Quantity," + "Price," + "COGS,");
                    foreach (var o in _orderedDb) {
                        sw.WriteLine(o.Id + "," + o.Name + "," + o.Quantity + "," + o.Price + "," + cogs[o.Id]);
                    }
                }
            } catch (Exception e) {
                Console.WriteLine(e.ToString);
            }
        }

        public Dictionary<string, double> CalculateCOGS() {
            var result = new Dictionary<string, double>();
            foreach (var o in _orderedDb) {
                int quantity = 0;
                foreach (var c in _orderedDb) {
                    if (o.Id.Equals(c.Id)) {
                        quantity += c.Quantity;
                    }                                        
                }
                double cogs = GetAveragePrice(o) * quantity;
                if (!result.ContainsKey(o.Id)) result.Add(o.Id, cogs);
            }
            return result;
        }
    }
}
