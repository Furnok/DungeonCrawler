import tkinter as tk
from tkinter import filedialog, messagebox
import json

class CanvasGrid(tk.Canvas):
    def __init__(self, root, model):
        super().__init__(root, width=500, height=500, bg="lightgray")
        self.model = model
        self.scale = 1.0
        self.offset_x = 0
        self.offset_y = 0

    def draw_grid(self):
        self.delete("all")
        cell_size = 50 * self.scale
        for y in range(self.model.height):
            for x in range(self.model.width):
                x0, y0 = x * cell_size + self.offset_x, y * cell_size + self.offset_y
                x1, y1 = x0 + cell_size, y0 + cell_size

                if self.model.wall_layer[y * self.model.width + x] == 1:
                    self.create_rectangle(x0, y0, x1, y1, fill="gray")
                else:
                    self.create_rectangle(x0, y0, x1, y1, fill="white")

                if self.model.items_layer[y * self.model.width + x] == 1:
                    self.create_oval(x0+10*self.scale, y0+10*self.scale, x1-10*self.scale, y1-10*self.scale, fill="yellow")

                if (x, y) == self.model.exit_coordinates:
                    self.create_rectangle(x0+10*self.scale, y0+10*self.scale, x1-10*self.scale, y1-10*self.scale, fill="red")
                
                if (x, y) == self.model.spawn_coordinates:
                    self.create_rectangle(x0+10*self.scale, y0+10*self.scale, x1-10*self.scale, y1-10*self.scale, fill="green")

    def bind_events(self, on_click, on_drag, zoom, start_pan, pan):
        self.bind("<Button-1>", on_click)
        self.bind("<B1-Motion>", on_drag)
        self.bind("<MouseWheel>", zoom)
        self.bind("<ButtonPress-3>", start_pan)
        self.bind("<B3-Motion>", pan)

    def zoom(self, event):
        if event.delta > 0:
            self.scale *= 1.1
        elif event.delta < 0:
            self.scale *= 0.9

        mouse_x = self.canvasx(event.x)
        mouse_y = self.canvasy(event.y)

        self.offset_x = mouse_x - (mouse_x - self.offset_x) * 1.1 if event.delta > 0 else mouse_x - (mouse_x - self.offset_x) * 0.9
        self.offset_y = mouse_y - (mouse_y - self.offset_y) * 1.1 if event.delta > 0 else mouse_y - (mouse_y - self.offset_y) * 0.9

    def start_pan(self, event):
        self.last_pan_x = event.x
        self.last_pan_y = event.y

    def pan(self, event):
        delta_x = event.x - self.last_pan_x
        delta_y = event.y - self.last_pan_y

        self.offset_x += delta_x
        self.offset_y += delta_y

        self.last_pan_x = event.x
        self.last_pan_y = event.y

class ControlPanel(tk.Frame):
    def __init__(self, root, model, view):
        super().__init__(root)
        self.model = model
        self.view = view

        self.mode_frame = tk.Frame(self)
        self.mode_frame.pack()

        self.wall_button = tk.Button(self.mode_frame, text="Edit Walls", command=self.set_wall_mode)
        self.wall_button.pack(side=tk.LEFT)

        self.item_button = tk.Button(self.mode_frame, text="Edit Items", command=self.set_item_mode)
        self.item_button.pack(side=tk.LEFT)

        self.exit_button = tk.Button(self.mode_frame, text="Set Exit", command=self.set_exit_mode)
        self.exit_button.pack(side=tk.LEFT)

        self.spawn_button = tk.Button(self.mode_frame, text="Set Spawn", command=self.set_spawn_mode)
        self.spawn_button.pack(side=tk.LEFT)

        self.mode_label = tk.Label(self, text="Mode: Editing Walls")
        self.mode_label.pack()

        self.size_frame = tk.Frame(self)
        self.size_frame.pack()

        self.width_label = tk.Label(self.size_frame, text="Width:")
        self.width_label.pack(side=tk.LEFT)

        self.width_entry = tk.Entry(self.size_frame, width=5)
        self.width_entry.insert(0, str(self.model.width))
        self.width_entry.pack(side=tk.LEFT)

        self.height_label = tk.Label(self.size_frame, text="Height:")
        self.height_label.pack(side=tk.LEFT)

        self.height_entry = tk.Entry(self.size_frame, width=5)
        self.height_entry.insert(0, str(self.model.height))
        self.height_entry.pack(side=tk.LEFT)

        self.apply_size_button = tk.Button(self.size_frame, text="Apply", command=self.apply_size)
        self.apply_size_button.pack(side=tk.LEFT)

        self.file_frame = tk.Frame(self)
        self.file_frame.pack()

        self.load_button = tk.Button(self.file_frame, text="Load Data", command=self.load_data)
        self.load_button.pack(side=tk.LEFT)

        self.save_button = tk.Button(self.file_frame, text="Save Data", command=self.save_data)
        self.save_button.pack(side=tk.LEFT)

    def set_wall_mode(self):
        self.model.edit_mode = "walls"
        self.mode_label.config(text="Mode: Editing Walls")

    def set_item_mode(self):
        self.model.edit_mode = "items"
        self.mode_label.config(text="Mode: Editing Items")

    def set_exit_mode(self):
        self.model.edit_mode = "exit"
        self.mode_label.config(text="Mode: Setting Exit")

    def set_spawn_mode(self):
        self.model.edit_mode = "spawn"
        self.mode_label.config(text="Mode: Setting Spawn")

    def apply_size(self):
        try:
            new_width = int(self.width_entry.get())
            new_height = int(self.height_entry.get())
            if new_width > 0 and new_height > 0:
                self.model.resize(new_width, new_height)
                self.view.draw_grid()
            else:
                messagebox.showerror("Error", "Width and height must be positive integers.")
        except ValueError:
            messagebox.showerror("Error", "Invalid input. Please enter integers for width and height.")

    def load_data(self):
        file_path = filedialog.askopenfilename(filetypes=[("JSON files", "*.json")], initialdir="saves")
        if file_path:
            with open(file_path, 'r') as file:
                data = json.load(file)
                self.model.load_data(data)
                self.width_entry.delete(0, tk.END)
                self.width_entry.insert(0, str(self.model.width))
                self.height_entry.delete(0, tk.END)
                self.height_entry.insert(0, str(self.model.height))
                self.view.draw_grid()

    def save_data(self):
        file_path = filedialog.asksaveasfilename(defaultextension=".json", filetypes=[("JSON files", "*.json")], initialdir="saves")
        if file_path:
            data = self.model.get_data()
            with open(file_path, 'w') as file:
                json.dump(data, file)
