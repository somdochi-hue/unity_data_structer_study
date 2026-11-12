using System.Collections.Generic;
using System.Collections;
using System;

public class DoublyLinkedListNode<T>
{
    public T Value { get; set; }
    public DoublyLinkedListNode<T> Next { get; set; }
    public DoublyLinkedList<T> List { get; set; }
    public DoublyLinkedListNode<T> Previous  { get; set; }
    public DoublyLinkedListNode(T value)
    {
        Value = value;
        Next = null;
        Previous = null;
    }
}

public class DoublyLinkedList<T> : IEnumerable<T>
{
    private DoublyLinkedListNode<T> head;
    private DoublyLinkedListNode<T> tail;

    public int Count { get; private set; }
    public DoublyLinkedListNode<T> First => head;
    public DoublyLinkedListNode<T> Last => tail;

    public IEnumerator<T> GetEnumerator()
    {
        DoublyLinkedListNode<T> current = head;
        while (current != null)
        {
            yield return current.Value;
            current = current.Next;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public DoublyLinkedListNode<T> AddFirst(T value)
    {
        var node = new DoublyLinkedListNode<T>(value);
        if (head == null)
        {
            head = node;
            tail = node;
        }
        else
        {
            node.Next = head;
            head.Previous = node;
            head = node;
        }
        Count++;
        return node;
    }
   
    public DoublyLinkedListNode<T> AddLast(T value)
    {
        var node = new DoublyLinkedListNode<T>(value);
        node.List = this;
        if (tail == null)
        {
            head = node;
            tail = node;
        }
        else
        {
            tail.Next = node;
            node.Previous = tail;
            tail = node;
        }
        Count++;
        return node;
    }

    public DoublyLinkedListNode<T> AddAfter(DoublyLinkedListNode<T> node, T value)
    {
        ValidataNode(node);
        if (node == tail)
        {
            return AddLast(value);
        }

        var newNode = new DoublyLinkedListNode<T>(value);
        newNode.List = this;
        var nextNode = node.Next;

        newNode.Next = nextNode;
        newNode.Previous = node;
        node.Next = newNode;

        if(nextNode != null)
        {
            nextNode.Previous = newNode;
        }

        Count++;
        return newNode;
    }

    public DoublyLinkedListNode<T> AddBefore(DoublyLinkedListNode<T> node, T value)
    {
        ValidataNode(node);
        if (node == head)
        {
            return AddFirst(value);
        }

        var previous = node.Previous;        
        var newNode = new DoublyLinkedListNode<T>(value);
        newNode.List = this;

        newNode.Next = node;
        newNode.Previous = previous;

        previous.Next = newNode;
        node.Previous = newNode;

        Count++;
        return newNode;
    }

    public void RemoveFirst()
    {
        if (head == null)
        {
            throw new InvalidOperationException("리스트가 비어있습니다.");
        }
        Remove(head);
    }

    public void RemoveLast()
    {
        if (tail == null)
        {
            throw new InvalidOperationException("리스트가 비어있습니다.");
        }
        Remove(tail);
    }

    public bool Remove(T value)
    {
        var node = Find(value);
        if (node == null)
        {
            return false;
        }
        Remove(node);
        return true;
    }

    public void Remove(DoublyLinkedListNode<T> node)
    {
        ValidataNode(node);

        DoublyLinkedListNode<T> previous = null;
        DoublyLinkedListNode<T> current = head;

        while (current != node)
        {
            previous = current;
            current = current.Next;
        }

        if (previous == null)
        {
            head = node.Next;
        }
        else
        {
            previous.Next = node.Next;
        }

        if (node == tail)
        {
            tail = previous;
        }

        node.Next = null;
        node.List = null;
        Count--;
    }

    public DoublyLinkedListNode<T> Find(T value)
    {
        var comparer = EqualityComparer<T>.Default;
        DoublyLinkedListNode<T> current = head;
        while (current != null)
        {
            if (comparer.Equals(current.Value, value))
            {
                return current;
            }
            current = current.Next;
        }
        return null;
    }

    public bool Contains(T value)
    {
        return Find(value) != null;
    }

    public void Clear()
    {
        var current = head;
        while (current != null)
        {
            var next = current.Next;
            current.Previous = null;
            current.Next = null;
            current = next;
        }
        head = tail = null;
        Count = 0;
    }

    private void ValidataNode(DoublyLinkedListNode<T> node)
    {
        if (node == null)
        {
            throw new ArgumentException(nameof(node));
        }

        if (node.List != this)
        {
            throw new InvalidOperationException("이 리스트에 속한 노드가 아닙니다.");
        }
    }

    internal object[] Backward()
    {
        throw new NotImplementedException();
    }
}