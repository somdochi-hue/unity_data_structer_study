using UnityEngine;

public class TreeNode<TKey, TValue>
{
    public TKey Key { get; set; }
    public TValue Value { get; set; }

    public TreeNode<TKey, TValue> Left { get; set; }
    public TreeNode<TKey, TValue> Right { get; set; }

    public int Height { get; set; }

    public TreeNode(TKey key, TValue value)
    {
        Key = key;
        Value = value;
        Height = 1;
    }

    /*public void UpdateHeight()
    {
        int LeftHeight = Left != null ? Left.Height : 0;
        int RightHeight = Right != null ? Left.Height : 0;
        Height = 1 + Mathf.Max(LeftHeight, RightHeight)
    }*/
}
