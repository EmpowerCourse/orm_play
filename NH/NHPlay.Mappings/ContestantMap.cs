using System;
using FluentNHibernate.Mapping;
using NHPlay.Models;

namespace NHPlay.Mappings
{
    public class ContestantMap : ClassMap<Contestant>
    {
        public ContestantMap()
        {
            Table("Contestant");
            Id(x => x.Id);
            References(x => x.Player)
                            .Column("PlayerId")
                            .ForeignKey("Id");
            References(x => x.Contest)
                            .Column("ContestId")
                            .ForeignKey("Id");
            Map(x => x.Score);
        }
    }
}