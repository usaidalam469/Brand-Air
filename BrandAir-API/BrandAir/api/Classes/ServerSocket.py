import socket


class ServerSocket:
    def __init__(self, host, port):
        self.server = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        self.server.setsockopt(socket.SOL_SOCKET, socket.SO_REUSEADDR, 1)
        self.host = host
        self.port = port
        self.CHUNK_SIZE = 4 * 1024
        self.conn = None

    def Start(self):
        self.server.bind((self.host, self.port))
        self.server.listen(5)
        print("Waiting")
        self.conn, addr = self.server.accept()
        print("Connected")

    def Receive(self):
        try:
            data = b''
            bytes_len = self.conn.recv(5)
            while True:
                send_bytes = self.conn.recv(int(bytes_len))
                data += send_bytes
                if int(bytes_len) == len(data):
                    print(data)
                    break
            return data
        except:
            self.Stop()

    def ReceiveFile(self, filename, t):
        try:
            data = b''
            if t == "video":
                file = open("video.mp4", "w+b")
            else:
                file = open("audio.wav", "w+b")
            while True:
                send_bytes = self.conn.recv(self.CHUNK_SIZE)
                file.write(send_bytes)
                if len(send_bytes) < self.CHUNK_SIZE:
                    break
            file.close()
        except:
            self.Stop()

    def Send(self, dataBytes):
        try:
            print(len(dataBytes))
            self.conn.send(dataBytes)
        except:
            self.Stop()

    def Stop(self):
        self.server.close()
        self.conn.close()
