using System;
using FluentNHibernate.Mapping;
using NHPlay.Models;

namespace NHPlay.Mappings
{
    public class GameMap : ClassMap<Game>
    {
        public GameMap()
        {
            Table("Game");
            Id(x => x.Id);
            Map(x => x.Name);
            HasMany(x => x.Contests).KeyColumn("GameId").Cascade.None().Inverse();
            HasMany(x => x.HighScores).KeyColumn("GameId").Cascade.None().Inverse();
        }
    }
}
