using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Kzrnm.WindowScreenshot;
public class SelectorObservableCollection<T> : ObservableCollection<T>
{
    public SelectorObservableCollection() : base() { }
    public SelectorObservableCollection(IEnumerable<T> collection) : base(collection) { }

    private int _SelectedIndex = -1;
    public int SelectedIndex
    {
        set
        {
            if (_SelectedIndex != value)
            {
                var prevIndex = _SelectedIndex;
                _SelectedIndex = value;
                OnSelectedIndexChanged(prevIndex, value);
            }
        }
        get => _SelectedIndex;
    }
    public T? SelectedItem => GetOrDefault(SelectedIndex);
    private T? GetOrDefault(int index) => (uint)index < (uint)Count ? this[index] : default;

    public event EventHandler<SelectedItemChangedEventArgs>? SelectedChanged;
    private void OnSelectedItemChanged(T? oldItem, T? newItem)
    {
        SelectedChanged?.Invoke(this, new(SelectedIndex, SelectedIndex, oldItem, newItem));
        OnPropertyChanged(new(nameof(SelectedItem)));
    }
    private void OnSelectedIndexChanged(int oldIndex, int newIndex)
    {
        var oldItem = GetOrDefault(oldIndex);
        var newItem = GetOrDefault(newIndex);
        SelectedChanged?.Invoke(this, new(oldIndex, newIndex, oldItem, newItem));
        OnPropertyChanged(new(nameof(SelectedIndex)));
    }
    private void OnSelectedChanged(int oldIndex, int newIndex, T? oldItem, T? newItem)
    {
        SelectedChanged?.Invoke(this, new(oldIndex, newIndex, oldItem, newItem));
        OnPropertyChanged(new(nameof(SelectedIndex)));
        OnPropertyChanged(new(nameof(SelectedItem)));
    }
    protected override void InsertItem(int index, T item)
    {
        var oldIndex = SelectedIndex;
        var oldItem = SelectedItem;
        base.InsertItem(index, item);
        _SelectedIndex = index;
        OnSelectedChanged(oldIndex, index, oldItem, item);
    }

    protected override void ClearItems()
    {
        var oldIndex = SelectedIndex;
        var oldItem = SelectedItem;
        base.ClearItems();
        if (oldIndex != -1)
            OnSelectedChanged(oldIndex, _SelectedIndex = -1, oldItem, default);
    }

    protected override void MoveItem(int oldIndex, int newIndex)
    {
        if (oldIndex == newIndex)
            return;

        base.MoveItem(oldIndex, newIndex);
        if (oldIndex == SelectedIndex)
        {
            _SelectedIndex = newIndex;
            var item = SelectedItem;
            OnPropertyChanged(new(nameof(SelectedIndex)));
            SelectedChanged?.Invoke(this, new(oldIndex, newIndex, item, item));
        }
    }
    protected override void RemoveItem(int index)
    {
        var prevSelected = SelectedIndex;
        var prevSelectedItem = SelectedItem;
        base.RemoveItem(index);
        if (Count == 0)
        {
            _SelectedIndex = -1;
            OnSelectedChanged(prevSelected, -1, prevSelectedItem, SelectedItem);
        }
        else if (index == prevSelected)
        {
            _SelectedIndex = Math.Min(prevSelected, Count - 1);
            OnSelectedChanged(prevSelected, SelectedIndex, prevSelectedItem, SelectedItem);
        }
        else if (index < prevSelected)
        {
            _SelectedIndex = prevSelected - 1;
            OnSelectedChanged(prevSelected, SelectedIndex, prevSelectedItem, SelectedItem);
        }
    }
    protected override void SetItem(int index, T item)
    {
        if (index != SelectedIndex)
        {
            base.SetItem(index, item);
            return;
        }
        var prevSelectedItem = SelectedItem;
        base.SetItem(index, item);
        OnSelectedItemChanged(prevSelectedItem, item);
    }

    public void RemoveSelectedItem()
    {
        var ix = SelectedIndex;
        if ((uint)ix < (uint)Count)
            RemoveAt(ix);
    }


    public class SelectedItemChangedEventArgs : EventArgs
    {
        public int OldIndex { get; }
        public int NewIndex { get; }
        public T? OldItem { get; }
        public T? NewItem { get; }
        public SelectedItemChangedEventArgs(int oldIndex, int newIndex, T? oldItem, T? newItem)
        {
            OldIndex = oldIndex;
            NewIndex = newIndex;
            OldItem = oldItem;
            NewItem = newItem;
        }
    }
}
