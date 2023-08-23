// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Microsoft.AspNetCore.SignalR;
using osu.Server.Spectator.Entities;

namespace osu.Server.Spectator.Hubs.Editor;

public class EditorHubContext : IEditorHubContext
{
    private readonly IHubContext<EditorHub> context;
    private readonly EntityStore<ServerEditorRoom> rooms;
    private readonly EntityStore<EditorClientState> users;

    public EditorHubContext(IHubContext<EditorHub> context, EntityStore<ServerEditorRoom> rooms, EntityStore<EditorClientState> users)
    {
        this.context = context;
        this.rooms = rooms;
        this.users = users;
    }
}
