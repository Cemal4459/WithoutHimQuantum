# WithoutHim

## Concept
**WithoutHim** is a short, emotional 2D pixel-art platformer about a mother searching for her lost child in a dark, colorless world. As the player explores dangerous environments, overcomes traps, and finds the child's belongings, the world slowly regains its colors and hope. The game combines platforming dynamics, environmental puzzles, and emotional storytelling into a cinematic experience.

## Core Loop
Player explores the dark world → Uses jump and dash mechanics to avoid hazards → Solves environmental light puzzles → Collects the child's lost items → Restores color to the world → Reaches the next area to repeat the loop.

## Win & Lose Conditions
- **Win Condition:** Collect the child's important items, complete environmental puzzles (collapsing bridges, falling platforms), and safely exit the cave by following the light in the Boss section.
- **Lose Condition:** Lose all 3 lives (hearts). Falling into environmental hazards, hitting falling stalactites, or taking damage from staying in the dark respawns the player at the last checkpoint and costs 1 life. The game is lost when all lives are depleted.

## Scenes
- **IntroScene:** A short cinematic video scene showing the mother losing her child, preparing the player for the emotional and dark atmosphere of the story.
- **MainMenu:** The starting screen reflecting the game's core theme with "WITHOUT" (colorless) and "HIM" (colorful) typography. It contains "START", "CREDITS", and "QUIT" buttons.
- **ForestScene (Level 1):** The first level set in a black-and-white atmosphere. It features pushable log physics (Rigidbody), a bridge that collapses when crossed (Trigger), and collectible items. The player proceeds to the next level through a cave entrance.
- **CaveScene (Level 2):** A challenging cave level featuring falling stalactites, geysers that launch the player, falling platforms, and wall jump mechanics.
- **Boss Section (End of CaveScene):** A cinematic escape sequence where the screen goes dark, and the player must stay under the "hope of motherhood" light controlled by the mouse.
- **VictoryScene:** A nature and hope-filled pixel art ending screen where colors are fully restored upon finding the child. Returns to the Main Menu via ESC or clicking the screen.
- **GameOver / Pause Menu:** The menu that opens when the player loses all 3 lives or presses ESC at any time during the game. Offers "MAIN MENU" and "RESTART" options.
- **Credits:** A blackboard-themed scene displaying team members' names and roles.

## Controls
- **A, D** — Movement
- **Space** — Jump / Wall Jump
- **Left Shift** — Dash
- **Mouse** — Clicking UI buttons and guiding the light in the Boss section
- **ESC** — Pause the game / Exit the Victory screen

## Team & Responsibilities
- **Cemal Yıldız (Developer):** Player movement (Rigidbody2D/CharacterController), health/damage system (3 Hearts & Respawn), camera tracking (Camera Follow), dash/jump mechanics, and core gameplay scripting (C#).
- **Mustafa Küçükbaş (Level Designer):** Scene Setup, prefab placement, integrating environmental puzzles (log, collapsing bridge, geyser, stalactites) into the map using Trigger and Collider logic, and hazard design.
- **Kerem Acar (Artist & Tech Art):** 2D Pixel art/animations, coding the Global Color Mechanic via Color Manager Script (C#), and integrating dynamic color restoration effects into Unity.
- **Doruk Kaya (Artist & UI/UX):** 2D Pixel art support, Unity 2D cinematic lighting setup, UI design/button functionality, and preparing Scene Management scripts.
