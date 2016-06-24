using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptSharp
{
    public enum KeyedBy { Key, Value };
    class KeyIndexer
    {
        public KeyIndexer()
        {
            keyedBy = KeyedBy.Key;
        }

        List<Pair<int, int>> pairs = new List<Pair<int, int>>();

        public int Count
        {
            get { return pairs.Count; }
        }

        protected KeyedBy keyedBy;
        public KeyedBy KeyedBy
        {
            get
            {
                return keyedBy;
            }
            set
            {
                keyedBy = value;
                if (value == KeyedBy.Key)
                {
                    pairs.Sort((pair1, pair2) => pair1.Value1.CompareTo(pair2.Value1));
                }
                else
                {
                    pairs.Sort((pair1, pair2) => pair1.Value2.CompareTo(pair2.Value2));
                }
            }
        }

        public void ReindexValues()
        {
            int max = -1;
            int maxIndex = -1;
            int len = pairs.Count - 1;
            for (int i = 0; i < pairs.Count; i++)
            {
                //find max
                for (int j = 0; j < pairs.Count; j++)
                {
                    if (pairs[j].Value2 > max)
                    {
                        maxIndex = j;
                        max = pairs[j].Value2;
                    }
                }

                //set maxIndex to be len
                Pair<int, int> p = pairs[maxIndex];
                p.Value2 = -len;
                pairs[maxIndex] = p;

                //decrement len, reset max = -1 and maxIndex = -1
                len--;
                max = -1;
                maxIndex = -1;
            }

            for (int i = 0; i < pairs.Count; i++)
            {
                //set maxIndex to be len
                Pair<int, int> p = pairs[i];
                p.Value2 = -p.Value2;
                pairs[i] = p;
            }

        }

        public void Add(int key, int value)
        {
            pairs.Add(new Pair<int, int>(key, value));
        }

        public int this[int i]
        {
            get
            {
                if (keyedBy == KeyedBy.Key)
                {
                    return pairs[i].Value2;

                    //for (int j = 0; j < pairs.Count; j++)
                    //{
                    //    if (pairs[j].Value1 == i)
                    //    {
                    //        return pairs[j].Value2;
                    //    }
                    //}
                }
                else
                {
                    return pairs[i].Value1;
                    //for (int j = 0; j < pairs.Count; j++)
                    //{
                    //    if (pairs[j].Value2 == i)
                    //    {
                    //        return pairs[j].Value1;
                    //    }
                    //}
                }
                throw new Exception("Invalid index");
            }
            set
            {
                if (keyedBy == KeyedBy.Key)
                {
                    Pair<int, int> p = pairs[i];
                    p.Value2 = value;
                    pairs[i] = p;
                    //for (int j = 0; j < pairs.Count; j++)
                    //{
                    //    if (pairs[j].Value1 == i)
                    //    {
                    //        Pair<int, int> p = pairs[j];
                    //        p.Value2 = value;
                    //        pairs[j] = p;
                    //    }
                    //}
                }
                else
                {
                    Pair<int, int> p = pairs[i];
                    p.Value1 = value;
                    pairs[i] = p;
                    //for (int j = 0; j < pairs.Count; j++)
                    //{
                    //    if (pairs[j].Value2 == i)
                    //    {
                    //        Pair<int, int> p = pairs[j];
                    //        p.Value1 = value;
                    //        pairs[j] = p;
                    //    }
                    //}
                }
                throw new Exception("Invalid index");
            }
        }
    }

    public struct Pair<TType1, TType2>
    {
        public Pair(TType1 v1, TType2 v2)
        {
            Value1 = v1;
            Value2 = v2;
        }

        public TType1 Value1 { get; set; }
        public TType2 Value2 { get; set; }
    }
}
