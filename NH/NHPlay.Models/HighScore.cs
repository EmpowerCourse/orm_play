using System;

namespace NHPlay.Models
{
    public class HighScore
    {
        public virtual int Id { get; set; }
        public virtual Player Player { get; set; }
        public virtual Game Game { get; set; }
        public virtual int Score { get; set; }
        public virtual DateTime OccurredAt { get; set; }
    }
}
