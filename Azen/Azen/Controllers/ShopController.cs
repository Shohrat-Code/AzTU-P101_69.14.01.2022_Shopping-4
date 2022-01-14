using Azen.Data;
using Azen.Models;
using Azen.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Azen.Controllers
{
    public class ShopController : Controller
    {
        private readonly AppDbContext _context;

        public ShopController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index(VmProduct model)
        {
            List<Product> products = _context.Products
                                                      .Include(cp => cp.ColorToProducts).ThenInclude(pi => pi.ProductImages)
                                                      .Include(cp => cp.ColorToProducts).ThenInclude(sc => sc.SizeColorToProducts)
                                                      .Where(p => (model.searchData != null ? p.Name.Contains(model.searchData) : true) &&
                                                                  (model.prdId != null ? p.CategoryId == model.prdId : true) &&
                                                                  (model.MinPrice != null ? p.ColorToProducts.FirstOrDefault().SizeColorToProducts.FirstOrDefault().Price >= model.MinPrice : true) &&
                                                                  (model.MaxPrice != null ? p.ColorToProducts.FirstOrDefault().SizeColorToProducts.FirstOrDefault().Price <= model.MaxPrice : true))
                                                      .ToList();

            model.PageCount = (int)Math.Ceiling(products.Count / model.ItemCount);
            model.Products = products.Skip((model.Page - 1) * (int)model.ItemCount).Take((int)model.ItemCount).ToList();
            model.ProductCategories = _context.ProductCategories.ToList();

            return View(model);
        }

        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Product product = _context.Products
                                               .Include(c => c.ColorToProducts).ThenInclude(pi => pi.ProductImages)
                                               .Include(c => c.ColorToProducts).ThenInclude(sp => sp.SizeColorToProducts).ThenInclude(s => s.Size)
                                               .Include(c => c.ColorToProducts).ThenInclude(co=>co.Color)
                                               .Include(ca=>ca.ProductCategory)
                                               .Include(t=>t.ProductTagToProducts)
                                               .FirstOrDefault(p => p.Id == id);

            return View(product);
        }

        public IActionResult AddToCart(int sizeColorProductId)
        {
            string oldCart = Request.Cookies["cart"];
            string newCart = "";

            if (string.IsNullOrEmpty(oldCart))
            {
                newCart = sizeColorProductId + "";
            }
            else
            {
                List<string> oldCartList = oldCart.Split("-").ToList();
                if (oldCartList.Any(i => i == sizeColorProductId.ToString()))
                {
                    oldCartList.Remove(sizeColorProductId.ToString());
                }
                else
                {
                    oldCartList.Add(sizeColorProductId.ToString());
                }

                newCart = string.Join("-", oldCartList);
            }

            Response.Cookies.Append("cart", newCart);
            return RedirectToAction("Index");
        }
        public IActionResult Cart()
        {
            string cart = Request.Cookies["cart"];
            List<SizeColorToProduct> sizeColorToProducts = new List<SizeColorToProduct>();
            if (!string.IsNullOrEmpty(cart))
            {
                List<string> cartList = cart.Split("-").ToList();

                sizeColorToProducts = _context.SizeColorToProducts.Include(cp => cp.ColorToProduct).ThenInclude(pi => pi.ProductImages)
                                                                  .Include(cp => cp.ColorToProduct).ThenInclude(pi => pi.Product)
                                                                  .Where(sp => cartList.Any(cl => cl == sp.Id.ToString())).ToList();
            }

            return View(sizeColorToProducts);
        }
        public IActionResult Checkout()
        {
            VmOrder model = new VmOrder();
            string cart = Request.Cookies["cart"];
            if (!string.IsNullOrEmpty(cart))
            {
                List<string> cartList = cart.Split("-").ToList();

                model.SizeColorToProducts = _context.SizeColorToProducts.Include(cp => cp.ColorToProduct).ThenInclude(pi => pi.Product)
                                                                  .Where(sp => cartList.Any(cl => cl == sp.Id.ToString())).ToList();
            }

            return View(model);
        }
        [HttpPost]
        public IActionResult Checkout(VmOrder model)
        {
            if (ModelState.IsValid)
            {
                //Request to Bank api
                //
                //
                //
                //
                bool canWithdraw = true;


                if (canWithdraw)
                {
                    //Create customer
                    CustomUser costomer = new CustomUser();

                    if (!_context.CustomUsers.Any(c => c.Email == model.CustomUser.Email))
                    {
                        CustomUser newCostomer = new CustomUser()
                        {
                            Name = model.CustomUser.Name,
                            Surname = model.CustomUser.Surname,
                            Email = model.CustomUser.Email,
                            PhoneNumber = model.CustomUser.PhoneNumber,
                            Address = model.CustomUser.Address,
                            UserName = model.CustomUser.Email
                        };
                        _context.CustomUsers.Add(newCostomer);
                        _context.SaveChanges();

                        costomer = newCostomer;
                    }
                    else
                    {
                        costomer = _context.CustomUsers.FirstOrDefault(c => c.Email == model.CustomUser.Email);
                    }

                    //Update stock
                    string cart = Request.Cookies["cart"];
                    List<SizeColorToProduct> sizeColorToProducts = new List<SizeColorToProduct>();
                    if (!string.IsNullOrEmpty(cart))
                    {
                        List<string> cartList = cart.Split("-").ToList();

                        sizeColorToProducts = _context.SizeColorToProducts.Include(cp => cp.ColorToProduct).ThenInclude(pi => pi.Product)
                                                                          .Where(sp => cartList.Any(cl => cl == sp.Id.ToString())).ToList();
                    }

                    foreach (var item in sizeColorToProducts)
                    {
                        _context.SizeColorToProducts.Find(item.Id).Quantity--;
                    }
                    _context.SaveChanges();


                    //Invoice
                    Sale sale = new Sale();
                    int invoiceNo = 1;
                    if (_context.Sales.Any())
                    {
                        invoiceNo = Convert.ToInt32(_context.Sales.OrderBy(o => o.Id).LastOrDefault().No) + 1;
                    }

                    sale.No = invoiceNo.ToString().PadLeft(11, '0');
                    if (sizeColorToProducts.Sum(s => s.Price) < 100)
                    {
                        sale.Shipping = 10;
                    }
                    sale.CustomerId = costomer.Id;
                    sale.CreatedDate = DateTime.Now;
                    _context.Sales.Add(sale);
                    _context.SaveChanges();


                    foreach (var item in sizeColorToProducts)
                    {
                        SaleItem saleItem = new SaleItem();
                        if (item.DiscountDate != null && item.DiscountDate > DateTime.Now)
                        {
                            saleItem.Price = (decimal)item.DiscountPrice;
                        }
                        else
                        {
                            saleItem.Price = item.Price;
                        }
                        saleItem.Quantity = 1;
                        saleItem.ProductId = item.Id;
                        saleItem.SaleId = sale.Id;

                        _context.SaleItems.Add(saleItem);
                    }

                    _context.SaveChanges();
                    Response.Cookies.Delete("cart");


                    MailMessage mail = new MailMessage();
                    mail.From = new MailAddress("aztup101@gmail.com", "AzTU-P101");
                    mail.To.Add(costomer.Email);
                    mail.Subject = "Product invoice";
                    string body = "<h1 style='font-size:30px; color:green; font-weight: bold;'>You shopping complate successfully</h1>" +
                        "<h3>The invoice is below:</h3>";
                    foreach (var item in sizeColorToProducts)
                    {
                        body += $"<p>Product: {item.ColorToProduct.Product.Name}, Quantity: 1, Price: {item.Price}</p>";
                    }
                    mail.Body = body;
                    mail.IsBodyHtml = true;
                    mail.CC.Add("shohret550@gmail.com");
                    mail.Bcc.Add("shohrat@code.edu.az");

                    SmtpClient client = new SmtpClient();
                    client.Host = "smtp.gmail.com";
                    client.Port = 587;
                    client.EnableSsl = true;
                    client.Credentials = new NetworkCredential("aztup101@gmail.com", "Aztu123456!");

                    client.Send(mail);


                    HttpContext.Session.SetString("success", "You shopping complate successfully");
                    return RedirectToAction("index");
                }

                //HttpContext.Session.SetString("BankError","You do not have enough money");
                ModelState.AddModelError("BankError", "You do not have enough money");
                string cart2 = Request.Cookies["cart"];
                if (!string.IsNullOrEmpty(cart2))
                {
                    List<string> cartList = cart2.Split("-").ToList();

                    model.SizeColorToProducts = _context.SizeColorToProducts.Include(cp => cp.ColorToProduct).ThenInclude(pi => pi.Product)
                                                                      .Where(sp => cartList.Any(cl => cl == sp.Id.ToString())).ToList();
                }
                return View(model);
            }


            string cart3 = Request.Cookies["cart"];
            if (!string.IsNullOrEmpty(cart3))
            {
                List<string> cartList = cart3.Split("-").ToList();

                model.SizeColorToProducts = _context.SizeColorToProducts.Include(cp => cp.ColorToProduct).ThenInclude(pi => pi.Product)
                                                                  .Where(sp => cartList.Any(cl => cl == sp.Id.ToString())).ToList();
            }
            return View(model);
        }
    }
}
