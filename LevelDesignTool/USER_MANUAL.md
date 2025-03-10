# Dungeon Crawler Level Design Tool User Manual

## Introduction
Welcome to the Dungeon Crawler Level Design Tool! This tool allows you to create and edit levels for your dungeon crawler game. You can design the layout, place items, and set spawn and exit points.

## Installation
1. Ensure you have Python installed on your system.
2. Install the required dependencies using the following command:
   ```
   pip install tkinter
   ```

## Getting Started
1. Run the `main.py` script to start the application:
   ```
   python main.py
   ```
2. The application window will open, displaying the grid editor and control panel.

## User Interface

### Canvas Grid
- The main area where you can draw and edit the level.
- Use the mouse to interact with the grid:
  - **Left Click**: Toggle walls, items, spawn, or exit points based on the current mode.
  - **Drag**: Draw continuously by holding the left mouse button.
  - **Mouse Wheel**: Zoom in and out.
  - **Right Click and Drag**: Pan the view.

### Control Panel
- Located on the right side of the window.
- Contains buttons and input fields to control the editing modes and grid size.

#### Modes
- **Edit Walls**: Toggle wall cells.
- **Edit Items**: Toggle item cells.
- **Set Exit**: Set the exit point.
- **Set Spawn**: Set the spawn point.

#### Grid Size
- Adjust the width and height of the grid.
- Click "Apply" to resize the grid.

#### File Operations
- **Load Data**: Load a level from a JSON file.
- **Save Data**: Save the current level to a JSON file.
- **Shortcut**: Use `CTRL+S` to quickly save the current level.

## Saving and Loading Levels
- Levels are saved in JSON format.
- Use the "Load Data" button to load an existing level.
- Use the "Save Data" button or `CTRL+S` to save the current level.

## Example JSON Structure
```json
{
    "width": 10,
    "height": 10,
    "wallLayer": [1, 1, 1, 1, 1, 0, 1, 1, 1, 1, ...],
    "itemsLayer": [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, ...],
    "exitCoordinates": [9, 9],
    "spawnCoordinates": [0, 0]
}
```

## Troubleshooting
- Ensure all dependencies are installed.
- Check the console for error messages.
- Verify the JSON file structure when loading levels.

## Conclusion
Thank you for using the Dungeon Crawler Level Design Tool! We hope it helps you create amazing levels for your game. If you have any questions or feedback, please contact us.
