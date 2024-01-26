﻿using Domain.ShopItemAggregate;

namespace Application.ShopItems.Commands;

public record CreateShopItemCommand(string Id, string Name) : IRequest<ErrorOr<ShopItem>>;