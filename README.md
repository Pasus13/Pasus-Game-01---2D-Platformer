# 1st Pasus Game – 2D Platformer

**1st Pasus Game** is a small but challenging 2D platformer built in Unity as a learning and portfolio project.  
Run, jump and double jump across floating platforms, collect all the coins in each level, and reach the exit door before the timer runs out.

If you fall into the void, you’ll respawn at the start – but the clock keeps ticking…

---

## 🎮 Gameplay Overview

- Time-based levels with a visible timer bar.
- Collect all coins to open the exit door.
- Kill zones that respawn the player at the initial spawn point.
- Coyote time and double jump for more responsive platforming.
- Multiple levels with increasing difficulty.
- Main menu, pause menu, game over and victory screens.
- Background music and SFX for jumps, coins, doors and game over.

This project was created to practice 2D character movement, jump physics (buffering, coyote time, variable jump height), level flow and basic game architecture (GameManager, UIManager, InputManager, AudioManager, Scene management, etc.).

---

## 🕹️ Controls

**Keyboard:**

- Move left / right: `A` / `D` or Arrow keys  
- Jump / Double Jump: `Space`  
- Pause / Resume: `Esc`  

---

## 🧩 Tech & Structure

- **Engine:** Unity (2D, WebGL & Windows builds)
- **Language:** C#
- **Main systems:**
  - `PlayerMovement` – horizontal movement, jump logic, coyote time, buffer, double jump.
  - `GameManager` – coins, win/lose state, level progression.
  - `UIManager` – timer bar, level and coin display, panels (Level Complete, Game Over, Victory).
  - `ScManager` – scene loading & reloading.
  - `AudioManager` – background music and SFX playback.
  - `InputManager` – centralised input handling.
- **UI:**
  - Timer bar (fill-based).
  - Coin counter: `current / total`.
  - Level indicator: `LEVEL X`.
  - Panels: Main Menu, Pause, Level Complete, Game Over, Victory.

---

## ▶️ How to Play

You can play the game directly in your browser:
👉 **[Play on itch.io] https://pasus.itch.io/pasus-game-01

Alternatively, you can download the repo and open it in Unity:
1. Clone the repository:
   ```bash
   git clone https://github.com/Pasus13/Pasus-Game-01---2D-Platformer.git
2. Open the project in Unity (same version used to develop the game).
3. Open the Main Menu scene.
4. Press Play.


🚧 Roadmap / Ideas for Future Improvements

- More levels with new platform layouts and hazards.
- Checkpoints within larger levels.
- Additional enemy types or moving obstacles.
- Simple score system or time ranking.
- Gamepad support.
- Settings menu (audio volume, key rebinding).

👤 About

This project was created by Francisco Muñoz as part of a personal journey into game development with Unity and C#.
Any feedback, suggestions or bug reports are very welcome.
