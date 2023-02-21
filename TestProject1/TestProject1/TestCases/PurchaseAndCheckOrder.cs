﻿using OpenQA.Selenium;
using FinalProject.POMClasses;
using FinalProject.Utils;
using System.Security.Principal;

namespace FinalProject.TestCases
{
    internal class PurchaseAndCheckOrder : BaseTest
    {
        [Test]
        public void TestCase2()
        {
            MyHelpers help = new MyHelpers(driver);

            // Load in username and password from external file.
            string username = help.LoadParameterFromRunsettings("username");
            string password = help.LoadParameterFromRunsettings("password");

            LoginPagePOM login = new LoginPagePOM(driver);
            login.Login(username, password);

            SiteWidePOM site = new SiteWidePOM(driver);
            site.NavigateUsingNavLink("Shop");

            ShopPOM shop = new ShopPOM(driver);
            shop.AddProductToCart("Belt");

            site.NavigateUsingNavLink("Cart");

            // Consider using site.NavigateUsingNavLink here instead.
            CartPOM cart = new CartPOM(driver);
            cart.GoToCheckout();

            CheckoutPOM checkout = new CheckoutPOM(driver);
            checkout.EnterDetails();
            checkout.PlaceOrder();
            string orderNumber = checkout.GetOrderNumber();

            help.TakeScreensot("Order_received_page");

            site.NavigateUsingNavLink("My account");
            site.NavigateUsingNavLink("Orders");
            MyAccountPOM account = new MyAccountPOM(driver);
            string accountOrderNumber = account.GetRecentOrderNumber();

            Assert.That(orderNumber, Is.EqualTo(accountOrderNumber));

            help.TakeScreensot("My_account_Orders_page");

            login.Logout();
        }
    }
}
