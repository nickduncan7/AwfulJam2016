using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// Unity-friendly version of the priority queue from Red Blob Games.
// http://www.redblobgames.com/pathfinding/a-star/implementation.html#csharp
public class PriorityQueue<T>
{
    private List<KeyValuePair<T, double>> elements = new List<KeyValuePair<T, double>>();

    public int Count
    {
        get { return elements.Count; }
    }

    public void Enqueue(T item, double priority)
    {
        elements.Add(new KeyValuePair<T, double>(item, priority));
    }

    public T Dequeue()
    {
        int bestIndex = 0;

        for (int i = 0; i < elements.Count; i++)
        {
            if (elements[i].Value < elements[bestIndex].Value)
            {
                bestIndex = i;
            }
        }

        T bestItem = elements[bestIndex].Key;
        elements.RemoveAt(bestIndex);
        return bestItem;
    }
}