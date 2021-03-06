using EM.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace EM.Processing
{
    public class InformationExtractor
    {
        public enum InformationType
        {
            ShopName,
            Address,
            ProductName,
            Category,
            Price,
            Quantity,
            Date,
            Other
        }
        private const int ProcessingError = 4;

        public List<Tuple<InformationType, string>> GetFilteredInformation(string receiptText)
        {
            receiptText = receiptText.ToUpper();
            List<string> lines = receiptText.Split('\n').ToList();

            string numberPattern = @"(\d+(\.|,)?\d+|\d{1,3}(,\d{3})*(\.\d+)?)";
            string measureUnitPattern = @"\s*(KG|BUC|UM|L|LTR|PCS)\s*(\.)*\s*";
            string quantityPattern = numberPattern + measureUnitPattern;
            string quantityPricePattern = @"\s*" + numberPattern + measureUnitPattern + @"\s*(X)\s*" + numberPattern + @"\s*";
            string priceQuantityPattern = @"\s*" + numberPattern + @"\s*(X)\s*" + numberPattern + measureUnitPattern + @"\s*";
            string datePattern = @"(\d{2}\/\d{2}\/\d{4}|\d{4}\/\d{2}\/\d{2}|\d{2}-\d{2}-\d{4}|\d{2}\.\d{2}\.\d{4})";

            List<string> shopNameAbreviations = new List<string>() { "SC", "SRC", "S.C.", "S.R.C", "S. C.", "S. R. C.", "SRL", "S.R.L." };
            List<string> shopAddressWords = new List<string>() { "JUD", "JUDET", "STR", "STRADA", "MUN", "MUNICIPIUL" }; //, "NR"
            List<string> nameAndAddressLimit = new List<string>() { "NR.CRT", "CIF", "C.I.F.", "COD IDENTIFICARE FISCALA", "TRANZACTIE", "BON", "COD FISCAL", "C.I.F", "LEL", "LEI", };
            List<string> endOfReceipt = new List<string>() { "TOTAL" };

            List<Tuple<InformationType, string>> filteredInformationTuples = new List<Tuple<InformationType, string>>();


            bool shopNameAndAddressSaved = false;
            Regex dateRegex = new Regex(datePattern);
            Regex quantityAndPriceRegex = new Regex(quantityPricePattern + "|" + priceQuantityPattern);
            bool? isPriceBeforeProductName = null;
            bool foundDate = false;
            bool totalReached = false;

            using (ProductDictionaryFileManager productDictionaryFileManager = new ProductDictionaryFileManager())
            {
                int linesNo = lines.Count();
                for (int index = 0; index < linesNo; index++)
                {
                    if (dateRegex.IsMatch(lines[index]))
                    {
                        filteredInformationTuples.Add(Tuple.Create(InformationType.Date, dateRegex.Match(lines[index]).ToString()));
                        foundDate = true;
                        if (totalReached)
                            break;
                    }
                    if (!totalReached)
                    {
                        if (nameAndAddressLimit.Any(lines[index].Contains))
                        {
                            shopNameAndAddressSaved = true;
                            continue;
                        }

                        if (!shopNameAndAddressSaved)
                        {
                            if (shopNameAbreviations.Any(lines[index].Contains))
                            {
                                filteredInformationTuples.Add(Tuple.Create(InformationType.ShopName, lines[index]));
                            }
                            else if (shopAddressWords.Any(lines[index].Contains))
                            {
                                filteredInformationTuples.Add(Tuple.Create(InformationType.Address, lines[index]));
                            }
                        }
                        else 
                        {
                            if (isPriceBeforeProductName == null)
                            {
                                
                                if (quantityAndPriceRegex.IsMatch(lines[index]))
                                {
                                    isPriceBeforeProductName = true;
                                }
                                else 
                                {
                                    string productName = FindProductNameInCurrentLine(productDictionaryFileManager.ProductNameList, lines[index]);
                                    if (productName == null || productName.Equals(""))
                                    {
                                        continue;
                                    }
                                    isPriceBeforeProductName = false;
                                }
                            }

                            Match match = quantityAndPriceRegex.Match(lines[index]);
                            if (match.Success)
                            {
                                string information = match.ToString();
                                string beforeInfo = lines[index].Remove(lines[index].IndexOf(information));
                                if (beforeInfo.Length > ProcessingError)
                                {
                                    beforeInfo = DeleteUnnecessaryInformation(InformationType.ProductName, beforeInfo);
                                    string productName = FindProductNameInCurrentLine(productDictionaryFileManager.ProductNameList, beforeInfo);
                                    if (productName == null)
                                    {
                                        continue;
                                    }
                                    filteredInformationTuples.Add(Tuple.Create(InformationType.ProductName, beforeInfo));
                                    string currentCateg = productDictionaryFileManager.GetProductCategoryByProductName(productName);
                                    filteredInformationTuples.Add(Tuple.Create(InformationType.Category, currentCateg));
                                }
                                else
                                {
                                    string productName = null;
                                    if (isPriceBeforeProductName == true)
                                    {
                                        if (index + 1 >= linesNo)
                                            break;

                                        productName = FindProductNameInCurrentLine(productDictionaryFileManager.ProductNameList, lines[index + 1]);
                                        if (productName == null)
                                        {
                                            ProductDictionary closestProdDict = Levenstein.FindTheClosestWordToTheCurrentOneFromList(lines[index + 1], productDictionaryFileManager.ProductDictionaryList);
                                            filteredInformationTuples.Add(Tuple.Create(InformationType.Category, closestProdDict.ProdCategory));
                                            filteredInformationTuples.Add(Tuple.Create(InformationType.ProductName, closestProdDict.ProdName.ToUpper()));

                                            string quantity1 = new Regex(quantityPattern).Match(information).ToString();
                                            information = information.Replace(quantity1, "");
                                            string price1 = new Regex(numberPattern).Match(information).ToString();

                                            filteredInformationTuples.Add(Tuple.Create(InformationType.Price, GetPriceByQuantityAndPricePerKGProduct(quantity1, price1)));
                                            filteredInformationTuples.Add(Tuple.Create(InformationType.Quantity, quantity1));
                                            index++;
                                            continue;
                                        }
                                        filteredInformationTuples.Add(Tuple.Create(InformationType.ProductName, lines[index + 1]));
                                        index++;
                                    }
                                    else
                                    {
                                        productName = FindProductNameInCurrentLine(productDictionaryFileManager.ProductNameList, lines[index - 1]);
                                        if (productName == null)
                                        {
                                            ProductDictionary closestProdDict = Levenstein.FindTheClosestWordToTheCurrentOneFromList(lines[index - 1], productDictionaryFileManager.ProductDictionaryList);
                                            filteredInformationTuples.Add(Tuple.Create(InformationType.Category, closestProdDict.ProdCategory));
                                            filteredInformationTuples.Add(Tuple.Create(InformationType.ProductName, closestProdDict.ProdName.ToUpper()));
                                            string quantity1 = new Regex(quantityPattern).Match(information).ToString();
                                            information = information.Replace(quantity1, "");
                                            string price1 = new Regex(numberPattern).Match(information).ToString();

                                            filteredInformationTuples.Add(Tuple.Create(InformationType.Price, GetPriceByQuantityAndPricePerKGProduct(quantity1, price1)));
                                            filteredInformationTuples.Add(Tuple.Create(InformationType.Quantity, quantity1));

                                            index++;
                                            continue;
                                        }
                                        filteredInformationTuples.Add(Tuple.Create(InformationType.ProductName, lines[index - 1]));
                                    }

                                    if (productName != null)
                                    {
                                        string currentCateg = productDictionaryFileManager.GetProductCategoryByProductName(productName);
                                        filteredInformationTuples.Add(Tuple.Create(InformationType.Category, currentCateg));
                                    }
                                }
                                string quantity = new Regex(quantityPattern).Match(information).ToString();
                                information = information.Replace(quantity, "");
                                string price = new Regex(numberPattern).Match(information).ToString();

                                filteredInformationTuples.Add(Tuple.Create(InformationType.Price, GetPriceByQuantityAndPricePerKGProduct(quantity, price)));
                                filteredInformationTuples.Add(Tuple.Create(InformationType.Quantity, quantity));

                            }
                        }
                        totalReached = endOfReceipt.Any(lines[index].Contains);
                    }
                    if (totalReached && foundDate)
                    {
                        break;
                    }
                }
                return filteredInformationTuples;
            }
        }

        private string FindProductNameInCurrentLine(List<string> productsNames, string line)
        {
            foreach (string word in productsNames)
            {
                if (line.Contains(word.ToUpper()) && !word.Equals(""))
                {
                    return word;
                }
            }
            return null;
        }

        private double? GetQuantityWithoutMeasurementUnit(string quantity)
        {
            string numberPattern = @"(\d+(\.|,)?\d+|\d{1,3}(,\d{3})*(\.\d+)?)";
            Regex numberRegex = new Regex(numberPattern);
            Match match = numberRegex.Match(quantity);
            if (match.Success)
            {
                double result = double.Parse(match.Value, CultureInfo.InvariantCulture);
                return result;
            }
            return null;
        }

        private string GetPriceByQuantityAndPricePerKGProduct(string quantity, string pricePerKG)
        {
            double? quantityWithoutMeasurementUnit = GetQuantityWithoutMeasurementUnit(quantity);
            if (quantityWithoutMeasurementUnit == null || quantityWithoutMeasurementUnit.Value == 1.0)
            {
                return pricePerKG;
            }
            double realPrice = quantityWithoutMeasurementUnit.Value * double.Parse(pricePerKG, CultureInfo.InvariantCulture);
            return (Math.Round(realPrice, 2)).ToString();
        }

        private string DeleteUnnecessaryInformation(InformationType informationType, string line)
        { 
            string result = line;
            if (informationType.Equals(InformationType.ProductName))
            {
                List<string> wordList = line.Split(' ').ToList();
                result = "";
                foreach (string word in wordList)
                {
                    int digitsNb = word.Count(character => char.IsDigit(character));
                    bool containsLetters = word.Count(character => char.IsLetter(character)) > 0;
                    if (digitsNb > 2 * word.Length / 3 && word.Length > 3 && !containsLetters)
                        continue;
                    result += word + " ";
                }
            }

            return result;
        }
    }
}
