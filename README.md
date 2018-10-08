# orm_play
NHibernate and ActiveRecord comparison

# Setup the C#, NHibernate application
* Ensure .net core 2.0 is installed
* Ensure a version of SQL Server is installed
* Execute the NH/t-sql_atari.sql script against your local instance of SQL Server to create and populate the database
* Open the NH/NHPlay/NHPlay.csproj file in Visual Studio 2017
* Modify the CN_STR variable in NH/NHPlay/Program.cs to have a connection string valid for your local SQL Server instance

# Setup the Ruby, Rails, ActiveRecord application
* Ensure ruby 2.4 or later is installed
* Ensure rails 5 or later is installed
* Ensure version 9.6 or later of PostgreSql is installed
* execute `rails new ar_play --database=postgresql`
* ensure your config/database.yml has something close to this
default: &default
  adapter: postgresql
  encoding: unicode
  pool: <%= ENV.fetch("RAILS_MAX_THREADS") { 5 } %>

development:
  <<: *default
  database: ar_play_development
  username: postgres
  password: postgres

test:
  <<: *default
  database: ar_play_test
  username: postgres
  password: postgres

production:
  <<: *default
  database: ar_play_production

* execute `rake db:create`
* execute `rake db:migrate`
* execute `rake db:seed`
* execute rails s

// rake db:migrate:reset (drop,create,migrate)
// accepts_nested_attributes_for
// included do
// has_one :task, through: :source_collection_task
// has_one :source_collection_task
