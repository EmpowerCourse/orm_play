using System;
using System.Collections.Generic;
using System.Text;

namespace NHPlay.Models
{
    public class Game
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual ICollection<Contest> Contests { get; set; }
        public virtual ICollection<HighScore> HighScores { get; set; }
    }
}
