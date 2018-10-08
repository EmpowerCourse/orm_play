class Player < ApplicationRecord
  has_many :high_score_games, through: :high_scores
  has_many :contestants
end
