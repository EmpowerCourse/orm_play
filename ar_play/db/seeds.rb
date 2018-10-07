# This file should contain all the record creation needed to seed the database with its default values.
# The data can then be loaded with the rails db:seed command (or created alongside the database with db:setup).
#
# Examples:
#
#   movies = Movie.create([{ name: 'Star Wars' }, { name: 'Lord of the Rings' }])
#   Character.create(name: 'Luke', movie: movies.first)

def random_score
  (rand(100_000) / 100) * 100
end

games = Game.create([{ name: 'Centipede' }, { name: 'Space Invaders' }, { name: 'Pong'}, { name: 'Pac-Man'}, { name: 'Q*bert'}, { name: 'DigDug'}, { name: 'Donkey Kong'}])
players = Player.create([{ initials: 'AT' }, { initials: 'TT' }, { initials: 'MC'}, { initials: 'BA'}, { initials: 'BG'}, { initials: 'SG'}, { initials: 'AW'}, { initials: 'SH'}, { initials: 'HT'}])
# add a bunch more randomly
10_000.times do |i|
  p = Player.new(initials: ('A'..'Z').to_a.shuffle[0,3].join)
  p.save!
  players << p
end

(1..7).to_a.shuffle.each_with_index do |game_id, player_id|
  hs = HighScore.new(player_id: player_id+1, game_id: game_id, score: random_score)
  hs.save!
end

700.times do |i| 
  contest = Contest.new(game_id: games.shuffle[0].id)
  contest.save!
  Contestant.new(contest_id: contest.id, player_id: players.shuffle[0].id, score: random_score).save!
  Contestant.new(contest_id: contest.id, player_id: players.shuffle[0].id, score: random_score).save!
end
