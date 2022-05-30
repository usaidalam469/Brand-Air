import numpy as np

class Tracker:
    def __init__(self):
        self.old_boxes = []
        self.object_count = 0
        self.new_boxes = []
        self.current_boxes = []
        self.obj_id = 0

    def update(self, boxes):
        if self.object_count == 0:
            for box in boxes:
                box.obj_id = self.obj_id
                self.object_count += 1
                self.obj_id += 1
            self.old_boxes = boxes
            self.new_boxes = boxes
            self.current_boxes = boxes
            return boxes
        self.new_boxes = boxes

        for i in range(len(self.old_boxes)):
            if len(self.new_boxes) > 0:
                min_box = None
                min_index = -1
                min_dist = 9999
                for j in range(len(self.new_boxes)):
                    point_a = np.array((self.new_boxes[j].centroid[0], self.new_boxes[j].centroid[1]))
                    point_b = np.array((self.old_boxes[i].centroid[0], self.old_boxes[i].centroid[1]))
                    distance = np.linalg.norm(point_a - point_b)
                    if distance < min_dist and distance < 100:
                        min_dist = distance
                        min_box = self.new_boxes[j]
                        min_index = j
                if min_box:
                    self.new_boxes.pop(min_index)
                    min_box.obj_id = self.old_boxes[i].obj_id
                    self.current_boxes.append(min_box)

        for new in self.new_boxes:
            new.obj_id = self.obj_id
            self.object_count += 1
            self.obj_id += 1
            self.current_boxes.append(new)
        self.old_boxes = self.current_boxes
        self.current_boxes = []
        return self.old_boxes

