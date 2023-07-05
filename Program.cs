using System;
using System.Collections.Generic;

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
        TrieNode node = FindNode(word);
        return node != null && node.IsEndOfWord;
    }

    public bool StartsWith(string prefix)
    {
        TrieNode node = FindNode(prefix);
        return node != null;
    }

    private TrieNode FindNode(string str)
    {
        TrieNode current = root;
        foreach (char c in str)
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
}

class Program
{
    static void Main()
    {
        Trie trie = new();

        // 插入詞彙到 Trie
        trie.Insert("apple");
        trie.Insert("app");
        trie.Insert("banana");
        trie.Insert("car");

        // 搜尋詞彙
        Console.WriteLine(trie.Search("apple"));   // true
        Console.WriteLine(trie.Search("app"));     // true
        Console.WriteLine(trie.Search("appl"));    // false
        Console.WriteLine(trie.Search("banana"));  // true
        Console.WriteLine(trie.Search("car"));     // true
        Console.WriteLine(trie.Search("cat"));     // false

        // 判斷是否以指定字首開頭
        Console.WriteLine(trie.StartsWith("ap"));  // true
        Console.WriteLine(trie.StartsWith("ban")); // true
        Console.WriteLine(trie.StartsWith("ca"));  // true
        Console.WriteLine(trie.StartsWith("dog")); // false
    }
}
