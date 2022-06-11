using EM.Models;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace EM.Processing
{
    public class ProductDictionaryFileManager : IDisposable
    {
        private const string PYTHON_SERVER_HOST = "192.168.100.14"; //facultate: "10.146.1.102"; /"10.146.1.156"; /"10.146.1.98"; 
        private const int PYTHON_PORT = 9000;


        private List<ProductDictionary> productDictionaryList;
        private List<string> productNameList;
        private string productDictionaryString;
        public bool productDictionaryListWasModified;

        public ProductDictionaryFileManager()
        {
            productDictionaryString = GetProductDictionaryJsonFileString();
            if (productDictionaryString != null)
            {
                UpdateListWithInformationWithinJsonFile();
                UpdateProductNameList();
            }
            productDictionaryListWasModified = false;
        }

        #region Properties
        public string ProductDictionaryString
        {
            get => productDictionaryString;
            set
            {
                productDictionaryString = value;
            }
        }

        public List<ProductDictionary> ProductDictionaryList
        {
            get => productDictionaryList;
            set
            {
                productDictionaryList = value;
                productDictionaryListWasModified = true;
            }
        }

        public List<string> ProductNameList
        {
            get => productNameList;
            set
            {
                productNameList = value;
            }
        }
        #endregion


        //https://restsharp.dev/usage.html#making-a-call
        public string GetProductDictionaryJsonFileString()
        {
            var client = new RestClient("http://" + PYTHON_SERVER_HOST + ":" + PYTHON_PORT + "/json");

            var request = new RestRequest();
            var responseMessage = client.ExecuteGetAsync(request).Result;

            return responseMessage.Content.ToString();
        }

        public async void UpdateProductDictionaryJsonFile()
        {
            var client = new RestClient("http://" + PYTHON_SERVER_HOST + ":" + PYTHON_PORT + "/json");

            var jsonBody = JsonConvert.SerializeObject(productDictionaryList);

            var request = new RestRequest().AddJsonBody(jsonBody);

            var responseMessage = await client.PostAsync(request);

            var x = responseMessage.Content.ToString();
            Console.WriteLine("x" + x);
        }

        private void UpdateListWithInformationWithinJsonFile()
        {
            productDictionaryList = JsonConvert.DeserializeObject<List<ProductDictionary>>(productDictionaryString);
            ShowList();
        }

        public void UpdateProductNameList()
        {
            productNameList = new List<string>();
            foreach (ProductDictionary productDictionary in productDictionaryList)
            {
                productNameList.Add(productDictionary.ProdName);
            }
        }

        public void AddItemToList(ProductDictionary productDictionary)
        {
            if (!productDictionaryList.Contains(productDictionary))
            {
                productDictionaryList.Add(productDictionary);
            }
        }

        public string ConvertFromTableToJsonString()
        {
            return JsonConvert.SerializeObject(productDictionaryList);
        }

        public ProductDictionary GetProductDictionaryByProductName(string productName)
        {
            foreach (ProductDictionary productDictionary in productDictionaryList)
            {
                if (productDictionary.ProdName.Equals(productName))
                    return productDictionary;
            }
            return null;
        }

        public string GetProductCategoryByProductName(string productName)
        {
            foreach (ProductDictionary productDictionary in productDictionaryList)
            {
                if (productDictionary.ProdName.Equals(productName))
                    return productDictionary.ProdCategory;
            }
            return null;
        }

        public void ShowList()
        {
            foreach (ProductDictionary product in productDictionaryList)
            {
                Console.WriteLine(" productDictionaryProductName: " + product.ProdName +
                    " Category: " + product.ProdCategory);
            }
        }

        public void Dispose()
        {
            if (productDictionaryListWasModified)
            {
                UpdateProductDictionaryJsonFile();
            }
        }
    }
}
