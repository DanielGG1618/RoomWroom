﻿using System.Text.Json;
using Application.Receipts.Interfaces;
using Domain.Common.Enums;
using Domain.Common.ValueObjects;
using Domain.ReceiptAggregate;
using Domain.ReceiptAggregate.ValueObjects;

namespace Infrastructure.Receipts.Perception;

public class FileReceiptRepository : IReceiptRepository
{
    private const string RECEIPTS_FILE = "Receipts.json";
    
    private static readonly Dictionary<string, Receipt> Receipts = InitReceipts();

    public Task<Receipt?> GetAsync(string id, CancellationToken cancellationToken) => 
        Task.FromResult(Receipts.GetValueOrDefault(id));

    public Task<Receipt?> GetFromQrAsync(string qr, CancellationToken cancellationToken = default) => 
        Task.FromResult(Receipts.Values.SingleOrDefault(receipt => receipt.Qr == qr));

    public Task AddAsync(Receipt receipt, CancellationToken cancellationToken)
    {
        Receipts.TryAdd(receipt.Id.Value.ToString(), receipt);
        UpdateReceiptsFile();

        return Task.CompletedTask;
    }

    private static Dictionary<string, Receipt> InitReceipts()
    {
        using FileStream stream = new(RECEIPTS_FILE, FileMode.OpenOrCreate, FileAccess.Read);
        Span<byte> buffer = stackalloc byte[(int)stream.Length];
        _ = stream.Read(buffer);

        if (buffer.Length == 0)
            return [];

        Utf8JsonReader jsonReader = new(buffer);
        JsonElement jsonElement = JsonElement.ParseValue(ref jsonReader);

        Dictionary<string, Receipt>? receipts = jsonElement.Deserialize();
        
        return receipts ?? [];
    }
    
    private static void UpdateReceiptsFile()
    {
        using FileStream stream = new(RECEIPTS_FILE, FileMode.OpenOrCreate, FileAccess.Write);
        string json = Receipts.Serialize();
        Span<byte> buffer = System.Text.Encoding.UTF8.GetBytes(json);

        stream.Write(buffer);
    }
}

file static class SerializationExtensions
{
    public static Dictionary<string, Receipt>? Deserialize(this JsonElement jsonElement)
    {
        var receiptDtos = jsonElement.Deserialize<Dictionary<string, ReceiptDto>>();
        if (receiptDtos is null)
            return null;
        
        Dictionary<string, Receipt> receipts = [];
        foreach ((string? key, (string? id, string? qr, List<ReceiptItemDto>? receiptItemDtos)) in receiptDtos)
        {
            receipts.Add(key, Receipt.Create(
                id,
                qr,
                receiptItemDtos.ConvertAll(item =>
                {
                    Enum.TryParse(item.PriceCurrency, true, out Currency currency);
                    return new ReceiptItem(
                        item.Name,
                        new Money(item.PriceAmount, currency),
                        item.Quantity,
                        item.AssociatedShopItemId);
                })));
        }

        return receipts;
    }

    public static string Serialize(this Dictionary<string, Receipt> receipts)
    {
        Dictionary<string, ReceiptDto> receiptDtos = [];
        foreach ((string? key, Receipt? receipt) in receipts)
        {
            receiptDtos.Add(key, new(
                receipt.Id,
                receipt.Qr,
                receipt.Items.Select(item => new ReceiptItemDto(
                    item.Name,
                    item.Price.Amount, 
                    item.Price.Currency.ToString(),
                    item.Quantity,
                    item.AssociatedShopItemId)).ToList()));
        }

        return JsonSerializer.Serialize(receiptDtos);
    }

    private record ReceiptDto(string Id, string? Qr, List<ReceiptItemDto> Items);

    private record ReceiptItemDto(
        string Name,
        decimal PriceAmount,
        string PriceCurrency,
        decimal Quantity,
        string? AssociatedShopItemId);
}