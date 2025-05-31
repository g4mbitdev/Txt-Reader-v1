using System.Text.RegularExpressions;

namespace Data_Structures_Assignment;

internal static class LinkedListProg
{
    // private static void Main()
    // {
    //     FileChecker();
    // }

    /**
     * 
     */
    private static void FileChecker()
    {        
        
        Console.WriteLine("=====Running Linked List Version=====");
        Console.WriteLine("=====================================");
        Console.WriteLine("Please enter text your file name (without extension):");
        string inputFileName = Console.ReadLine() ?? string.Empty;
        string cleansedFileName = "../../../" + inputFileName + ".txt";

        try
        {
            File.Exists(cleansedFileName);
            //Console.WriteLine("The file " + inputFileName + " has been found.");
            FileReader(cleansedFileName);
        } catch(FileNotFoundException)
        {
            Console.WriteLine("The file " + inputFileName + " could not be found. Check if name was typed properly and if the file is in the right folder and/or path. Please try again.");
        }
    }

    /**
     * 
     */
    private static void FileReader(string file)
    {
        string [] textFile = File.ReadAllLines(file);
        LinkedList<string> wordList = new();

        int wordCount = 0;
        int fakeWordCount = 0;
        int lineCount = 0;
        char[] delimiters = [' ', ',', '.', '!', '?', ';', ':', '"', '}' ,'{', '-'];
        //Console.WriteLine(textFile[0]);

        foreach (string line in textFile)
        {
            string[] words = line.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
            
            foreach (string word in words)
            {
                if (WordChecker(word))
                {
                    wordList.AddLast(word.ToLower());
                }
                else
                {
                    fakeWordCount++;
                }
            }
            lineCount++;
        }
        
        Console.WriteLine("====================================");
        Console.WriteLine("There are " + lineCount + " lines and " + wordCount + " words in this file.");
        Console.WriteLine("Erroneous word count: " + fakeWordCount);
        
        try
        {
            UniqueWords(wordList);
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine("There was a problem fetching the unique word list.");
        }

        try
        {
            FileInfo(wordList);
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine("There was a problem fetching the file's information.");
        }
        
        Console.WriteLine("====================================");
        Console.WriteLine("Type in any word to see the statistics on it in this file:");
        string search = Console.ReadLine() ?? string.Empty;

        try
        {
            WordInfo(search, textFile);
        }
        catch (NotSupportedException)
        {
            Console.WriteLine("There was a problem fetching this word's information.");
        }
    }

    /**
     * 
     */
    private static bool WordChecker(string word)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(word))
            {
                return false;
            }
            
            if (word == "&")
            {
                return true;
            }
            else
            {
                return Regex.IsMatch(word, @"^[a-zA-Z]+(?:'[a-zA-Z]+)*$");
            }
        }
        catch (ArgumentException)
        {
            return false;
        }
    }
    
    /**
     * 
     */
    private static void UniqueWords(LinkedList<string> words)
    {
        int counter = 0;
        //string[] uniqueWords = [];

        try
        {
            foreach (string word1 in words)
            {
                foreach (string word2 in words)
                {
                    if (word2 == word1 && WordChecker(word1))
                    {
                        counter++;
                    }
                }
            }
        }
        catch (ArgumentException)
        {
            Console.WriteLine("There was an error processing the file's unique words.");
        }
        Console.WriteLine("====================================");
        Console.WriteLine("Unique word count:" + counter);

        //Console.WriteLine(string.Join(", ", uniqueWords));
    }

    /**
     *
     */
    private static void FileInfo(LinkedList<string> file)
    {
        
        //Array.Sort(file);
        var sorted = file.OrderBy(w => w).ToList();
        Dictionary<string, int> wordDictionary = new();

        string longest = sorted.FirstOrDefault() ?? "";
        string repeated = "";
        int counter = 0;
        
        foreach (string word in file)
        {
            if (wordDictionary.ContainsKey(word))
            {
                wordDictionary[word]++;
            }
            else
            {
                wordDictionary[word] = 1;
            }

            if (word.Length > longest.Length)
            {
                longest = word;
            }
        }

        foreach (var pair in wordDictionary)
        {
            if (pair.Value > counter)
            {
                repeated = pair.Key;
                counter = pair.Value;
            }
        }
        
        foreach (var pair in wordDictionary)
        {
            Console.WriteLine($"'{pair.Key}' appears {pair.Value} time(s).");
        }
        
        Console.WriteLine("====================================");
        Console.WriteLine($"Longest word: '{longest}'. Length: {longest.Length} digits.");
        Console.WriteLine($"Most repeated word: '{repeated}'. Repetitions: {counter}.");

        //Output every word in descending alphabetical order, and each number of occurrences
        //Output the longest word and number of occurrences
        //Output the most frequent word and number of occurrences
    }
    
    /**
     * 
     */
    private static void WordInfo(string word, string[] file)
    {
        bool found = false;
        int[] lines = [];
        
        try
        {
            for (int i = 0; i < file.Length; i++)
            {
                if (file[i].Contains(word, StringComparison.OrdinalIgnoreCase))
                {
                    lines = lines.Append(i + 1).ToArray();
                    found = true;
                }
            }
            Console.WriteLine("====================================");
            Console.WriteLine("This word is used a total of " + lines.Length + " time(s).");
            Console.WriteLine("This word can be found in line(s): " + string.Join(", ", lines) );
            
            if (!found)
            {
                Console.WriteLine("This word could not be found.");
            }
        }
        catch (Exception)
        {
            Console.WriteLine("There was an error finding word information.");
        }
    }
}

