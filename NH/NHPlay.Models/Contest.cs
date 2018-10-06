using System;
using System.Collections.Generic;

namespace NHPlay.Models
{
    public class Contest
    {
        public virtual int Id { get; set; }
        public virtual Game Game { get; set; }
        public virtual TypeOfGame TypeOfGame { get; set; }
        public virtual DateTime OccurredAt { get; set; }
        public virtual ICollection<Contestant> Contestants { get; set; }
        public virtual ICollection<Player> Players { get; set; }
    }
}
