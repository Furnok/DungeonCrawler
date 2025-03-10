class GridModel:
    def __init__(self, width, height, wall_layer, items_layer):
        self.width = width
        self.height = height
        self.wall_layer = wall_layer
        self.items_layer = items_layer
        self.edit_mode = "walls"
        self.exit_coordinates = (9, 9)
        self.last_toggled_cell = None
        self.initial_state = None
        self.spawn_coordinates = (0, 0)

    def toggle_cell(self, x, y, scale, offset_x, offset_y, force_state=None):
        cell_size = 50 * scale
        x = int((x - offset_x) // cell_size)
        y = int((y - offset_y) // cell_size)
        if 0 <= x < self.width and 0 <= y < self.height:
            current_cell = (x, y)
            if current_cell != self.last_toggled_cell:
                index = y * self.width + x
                if self.edit_mode == "walls":
                    if force_state is None:
                        self.wall_layer[index] = 1 - self.wall_layer[index]
                    else:
                        self.wall_layer[index] = force_state
                elif self.edit_mode == "items":
                    if force_state is None:
                        self.items_layer[index] = 1 - self.items_layer[index]
                    else:
                        self.items_layer[index] = force_state
                elif self.edit_mode == "exit":
                    self.exit_coordinates = (x, y)
                elif self.edit_mode == "spawn":
                    self.spawn_coordinates = (x, y)
                self.last_toggled_cell = current_cell

    def start_drag(self, x, y, scale, offset_x, offset_y):
        cell_size = 50 * scale
        x = int((x - offset_x) // cell_size)
        y = int((y - offset_y) // cell_size)
        if 0 <= x < self.width and 0 <= y < self.height:
            index = y * self.width + x
            if self.edit_mode == "walls":
                self.initial_state = self.wall_layer[index]
            elif self.edit_mode == "items":
                self.initial_state = self.items_layer[index]
            self.reset_last_toggled_cell()

    def reset_last_toggled_cell(self):
        self.last_toggled_cell = None

    def resize(self, new_width, new_height):
        old_width, old_height = self.width, self.height
        new_wall_layer = [0] * (new_width * new_height)
        new_items_layer = [0] * (new_width * new_height)

        for y in range(min(old_height, new_height)):
            for x in range(min(old_width, new_width)):
                old_index = y * old_width + x
                new_index = y * new_width + x
                new_wall_layer[new_index] = self.wall_layer[old_index]
                new_items_layer[new_index] = self.items_layer[old_index]

        self.width = new_width
        self.height = new_height
        self.wall_layer = new_wall_layer
        self.items_layer = new_items_layer

    def load_data(self, data):
        self.width = data["width"]
        self.height = data["height"]
        self.wall_layer = data["wallLayer"]
        self.items_layer = data["itemsLayer"]
        self.exit_coordinates = tuple(data.get("exitCoordinates", (9, 9)))
        self.spawn_coordinates = tuple(data.get("spawnCoordinates", (0, 0)))

    def get_data(self):
        return {
            "width": self.width,
            "height": self.height,
            "wallLayer": self.wall_layer,
            "itemsLayer": self.items_layer,
            "exitCoordinates": self.exit_coordinates,
            "spawnCoordinates": self.spawn_coordinates
        }
