using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

var list1 = new List<string> { "Alice", "Bob", "Charlie", "David" };
var list2 = new List<int>    { 25, 17, 30, 15 };
var list3 = new List<string> { "NY", "LA", "SF", "CHI" };
var list4 = new List<double> { 88.5, 92.0, 75.3, 60.1 };

// 条件基于 list1，取出对应位置的 list2/list3/list4
var result = Enumerable.Range(0, list1.Count)
    .Where(i => list1[i].StartsWith("A"))     
    .Select(i => (
        Item1: list1[i],   
        Item2: list2[i],
        Item3: list3[i],
        Item4: list4[i]))
    .ToList();

foreach (var r in result)
    Console.WriteLine($"{r.Item1} {r.Item2} {r.Item3} {r.Item4}");