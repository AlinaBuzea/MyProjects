using EM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EM.Processing
{
    public class Levenstein
    {
        /// <summary>
        ///  Applies Levenstein algorithm on the words passed as parameters
        /// </summary>
        /// <param name="word1"></param>
        /// <param name="word2"></param>
        /// <returns> Number of distinct characters </returns>
        public static int LevensteinAlgorithm(string word1, string word2)
        {
            int[,] levensteinMatrix = new int[word1.Length + 1, word2.Length + 2];
            for (int index = 0; index < word1.Length + 1; index++)
                for (int index2 = 0; index2 < word2.Length + 1; index2++)
                {
                    int min = Minimum(index, index2);
                    if (min == 0)
                    {
                        levensteinMatrix[index, index2] = index > index2 ? index : index2;
                    }
                    else
                    {
                        int rightPlusOne = levensteinMatrix[index - 1, index2] + 1;
                        int upPlusOne = levensteinMatrix[index, index2 - 1] + 1;
                        int upRight = levensteinMatrix[index - 1, index2 - 1] + (word1[index - 1] != word2[index2 - 1] ? 1 : 0);

                        levensteinMatrix[index, index2] = Minimum(Minimum(rightPlusOne, upPlusOne), upRight);

                    }
                }
            return levensteinMatrix[word1.Length, word2.Length];
        }

        private static int Minimum(int first, int second)
        {
            return first < second ? first : second;
        }

        public static ProductDictionary FindTheClosestWordToTheCurrentOneFromList(string word, List<ProductDictionary> productDictionary)
        {
            Dictionary<ProductDictionary, int> levensteinValuesPairs = new Dictionary<ProductDictionary, int>();
            foreach (ProductDictionary item in productDictionary)
            {
                levensteinValuesPairs.Add(item, LevensteinAlgorithm(word, item.ProdName.ToUpper()));
            }
            ProductDictionary chosenDictProduct = levensteinValuesPairs.FirstOrDefault(x => x.Value ==
                                                                 levensteinValuesPairs.Aggregate((left, rigth) => left.Value < rigth.Value ? left : rigth).Value
                                                                 ).Key;
            return chosenDictProduct;
        }
    }
}
