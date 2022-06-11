using EM.Models;
using System;
using System.Collections.Generic;
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
            string measureUnitPattern = @"\s*(KG|BUC|UM|L)(\.)*\s*";// "\s*(KG|BUC|UM|L)*\s*"
            string quantityPattern = numberPattern + measureUnitPattern;
            //string tvaLetter = @"\s*(C|B|D|A|E)\s*";
            string quantityPricePattern = @"\s*" + numberPattern + measureUnitPattern + @"\s*(X)\s*" + numberPattern + @"\s*";
            string priceQuantityPattern = @"\s*" + numberPattern + @"\s*(X)\s*" + numberPattern + measureUnitPattern + @"\s*";
            string datePattern = @"(\d{2}\/\d{2}\/\d{4}|\d{4}\/\d{2}\/\d{2}|\d{2}-\d{2}-\d{4}|\d{2}\.\d{2}\.\d{4})"; // cauta prin bonuri si vezi si adauga si alte pattern-uri pt data

            List<string> shopNameAbreviations = new List<string>() { "SC", "SRC", "S.C.", "S.R.C", "S. C.", "S. R. C.", "SRL", "S.R.L." };
            List<string> shopAddressWords = new List<string>() { "JUD", "JUDET", "STR", "STRADA", "MUN", "MUNICIPIUL" }; //, "NR"
            List<string> nameAndAddressLimit = new List<string>() { "NR.CRT", "CIF", "C.I.F.", "COD IDENTIFICARE FISCALA", "TRANZACTIE", "BON", "COD FISCAL", "C.I.F" };
            List<string> endOfReceipt = new List<string>() { "TOTAL" };

            List<Tuple<InformationType, string>> filteredInformationTuples = new List<Tuple<InformationType, string>>();


            bool shopNameAndAddressSaved = false;
            Regex dateRegex = new Regex(datePattern);
            Regex regex = new Regex(quantityPricePattern + "|" + priceQuantityPattern);
            bool? isPriceBeforeProductName = null;
            bool foundDate = false;
            bool totalReached = false;

            using (ProductDictionaryFileManager productDictionaryFileManager = new ProductDictionaryFileManager())
            {
                int linesNo = lines.Count();
                for (int index = 0; index < linesNo; index++)
                {
                    //linia este o data calendaristica
                    if (dateRegex.IsMatch(lines[index]))
                    {
                        filteredInformationTuples.Add(Tuple.Create(InformationType.Date, dateRegex.Match(lines[index]).ToString()));
                        foundDate = true;
                        if (totalReached)
                            break;
                    }
                    // daca s-a ajuns la cuvantul "Total"
                    if (!totalReached)
                    {
                        //linia curenta reprezinta sfarsitul sectiunii cu informatiile magazinului
                        if (nameAndAddressLimit.Any(lines[index].Contains))
                        {
                            shopNameAndAddressSaved = true;
                            continue;
                        }

                        if (!shopNameAndAddressSaved)// daca informatiile magazinului NU au fost salvate
                        {
                            //se gasesc abrevieri obisnuite ale numele magazinului?
                            if (shopNameAbreviations.Any(lines[index].Contains))
                            {
                                filteredInformationTuples.Add(Tuple.Create(InformationType.ShopName, lines[index]));///"SPATIU COMERCIAL CHIOSC IN SUPRAFATA DE 33,70MP\n" - il pune si pe asta
                            }
                            // se gasesc cuvinte cheie care sa denote adresa unui magazin?
                            else if (shopAddressWords.Any(lines[index].Contains))
                            {
                                filteredInformationTuples.Add(Tuple.Create(InformationType.Address, lines[index]));
                            }
                        }
                        else // informatiile magazinului au fost salvate => se concentreaza pe produse si preturi
                        {
                            /// daca nu s-a ajuns inca la denumirea unui produs sau la lina cu pretul si cantitatea
                            if (isPriceBeforeProductName == null)
                            {
                                /// linia e de forma PRET X CANTITATE sau CANTITATE X PRET
                                if (regex.IsMatch(lines[index]))
                                {
                                    isPriceBeforeProductName = true;
                                }
                                else // denumire produs
                                {
                                    string productName = FindProductNameInCurrentLine(productDictionaryFileManager.ProductNameList, lines[index]);
                                    if (productName == null || productName.Equals(""))
                                    {
                                        continue;
                                    }
                                    isPriceBeforeProductName = false;
                                }
                            }

                            /// daca linia e de forma PRET X CANTITATE sau CANTITATE X PRET
                            //if (regex.IsMatch(lines[index])) // functioneaza bine doaar daca informatia e procesata bine (exista quantity)
                            Match match = regex.Match(lines[index]);
                            if (match.Success) // functioneaza bine doaar daca informatia e procesata bine (exista quantity)
                                               // iar daca ordinea este produs-price (nu price-product)
                            {
                                /// verificare caz in care exista informatii inutile pe linie (ex codul produsului) 
                                string information = match.ToString();
                                string beforeInfo = lines[index].Remove(lines[index].IndexOf(information));
                                if (beforeInfo.Length > ProcessingError)
                                {
                                    /// linie tip "denumire produs PRET X CANTITATE"
                                    beforeInfo = DeleteUnnecessaryInformation(InformationType.ProductName, beforeInfo);
                                    //filteredInformationTuples.Add(Tuple.Create(InformationType.ProductName, beforeInfo));// aici e ceva putred (e pus oricum si fara stabilirea categoriei)
                                    string productName = FindProductNameInCurrentLine(productDictionaryFileManager.ProductNameList, beforeInfo);
                                    if (productName == null)
                                    {
                                        continue;
                                    }
                                    filteredInformationTuples.Add(Tuple.Create(InformationType.ProductName, beforeInfo));// cred ca asa e mai bine
                                    string currentCateg = productDictionaryFileManager.GetProductCategoryByProductName(productName);
                                    filteredInformationTuples.Add(Tuple.Create(InformationType.Category, currentCateg));
                                }
                                else
                                {
                                    string productName = null;
                                    /// linia PRET X CANTITATE este inaintea denumirii produsului
                                    /// linia curenta este cea PRET X CANTITATE
                                    if (isPriceBeforeProductName == true)
                                    {
                                        if (index + 1 >= linesNo)
                                            break;

                                        productName = FindProductNameInCurrentLine(productDictionaryFileManager.ProductNameList, lines[index + 1]);
                                        if (productName == null)// produsul NU e in dictionar => se aplica ALG LEVENSHTEIN
                                        {
                                            ProductDictionary closestProdDict = Levenstein.FindTheClosestWordToTheCurrentOneFromList(lines[index + 1], productDictionaryFileManager.ProductDictionaryList);
                                            filteredInformationTuples.Add(Tuple.Create(InformationType.Category, closestProdDict.ProdCategory));
                                            filteredInformationTuples.Add(Tuple.Create(InformationType.ProductName, closestProdDict.ProdName.ToUpper()));
                                            index++;
                                            continue;
                                        }
                                        // produsul ESTE in dictionar => e introdus in lista de tuple
                                        filteredInformationTuples.Add(Tuple.Create(InformationType.ProductName, lines[index + 1]));
                                        index++; // a fost introdus, deci se trece peste el
                                    }
                                    else /// linia denumirii produsului este inaintea liniei PRET X CANTITATE
                                         /// linia curenta este cea PRET X CANTITATE
                                    {
                                        productName = FindProductNameInCurrentLine(productDictionaryFileManager.ProductNameList, lines[index - 1]);
                                        if (productName == null) // produsul NU e in dictionar => se aplica ALG LEVENSHTEIN
                                        {
                                            ProductDictionary closestProdDict = Levenstein.FindTheClosestWordToTheCurrentOneFromList(lines[index - 1], productDictionaryFileManager.ProductDictionaryList);
                                            filteredInformationTuples.Add(Tuple.Create(InformationType.Category, closestProdDict.ProdCategory));
                                            filteredInformationTuples.Add(Tuple.Create(InformationType.ProductName, closestProdDict.ProdName.ToUpper()));
                                            index++;
                                            continue;
                                        }
                                        // produsul ESTE in dictionar => e introdus in lista de tuple
                                        filteredInformationTuples.Add(Tuple.Create(InformationType.ProductName, lines[index - 1]));
                                    }

                                    if (productName != null) // produsul ESTE in dictionar => trebuie cautata categoria si introdusa si ea
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
                        totalReached = endOfReceipt.Any(lines[index].Contains); // cauta cuvantul "Total"
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
                return double.Parse(match.Value);
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
            double realPrice = quantityWithoutMeasurementUnit.Value * double.Parse(pricePerKG);
            return Math.Round(realPrice, 2).ToString();
        }

        private string DeleteUnnecessaryInformation(InformationType informationType, string line)
        {
            //nu e bun pt cuvintele de o litera si pt denumirile care chiar contin numere in ele 
            string result = line;
            if (informationType.Equals(InformationType.ProductName))
            {
                List<string> wordList = line.Split(' ').ToList();
                result = "";
                foreach (string word in wordList)
                {
                    int digitsNb = word.Count(character => char.IsDigit(character));
                    bool containsLetters = word.Count(character => char.IsLetter(character)) > 0;
                    if (digitsNb > 2 * word.Length / 3 && word.Length > 3 && !containsLetters) // trec de asta numerele de cel mult 3 cifre si sirurile de caractere fara sens care contin cel putin o litera
                        continue;
                    result += word + " ";
                }
            }

            return result;
        }
    }
}
