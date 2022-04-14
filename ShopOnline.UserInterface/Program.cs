using ShopOnline.BL;
using ShopOnline.DataAccessLayer;
using ShopOnline.Model;
using ShopOnlineApp.Proxy;
using System;
using System.Linq;

namespace ShopOnline.UserInterface
{
    class Program
    {
        //public static IUserDAO userDAO;
        //public static IProductDAO productDAO;
        //public static IPurchaseDAO purchaseDAO;

        public static IUserProxy userProxy;
        public static IProductProxy productProxy;
        public static IPurchaseProxy purchaseProxy;

        //public static BusinessLogic businessLogic;

        public static LoginResult loginResult;
        static void Main(string[] args)
        {

            //userDAO = new UserDAOForDB();
            //productDAO = new ProductDAOForDB();
            //purchaseDAO = new PurchaseDAOForDB();

            userProxy = new UserRESTProxy();
            productProxy = new ProductRESTProxy();
            purchaseProxy = new PurchaseRESTProxy();

            //businessLogic = new BusinessLogic(userDAO, purchaseDAO, productDAO);

            User userToLogin = new User();


            Console.WriteLine("Hi! Welcome to ShopOnline App! Log in to use it or register!");

            bool isLogOrReg = true;
            do
            {
                Console.WriteLine("If you want to log in type Log, if you want to register type Reg!");
                string answerLoginOrRegister = Console.ReadLine();

                if (string.Equals(answerLoginOrRegister, "Reg", StringComparison.InvariantCultureIgnoreCase))
                {
                    User newUser = new User();
                    bool choiceInsertCompleteDataForReg = true;
                    do
                    {
                        Console.Write("Insert Username: ");
                        newUser.Username = Console.ReadLine();
                        Console.Write("Insert Password: ");
                        newUser.Password = Console.ReadLine();
                        bool choiceRole = true;
                        string answerRole = string.Empty;
                        do
                        {
                            Console.Write("Insert Role (type Administrator for admin role, type User for user role): ");
                            answerRole = Console.ReadLine();
                            if (answerRole == "Administrator" || answerRole == "User")
                            {
                                choiceRole = false;
                            }
                            else if (string.IsNullOrEmpty(answerRole))
                            {
                                answerRole = "None";
                            }
                            else
                            {
                                Console.WriteLine("Error! Double check that you have entered the roles correctly.");
                            }
                        } while (choiceRole);

                        newUser.Roles = (Role)Enum.Parse(typeof(Role), answerRole);

                        if (string.IsNullOrEmpty(newUser.Username) || string.IsNullOrEmpty(newUser.Password) || newUser.Roles.ToString() == "None")
                        {
                            Console.WriteLine("You are miss a field! Re-enter the data!");
                        }
                        else
                        {
                            choiceInsertCompleteDataForReg = false;
                        }
                    } while (choiceInsertCompleteDataForReg);

                    RegistrationResult registrationResult = new RegistrationResult();

                    registrationResult = userProxy.Registration(newUser);

                    if (registrationResult.isRegistered)
                    {
                        Console.WriteLine($"Compliment {registrationResult.Username}! You are registered!");
                    }
                    else
                    {
                        Console.WriteLine("Oops! The user already exist! Try again!");
                    }

                }
                else if (string.Equals(answerLoginOrRegister, "Log", StringComparison.InvariantCultureIgnoreCase))
                {

                    bool isLogInOrNot = true;

                    do
                    {
                        Console.Write("Username: ");
                        userToLogin.Username = Console.ReadLine();
                        Console.Write("Password: ");
                        userToLogin.Password = Console.ReadLine();

                        if (string.IsNullOrEmpty(userToLogin.Username) || string.IsNullOrEmpty(userToLogin.Password))
                        {
                            Console.WriteLine("You are miss a field! Re-enter the data!"); ;
                        }
                        else
                        {

                            loginResult = userProxy.Login(userToLogin);

                            if (loginResult.IsLogged && loginResult.UserLogged.FirstOrDefault().Roles.ToString() == "Administrator")
                            {
                                Console.WriteLine($"Hi Admin {loginResult.UserLogged.FirstOrDefault().Username}! You are logged!");
                                isLogInOrNot = !loginResult.IsLogged;
                            }
                            else if (loginResult.IsLogged && loginResult.UserLogged.FirstOrDefault().Roles.ToString() == "User")
                            {
                                Console.WriteLine($"Hi User {loginResult.UserLogged.FirstOrDefault().Username}! You are logged!");
                                isLogInOrNot = !loginResult.IsLogged;
                            }
                            else if (!loginResult.IsLogged)
                            {
                                Console.WriteLine("There is something wrong! Re-enter the data!");
                                Console.WriteLine("Do you want to exit to the application? Type Yes if you want to exit or click Enter to retry login.");
                                string answerForRetryLogin = Console.ReadLine();
                                if (string.Equals(answerForRetryLogin, "Yes", StringComparison.InvariantCultureIgnoreCase))
                                {
                                    return;
                                }
                            }

                        }

                    } while (isLogInOrNot);
                    if (loginResult.IsLogged)
                    {
                        isLogOrReg = !loginResult.IsLogged;
                    }
                }
            } while (isLogOrReg);

            bool alwaysOpen = true;
            do
            {
                if (loginResult.IsLogged && loginResult.UserLogged.FirstOrDefault().Roles.ToString() == "Administrator")
                {
                    Console.WriteLine($"Admin {loginResult.UserLogged.FirstOrDefault().Username}, what action do you want to do? \n1 - Search Product\n2 - Add Product\n3 - Update Product\n4 - Delete Product\n" +
                        $"5 - Purchase Product\n6 - Return Product\n7 - Search Purchase\n8 - Exit");
                    bool success = int.TryParse(Console.ReadLine(), out int input);

                    if (success)
                    {
                        switch (input)
                        {
                            case 1:
                                Console.WriteLine("You are in Product Search! " +
                                    "\nPlease enter the search fields that will be shown to you (it is not necessary to enter all of them): ");

                                SearchProduct();

                                break;

                            case 2:
                                Console.WriteLine("You are in Add Product!" +
                                    "\nPlease enter all the product fields.");
                                AddResult addResult = new AddResult();

                                Product productToAdd = new Product();
                                bool choiceForInsertCompleteDataForAdd = true;
                                do
                                {
                                    Console.Write("Product Name: ");
                                    productToAdd.ProductName = Console.ReadLine();
                                    Console.Write("Brand: ");
                                    productToAdd.Brand = Console.ReadLine();
                                    Console.Write("Product Cost: ");
                                    productToAdd.Cost = Convert.ToInt32(Console.ReadLine());
                                    Console.Write("Product Quantity: ");
                                    productToAdd.Quantity = Convert.ToInt32(Console.ReadLine());
                                    productToAdd.Sectors = InsertSector(productToAdd).Sectors;


                                    if (string.IsNullOrEmpty(productToAdd.ProductName) || string.IsNullOrEmpty(productToAdd.Brand) || productToAdd.Cost == 0 || productToAdd.Quantity == 0 || productToAdd.Sectors.ToString() == "None")
                                    {
                                        Console.WriteLine("You are miss a field! Re-enter the data!");
                                    }
                                    else
                                    {
                                        choiceForInsertCompleteDataForAdd = false;
                                    }

                                } while (choiceForInsertCompleteDataForAdd);


                                addResult = productProxy.AddProduct(productToAdd);

                                if (addResult.isAdded)
                                {
                                    Console.WriteLine($"The product {addResult.ProductAdded.ProductName} has been added!");
                                }
                                else
                                {
                                    Console.WriteLine($"The quantity of product {addResult.ProductAdded.ProductName} has been updated!");
                                }
                                break;

                            case 3:
                                Console.WriteLine("You are in Update Product!" +
                                    "\nPlease enter the fields of product that you want to modify.");
                                UpdateResult updateResult = new UpdateResult();
                                bool choiceForReEnterData = true;
                                do
                                {
                                    Product oldProduct = new Product();
                                    bool choiceForInsertCompleteDataForUpdate = true;
                                    do
                                    {
                                        Console.Write("Product Name: ");
                                        oldProduct.ProductName = Console.ReadLine();
                                        Console.Write("Brand: ");
                                        oldProduct.Brand = Console.ReadLine();
                                        oldProduct.Sectors = InsertSector(oldProduct).Sectors;


                                        if (string.IsNullOrEmpty(oldProduct.ProductName) || string.IsNullOrEmpty(oldProduct.Brand) || oldProduct.Sectors.ToString() == "None")
                                        {
                                            Console.WriteLine("You are miss a field! Re-enter the data!");
                                        }
                                        else
                                        {
                                            choiceForInsertCompleteDataForUpdate = false;
                                        }
                                    } while (choiceForInsertCompleteDataForUpdate);

                                    Console.WriteLine("Now, enter the new fields.");
                                    Product newProduct = new Product();
                                    bool choiceForInsertCompleteDataForUpdateNewProduct = true;
                                    do
                                    {
                                        Console.Write("Product Name: ");
                                        newProduct.ProductName = Console.ReadLine();
                                        Console.Write("Brand: ");
                                        newProduct.Brand = Console.ReadLine();
                                        bool conv = true;
                                        do
                                        {
                                            Console.Write("Product Cost: ");
                                            conv = !int.TryParse(Console.ReadLine(), out int cost);
                                            if (!conv)
                                            {
                                                newProduct.Cost = cost;
                                            }
                                            else
                                            {
                                                Console.WriteLine("Attention, you have to enter a number! ");
                                            }
                                        } while (conv);
                                        newProduct.Sectors = InsertSector(newProduct).Sectors;

                                        if (string.IsNullOrEmpty(newProduct.ProductName) || string.IsNullOrEmpty(newProduct.Brand) || newProduct.Cost == 0 || newProduct.Sectors.ToString() == "None")
                                        {
                                            Console.WriteLine("You are miss a field! Re-enter the data!");
                                        }
                                        else
                                        {
                                            choiceForInsertCompleteDataForUpdateNewProduct = false;
                                        }
                                    } while (choiceForInsertCompleteDataForUpdateNewProduct);

                                    updateResult = productProxy.UpdateProduct(oldProduct, newProduct);

                                    if (updateResult.isUpToDate)
                                    {
                                        Console.WriteLine("The product is up to date.");
                                        choiceForReEnterData = false;
                                    }
                                    else
                                    {
                                        if (updateResult.isNotExisting)
                                        {
                                            Console.WriteLine("The old product does not exist. Re-enter the data.");
                                        }
                                        else if (updateResult.changeAlreadyExist)
                                        {
                                            Console.WriteLine("Changes already exist. Re-enter the data.");
                                        }
                                    }

                                } while (choiceForReEnterData);

                                break;

                            case 4:
                                Console.WriteLine("You are in Delete Product!" +
                                    "\nPlease enter the product fields that you want to delete.");

                                DeleteResult deleteResult = new DeleteResult();

                                Product productToDelete = new Product();
                                bool choiceForReEnterProductToDelete = true;
                                do
                                {
                                    bool choiceForReEnterDataToDelete = true;
                                    do
                                    {
                                        Console.Write("ProductName: ");
                                        productToDelete.ProductName = Console.ReadLine();
                                        Console.Write("Brand: ");
                                        productToDelete.Brand = Console.ReadLine();
                                        productToDelete.Sectors = InsertSector(productToDelete).Sectors;

                                        if (productToDelete.ProductName == null || productToDelete.Brand == null || productToDelete.Sectors.ToString() == "None")
                                        {
                                            Console.WriteLine("You are miss a field! Re-enter the data!");
                                        }
                                    } while (choiceForReEnterDataToDelete);

                                    deleteResult = productProxy.DeleteProduct(productToDelete);

                                    if (deleteResult.isDeleted)
                                    {
                                        Console.WriteLine($"The product {deleteResult.ProductDeleted.ProductName} has been deleted!");
                                        choiceForReEnterProductToDelete = false;
                                    }
                                    else
                                    {
                                        Console.WriteLine("The product does not exist! Re-enter the data.");
                                    }
                                } while (choiceForReEnterProductToDelete);

                                break;

                            case 5:
                                Console.WriteLine("You are in Purchase Product!" +
                                    "Please, Enter all fields to view a specific product " +
                                    "or some of them to filter products by Name, Brand or Sector" +
                                    " or leave no fields to view all products.");

                                bool choiceForProductPurchase = true;
                                do
                                {
                                    SearchProduct();

                                    ShoppingResult shoppingResult = new ShoppingResult();

                                    Console.WriteLine("Which of these products would you like to buy?");
                                    Product productToPurchase = new Product();
                                    Console.Write("Product Name: ");
                                    productToPurchase.ProductName = Console.ReadLine();

                                    shoppingResult = purchaseProxy.Shopping(productToPurchase, loginResult.UserLogged.FirstOrDefault().UserId);

                                    if (shoppingResult.isPurchase && !shoppingResult.productDoesNotExist)
                                    {
                                        Console.WriteLine($"Compliment! You bought {shoppingResult.ProductPurchased.ProductName}!");
                                        choiceForProductPurchase = false;
                                    }
                                    else if (!shoppingResult.isPurchase && !shoppingResult.productDoesNotExist)
                                    {
                                        Console.WriteLine("You have already bought this product. Choose another one!");
                                    }
                                    else if (shoppingResult.productDoesNotExist)
                                    {
                                        Console.WriteLine("This product does not exist! Choose another one!");
                                    }
                                } while (choiceForProductPurchase);

                                break;

                            case 6:
                                Console.WriteLine("You are in Return Product" +
                                    "\nPlease, enter the name of the product that you want to return.");

                                ResultReturn resultReturn = new ResultReturn();

                                Product productForReturn = new Product();
                                bool choiceForReEnterProductNameForReturnProduct = true;
                                do
                                {
                                    Console.Write("Product Name: ");
                                    productForReturn.ProductName = Console.ReadLine();

                                    resultReturn = purchaseProxy.ReturnProduct(productForReturn, loginResult.UserLogged.FirstOrDefault().UserId);

                                    if (resultReturn.isSuccessful && resultReturn.isAvailable)
                                    {
                                        Console.WriteLine($"The product {resultReturn.ProductToReturn.ProductName} has been returned!" +
                                            $"\nThe product is available again!");
                                        choiceForReEnterProductNameForReturnProduct = false;
                                    }
                                    else if (resultReturn.isSuccessful && !resultReturn.isAvailable)
                                    {
                                        Console.WriteLine($"The product {resultReturn.ProductToReturn.ProductName} has been returned!");
                                        choiceForReEnterProductNameForReturnProduct = false;
                                    }
                                    else if (!resultReturn.isSuccessful)
                                    {
                                        Console.WriteLine("The product does not present! Please, re-enter the product name.");
                                    }
                                    else if (!resultReturn.isAvailable)
                                    {
                                        Console.WriteLine("The product has never been bought. Please, re-enter the product name.");
                                    }
                                } while (choiceForReEnterProductNameForReturnProduct);

                                break;

                            case 7:
                                Console.WriteLine("You are in Search Purchases!" +
                                    "\nPlease, select the filters you would like to use.");
                                SearchPurchaseResult searchPurchaseResult = new SearchPurchaseResult();
                                bool returnOrNot = true;
                                Product productForFilter = new Product();
                                User userForFilter = new User();
                                Console.WriteLine("Would you like to filter by Product? Type Yes or Not.");
                                string answerFilterProduct = Console.ReadLine();
                                if (string.Equals(answerFilterProduct, "Yes", StringComparison.InvariantCultureIgnoreCase))
                                {
                                    Console.Write("Product Name: ");
                                    productForFilter.ProductName = Console.ReadLine();
                                    Console.Write("Brand: ");
                                    productForFilter.Brand = Console.ReadLine();
                                    InsertSector(productForFilter);

                                    FilterByUser(productForFilter,userForFilter,returnOrNot,searchPurchaseResult);
                                }
                                else if (string.Equals(answerFilterProduct, "Not", StringComparison.InvariantCultureIgnoreCase))
                                {
                                    FilterByUser(productForFilter, userForFilter, returnOrNot, searchPurchaseResult);
                                }

                                break;

                            case 8:
                                return;
                        }
                    }
                    else
                    {
                        continue;
                    }

                }
                else if (loginResult.IsLogged && loginResult.UserLogged.FirstOrDefault().Roles.ToString() == "User")
                {
                    Console.WriteLine("What action do you wabt to do? \n1 - Search Product\n2 - Purchase Product\n3 - Return Product\n4 - Exit");
                    bool success = int.TryParse(Console.ReadLine(), out int input);

                    if (success)
                    {
                        switch (input)
                        {
                            case 1:
                                Console.WriteLine("You are in Product Search! " +
                                    "\nPlease enter the search fields that will be shown to you (it is not necessary to enter all of them): ");

                                SearchProduct();

                                break;
                            case 2:
                                Console.WriteLine("You are in Purchase Product!" +
                                    "Please, Enter all fields to view a specific product " +
                                    "or some of them to filter products by Name, Brand or Sector" +
                                    " or leave no fields to view all products.");

                                bool choiceForProductPurchase = true;
                                do
                                {
                                    SearchProduct();

                                    ShoppingResult shoppingResult = new ShoppingResult();

                                    Console.WriteLine("Which of these products would you like to buy?");
                                    Product productToPurchase = new Product();
                                    Console.Write("Product Name: ");
                                    productToPurchase.ProductName = Console.ReadLine();

                                    shoppingResult = purchaseProxy.Shopping(productToPurchase, loginResult.UserLogged.FirstOrDefault().UserId);

                                    if (shoppingResult.isPurchase && !shoppingResult.productDoesNotExist)
                                    {
                                        Console.WriteLine($"Compliment! You bought {shoppingResult.ProductPurchased.ProductName}!");
                                        choiceForProductPurchase = false;
                                    }
                                    else if (!shoppingResult.isPurchase && !shoppingResult.productDoesNotExist)
                                    {
                                        Console.WriteLine("You have already bought this product. Choose another one!");
                                    }
                                    else if (shoppingResult.productDoesNotExist)
                                    {
                                        Console.WriteLine("This product does not exist! Choose another one!");
                                    }
                                } while (choiceForProductPurchase);

                                break;

                            case 3:
                                Console.WriteLine("You are in Return Product" +
                                    "\nPlease, enter the name of the product that you want to return.");

                                ResultReturn resultReturn = new ResultReturn();

                                Product productForReturn = new Product();
                                bool choiceForReEnterProductNameForReturnProduct = true;
                                do
                                {
                                    Console.Write("Product Name: ");
                                    productForReturn.ProductName = Console.ReadLine();

                                    resultReturn = purchaseProxy.ReturnProduct(productForReturn, loginResult.UserLogged.FirstOrDefault().UserId);

                                    if (resultReturn.isSuccessful && resultReturn.isAvailable)
                                    {
                                        Console.WriteLine($"The product {resultReturn.ProductToReturn.ProductName} has been returned!" +
                                            $"\nThe product is available again!");
                                        choiceForReEnterProductNameForReturnProduct = false;
                                    }
                                    else if (resultReturn.isSuccessful && !resultReturn.isAvailable)
                                    {
                                        Console.WriteLine($"The product {resultReturn.ProductToReturn.ProductName} has been returned!");
                                        choiceForReEnterProductNameForReturnProduct = false;
                                    }
                                    else if (!resultReturn.isSuccessful)
                                    {
                                        Console.WriteLine("The product does not present! Please, re-enter the product name.");
                                    }
                                    else if (!resultReturn.isAvailable)
                                    {
                                        Console.WriteLine("The product has never been bought. Please, re-enter the product name.");
                                    }
                                } while (choiceForReEnterProductNameForReturnProduct);

                                break;

                            case 4:
                                return;
                        }
                    }
                    else
                    {
                        continue;
                    }

                }

            } while (alwaysOpen);

        }

        private static Product InsertSector(Product productToSearch)
        {
            bool choiceSectorForSearch = true;
            string answerSector = string.Empty;
            do
            {
                Console.Write("Product Sector (type one of this: Food, Tool, Technology, Sport): ");
                answerSector = Console.ReadLine();
                if (string.IsNullOrEmpty(answerSector))
                {
                    answerSector = "None";
                    productToSearch.Sectors = (Sector)Enum.Parse(typeof(Sector), answerSector);
                    choiceSectorForSearch = false;
                }
                else if (answerSector == "Food" || answerSector == "Tool" || answerSector == "Technology" || answerSector == "Sport")
                {
                    productToSearch.Sectors = (Sector)Enum.Parse(typeof(Sector), answerSector);
                    choiceSectorForSearch = false;
                }
                else
                {
                    productToSearch.Sectors = (Sector)Enum.Parse(typeof(Sector), answerSector);
                    choiceSectorForSearch = false;
                }
            } while (choiceSectorForSearch);

            return productToSearch;
        }

        private static void SearchProduct()
        {
            SearchResult searchResultForPurchase = new SearchResult();

            Product productToSearchForPurchase = new Product();
            Console.Write("ProductName: ");
            productToSearchForPurchase.ProductName = Console.ReadLine();
            Console.Write("Brand: ");
            productToSearchForPurchase.Brand = Console.ReadLine();
            productToSearchForPurchase.Sectors = InsertSector(productToSearchForPurchase).Sectors;

            Console.WriteLine("");
            searchResultForPurchase = productProxy.SearchProducts(productToSearchForPurchase);

            if (searchResultForPurchase.isFound)
            {
                foreach (var s in searchResultForPurchase.Products)
                {
                    if (s.Quantity > 3)
                    {
                        Console.WriteLine($"Product name: {s.ProductName}\nBrand: {s.Brand}\nCost: {s.Cost}\nSector: {s.Sectors.ToString()}\nIt's available.\n");
                    }
                    else if (s.Quantity > 0 && s.Quantity <= 3)
                    {
                        Console.WriteLine($"Product name: {s.ProductName}\nBrand: {s.Brand}\nCost: {s.Cost}\nSector: {s.Sectors.ToString()}\nQuantity: Only {s.Quantity}. Availability is ending.\n");
                    }
                    else
                    {
                        Console.WriteLine($"Product name: {s.ProductName}\nBrand: {s.Brand}\nCost: {s.Cost}\nSector: {s.Sectors.ToString()}\nThe product isn't available!\n");
                    }
                }
            }
            else
            {
                Console.WriteLine("\nWe are sorry! The product is not present in the store!");
            }
        }

        private static void FilterByRetOrPur(Product productForFilter, User userForFilter, bool returnOrNot, SearchPurchaseResult searchPurchaseResult)
        {
            Console.WriteLine("Would you like to filter with respect to purchases or returns? Type Pur for purchases or Ret for returns.");
            string answerFilterPurOrRet = Console.ReadLine();
            if (string.Equals(answerFilterPurOrRet, "Pur", StringComparison.InvariantCultureIgnoreCase))
            {
                searchPurchaseResult = purchaseProxy.SearchPurchase(productForFilter, userForFilter, !returnOrNot);

                foreach (var s in searchPurchaseResult.PurchaseList)
                {
                    Console.WriteLine($"Product Name: {s.ProductName}\nBrand: {s.Brand}\nCost: {s.Cost}\nSector: {s.Sector}\nShop Date: {s.ShopDate}\n");
                }
            }
            else if (string.Equals(answerFilterPurOrRet, "Ret", StringComparison.InvariantCultureIgnoreCase))
            {
                searchPurchaseResult = purchaseProxy.SearchPurchase(productForFilter, userForFilter, returnOrNot);

                foreach (var s in searchPurchaseResult.PurchaseList)
                {
                    Console.WriteLine($"Product Name: {s.ProductName}\nBrand: {s.Brand}\nCost: {s.Cost}\nSector: {s.Sector}\nShop Date: {s.ShopDate}\nReturn Date: {s.ReturnDate}\n");
                }
            }
        }

        private static void FilterByUser(Product productForFilter, User userForFilter, bool returnOrNot, SearchPurchaseResult searchPurchaseResult)
        {
            Console.WriteLine("Would you like to filter by User? Type Yes or Not.");
            string answerFilterUser = Console.ReadLine();
            if (string.Equals(answerFilterUser, "Yes", StringComparison.InvariantCultureIgnoreCase))
            {
                userForFilter = loginResult.UserLogged.FirstOrDefault();

                FilterByRetOrPur(productForFilter, userForFilter, returnOrNot, searchPurchaseResult);
            }
            else if (string.Equals(answerFilterUser, "Not", StringComparison.InvariantCultureIgnoreCase))
            {
                FilterByRetOrPur(productForFilter, userForFilter, returnOrNot, searchPurchaseResult);
            }
        }
    }
}
