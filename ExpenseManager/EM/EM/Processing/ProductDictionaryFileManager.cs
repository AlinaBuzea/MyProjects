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
            var client = new RestClient("http://" + App.pythonServerHost + ":" + App.pythonPort + "/json");

            var request = new RestRequest();
            var responseMessage = client.ExecuteGetAsync(request).Result;

            if (responseMessage != null)
            {
                return responseMessage.Content.ToString();
            }
            return null;
        }

        public async void UpdateProductDictionaryJsonFile()
        {
            var client = new RestClient("http://" + App.pythonServerHost + ":" + App.pythonPort + "/json");

            var jsonBody = JsonConvert.SerializeObject(productDictionaryList);

            var request = new RestRequest().AddJsonBody(jsonBody);

            _ = await client.PostAsync(request);
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
