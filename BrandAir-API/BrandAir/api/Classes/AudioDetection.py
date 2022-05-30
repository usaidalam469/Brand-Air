import speech_recognition as sr

class AudioDetection:
    def __init__(self, filename):
        self.r = sr.Recognizer()
        self.offset = 0
        self.duration = 8
        self.file = filename
        self.total = 0

    def Extraction(self):
        r = sr.Recognizer()
        dict = {}
        with sr.AudioFile(self.file) as source:
            r.adjust_for_ambient_noise(source)
            audio = r.record(source, offset=self.offset, duration=self.duration)
            self.total += self.duration
            print(self.total)
            try:
                response = r.recognize_google(audio, show_all=True)
                self.offset += 8
                if type(response) == type(dict):
                    return response['alternative'][0]['transcript']
            except Exception as e:
                return "None"



