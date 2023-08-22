// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

namespace osu.Server.Spectator.Hubs.Editor;

public class EditorClientState : ClientState
{
    public EditorClientState(in string connectionId, in int userId)
        : base(in connectionId, in userId)
    {
    }
}
