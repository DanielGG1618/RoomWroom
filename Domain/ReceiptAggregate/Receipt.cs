﻿using Domain.ReceiptAggregate.ValueObjects;

namespace Domain.ReceiptAggregate;

public class Receipt : AggregateRoot<ReceiptId>
{
    private readonly List<ReceiptItem> _items;

    public IReadOnlyList<ReceiptItem> Items => _items.AsReadOnly();
    
    public string? Qr { get; private set; }

    private Receipt(ReceiptId id, string? qr, IEnumerable<ReceiptItem> items) : base(id)
    {
        _items = items
            .OrderBy(item => item.Name)
            .CombineSame()
            .Where(item => item is { Quantity: > 0, Price.Amount: > 0 })
            .ToList();
        
        Qr = qr;
    }

    private Receipt() { }

    public static Receipt CreateNew(IEnumerable<ReceiptItem> items, string? qr) => 
        new(ReceiptId.CreateUnique(), qr, items);

    public static Receipt Create(ReceiptId id, string? qr, IEnumerable<ReceiptItem> items) =>
        new(id, qr, items);
}

file static class ReceiptItemExtensions
{
    public static IEnumerable<ReceiptItem> CombineSame(this IOrderedEnumerable<ReceiptItem> items)
    {
        ReceiptItem? previousItem = null;

        foreach (ReceiptItem currentItem in items)
        {
            if (currentItem.Price == previousItem?.Price && currentItem.Name == previousItem.Name)
            {
                decimal quantitySum = previousItem.Quantity + currentItem.Quantity;
                previousItem = new(currentItem.Name, currentItem.Price, quantitySum);
                
                continue;
            }
            
            if (previousItem is not null)
                yield return previousItem;

            previousItem = currentItem;
        }

        if (previousItem is not null)
            yield return previousItem;
    }
}