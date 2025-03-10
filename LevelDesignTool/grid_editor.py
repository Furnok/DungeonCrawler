import tkinter as tk
from tkinter import filedialog, messagebox
from grid_model import GridModel
from ui_components import CanvasGrid, ControlPanel

class GridEditor:
    def __init__(self, root, width, height, wall_layer, items_layer):
        self.model = GridModel(width, height, wall_layer, items_layer)
        self.view = CanvasGrid(root, self.model)
        self.control_panel = ControlPanel(root, self.model, self.view)

        self.view.pack(expand=True, fill="both")
        self.control_panel.pack()

        self.view.bind_events(self.on_click, self.on_drag, self.zoom, self.start_pan, self.pan)

        self.view.draw_grid()

    def on_click(self, event):
        self.model.start_drag(event.x, event.y, self.view.scale, self.view.offset_x, self.view.offset_y)
        self.model.toggle_cell(event.x, event.y, self.view.scale, self.view.offset_x, self.view.offset_y)
        self.view.draw_grid()

    def on_drag(self, event):
        force_state = 1 if self.model.initial_state == 0 else 0
        self.model.toggle_cell(event.x, event.y, self.view.scale, self.view.offset_x, self.view.offset_y, force_state)
        self.view.draw_grid()

    def zoom(self, event):
        self.view.zoom(event)
        self.view.draw_grid()

    def start_pan(self, event):
        self.view.start_pan(event)

    def pan(self, event):
        self.view.pan(event)
        self.view.draw_grid()
