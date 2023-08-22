// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using osu.Game.Online.Editor;
using osu.Server.Spectator.Entities;

namespace osu.Server.Spectator.Hubs.Editor;

public class EditorHub : StatefulUserHub<IEditorClient, EditorClientState>, IEditorServer
{
    public EditorHub(IDistributedCache cache, EntityStore<EditorClientState> userStates)
        : base(cache, userStates)
    {
    }

    public Task LeaveRoom()
    {
        throw new System.NotImplementedException();
    }

    public Task<EditorRoom> CreateAndJoinRoom(SerializedEditorBeatmap beatmap)
    {
        throw new System.NotImplementedException();
    }

    public Task<EditorRoomJoinedResult> JoinRoom(long roomId)
    {
        throw new System.NotImplementedException();
    }

    public Task SubmitCommands(SerializedEditorCommands commands)
    {
        throw new System.NotImplementedException();
    }

    public Task ChangeState(EditorUserState state)
    {
        throw new System.NotImplementedException();
    }
}
