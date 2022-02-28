using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TreeNode<T>
{
    public T Value { get; set; }
    public List<TreeNode<T>> Childs { get; }

    public TreeNode()
    {
        Childs = new List<TreeNode<T>>();
    }
}

public class TreeGenerator
{
    private readonly int maxChilds;
    private readonly System.Random rnd = new System.Random();

    public TreeGenerator(int maxChilds)
    {
        this.maxChilds = maxChilds;
    }

    public TreeNode<T> CreateTree<T>(int maxDepth, List<T> values)
    {
        var node = new TreeNode<T>();
        node.Value = values[0];
        values.RemoveAt(0);
        if (maxDepth > 0)
        {
            var childsCount = rnd.Next(maxChilds);
            for (var i = 0; i < childsCount; ++i)
            {
                node.Childs.Add(CreateTree(maxDepth - 1, values));
            }
                
        }
        return node;
    }

    public static void Demo()
    {
        //var rnd = new System.Random();
        //var generator = new TreeGenerator(3 /* max childs count*/);
        //var tree = generator.CreateTree(4 /*max depth*/, () => rnd.Next() /*node value*/);
    }
}
