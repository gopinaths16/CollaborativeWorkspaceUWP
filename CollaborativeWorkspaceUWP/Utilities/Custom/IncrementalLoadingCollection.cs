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
        private ObservableCollection<T> source;
        private bool _hasMoreItems = true;
        private bool _isLoading = false;
        private int _currentPage = 0;
        private int PageSize = 0;

        public IncrementalLoadingCollection(ObservableCollection<T> source, int pageSize)
        {
            this.source = source;
            PageSize = pageSize;
            LoadMoreItemsAsync();
        }

        public bool HasMoreItems => _hasMoreItems;

        public async Task<LoadMoreItemsResult> LoadMoreItemsAsync()
        {
            var items = source.Skip(PageSize * _currentPage).Take(PageSize);
            if (_isLoading || !HasMoreItems)
            {
                return new LoadMoreItemsResult { Count = (uint)items.Count() };
            }
            _isLoading = true;
            try
            {
                
                if (items.Count() < PageSize) _hasMoreItems = false;
                _currentPage++;
                foreach (var item in items)
                {
                    Add(item);
                }
            }
            finally
            {
                _isLoading = false;
            }
            return new LoadMoreItemsResult { Count = (uint)items.Count() };
        }

        public IAsyncOperation<LoadMoreItemsResult> LoadMoreItemsAsync(uint count)
        {
            return LoadMoreItemsAsync().AsAsyncOperation();
        }
    }
}
