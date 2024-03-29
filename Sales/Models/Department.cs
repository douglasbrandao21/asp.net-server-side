﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Sales.Models
{
    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<Seller> Sellers { get; set; } = new List<Seller>();

        public Department() 
        {
        }

        public Department(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public void AddSeller(Seller seller)
        {
            Sellers.Add(seller);
        }

        public double TotalSales(DateTime initial, DateTime final)
        {
            double amount = Sellers.Sum(seller => seller.TotalSales(initial, final));

            return amount;
        }
    }
}
