import os
import glob
from grid_editor import GridEditor
import tkinter as tk
import json

def get_latest_save_file(directory="saves"):
    # Crée le répertoire s'il n'existe pas
    if not os.path.exists(directory):
        os.makedirs(directory)

    # Recherche le dernier fichier JSON dans le répertoire
    list_of_files = glob.glob(f"{directory}/*.json")
    if not list_of_files:
        return None

    latest_file = max(list_of_files, key=os.path.getctime)
    return latest_file

def main():
    latest_file = get_latest_save_file()
    if (latest_file):
        with open(latest_file, 'r') as file:
            data = json.load(file)
    else:
        # Données par défaut si aucun fichier n'est trouvé
        data = {
            "width": 10,
            "height": 10,
            "spawnCoordinates": [0, 0],
            "exitCoordinates": [9, 9],
            "wallLayer": [
                1,1,1,1,1,1,1,1,1,1,
                1,0,0,0,0,0,0,0,0,1,
                1,0,0,0,0,0,0,0,0,1,
                1,1,1,1,1,0,0,0,0,1,
                1,0,0,0,0,0,0,1,1,1,
                1,0,1,0,1,1,0,0,0,1,
                1,0,1,0,1,1,0,0,0,1,
                1,0,1,0,0,0,0,0,0,1,
                1,0,1,0,1,1,1,1,1,1,
                1,1,1,1,1,1,1,1,1,1
            ],
            "itemsLayer": [
                0,0,0,0,0,0,0,0,0,0,
                0,0,0,0,0,0,0,0,1,0,
                0,0,0,0,0,0,0,0,0,0,
                0,0,0,0,0,0,0,0,0,0,
                0,0,0,0,0,0,0,0,0,0,
                0,0,0,0,0,0,0,0,0,0,
                0,0,0,0,0,0,0,0,0,0,
                0,0,0,0,0,0,0,0,1,0,
                0,0,0,1,0,0,0,0,0,0,
                0,0,0,0,0,0,0,0,0,0
            ]
        }

    root = tk.Tk()
    editor = GridEditor(root, data["width"], data["height"], data["wallLayer"], data["itemsLayer"])
    
    def save_shortcut(event):
        editor.control_panel.save_data()

    root.bind('<Control-s>', save_shortcut)
    
    root.mainloop()

if __name__ == "__main__":
    main()
