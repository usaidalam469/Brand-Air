class DetectedObject:
    def __init__(self, box, class_id, obj_id):
        self.box = box
        self.class_id = class_id
        self.obj_id = obj_id
        cX = int((box[0] + (box[2] + box[0]) / 2.0))
        cY = int((box[1] + (box[3] + box[1]) / 2.0))
        self.centroid = (cX, cY)
