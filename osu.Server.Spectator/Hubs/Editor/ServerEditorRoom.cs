// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Game.Online.Editor;

namespace osu.Server.Spectator.Hubs.Editor;

public class ServerEditorRoom : EditorRoom
{
    private readonly IEditorHubContext context;

    public BeatmapSnapshot Beatmap;

    public List<EditorCommandEvent> StagedCommands = new();

    public Dictionary<string, byte[]> BeatmapFiles;

    public ServerEditorRoom(long roomId, IEditorHubContext context, BeatmapSnapshot beatmap, Dictionary<string, byte[]> beatmapFiles)
        : base(roomId)
    {
        this.context = context;
        Beatmap = beatmap;
        BeatmapFiles = beatmapFiles;
    }
}
