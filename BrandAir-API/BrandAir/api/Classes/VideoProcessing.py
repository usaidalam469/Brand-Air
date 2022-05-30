import cv2
from . import ServerSocket
from . import AudioDetection
from . import LogoDetection
from . import AdvertismentDetection

class VideoProcessing:
    def __init__(self):
        self.server = None
        self.offset = 0
        self.duration = 5
        self.total = 0

    def ConnectToServer(self):
        self.server = ServerSocket.ServerSocket('192.168.100.47', 12347)
        self.server.Start()
    def CloseServer(self):
        self.server.Stop()

    def UploadFile(self, filename):
        t = self.server.Receive().decode('utf-8')
        self.server.ReceiveFile(filename, t)
        self.server.Send(bytes(filename, 'utf-8'))
        self.server.Stop()

    def ProcessAudio(self):
        if self.server != None:
            filename = self.server.Receive().decode('utf-8')
            print(filename)
            a = AudioDetection.AudioDetection(filename)
            while True:
                res = a.Extraction()
                if res == None:
                    res = "None"
                data = bytes(f'{len(res):<10}' + res, 'utf-8')
                self.server.Send(data)
                status = self.server.Receive().decode('utf-8')
                if status == "stop":
                    a.offset = 0
                    break
            self.ProcessAudio()

    def ProcessVideo(self, debug=True):
        if self.server != None:
            print("Processing")
            logoDetector = LogoDetection.LogoDetection()
            advertisementDetector = AdvertismentDetection.AdvertismentDetection()
            advertisementDetector.GetUserAd()
            p = self.server.Receive().decode('utf-8')
            if p.split("-")[0] == "link":
                print(p.split("-")[1])
                logoDetector.GetVideo(p.split("-")[1], True)
            else:
                logoDetector.GetVideo(p.split("-")[1])
            logoDetector.class_id = int(p.split("-")[2])
            while True:
                _, frame = logoDetector.GetFrame()
                if _:
                    ad, ad_frame = advertisementDetector.Detect(frame)
                    res, imagebytes = cv2.imencode('.jpg', frame)
                    data = bytes(f'{len(imagebytes):<10}', 'utf-8') + imagebytes.tobytes()
                    self.server.Send(data)
                    blob = logoDetector.GetBlobs()
                    for b in blob:
                        for n, img_blob in enumerate(b):
                            blb = img_blob
                    res, imagebytes = cv2.imencode('.jpg', blb)
                    data = bytes(f'{len(imagebytes):<10}', 'utf-8') + imagebytes.tobytes()
                    self.server.Send(data)
                    response, detframe = logoDetector.Detect()
                    data = bytes(f'{len(response):<10}' + response, 'utf-8')
                    self.server.Send(data)
                    if ad:
                        cv2.putText(detframe, "Ad frame", (500, 10), cv2.FONT_HERSHEY_PLAIN, 1, 1, 1)
                        cv2.circle(detframe, (600, 10), 10, (0, 0, 255), -1)
                        ad_res = "Detected"
                        data = bytes(f'{len(ad_res):<10}', 'utf-8') + bytes(ad_res, 'utf-8')
                        self.server.Send(data)
                    else:
                        ad_res = "Not Detected"
                        data = bytes(f'{len(ad_res):<10}', 'utf-8') + bytes(ad_res, 'utf-8')
                        self.server.Send(data)
                    res, imagebytes = cv2.imencode('.jpg', detframe)
                    data = bytes(f'{len(imagebytes):<10}', 'utf-8') + imagebytes.tobytes()
                    self.server.Send(data)
                else:
                    break
                if self.server.Receive().decode('utf-8') == "stop":
                    break
            self.ProcessVideo()
