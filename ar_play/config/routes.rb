Rails.application.routes.draw do
  # For details on the DSL available within this file, see http://guides.rubyonrails.org/routing.html
  get 'q1' => 'query#q1'
  get 'q2' => 'query#q2'
  get 'q3' => 'query#q3'
  get 'q4' => 'query#q4'
end
