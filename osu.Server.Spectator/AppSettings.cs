// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;

namespace osu.Server.Spectator
{
    public static class AppSettings
    {
        public static bool SaveReplays { get; set; }

        #region For use with FileScoreStorage

        public static string ReplaysPath { get; set; }

        #endregion

        #region For use with S3ScoreStorage

        public static string S3Key { get; }
        public static string S3Secret { get; }
        public static string ReplaysBucket { get; }

        #endregion

        public static string RedisHost { get; }

        static AppSettings()
        {
            SaveReplays = Environment.GetEnvironmentVariable("SAVE_REPLAYS") == "1";
            ReplaysPath = Environment.GetEnvironmentVariable("REPLAYS_PATH") ?? "replays";
            S3Key = Environment.GetEnvironmentVariable("S3_KEY") ?? string.Empty;
            S3Secret = Environment.GetEnvironmentVariable("S3_SECRET") ?? string.Empty;
            ReplaysBucket = Environment.GetEnvironmentVariable("REPLAYS_BUCKET") ?? string.Empty;

            RedisHost = Environment.GetEnvironmentVariable("REDIS_HOST") ?? "localhost";
        }
    }
}
