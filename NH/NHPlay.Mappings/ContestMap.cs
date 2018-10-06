using System;
using FluentNHibernate.Mapping;
using NHPlay.Models;

namespace NHPlay.Mappings
{
    public class ContestMap : ClassMap<Contest>
    {
        public ContestMap()
        {
            Table("Contest");
            Id(x => x.Id);
            References(x => x.Game)
                            .Column("GameId")
                            .ForeignKey("Id");
            // how to map an enum value from an integer field
            // notice also that this is a DOUBLE mapping of the same column
            Map(x => x.TypeOfGame)
                            .Column("GameId")
                            .CustomType<TypeOfGame>();
            Map(x => x.OccurredAt).ReadOnly();

            HasMany(x => x.Contestants).KeyColumn("ContestId").Cascade.None().Inverse();
            HasManyToMany(x => x.Players)
                            .Table("Contestant")
                            .ParentKeyColumn("ContestId")
                            .ChildKeyColumn("PlayerId")
                            .Cascade.None().Inverse();
        }
    }
}