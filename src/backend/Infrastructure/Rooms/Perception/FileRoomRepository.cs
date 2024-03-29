﻿using System.Collections.Concurrent;
using System.Text.Json;
using Application.Common.Interfaces.Perception;
using Domain.Common.Enums;
using Domain.Common.ValueObjects;
using Domain.RoomAggregate;
using Domain.RoomAggregate.ValueObjects;
using Domain.UserAggregate.ValueObjects;

namespace Infrastructure.Rooms.Perception;

public class FileRoomRepository : IRoomRepository
{
    private const string SHOP_ITEMS_FILE = "Rooms.json";
    
    private static readonly ConcurrentDictionary<RoomId, Room> Rooms = InitRooms();
    
    public Task<Room?> GetAsync(RoomId id, CancellationToken cancellationToken = default)
        => Task.FromResult(Rooms.GetValueOrDefault(id));
    
    public Task AddAsync(Room room, CancellationToken cancellationToken = default)
    {
        Rooms.TryAdd(room.Id, room);
        UpdateShopItemsFile();
        
        return Task.CompletedTask;
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateShopItemsFile();
        
        return Task.CompletedTask;
    }

    private static ConcurrentDictionary<RoomId, Room> InitRooms()
    {
        using FileStream stream = new(SHOP_ITEMS_FILE, FileMode.OpenOrCreate, FileAccess.Read);
        Span<byte> buffer = stackalloc byte[(int)stream.Length];
        _ = stream.Read(buffer);

        if (buffer.Length == 0)
            return [];

        Utf8JsonReader jsonReader = new(buffer);
        JsonElement jsonElement = JsonElement.ParseValue(ref jsonReader);

        ConcurrentDictionary<RoomId, Room>? shopItems = jsonElement.Deserialize();
        
        return shopItems ?? [];
    }
    
    private static void UpdateShopItemsFile()
    {
        using FileStream stream = new(SHOP_ITEMS_FILE, FileMode.OpenOrCreate, FileAccess.Write);
        string json = Rooms.Serialize();
        Span<byte> buffer = System.Text.Encoding.UTF8.GetBytes(json);

        stream.Write(buffer);
    }
}

file static class SerializationExtensions
{
    public static ConcurrentDictionary<RoomId, Room>? Deserialize(this JsonElement jsonElement)
    {
        if (jsonElement.Deserialize<Dictionary<string, RoomDto>>() is not { } roomDtos)
            return null;

        return new(roomDtos.Select(pair =>
            new KeyValuePair<RoomId, Room>(
                pair.Key!,
                Room.Create(
                    pair.Value.Id!,
                    pair.Value.Name,
                    new Money(pair.Value.BudgetAmount, Enum.Parse<Currency>(pair.Value.BudgetCurrency)),
                    pair.Value.BudgetLowerBound,
                    pair.Value.MoneyRoundingRequired,
                    pair.Value.UserIds.Select(id => UserId.Create(Guid.Parse(id))),
                    pair.Value.OwnedShopItemDtos.Select(item =>
                        new OwnedShopItem(
                            item.ShopItemId!,
                            item.Quantity,
                            new Money(item.PriceAmount, Enum.Parse<Currency>(item.PriceCurrency))
                        )
                    )
                )
            )
        ));
    }

    public static string Serialize(this ConcurrentDictionary<RoomId, Room> rooms) =>
        JsonSerializer.Serialize(rooms.Select(pair =>
            new KeyValuePair<string, RoomDto>(
                pair.Key!,
                new RoomDto(
                    pair.Value.Id!,
                    pair.Value.Name,
                    pair.Value.Budget.Amount,
                    pair.Value.Budget.Currency.ToString(),
                    pair.Value.BudgetLowerBound,
                    pair.Value.MoneyRoundingRequired,
                    pair.Value.UserIds.Select(id =>
                        id.Value.ToString()
                    ),
                    pair.Value.OwnedShopItems.Select(item =>
                        new OwnedShopItemDto
                        (
                            item.ShopItemId!,
                            item.Quantity,
                            item.Price.Amount,
                            item.Price.Currency.ToString()
                        )
                    )
                )
            )
        ).ToDictionary());

    private record RoomDto(
        string Id,
        string Name,
        decimal BudgetAmount,
        string BudgetCurrency,
        decimal BudgetLowerBound,
        bool MoneyRoundingRequired,
        IEnumerable<string> UserIds,
        IEnumerable<OwnedShopItemDto> OwnedShopItemDtos
    );

    private record OwnedShopItemDto(
        string ShopItemId,
        decimal Quantity,
        decimal PriceAmount,
        string PriceCurrency
    );
}
