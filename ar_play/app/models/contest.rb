class Contest < ApplicationRecord
  belongs_to :game
  has_many :contestants
end
