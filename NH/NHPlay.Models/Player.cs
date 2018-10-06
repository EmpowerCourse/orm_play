using System;
using System.Collections.Generic;

namespace NHPlay.Models
{
    public class Player
    {
        public virtual int Id { get; set; }
        public virtual string Initials { get; set; }
        public virtual DateTime CreatedAt { get; set; }
        public virtual int WinCount { get; set; }
        public virtual ICollection<Contest> Contests { get; set; }
        public virtual ICollection<Game> HighScoreGames { get; set; }
    }
}
