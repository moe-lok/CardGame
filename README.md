# Card Game - Console Based Card Dealing Simulator

A C# console application that simulates a 4-player card game with animated card dealing, interactive gameplay, and visual effects.

![AlienClock Screenshot](https://github.com/moe-lok/CardGame/blob/master/CardGameScreenshot.PNG?raw=true)

## Features

### Core Game Mechanics
- Standard 52-card deck implementation
- 4-player card distribution
- Card ranking and comparison system
- Winner determination based on matching cards

### Visual Elements
- ASCII art card representations
- Color-coded card symbols:
  - \@ (Cyan)
  - \# (Yellow)
  - \^ (Green)
  - \* (Red)
- Interactive console-based UI

### Animations
1. **Card Dealing Animation**
   - Center deck display with visible top card
   - Smooth card movement animations
   - Dynamic player positions
   - Player labels for clear identification

2. **Winner Celebration**
   - ASCII art fireworks
   - Animated winner banner
   - Trophy display
   - Victory sound effects (Windows only)

## Prerequisites

- Windows/Linux/MacOS
- .NET 8.0 or later
- Console that supports Unicode characters and colors

## Installation

1. Clone the repository:
```bash
git clone https://github.com/moe-lok/CardGame.git
```

2. Navigate to the project directory:
```bash
cd CardGame
```

3. Build the project:
```bash
dotnet build
```

## Running the Application

1. From Visual Studio:
   - Open the solution file `CardGame.sln`
   - Press F5 or click the "Start" button

2. From Command Line:
```bash
cd CardGame
dotnet run
```

## Project Structure

```
CardGame/
│
├── Models/
│   ├── Card.cs          # Card class definition
│   └── Player.cs        # Player class definition
│
├── UI/
│   ├── UserInterface.cs    # Basic UI components
│   ├── DealingAnimation.cs # Card dealing animations
│   └── CelebrationEffects.cs # Winner celebrations
│
├── Game.cs              # Main game logic
├── Program.cs           # Entry point
└── README.md           # This file
```

## How to Play

1. Run the application
2. Watch as cards are dealt to each player with animations
3. View each player's cards
4. Winner is determined based on:
   - Highest number of matching cards
   - If tied, highest card value
   - If still tied, highest symbol value

## Technical Details

- Built with C# and .NET 8.0
- Uses Console.Beep() for sound effects (Windows only)
- Implements custom card comparison logic
- Features object-oriented design principles

## Notes for Running

- Maximize console window for best experience
- Enable console Unicode support if needed
- Some visual elements may require adjustment based on console size

## Future Improvements

- Statistics tracking
- Multiple game modes
- Save/load game state
- High score system

## License

This project was created as part of a technical assessment and is for demonstration purposes.
