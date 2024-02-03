﻿using Domain.RoomAggregate;
using Domain.RoomAggregate.ValueObjects;

namespace Application.Common.Interfaces.Perception;

public interface IRoomRepository
{
    public Task<Room?> GetAsync(RoomId id, CancellationToken cancellationToken = default);

    public Task AddAsync(Room room, CancellationToken cancellationToken = default!);
    
    public Task AddShopItemToRoomAsync(
        OwnedShopItem shopItem, RoomId roomId, CancellationToken cancellationToken = default);
    
    public Task AddShopItemsToRoomAsync(
        IEnumerable<OwnedShopItem> shopItems, RoomId roomId, CancellationToken cancellationToken = default);
}