﻿using Application.Common.Interfaces.Perception;
using Domain.Common.Errors;
using Domain.ReceiptAggregate;
using Domain.ReceiptAggregate.ValueObjects;
using Domain.RoomAggregate;
using Domain.RoomAggregate.ValueObjects;

namespace Application.Rooms.Commands.WithReceipts;

public class AddReceiptToRoomHandler(
    IRoomRepository repository,
    IReceiptRepository receiptRepository
) : IRequestHandler<AddReceiptToRoomCommand, ErrorOr<Success>>
{
    private readonly IRoomRepository _repository = repository;
    private readonly IReceiptRepository _receiptRepository = receiptRepository;

    public async Task<ErrorOr<Success>> Handle(AddReceiptToRoomCommand request, CancellationToken cancellationToken)
    {
        var (receiptId, excludedItemsId, roomId) = request;

        Receipt? receipt = await _receiptRepository.GetAsync(receiptId, cancellationToken);
        if (receipt is null)
            return Errors.Receipt.NotFound(receiptId);

        IEnumerable<OwnedShopItem> shopItemsToAdd = GetItemsAfterExclusion(receipt.Items, excludedItemsId);

        Room? room = await _repository.GetAsync(roomId, cancellationToken);
        if (room is null)
            return Errors.Room.NotFound(roomId);

        room.AddOwnedShopItems(shopItemsToAdd);

        return new Success();
    }

    private static IEnumerable<OwnedShopItem> GetItemsAfterExclusion(
        IReadOnlyList<ReceiptItem> items,
        IReadOnlyList<int> indexesToExclude)
    {
        for (var i = 0; i < items.Count; i++)
        {
            if (indexesToExclude.Contains(i))
                continue;
            
            if (items[i].AssociatedShopItemId is not { } associatedShopItemId)
                continue;

            yield return new OwnedShopItem(associatedShopItemId, items[i].Quantity, items[i].Sum);
        }
    }
}