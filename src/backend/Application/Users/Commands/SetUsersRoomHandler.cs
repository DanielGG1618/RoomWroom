﻿using Application.Common.Interfaces.Perception;
using Domain.Common.Errors;

namespace Application.Users.Commands;

public class SetUsersRoomHandler(
    IUserRepository userRepository,
    IRoomRepository roomRepository
) : IRequestHandler<SetUsersRoomCommand, ErrorOr<Success>>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IRoomRepository _roomRepository = roomRepository;
    
    public async Task<ErrorOr<Success>> Handle(SetUsersRoomCommand command, CancellationToken cancellationToken)
    {
        var (userId, roomId) = command;

        var user = await _userRepository.GetAsync(userId, cancellationToken);
        if (user is null)
            return Errors.User.NotFound;

        var room = await _roomRepository.GetAsync(roomId, cancellationToken);
        if (room is null)
            return Errors.Room.NotFound;
        
        room.AddUser(userId);
        
        user.SetRoom(roomId);

        return Result.Success;
    }
}