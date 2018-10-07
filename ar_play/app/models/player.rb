class Player < ApplicationRecord
  has_many :contests
  has_many :high_score_games, through: :high_scores
  
  def win_count
  	# 
  end
end
