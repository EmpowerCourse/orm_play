using System;

namespace NHPlay.Models
{
    public class Contestant
    {
        public virtual int Id { get; set; }
        public virtual Contest Contest { get; set; }
        public virtual Player Player { get; set; }
        public virtual int Score { get; set; }
    }
}
