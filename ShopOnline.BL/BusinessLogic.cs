using ShopOnline.DataAccessLayer;
using ShopOnline.Model;
using System.Collections.Generic;
using System.Linq;
using System;

namespace ShopOnline.BL
{
    public class BusinessLogic
    {
        private IUserDAO _userDAO;
        private IProductDAO _productDAO;
        private IPurchaseDAO _purchaseDAO;

        public BusinessLogic(IUserDAO userDAO, IPurchaseDAO purchaseDAO, IProductDAO productDAO)
        {
            _userDAO = userDAO;
            _purchaseDAO = purchaseDAO;
            _productDAO = productDAO;
        }

        public LoginResult Login(User user)
        {
            LoginResult loginResult = new LoginResult();

            loginResult.UserLogged = _userDAO.GetUsers(user);

            loginResult.IsLogged = loginResult.UserLogged.Any();

            return loginResult;
        }

        public RegistrationResult Registration(User user)
        {
            List<User> users = new List<User>();
            RegistrationResult registrationResult = new RegistrationResult();

            users = _userDAO.GetUsers(user);

            registrationResult.isRegistered = users.Any();
            registrationResult.Username = user.Username;

            if (!registrationResult.isRegistered)
                _userDAO.CreateUser(user);

            return registrationResult;

        }

        public SearchResult SearchProducts(Product product)
        {
            SearchResult searchResult = new SearchResult();

            searchResult.Products = _productDAO.GetProducts(product);
            searchResult.isFound = searchResult.Products.Any();

            return searchResult;

        }

        public AddResult AddProduct(Product product)
        {
            List<Product> products = new List<Product>();
            AddResult addResultProduct = new AddResult();

            products = _productDAO.GetProducts(product);

            addResultProduct.isAdded = products.Any();
            addResultProduct.ProductAdded = product;

            if (products.Any())
            {
                products.FirstOrDefault().Quantity += 1;
                _productDAO.UpdateProduct(products.FirstOrDefault());
            }
            else
            {
                _productDAO.CreateProduct(product);
            }

            return addResultProduct;
        }

        public UpdateResult UpdateProduct(Product oldProduct, Product newProduct)
        {
            List<Product> products = _productDAO.GetProducts(oldProduct);
            UpdateResult updateResult = new UpdateResult();

            updateResult.isNotExisting = !products.Any();
            List<Product> newProductList = _productDAO.GetProducts(newProduct);

            if (!newProductList.Any())
            {
                Product productToUpdate = new Product()
                {
                    ProductId = products.FirstOrDefault().ProductId,
                    ProductName = newProduct.ProductName,
                    Brand = newProduct.Brand,
                    Cost = newProduct.Cost,
                    Sectors = newProduct.Sectors,
                    Quantity = products.FirstOrDefault().Quantity
                };
                _productDAO.UpdateProduct(productToUpdate);
                updateResult.isUpToDate = !newProductList.Any();
            }
            else
            {
                updateResult.changeAlreadyExist = newProductList.Any();
            }

            return updateResult;
        }

        public DeleteResult DeleteProduct(Product product)
        {
            List<Product> productToDelete = _productDAO.GetProducts(product);
            DeleteResult deleteResult = new DeleteResult();

            if (productToDelete.Any())
            {
                _productDAO.DeleteProduct(productToDelete.FirstOrDefault().ProductId);
                deleteResult.ProductDeleted = productToDelete.FirstOrDefault();
                deleteResult.isDeleted = productToDelete.Any();
            }
            else
            {
                deleteResult.isDeleted = productToDelete.Any();
            }

            return deleteResult;
        }

        public ShoppingResult Shopping(Product product, int idUser)
        {
            List<Product> productList = _productDAO.GetProducts(product);
            ShoppingResult shoppingResult = new ShoppingResult();

            if (productList.Any())
            {
                Purchase purchaseToAdd = new Purchase()
                {
                    ProductId = productList.FirstOrDefault().ProductId,
                    UserId = idUser
                };

                List<Purchase> purchaseList = _purchaseDAO.GetPurchases(purchaseToAdd);

                List<Purchase> purchasesReturn = new List<Purchase>();
                foreach (var p in purchaseList)
                {
                    if (p.ReturnDate != null)
                        purchasesReturn.Add(p);
                }

                if (!purchaseList.Any() || purchasesReturn.Count() == purchaseList.Count())
                {
                    purchaseToAdd.ShopDate = DateTime.Now;
                    _purchaseDAO.CreatePurchase(purchaseToAdd);
                    productList.FirstOrDefault().Quantity -= 1;
                    _productDAO.UpdateProduct(productList.FirstOrDefault());
                    shoppingResult.ProductPurchased = productList.FirstOrDefault();
                    shoppingResult.isPurchase = true;
                }
                else
                {
                    shoppingResult.isPurchase = false;
                }
            }
            else
            {
                shoppingResult.productDoesNotExist = !productList.Any();
            }

            return shoppingResult;
        }

        public ResultReturn ReturnProduct(Product product, int idUser)
        {
            ResultReturn resultReturn = new ResultReturn()
            {
                ProductToReturn = product
            };

            List<Product> productList = _productDAO.GetProducts(product);

            if (productList.Any())
            {
                Purchase purchase = new Purchase()
                {
                    ProductId = productList.FirstOrDefault().ProductId,
                    UserId = idUser
                };

                List<Purchase> purchaseList = _purchaseDAO.GetPurchases(purchase);

                foreach (var p in purchaseList)
                {
                    if (p.ReturnDate == null)
                    {
                        p.ReturnDate = DateTime.Now;
                        _purchaseDAO.EditPurchase(p);

                        resultReturn.isAvailable = productList.FirstOrDefault().Quantity == 0;

                        productList.FirstOrDefault().Quantity += 1;
                        _productDAO.UpdateProduct(productList.FirstOrDefault());
                        resultReturn.isSuccessful = p.ReturnDate == null;
                    }
                    else
                    {
                        resultReturn.isSuccessful = p.ReturnDate == null;
                    }
                }
            }
            else
            {
                resultReturn.isSuccessful = productList.Any();
            }

            return resultReturn;
        }

        public SearchPurchaseResult SearchPurchase(Product product, User user, bool returnOrNot)
        {
            List<Product> products = _productDAO.GetProducts(product);
            List<User> users = _userDAO.GetUsers(user);
            Purchase resultPurchase = new Purchase();
            List<Purchase> resultPurchaseList = new List<Purchase>();
            SearchPurchaseResult searchPurchaseResult = new SearchPurchaseResult();
            List<SearchPurchaseResult> searchPurchaseResultsList = new List<SearchPurchaseResult>();
            List<Purchase> purchasesByProductAndUserFilter = new List<Purchase>();
            List<Product> productList = new List<Product>();
            searchPurchaseResult.PurchaseList = new List<SearchPurchaseResult>();

            searchPurchaseResult.isSuccessful = products.Any();

            foreach (var p in products)
            {
                foreach (var u in users)
                {
                    Purchase purchase = new Purchase()
                    {
                        ProductId = p.ProductId,
                        UserId = u.UserId
                    };
                    purchasesByProductAndUserFilter = _purchaseDAO.GetPurchases(purchase)?.Distinct().ToList();
                    resultPurchaseList.AddRange(returnOrNot ? purchasesByProductAndUserFilter.Where(purchases => purchases.ReturnDate > purchases.ShopDate).ToList() : purchasesByProductAndUserFilter.Where(purchases => purchases.ReturnDate == null).ToList());
                }
            }

            foreach (var r in resultPurchaseList)
            {
                Product productByResultList = new Product()
                {
                    ProductId = r.ProductId
                };
                productList.Add(_productDAO.GetProducts(productByResultList).FirstOrDefault());
            }

            foreach (var r in resultPurchaseList)
            {
                foreach (var p in productList)
                {
                    if(r.ProductId == p.ProductId)
                    {
                        SearchPurchaseResult purchaseInList = new SearchPurchaseResult()
                        {
                            ProductName = p.ProductName,
                            Brand = p.Brand,
                            Cost = p.Cost,
                            Sector = p.Sectors,
                            ShopDate = (DateTime)r.ShopDate
                        };
                        if (returnOrNot)
                            purchaseInList.ReturnDate = (DateTime)r.ReturnDate;

                        searchPurchaseResultsList.Add(purchaseInList);
                    }
                }
            }

            searchPurchaseResult.PurchaseList.AddRange(searchPurchaseResultsList);

            return searchPurchaseResult;
        }
    }
}
