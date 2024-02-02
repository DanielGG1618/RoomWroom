﻿using Application.Rooms.Interfaces;
using Domain.RoomAggregate;
using Domain.RoomAggregate.ValueObjects;
using Domain.UserAggregate.ValueObjects;

namespace Application.Rooms.Commands;

public class CreateRoomHandler(IRoomRepository repository) : IRequestHandler<CreateRoomCommand, ErrorOr<Room>>
{
    private readonly IRoomRepository _repository = repository;

    public async Task<ErrorOr<Room>> Handle(CreateRoomCommand command, CancellationToken cancellationToken)
    {
        var room = Room.CreateNew(
            command.Name,
            command.UserIds.Select(id =>
                UserId.Create(Guid.Parse(id))),
            command.OwnedShopItems.Select(item =>
                OwnedShopItem.Create(item.ShopItemId!, item.Quantity)));

        await _repository.AddAsync(room, cancellationToken);

        return room;
    }
}