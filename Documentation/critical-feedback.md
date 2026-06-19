# Critical Engagement With Feedback

## What We Expected
I anticipated more critical feedback because I think there is a lot of potential in this game.

- I expected the UI and the narrative to draw the most attention.
- I thought players would want more in-depth storylines and broader character backstories.
- Our expectations aligned fairly well with the actual feedback.
- One unexpected variation was the suggestion to add an interactive background.

## What Surprised Us
- The addition of a map or interactive background was unexpected.
- Some of the attendees were not gamers, so that suggestion stood out.
- I did not expect attendees to critique some of the areas we thought were already relatively strong.
- I thought the main focus would be on longer and deeper storylines with more information.

## What We Chose Not To Implement

### Map Per Story
We chose not to include a map for each story because:
- time constraints made it unrealistic
- it would require too much additional work
- prompt engineering would become much harder
- each generated case would need a location-specific map
- this was beyond the scope of the game we were building

### Interactive Background
We decided not to implement an interactive background because:
- it would require extra art production
- it would require more animation and setup work
- there were more achievable and important tasks to focus on first

### More Questions
We chose not to simply allow more questions because:
- a limited number of questions adds difficulty
- it forces the player to think carefully with limited information
- the game already depends heavily on Ollama-generated content
- adding more generated content would increase waiting times

### Typed Player Questions
We did experiment with letting the player type their own questions, but:
- the process was slow
- it felt clunky
- the responses were rarely relevant enough to the current case

## Evaluation Of Feasibility
Some of the suggestions were realistic within the toolset, including:
- adding a timer
- limiting the number of questions
- experimenting with player-directed questioning

Other suggestions were not practical within the toolset or project scope, including:
- a map for each generated case
- an interactive background

Even if an interactive background had been possible, it might have made the clues either too obvious or too confusing, which could have weakened the experience instead of improving it.

## Final Judgement

### Main Feedback We Chose To Focus On
- Giving the player clear key clues to solving the case.
- Giving a limited amount of questions to be asked.
- Adding a timer variant of gameplay for increased difficulty.
- Updated and more navigable UI.

### Main Feedback We Chose To Decline
- More questions to ask.
- An increased amount of characters.
- Players typing in their own questions.
- An interactive map or other major visual components.

## Reflection
Because the game depends heavily on prompting and generated responses, this project became a learning experience in understanding what AI tools could and could not realistically support in a student project.

- We tried to implement many of the critiques from playtesters, with mixed levels of success.
- Some suggestions sounded easy at first but were difficult to implement smoothly without breaking other parts of the game.
- Even with Codex helping generate much of the code, integration still required a lot of iteration.
- At the same time, some ideas we expected to be too difficult ended up being implemented successfully with help from Codex and Ollama.

Overall, the feedback helped clarify which changes were both valuable and feasible, and it pushed the project further than our original technical comfort level.
