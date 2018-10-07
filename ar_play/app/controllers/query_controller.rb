class QueryController < ApplicationController
  include ActionController::MimeResponds

  def q1
    @players = Player.where("initials like ?", '%A%').where("id <= ?", 9).order(:initials)
  end

  def q2
  	sql = 'select g.id, g.name from games g limit 2' # can use %s to replace tokens positionally
    @games = ActiveRecord::Base.connection.select_all(ActiveRecord::Base.send(:sanitize_sql_array, [sql]))
    # @games = Game.all
    # @games = Game.take(2)
    # @games = Game.joins(contests: :players).take(2)
    # @games = Game.includes(contests: :players).take(2)
  end

  def q3
  	# self.dependent_source_versions.joins(:source).where(active: true, 'core_sources.deleted' => false)
    # Game.joins(:source).where(active: true, 'core_sources.deleted' => false)
    # Post.joins(:comments).where(:comments => {author: 'Derek'}).map { |post| post.comments.size }
  end

  def q4
  end
end
