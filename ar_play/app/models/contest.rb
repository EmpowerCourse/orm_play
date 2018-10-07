class Contest < ApplicationRecord
  belongs_to :game
  has_many :contestants
  has_many :players, through: :contestants
end
