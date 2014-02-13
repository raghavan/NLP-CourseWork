using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LearnDotNet
{
    class Program
    {
       
        static void Main(string[] args)
        {
            string actualSentence = " <s> English Wikipedia concerns which national variety of the English language with the most commonly advocated being American English and British English . </s> "
                    + " <s> Perennial suggestions range from standardizing upon a single form of English is the forking tool for Wikipedia project with British Candidates . </s> "
                    + " <s>  A style guideline states , the English Wikipedia has no general preference for a major national variety of the language for and an article on a topic that has strong ties to a particular English  speaking nation uses the appropriate variety of English for that nation . </s> "
                    + " <s>  An article should use spelling and grammar variants consistently  for example , color and colour are not to be used in the same article , since they represent for American and British English , respectively . </s>"
                    + " <s> The guide also states that an article remain in its original national variant is the preferred language for American. candidates </s> ";

            //Cleaning the sentence removing trailing spaces and double spaces if present between words
            actualSentence = actualSentence.Trim().ToLower();

            SortedDictionary<string, int> actualSentenceUnigramCountMap = unigramMapMaker(actualSentence);
            SortedDictionary<string, int> actualSentenceBigramCountMap = bigramMapMaker(actualSentence);
            int totalNumberWordsInActualSentence = actualSentence.Split(new Char[] { ' ' }).Length;

            string ques1Sentence = "<s> English is the preferred language for American and British candidates . </s>";
            ques1Sentence = ques1Sentence.Trim().ToLower();
            //doBiGram(ques1Sentence, actualSentenceBigramCountMap, actualSentenceUnigramCountMap, totalNumberWordsInActualSentence);

            string ques2Sentence = "<s> American English and British English are variants of the same language . </s>";
            ques2Sentence = ques2Sentence.Trim().ToLower();
            //doUniGram(ques2Sentence, actualSentenceUnigramCountMap, totalNumberWordsInActualSentence);
            doBiGram(ques2Sentence, actualSentenceBigramCountMap, actualSentenceUnigramCountMap, totalNumberWordsInActualSentence);
            Console.ReadLine();
        }

        private static void doUniGram(string sentence, SortedDictionary<string, int> actualSentenceUnigramCountMap, int totalNumberWordsInActualSentence)
        {
            string[] splittedSentence = sentence.Split(new Char[] { ' ' });
            double result = 1;
            for (int i = 0; i < splittedSentence.Length; i++)
            {
                string word = splittedSentence[i].Trim();
                if(!actualSentenceUnigramCountMap.ContainsKey(word))
                {
                    Console.WriteLine("Word not present = " + word);
                }else
                {
                    double wordProb =  actualSentenceUnigramCountMap[word] / (float)totalNumberWordsInActualSentence;
                    result *=  wordProb;
                    Console.WriteLine("P(" + word + ") = " + Math.Round(wordProb,3));
                }             
            }
            Console.WriteLine(" Unigram sentence probability = " + result);
            
        }

        private static void doBiGram(string sentence, SortedDictionary<string, int> actualSentenceBigramCountMap, SortedDictionary<string, int> actualSentenceUnigramCountMap, int totalNumberWordsInActualSentence)
        {
            string[] splittedSentence = sentence.Split(new Char[] { ' ' });
            double totalProb = 1.0;
            int numberOfUniqueWords = actualSentenceUnigramCountMap.Keys.Count();
            for (int i = 0; i < splittedSentence.Length - 1; i++)
            {
                string numerator = splittedSentence[i] + " " + splittedSentence[i+1];
                numerator = numerator.Trim();
                string denominator = splittedSentence[i];
                denominator = denominator.Trim();
                if (actualSentenceBigramCountMap.ContainsKey(numerator) && actualSentenceUnigramCountMap.ContainsKey(denominator))
                {      
                    double bigramProb = (actualSentenceBigramCountMap[numerator] + 1) / ((double)actualSentenceUnigramCountMap[denominator] + numberOfUniqueWords);
                    totalProb *= bigramProb;
                    Console.Write("P(" + numerator + ") = " + Math.Round(bigramProb,3) + "\n");
                
                }
                else if (!actualSentenceBigramCountMap.ContainsKey(numerator) && actualSentenceUnigramCountMap.ContainsKey(denominator))
                {
                    Console.WriteLine(" Not present in bigram map = " + numerator);
                    double bigramProb = 1 / ((double)actualSentenceUnigramCountMap[denominator] + numberOfUniqueWords);
                    totalProb *= bigramProb;
                    Console.Write("P(" + numerator + ") = " + Math.Round(bigramProb, 3) + "\n");
                }
                else if (actualSentenceBigramCountMap.ContainsKey(numerator) && !actualSentenceUnigramCountMap.ContainsKey(denominator))
                {
                    Console.WriteLine(" Not present in unigram map = " + denominator);
                    double bigramProb = (actualSentenceBigramCountMap[numerator] + 1)/ (double)numberOfUniqueWords;
                    totalProb *= bigramProb;
                    Console.Write("P(" + numerator + ") = " + Math.Round(bigramProb, 3) + "\n");
                }
                else
                {
                    Console.WriteLine(" Not present in unigram map = " + denominator);
                    double bigramProb = 1 / (double)numberOfUniqueWords;
                    totalProb *= bigramProb;
                    Console.Write("P(" + numerator + ") = " + Math.Round(bigramProb, 3) + "\n");
                }
               
            }
            Console.Write("Sentence bigram prob = " + totalProb + "\n");
        }

        private static SortedDictionary<string, int> bigramMapMaker(String sentence)
        {
            SortedDictionary<string, int> wordCountMap = new  SortedDictionary<string, int>();
            string [] splittedSentence = sentence.Split(new Char [] {' '});
            for (int i = 0; i < splittedSentence.Length-1; i++)
            {
                string bigram = splittedSentence[i].Trim() + " " + splittedSentence[i + 1].Trim();
                bigram = bigram.Trim();
                int count = 0;
                if(wordCountMap.ContainsKey(bigram))
                {
                    count = wordCountMap[bigram];                
                }
                count += 1;

                wordCountMap[bigram] = count;
            }

            return wordCountMap;
        }

          private static SortedDictionary<string, int> unigramMapMaker(String sentence)
        {
            SortedDictionary<string, int> wordCountMap = new  SortedDictionary<string, int>();
            string [] splittedSentence = sentence.Split(new Char [] {' '});
            for (int i = 0; i < splittedSentence.Length; i++)
            {
                string word = splittedSentence[i].Trim();
                int count = 0;
                if (wordCountMap.ContainsKey(word))
                {
                    count = wordCountMap[word];                
                }
                count += 1;

                wordCountMap[word] = count;
            }

            return wordCountMap;
        }
            

    }
}
