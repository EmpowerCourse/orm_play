using System;
using FluentNHibernate.Mapping;
using NHPlay.Models;

namespace NHPlay.Mappings
{
    public class PlayerMap : ClassMap<Player>
    {
        public PlayerMap()
        {
            Table("Player");
            // how to use a primary key that is not auto-generated
            // Id(x => x.Id).GeneratedBy.Assigned(); 

            // how to map a NULLable column (important!)
            // Map(x => x.Initials).Nullable();

            // how to map a column whose name does not match your desired property name
            // Map(x => x.Initials).Column("SomeCrazyColumnName");

            Id(x => x.Id);
            Map(x => x.Initials);
            Map(x => x.CreatedAt).ReadOnly();

            // how to map a 1 to many child table
            HasMany(x => x.Contests).KeyColumn("ContestId").Cascade.None().Inverse();

            // how to map a many to many from one side of the cross reference table through to the other
            HasManyToMany(x => x.HighScoreGames)
                            .Table("HighScores")
                            .ParentKeyColumn("PlayerId")
                            .ChildKeyColumn("GameId")
                            // how to filter the relationship table if necessary
                            // .Where("DeactivatedAt IS NULL")
                            // how to explicitly avoid up front joins to get the data on initial data fetch (but this is the default)
                            // .LazyLoad()
                            // how to tell NH to not worry about data maintenance for this relationship, make it essentially a read only
                            .Cascade.None().Inverse();

            // how to map a query result as a virtual column for convenience
            Map(x => x.WinCount).Formula(
@"(SELECT COUNT(*)
FROM dbo.Contestant c INNER JOIN dbo.Contest t ON t.Id = c.ContestId
WHERE c.PlayerId = Id
	AND  (SELECT TOP 1 x.PlayerId FROM dbo.Contestant x WHERE x.ContestId = t.Id) = Id
)").ReadOnly();
        }
    }
}