using ShadowWatcher.Contract;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace ShadowWatcher
{
    public class CardList : IEnumerable<CardData>, INotifyCollectionChanged
    {
        private List<CardData> list = new List<CardData>();

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public void Add(CardData elem)
        {
            if (list.Contains(elem))
            {
                list.Find(e => e == elem).Amount += elem.Amount;
            }
            else
            {
                list.Add(elem);
                list.Sort();

                CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }
        }

        public void Add(IEnumerable<CardData> elems)
        {
            foreach (var card in elems)
            {
                if (list.Contains(card))
                {
                    list.Find(e => e == card).Amount += card.Amount;
                }
                else
                {
                    list.Add(card);
                }
            }

            list.Sort();
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public void Clear()
        {
            list.Clear();

            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public IEnumerator<CardData> GetEnumerator() => list.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
