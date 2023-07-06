using System;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;

class TrieNode
{
    public char Value { get; }
    public Dictionary<char, TrieNode> Children { get; }
    public bool IsEndOfWord { get; set; }

    public TrieNode(char value)
    {
        Value = value;
        Children = new Dictionary<char, TrieNode>();
        IsEndOfWord = false;
    }
}

class Trie
{
    private TrieNode root;

    public Trie()
    {
        root = new TrieNode('\0'); // 使用 null 字元作為根節點的值
    }

    public void Insert(string word)
    {
        TrieNode current = root;
        foreach (char c in word)
        {
            if (!current.Children.ContainsKey(c))
            {
                current.Children[c] = new TrieNode(c);
            }
            current = current.Children[c];
        }
        current.IsEndOfWord = true;
    }

    public bool Search(string word)
    {
        TrieNode node = StartsWith(word);
        return node != null && node.IsEndOfWord;
    }

    public TrieNode StartsWith(string prefix)
    {
        TrieNode current = root;
        foreach (char c in prefix)
        {
            if (current.Children.ContainsKey(c))
            {
                current = current.Children[c];
            }
            else
            {
                return null;
            }
        }
        return current;
    }

    public List<string> AutoComplete(string prefix)
    {
        TrieNode node = StartsWith(prefix);
        List<string> suggestions = new List<string>();
        
        if (node == null)
        {
            return suggestions;
        }

        CollectWords(node, prefix, suggestions);

        return suggestions;
    }

    void CollectWords(TrieNode node, string prefix, List<string> suggestions)
    {
        if (node.IsEndOfWord)
        {
            suggestions.Add(prefix);
        }

        foreach (TrieNode childNode in node.Children.Values)
        {
            CollectWords(childNode, prefix + childNode.Value, suggestions);
        }
    }

    public string ToJson()
    {
        return JsonConvert.SerializeObject(root, Formatting.Indented);
    }

    public static Trie FromJson(string json)
    {
        TrieNode root = JsonConvert.DeserializeObject<TrieNode>(json);
        Trie trie = new Trie();
        trie.root = root;
        return trie;
    }
}

class Program
{
    static void Main()
    {
        // try
        // {
        //     using (StreamReader sr = new StreamReader("lexicon.txt"))
        //     {
        //         string line;
        //         // for(int i = 0 ; i < 10 ; ++i){
        //         //     line = sr.ReadLine();
        //         //     trie.Insert(line);
        //         // }
        //         while ((line = sr.ReadLine()) != null)
        //         {
        //             trie.Insert(line);
        //         }
        //     }
        // }
        // catch (Exception ex)
        // {
        //     Console.WriteLine("An error occurred: " + ex.Message);
        // }

        // // 將 Trie 轉換成 JSON 字串
        // string json = trie.ToJson();

        // // 寫入 JSON 檔案
        // System.IO.File.WriteAllText("lexicon.json", json);

        // 從 JSON 檔案讀取 Trie
        string jsonText = System.IO.File.ReadAllText("lexicon.json");
        Trie trie = Trie.FromJson(jsonText);

        // 自動完成
        Console.Write("請輸入字首：");
        string input = "";
        while((input = Console.ReadLine()) != null)
        {
            List<string> suggestions = trie.AutoComplete(input);

            if (suggestions.Count == 0)
            {
                Console.WriteLine("無建議詞彙。");
            }
            else
            {
                Console.WriteLine("建議詞彙：");
                foreach (string suggestion in suggestions)
                {
                    Console.WriteLine(suggestion);
                }
            }
        }
    }
}