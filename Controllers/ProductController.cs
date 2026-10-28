
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;
using Practical8.Models;

namespace Practical8.Controllers
{
    public class ProductController : Controller
    {
        string cs = ConfigurationManager
                        .ConnectionStrings["cs"].ConnectionString;

        // 1. GET: Index - Display all products
        [HttpGet]
        public ActionResult Index()
        {
            List<Product> products = new List<Product>();

            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = "SELECT ProductId, ProductName, Quantity, Price FROM Products";

                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();

                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    Product p = new Product();

                    p.ProductId = Convert.ToInt32(dr["ProductId"]);
                    p.ProductName = dr["ProductName"].ToString();
                    p.Quantity = Convert.ToInt32(dr["Quantity"]);
                    p.Price = Convert.ToDecimal(dr["Price"]);

                    products.Add(p);
                }

                dr.Close();
            }

            return View(products);
        }

        // 2. GET: Create - Display product form
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        // 3. POST: Create - Insert product
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product product)
        {
            if (!ModelState.IsValid)
                return View(product);

            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = @"INSERT INTO Products
                                (ProductName, Quantity, Price)
                                VALUES (@ProductName, @Quantity, @Price)";

                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.Add("@ProductName", SqlDbType.NVarChar, 100)
                    .Value = product.ProductName;

                cmd.Parameters.Add("@Quantity", SqlDbType.Int)
                    .Value = product.Quantity;

                cmd.Parameters.Add("@Price", SqlDbType.Decimal)
                    .Value = product.Price;

                cmd.Parameters["@Price"].Precision = 10;
                cmd.Parameters["@Price"].Scale = 2;

                con.Open();
                cmd.ExecuteNonQuery();
            }

            return RedirectToAction("Index");
        }

        // 4. GET: Edit - Load existing product
        [HttpGet]
        public ActionResult Edit(int id)
        {
            Product product = null;

            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = @"SELECT ProductId, ProductName,
                                 Quantity, Price
                                 FROM Products
                                 WHERE ProductId = @ProductId";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.Add("@ProductId", SqlDbType.Int).Value = id;

                con.Open();

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    product = new Product
                    {
                        ProductId = Convert.ToInt32(dr["ProductId"]),
                        ProductName = dr["ProductName"].ToString(),
                        Quantity = Convert.ToInt32(dr["Quantity"]),
                        Price = Convert.ToDecimal(dr["Price"])
                    };
                }

                dr.Close();
            }

            if (product == null)
                return HttpNotFound();

            return View(product);
        }

        // 5. POST: Edit - Update product
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Product product)
        {
            if (!ModelState.IsValid)
                return View(product);

            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = @"UPDATE Products
                                 SET ProductName = @ProductName,
                                     Quantity = @Quantity,
                                     Price = @Price
                                 WHERE ProductId = @ProductId";

                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.Add("@ProductId", SqlDbType.Int)
                    .Value = product.ProductId;

                cmd.Parameters.Add("@ProductName", SqlDbType.NVarChar, 100)
                    .Value = product.ProductName;

                cmd.Parameters.Add("@Quantity", SqlDbType.Int)
                    .Value = product.Quantity;

                cmd.Parameters.Add("@Price", SqlDbType.Decimal)
                    .Value = product.Price;

                cmd.Parameters["@Price"].Precision = 10;
                cmd.Parameters["@Price"].Scale = 2;

                con.Open();

                int rows = cmd.ExecuteNonQuery();

                if (rows == 0)
                    return HttpNotFound();
            }

            return RedirectToAction("Index");
        }

        // 6. POST: Delete - Remove product
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = "DELETE FROM Products WHERE ProductId = @ProductId";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.Add("@ProductId", SqlDbType.Int).Value = id;

                con.Open();
                cmd.ExecuteNonQuery();
            }

            return RedirectToAction("Index");
        }
    }
}
