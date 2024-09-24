using Microsoft.Toolkit.Uwp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml.Data;

namespace CollaborativeWorkspaceUWP.Utilities.Custom
{
    public class IncrementalLoadingCollection<T> : ObservableCollection<T>, INotifyCollectionChanged, ISupportIncrementalLoading
    {
        public ObservableCollection<T> source;
        private bool _hasMoreItems = true;
        private bool _isLoading = false;
        private int _currentPage = 0;
        private int PageSize = 0;

        public IncrementalLoadingCollection(ObservableCollection<T> source, int pageSize)
        {
            this.source = source;
            PageSize = pageSize;
            Task.Run(async () =>
            {
                await LoadMoreItemsAsync();
            });
        }

        public bool HasMoreItems => _hasMoreItems;

        public async Task<LoadMoreItemsResult> LoadMoreItemsAsync()
        {
            var items = source.Skip(Count).Take(PageSize);
            if (_isLoading || !HasMoreItems)
            {
                return new LoadMoreItemsResult { Count = (uint)items.Count() };
            }
            _isLoading = true;
            try
            {
                if (items.Count() < PageSize) _hasMoreItems = false;
                if(source.Count > 0)
                {
                    _currentPage++;
                    foreach (var item in items)
                    {
                        base.Add(item);
                    }
                }
            }
            finally
            {
                _isLoading = false;
            }
            return new LoadMoreItemsResult { Count = (uint)items.Count() };
        }

        public int SourceCount
        {
            get
            {
                return source.Count();
            }
        }

        public new void Add(T item)
        {
            source.Add(item);
            if(base.Count < PageSize)
            {
                base.Add(item);
            }
            _hasMoreItems = true;
        }

        public void Insert(int index, T item)
        {
            base.Insert(index, item);
            source.Add(item);
        }

        public new bool Remove(T item)
        {
            bool result = base.Remove(item);
            source.Remove(item);
            return result;
        }

        public void Clear()
        {
            source.Clear();
            _currentPage = 0;
            base.Clear();
        }

        public IEnumerable<T> Where(Func<T, bool> predicate)
        {
            return source.Where(predicate);
        }

        public IAsyncOperation<LoadMoreItemsResult> LoadMoreItemsAsync(uint count)
        {
            return LoadMoreItemsAsync().AsAsyncOperation();
        }
    }
}
