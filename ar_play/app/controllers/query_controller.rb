class QueryController < ApplicationController
  include ActionController::MimeResponds

  def q1
    # @players = Player.where("initials like ?", '%A%').where("id <= ?", 9).order(:initials)
    # @players = Player.order(:id).offset(3).take(3)
    # players who have a high score on Pac-Man
    @players = Player.joins(contestants: [contest: :game]).where('games.name = ?', 'Pac-Man')
  end

  def q2
  	# sql = 'select g.id, g.name from games g limit 2' # can use %s to replace tokens positionally
    # @games = ActiveRecord::Base.connection.select_all(ActiveRecord::Base.send(:sanitize_sql_array, [sql]))
    @games = Game.all
    # @games = Game.take(2)
    # @games = Game.joins(contests: :players).take(2)
    # @games = Game.includes(contests: :players).take(2)
    # @games = Game.preload(:contests).take(3)
    # @games = Game.eager_load(:contests).take(4)
    # @games = [Game.find_by(name: 'Centipede')]
  end

  def q3
    @contests = Contest.joins(:game).includes(:game, :players).references(:players)
      .where('players.id > ? and games.name like ?', 100, '%C%')
  end

  def q4
    @players = Player.where('id < 50').pluck(:id, :initials)
    # @players = Player.select(:id, :initials)
  end

  def q5
    # list all games and a list of related high scores
    @games = Game.left_joins(:high_scores).where('high_scores.score > 5000')
    # @games = Game.includes(:high_scores).references(:high_scores).where('high_scores.score > 5000')
  end

  def q6
    @high_score_groups = HighScore.group(:player_id).count(:player_id)
  end

  def q7
    # note that ONLY player_id may be referenced in the HighScore model
    # attempts to access any other will result in a missing attribute error
    # EVEN THOUGH the attribute is clearly defined on the model! 
    @high_score_players = HighScore.select(:player_id).distinct
  end
end
