Here the dungeon crawler
To play it go to Scens -> Scenes_Game and run it, move with the arrows or ZQSD you can deactivate fog in the script on the DungeonManager prefab  and you active the gizmo to see what happening behin fog

To create  a levels you have the tools in the forlders of the project: DungeonCrawler -> LevelDesignTool (Made with python you have in it a read me how to use it)

The game have 2 important prefabs to work Player and DungeonManager

DungeonManager:
Have all the mechanics of the dungeon in the script attach to it:

LvelManager: put it all the level.json in the MpaDefinitionJSON to play one after the others one until finish the game don t forget to set the movements points in the script: MovementPoints

FogManager generate the fog of war in the dungeon you can activeate the generation or not

GhostPowerUpManager: generate the power up in the dungeon, you can set the duration of the power up and the player when collect it change apparence and can go throw the walls and walls will have a transparent material

On the Player prefab you can set the move duration to take time beetween movements to see the fog removing with what here beinds hit before reach the tile completly