class CreateContestants < ActiveRecord::Migration[5.2]
  def change
    create_table :contestants do |t|
      t.belongs_to :contest, foreign_key: true
      t.belongs_to :player, foreign_key: true
      t.integer :score

      t.timestamps
    end
  end
end
