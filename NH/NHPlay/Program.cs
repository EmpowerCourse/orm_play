using System;
using System.Linq;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;
using NHibernate.Linq;
using FluentNHibernate.Cfg;
using HibernatingRhinos.Profiler.Appender.NHibernate;
using NHPlay.Models;
using NHPlay.Mappings;
using FluentNHibernate.Cfg.Db;
using System.Collections.Generic;

namespace NHPlay
{
    class Program
    {
        private const string CN_STR = @"Data Source=.\S2014E;Initial Catalog=atari;Persist Security Info=True;Trusted_Connection=Yes;";
        private static ISessionFactory _sessionFactory;

        static void Main(string[] args)
        {
            _sessionFactory = initializeNH();

            // list a few players
            listPlayers();
            //Console.WriteLine("++++++++++++++++++++++++++++++++++++++++++++++++++++");
            // listACoupleOfGames();
            //Console.WriteLine("++++++++++++++++++++++++++++++++++++++++++++++++++++");
            // listHighScores();
            //Console.WriteLine("++++++++++++++++++++++++++++++++++++++++++++++++++++");
            // listPlayersOf("Pac-Man");
            // listPlayersOf("Pac-Man", "T");
            // Console.WriteLine("++++++++++++++++++++++++++++++++++++++++++++++++++++");
            // listAllGamesAndContestsForEach();
            Console.ReadKey(true);
        }

        private static void listPlayers()
        {
            Console.WriteLine("++ Listing players ++++++++++++++++++++++++++++++++");
            using (var session = _sessionFactory.OpenSession())
            {
                IList<Player> playerList = session.Query<Player>()
                    .Where(x => x.Initials.Contains("A") && x.Id <= 9)
                    .OrderBy(x => x.Initials)
                    .ToList();
                Console.WriteLine($"Player Count 1: {playerList.Count}");

                playerList = (from p in session.Query<Player>()
                                  where p.Initials.Contains("A") && p.Id <= 9
                                  orderby p.Initials
                                  select p).ToList();
                Console.WriteLine($"Player Count 2: {playerList.Count}");

                playerList = session.CreateQuery("from Player p where p.Initials like '%A%' and p.Id <= 9 order by p.Initials").List<Player>();
                Console.WriteLine($"Player Count 3: {playerList.Count}");

                playerList = session.CreateSQLQuery("select * from Player p where p.Initials like '%A%' and p.Id <= 9 order by p.Initials")
                    .SetResultTransformer(Transformers.AliasToBean(typeof(Player)))
                    .List<Player>();
                Console.WriteLine($"Player Count 4: {playerList.Count}");

                playerList = session.CreateCriteria<Player>()
                    .Add(Expression.Like("Initials", "%A%"))
                    .Add(Expression.Le("Id", 9))
                    .AddOrder(Order.Asc("Initials"))
                    .List<Player>();
                Console.WriteLine($"Player Count 5: {playerList.Count}");
                // ...and there's 1 more that I don't care for (not shown) called "QueryOver" syntax
            }
        }

        private static void listACoupleOfGames()
        {
            Console.WriteLine("++ Listing games ++++++++++++++++++++++++++++++++");
            Console.WriteLine("Lets examine 'lazy loading' behaviour, the default");
            Console.WriteLine("...first referencing only simple properties");
            using (var session = _sessionFactory.OpenSession())
            {
                // Note the position of the Take() method
                // one of these can be commented out, which one should we keep and why?
                var gameList = session.Query<Game>()
                    .OrderBy(x => x.Name)
                    .Take(2)
                    .ToList()
                    .Take(2);
                foreach(var g in gameList)
                {
                    Console.WriteLine($"Game Id: {g.Id}, Name: {g.Name}");
                }
                Console.WriteLine("now iterating over the same list but referencing the list properties");
                foreach (var g in gameList)
                {
                    Console.WriteLine($"Game Id: {g.Id}, Contests: {g.Contests.Count}, showing up to the first 3");
                    foreach(var c in g.Contests.Take(3))
                    {
                        Console.WriteLine($"    Contest Id: {c.Id}, Occurred: {c.OccurredAt: dd-MMM-yy hh:mm:ss}");
                    }
                }
                Console.WriteLine("Lets examine 'eager loading' behaviour to do the same operation");
                gameList = session.Query<Game>()
                        // .FetchMany(x => x.HighScores)
                        .FetchMany(x => x.Contests)
                    .OrderBy(x => x.Name)
                    .ToList()
                    .Take(2)
                    .ToList();
                foreach (var g in gameList)
                {
                    Console.WriteLine($"Game Id: {g.Id}, Name: {g.Name}");
                }
                Console.WriteLine("now iterating over the same list but referencing the list properties");
                foreach (var g in gameList)
                {
                    Console.WriteLine($"Game Id: {g.Id}, Contests: {g.Contests.Count}, showing up to the first 3");
                    foreach (var c in g.Contests.Take(3))
                    {
                        Console.WriteLine($"    Contest Id: {c.Id}, Occurred: {c.OccurredAt: dd-MMM-yy hh:mm:ss}");
                    }
                }
            }
        }

        private static void listHighScores()
        {
            Console.WriteLine("++ Listing high scores ++++++++++++++++++++++++++++++++");
            Console.WriteLine("Lets examine default behaviour");
            using (var session = _sessionFactory.OpenSession())
            {
                var highScores = session.Query<HighScore>()
                        .Fetch(x => x.Game)
                        .Fetch(x => x.Player)
                    .ToList();
                foreach (var hs in highScores)
                {
                    Console.WriteLine($"High Score Id: {hs.Id}, Player: {hs.Player.Initials}, Game: {hs.Game.Name}, Score: {hs.Score}");
                }
                Console.WriteLine("now lets be more specific with what we want back");
                var highScoreResults = session.Query<HighScore>()
                        .Fetch(x => x.Game)
                        .Fetch(x => x.Player)
                        .Select(x => new
                        {
                            HighScoreId = x.Id,
                            PlayerInitials = x.Player.Initials,
                            GameName = x.Game.Name,
                            Score = x.Score
                        })
                    .ToList();
                foreach (var hs in highScoreResults)
                {
                    Console.WriteLine($"High Score Id: {hs.HighScoreId}, Player: {hs.PlayerInitials}, Game: {hs.GameName}, Score: {hs.Score}");
                }
            }
        }

        private static void listPlayersOf(string gameName, string initialsMustContain = null)
        {
            Console.WriteLine($"++ Showing {gameName} players {((initialsMustContain == null) ? String.Empty : "where initials contain " + initialsMustContain)} ++++++++++++++++++++++++++++++++");
            using (var session = _sessionFactory.OpenSession())
            {
                // note the join syntax we can use with NH LINQ support
                // note that either object equivalence or field equivalence can be specified, up to you
                var query =
                        from p in session.Query<Player>()
                        join c in session.Query<Contestant>() on p equals c.Player
                        join t in session.Query<Contest>() on c.Contest.Id equals t.Id
                        where t.Game.Name == gameName
                        select p.Initials;
                // note how we can add to our query intent before sending to the database
                if (!String.IsNullOrEmpty(initialsMustContain))
                {
                    query = query.Where(x => x.Contains(initialsMustContain));
                }
                var playerInitialsList = query.Distinct().ToList();
                Console.WriteLine($"The following people play {gameName}, listing first 5 only");
                foreach (var p in playerInitialsList.Take(5))
                {
                    Console.WriteLine($"    {p}");
                }
            }
        }

        private static void listAllGamesAndContestsForEach()
        {
            Console.WriteLine("++ Show a left join and aggregation ++++++++++++++++++++++++++++++++");
            using (var session = _sessionFactory.OpenSession())
            {
                Console.WriteLine("++ NH is _very_ smart - take a peek at the SQL here +++++++++++++++");
                var games = session.Query<Game>()
                    .OrderBy(x => x.Name)
                    .Select(x => new
                    {
                        GameId = x.Id,
                        GameName = x.Name,
                        ContestantCount = x.Contests.Count
                    }).ToList();
                foreach (var g in games)
                {
                    Console.WriteLine($"Game Id: {g.GameId}, Name: {g.GameName}, Contestants: {g.ContestantCount}");
                }
                Console.WriteLine("++ Now lets force the left join here +++++++++++++++");
                // note how you can parameterize your HQL
                // note also how multiple objects can be returned from a single query
                var games2 = session.CreateQuery(
@"select g, p 
from Game g left join g.Contests t left join t.Players p 
where p.Initials like :initials or p is null 
order by g.Name")
                    .SetParameter("initials", "%A%")
                    .List();
                foreach (object[] r in games2)
                {
                    Game g = (Game)r[0];
                    Player p = (Player)r[1];
                    Console.WriteLine($"Game Id: {g.Id}, Name: {g.Name}, Player: {(p == null ? "(none)" : p.Initials)}");
                }
            }
        }

        private static ISessionFactory initializeNH()
        {
            FluentConfiguration config;
#if DEBUG
            config = Fluently.Configure()
                .Database(MsSqlConfiguration.MsSql2008
                              .ShowSql()
                              .FormatSql()
                              .ConnectionString(p => p.Is(CN_STR))
                              // .AdoNetBatchSize(20)
                              .DefaultSchema("dbo"))
                .Mappings(m =>
                {
                    m.FluentMappings.AddFromAssemblyOf<PlayerMap>();
                });
#else
            config = Fluently.Configure()
                .Database(MsSqlConfiguration.MsSql2008
                              .ConnectionString(p => p.Is(CN_STR))
                              .DefaultSchema("dbo"))
                .Mappings(m =>
                              {
                                  m.FluentMappings.AddFromAssemblyOf<UserMap>();
                              });
#endif
            NHibernateProfiler.Initialize();
            return config.BuildSessionFactory();
        }

    }
}
