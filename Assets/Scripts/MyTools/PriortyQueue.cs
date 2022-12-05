using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
 public class PriortyQueue<T>
    {
        private T[] items;
        private Comparison<T> comparison;
        private int count;
        public int Count { get => count; }
        public int Capacity { get => items == null ? 0 : items.Length; }
 
        public PriortyQueue()
        {
            items = new T[10];
            comparison = (x, y) => x.GetHashCode().CompareTo(y.GetHashCode());
        }
 
        public PriortyQueue(Comparison<T> comparison) : this()
        {
            this.comparison = comparison;
        }
 
        public void Enqueue(T item)
        {
            if (count >= Capacity)
                Expansion();
            items[count] = item;
 
            int cur = count++;
            if (cur == 0)
                return;
            int parent = cur;
            T oldValue;
            T newValue;
            do
            {
                cur = parent;
                parent = (cur - 1) / 2;
                oldValue = items[parent];
                Heapify(parent);
                newValue = items[parent];
            } while (!oldValue.Equals(newValue));
        }
 
        public T Dequeue()
        {
            if (count == 0)
                throw new Exception("the queue is empty");
 
            T result = items[0];
 
            Swap(items, 0, count - 1);
            count--;
            if (count > 0)
                Heapify(0);
 
            return result;
        }
 
        public T Peek()
        {
            if (count == 0)
                throw new Exception("the queue is empty");
            return items[0];
        }
 
        private void Heapify(int node)
        {
            int lc = 2 * node + 1;
            int rc = 2 * node + 2;
            int min = node;
            if (lc < count && comparison(items[min], items[lc]) > 0)
                min = lc;
            if (rc < count && comparison(items[min], items[rc]) > 0)
                min = rc;
            if (min != node)
            {
                Swap(items, node, min);
                Heapify(min);
            }
        }
 
        private void Swap(T[] array, int i, int j)
        {
            T t = array[i];
            array[i] = array[j];
            array[j] = t;
        }
 
        private void Expansion()
        {
            T[] newItems = new T[Capacity * 2];
            for (int i = 0; i < count; i++)
                newItems[i] = items[i];
            items = newItems;
        }
    }


