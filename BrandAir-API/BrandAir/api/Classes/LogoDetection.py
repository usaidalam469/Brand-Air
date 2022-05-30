import cv2
import numpy as np
import time
import pafy
from . import Tracker,DetectedObject


class LogoDetection:
    def __init__(self):
        self.net=cv2.dnn.readNet("H:\\pythonProject\\BrandAir-API\\BrandAir\\api\\Classes\\weights\\yolov3_custom_20000.weights","H:\\pythonProject\\BrandAir-API\\BrandAir\\api\\Classes\\weights\\yolov3_custom_10classes.cfg")
        self.classes=[]
        with open("H:\\pythonProject\\BrandAir-API\\BrandAir\\api\\Classes\\weights\\classes.names","r") as f:
            self.classes=[line.strip() for line in f.readlines()]
        self.layer_names=self.net.getLayerNames()
        self.outputLayers=[self.layer_names[i[0]-1] for i in self.net.getUnconnectedOutLayers()]
        self.cap=None
        self.r_fps=None
        self.frame_id=0
        self.frame=None
        self.height=0
        self.width=0
        self.channel=0
        self.threshold=0.4
        self.font=cv2.FONT_HERSHEY_PLAIN
        self.encode_param = [int(cv2.IMWRITE_JPEG_QUALITY), 90]
        self.starting_time=0
        self.logo_frame_count=0
        self.Track= Tracker.Tracker()
        self.class_id=-1
        #Capture video
    def GetVideo(self,filename,link=False):
        if link:
            url=filename
            video=pafy.new(url)
            best=None
            streams = video.allstreams
            for i in range(len(streams)):
                if streams[i].quality == "640x360" and streams[i].extension=="mp4" and streams[i].mediatype=="normal":
                    best=streams[i]
                    break
#             best=video.getbest(preftype="mp4")
            self.cap=cv2.VideoCapture()
            self.cap.open(best.url)
        else:
            self.cap=cv2.VideoCapture(filename)
        self.r_fps = int(self.cap.get(cv2.CAP_PROP_FPS))
        self.starting_time=time.time()
    #Extract frame from video
    def GetFrame(self):
        _, self.frame=self.cap.read();
        try:
            self.frame_id+=1
            self.height,self.width,self.channel=self.frame.shape
            print(self.frame.shape)
        except:
            return False,None
        return _,self.frame
    #Extracting Blobs
    def GetBlobs(self):
        blob=cv2.dnn.blobFromImage(self.frame,0.00392,(320,320),(0,0,0),True,crop=False)
        return blob
    #Detection
    def Detect(self):
        font=cv2.FONT_HERSHEY_PLAIN
        logo_count=0
        flag=False
        response=""
        logo_frame_count=0
        self.net.setInput(self.GetBlobs())
        outs=self.net.forward(self.outputLayers)
        boxes=[]
        confidences=[]
        class_ids=[]
        #output layers=3
        for out in outs:
            for detection in out: #detecting bounding boxes
                scores=detection[5:]
                class_id=np.argmax(scores)
                confidence=scores[class_id]
                if confidence >self.threshold and class_id==self.class_id:
                    center_x=int(detection[0]*self.width)
                    center_y=int(detection[1]*self.height)
                    w=int(detection[2]*self.width)
                    h=int(detection[3]*self.height)
                    #rectangle
                    x=int(center_x-w/2)
                    y=int(center_y-h/2)
                    boxes.append([x,y,w,h])
                    confidences.append(float(confidence))
                    class_ids.append(class_id)
        indexes=cv2.dnn.NMSBoxes(boxes,confidences,self.threshold,0.4)
        detected_obj=[]
        for i in range(len(boxes)):
            if i in indexes:
                logo_count+=1
                x,y,w,h=boxes[i]
                det= DetectedObject.DetectedObject(boxes[i], class_ids[i], -1)
                detected_obj.append(det)
                label=str(self.classes[class_ids[i]])
                response+="/"+str(x)+" "+str(y)+" "+str(w)+" "+str(h)+"/"+label
                if flag==False:
                    self.logo_frame_count+=1
                    flag=True
        if len(detected_obj)>0:
            detected_obj=self.Track.update(detected_obj)
        for det_obj in detected_obj:
            x,y,w,h=det_obj.box
            class_id=det_obj.class_id
            obj_id=det_obj.obj_id
            cv2.rectangle(self.frame,(x,y),(x+w,y+h),(0,255,0),1)
            cv2.putText(self.frame,str(obj_id),(x,y+30),font,1,1,2)
        elapsed_time=time.time()-self.starting_time
        fps=self.frame_id/elapsed_time
        cv2.putText(self.frame,"Time:" + str(self.logo_frame_count/self.r_fps),(10,265),font,1,(0,0,0),1)
        response+="-"+str(self.logo_frame_count/self.r_fps)
        cv2.putText(self.frame,"FPS:" + str(round(fps,2)),(10,280),font,1,(0,0,0),1)
        response+="/"+str(round(fps,2))
        cv2.putText(self.frame,"Count: " +str(self.Track.object_count),(10,295),font,1,(0,0,0),1)
        response+="/"+str(self.Track.object_count)
        cv2.putText(self.frame,"CPF: " +str(logo_count),(10,310),font,1,(0,0,0),1)
        response+="/"+str(logo_count)
        if response=="":
            response="None"
        self.frame=cv2.resize(self.frame,(850,450), fx=1, fy=1)
        cv2.imwrite("frames/"+str(self.frame_id)+".jpg",self.frame)
        return response,self.frame