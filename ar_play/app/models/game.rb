class Game < ApplicationRecord
  has_many :contests
  has_many :high_scores
end
