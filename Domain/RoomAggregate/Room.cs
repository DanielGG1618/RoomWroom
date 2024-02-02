﻿using Domain.RoomAggregate.ValueObjects;
using Domain.UserAggregate.ValueObjects;

namespace Domain.RoomAggregate;

public class Room : AggregateRoot<RoomId>
{
    public string Name { get; private set; } = default!;
    public IEnumerable<UserId> UserIds { get; private set; } = default!;
    public IEnumerable<OwnedShopItem> OwnedShopItems
    {
        get => _ownedShopItems;
        private set => _ownedShopItems = value.ToList();
    }
    
    private List<OwnedShopItem> _ownedShopItems = default!;

    private Room(RoomId id, string name, IEnumerable<UserId> userIds, IEnumerable<OwnedShopItem> ownedShopItems)
        : base(id)
    {
        Name = name;
        UserIds = userIds;
        OwnedShopItems = ownedShopItems;
    }
    
    private Room() { }

    public static Room Create(
        RoomId id, string name, IEnumerable<UserId> userIds,
        IEnumerable<OwnedShopItem> ownedShopItems)
        => new(id, name, userIds, ownedShopItems);

    public static Room CreateNew(
        string name, IEnumerable<UserId> userIds, IEnumerable<OwnedShopItem> ownedShopItems) =>
        new(RoomId.CreateUnique(), name, userIds, ownedShopItems);

    public void AddOwnedShopItem(OwnedShopItem shopItem) => _ownedShopItems.Add(shopItem);
}