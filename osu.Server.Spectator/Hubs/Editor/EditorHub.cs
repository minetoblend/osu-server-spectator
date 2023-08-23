// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using osu.Game.Online.Editor;
using osu.Game.Online.Multiplayer;
using osu.Server.Spectator.Database;
using osu.Server.Spectator.Entities;

namespace osu.Server.Spectator.Hubs.Editor;

public class EditorHub : StatefulUserHub<IEditorClient, EditorClientState>, IEditorServer
{
    private readonly IDatabaseFactory databaseFactory;
    protected readonly EntityStore<ServerEditorRoom> Rooms;
    protected readonly EditorHubContext HubContext;

    //TODO: replace this with a database backed ID
    private static long nextRoomId = 1;

    public EditorHub(IDistributedCache cache, EntityStore<EditorClientState> users, IDatabaseFactory databaseFactory, EntityStore<ServerEditorRoom> rooms, IHubContext<EditorHub> context)
        : base(cache, users)
    {
        this.databaseFactory = databaseFactory;
        Rooms = rooms;
        HubContext = new EditorHubContext(context, rooms, users);
    }

    public Task LeaveRoom()
    {
        throw new System.NotImplementedException();
    }

    public async Task<EditorRoom> CreateAndJoinRoom(SerializedEditorBeatmap beatmap)
    {
        Log($"Attempting to create and join room");

        bool isRestricted;
        using (var db = databaseFactory.GetInstance())
            isRestricted = await db.IsUserRestrictedAsync(CurrentContextUserId);

        if (isRestricted)
            throw new InvalidStateException("Can't create a room when restricted.");

        long roomId = nextRoomId++;

        using (var userUsage = await GetOrCreateLocalUserState())
        {
            if (userUsage.Item != null)
            {
                // if the user already has a state, it means they are already in a room and can't join another without first leaving.
                throw new InvalidStateException("Can't join a room when already in another room.");
            }
        }

        // add the user to the room.
        var roomUser = new EditorRoomUser(CurrentContextUserId);

        using (var roomUsage = await Rooms.GetForUse(roomId, true))
        {
            if (roomUsage.Item != null)
                throw new InvalidStateException($"Room {roomId} already exists.");

            roomUsage.Item = new ServerEditorRoom(
                roomId, HubContext, new BeatmapSnapshot(beatmap.EncodedBeatmap), beatmap.Files);

            roomUsage.Item.Users.Add(roomUser);

            return JsonConvert.DeserializeObject<EditorRoom>(JsonConvert.SerializeObject(roomUsage.Item))
                   ?? throw new InvalidOperationException();
        }
    }

    public async Task<EditorRoomJoinedResult> JoinRoom(long roomId)
    {
        Log($"Attempting to join room {roomId}");

        bool isRestricted;
        using (var db = databaseFactory.GetInstance())
            isRestricted = await db.IsUserRestrictedAsync(CurrentContextUserId);

        if (isRestricted)
            throw new InvalidStateException("Can't join a room when restricted.");

        using (var userUsage = await GetOrCreateLocalUserState())
        {
            if (userUsage.Item != null)
            {
                // if the user already has a state, it means they are already in a room and can't join another without first leaving.
                throw new InvalidStateException("Can't join a room when already in another room.");
            }
        }

        // add the user to the room.
        var roomUser = new EditorRoomUser(CurrentContextUserId);

        using (var roomUsage = await Rooms.GetForUse(roomId))
        {
            if (roomUsage.Item == null)

                throw new InvalidStateException("Can't join a room that doesn't exist.");

            ServerEditorRoom room = roomUsage.Item;

            if (room.Users.Any(u => u.UserID == roomUser.UserID))
                throw new InvalidOperationException($"User {roomUser.UserID} attempted to join room {room.RoomID} they are already present in.");

            await Clients.Group(GetGroupId(roomId)).UserJoined(roomUser);

            room.AddUser(roomUser);

            var settings = new JsonSerializerSettings
            {
                // explicitly use Auto here as we are not interested in the top level type being conveyed to the user.
                TypeNameHandling = TypeNameHandling.Auto,
            };

            var clonedRoom = JsonConvert.DeserializeObject<EditorRoom>(JsonConvert.SerializeObject(room, settings), settings)
                             ?? throw new InvalidOperationException();

            return new EditorRoomJoinedResult(clonedRoom, room.Beatmap, room.BeatmapFiles, room.StagedCommands);
        }
    }

    public Task SubmitCommands(byte[] commands)
    {
        throw new System.NotImplementedException();
    }

    public Task ChangeState(EditorUserState state)
    {
        throw new System.NotImplementedException();
    }

    /// <summary>
    /// Get the group ID to be used for multiplayer messaging.
    /// </summary>
    /// <param name="roomId">The databased room ID.</param>
    public static string GetGroupId(long roomId) => $"room:{roomId}";
}
