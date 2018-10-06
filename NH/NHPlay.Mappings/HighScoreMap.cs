using System;
using FluentNHibernate.Mapping;
using NHPlay.Models;

namespace NHPlay.Mappings
{
    public class HighScoreMap : ClassMap<HighScore>
    {
        public HighScoreMap()
        {
            Table("HighScore");
            Id(x => x.Id);
            References(x => x.Player)
                            .Column("PlayerId")
                            .ForeignKey("Id");
            References(x => x.Game)
                            .Column("GameId")
                            .ForeignKey("Id");
            Map(x => x.Score);
            Map(x => x.OccurredAt).ReadOnly();
        }
    }
}