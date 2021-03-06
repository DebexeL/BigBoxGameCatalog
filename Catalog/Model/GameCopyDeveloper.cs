﻿using System;

namespace Catalog.Model
{
    public class GameCopyDeveloper : ITimestamps
    {
        public int GameCopyId { get; set; }
        public GameCopy Game { get; set; }

        public int DeveloperId { get; set; }
        public Developer Developer { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime LastUpdated { get; set; }
    }
}
