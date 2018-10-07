class Contestant < ApplicationRecord
  belongs_to :contest
  belongs_to :player
end
