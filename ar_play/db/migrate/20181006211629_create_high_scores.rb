class CreateHighScores < ActiveRecord::Migration[5.2]
  def change
    create_table :high_scores do |t|
      t.belongs_to :game, foreign_key: true
      t.belongs_to :player, foreign_key: true
      t.integer :score

      t.timestamps
    end
  end
end
