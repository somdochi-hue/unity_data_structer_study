using System;
using UnityEngine;

public class AVLTreeTest<TKey, TValue> : AVLTree<TKey, TValue> where TKey : IComparable<TKey>
{
    public TreeNode<TKey, TValue> GetRoot()
    {
        return root;
    }
}

public class AVLTree<TKey, TValue> : BinarySearchTree<TKey, TValue> where TKey : IComparable<TKey>
{
    protected int BalanceFactor(TreeNode<TKey, TValue> node)
    {
        return node == null ? 0 : Height(node.Left) - Height(node.Right);
    }

    protected TreeNode<TKey, TValue> Balance(TreeNode<TKey, TValue> node)
    {
        /*if (node == null)
        {
            return null;
        }*/

        UpdateHeight(node);
        int balance = BalanceFactor(node);
        // TODO: LL, LR, RR, RL
        if (balance > 1)
        {
            if (BalanceFactor(node.Left) < 0)
            {
                node.Left = RotateLeft(node.Left);
            }
            return RotateRight(node);
        }

        if (balance < -1)
        {
            if (BalanceFactor(node.Right) > 0)
            {
                node.Right = RotateRight(node.Right);
            }
            return RotateLeft(node);
        }

        return node;
    }

    protected TreeNode<TKey, TValue> RotateLeft(TreeNode<TKey, TValue> node)
    {
        if (node == null || node.Right == null) 
        return node;

        TreeNode<TKey, TValue> RightChild = node.Right;
        TreeNode<TKey, TValue> temp = RightChild.Left;
        RightChild.Left = node;
        RightChild = temp;

        UpdateHeight(node);
        UpdateHeight(RightChild);

        return RightChild;
    }

    protected TreeNode<TKey, TValue> RotateRight(TreeNode<TKey, TValue> node)
    {
        if (node == null || node.Left == null) 
        return node;
        
        TreeNode<TKey, TValue> LeftChild = node.Left;
        TreeNode<TKey, TValue> temp = LeftChild.Right;
        LeftChild.Right = node;
        LeftChild = temp;

        UpdateHeight(node);
        UpdateHeight(LeftChild);

        return LeftChild;
    }

    protected override TreeNode<TKey, TValue> Add
    (TreeNode<TKey, TValue> node, TKey key, TValue value)
    {
        node = base.Add(node, key, value);
        return Balance(node);
    }

    protected override TreeNode<TKey, TValue> AddOrUpdate
    (TreeNode<TKey, TValue> node, TKey key, TValue value)
    {
        node = base.AddOrUpdate(node, key, value);
        return Balance(node);
    }

    protected override TreeNode<TKey, TValue> Remove
    (TreeNode<TKey, TValue> node, TKey key)
    {
        node = base.Remove(node, key);
        return Balance(node);
    }

    // 모든 노드의 BF ∈ {-1, 0, +1}인지 검증 (AVL 불변식).
    public bool IsBalanced()
    {
        return IsBalanced(root);
    }

    private bool IsBalanced(TreeNode<TKey, TValue> node)
    {
        if (node == null)
        {
            return true;
        }
        if (Mathf.Abs(BalanceFactor(node)) > 1)
        {
            return false;
        }
        return IsBalanced(node.Left) && IsBalanced(node.Right);
    }
}
