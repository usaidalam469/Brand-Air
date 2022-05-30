import cv2
import numpy as np
from skimage import measure


class AdvertismentDetection:
    def __init__(self):
        self.frame_count = 0
        self.advertise = []
        self.user_frames = []
        self.detected_frame = -1
        self.user_fps = None
        self.frame_id = 0
        self.chunk = 0

    def GetUserAd(self):
        cap = cv2.VideoCapture("H:\\pythonProject\\BrandAir-API\\BrandAir\\api\\Classes\\video4.mp4")
        self.user_fps = int(cap.get(cv2.CAP_PROP_FPS))
        while True:
            _, frame = cap.read()
            if _:
                self.user_frames.append(frame)
            else:
                break
        self.chunk = int(len(self.user_frames) / 10)
        print(len(self.user_frames))

    def CompareFrames(self, imageA, imageB):
        imageA, imageB = self.PreprocessImages(imageA, imageB)
        s = 0
        imageA = cv2.resize(imageA, (64, 36), fx=0.1, fy=0.1)
        imageB = cv2.resize(imageB, (64, 36), fx=0.1, fy=0.1)
        if imageA.shape == imageB.shape:
            s = self.CalculateSSIM(imageA, imageB)
        return s

    def PreprocessImages(self, imageA, imageB):
        img = imageA
        img1 = imageB
        gray_img = cv2.cvtColor(img, cv2.COLOR_BGR2GRAY)
        gray_img1 = cv2.cvtColor(img1, cv2.COLOR_BGR2GRAY)
        return gray_img, gray_img1

    def CalculateSSIM(self, imageA, imageB):
        return measure.compare_ssim(imageA, imageB)

    def CalculateMSE(self, imageA, imageB):
        err = np.sum((imageA.astype("float") - imageB.astype("float")) ** 2)
        err /= float(imageA.shape[0] * imageA.shape[1])
        return err

    def Detect(self, imageA):
        max_s = 0
        max_s_fid = -1
        if self.frame_id == 0:
            frame_id = self.frame_id
            for i in range(10):
                s = self.CompareFrames(imageA, self.user_frames[frame_id])
                if max_s < s:
                    max_s = s
                    max_s_fid = frame_id
                frame_id += self.chunk
            self.frame_id = max_s_fid
        else:
            max_s = self.CompareFrames(imageA, self.user_frames[self.frame_id])
        print(str(max_s))
        if max_s > 0.9:
            curr_frame = self.user_frames[self.frame_id]
            self.frame_count += 1
            self.detected_frame = self.frame_id
            self.frame_id += 1
            return True, curr_frame
        else:
            if self.frame_count > 0:
                self.detected_frame = -1
                self.advertise.append(self.frame_count)
                self.frame_count = 0
            self.frame_id = 0
            return False, None