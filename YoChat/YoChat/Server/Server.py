import socket
import threading
import random
import string
import json
import datetime


class Server:

    __sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)

    __all_rooms = {} # {"room_code":[{"client":client, "Nickname":nickname, "Owner":false/true}]}

    __alphabet = string.ascii_uppercase+string.digits

    __room_code_length = 5
    
    def __Serialize(self, data):
        data = json.dumps(data)
        return data.encode()

    def __DeSerialize(self, data):
        data_string = data.decode()
        data_json = json.loads(data_string)
        return data_json

    def __RoomCodeGenerator(self):
        while True:
            gen_code = "".join(random.choices(self.__alphabet, k=self.__room_code_length))
            if gen_code not in list(self.__all_rooms.keys()):
                 break

        return gen_code

    __port = int
    def __init__(self, port:int):
        self.__port = port
        self.__sock.bind(("0.0.0.0", port))
    
    def __connect(self):
        self.__sock.listen()
        print(f"Server is listening on >> {socket.gethostbyname(socket.gethostname())}:{self.__port}")
        while True:
            client, _ = self.__sock.accept()
            print(f"New Connection from {_[0]}:{_[1]} at - {datetime.datetime.now()}")
            thr = threading.Thread(target=self.__Authorize, args=(client,))
            thr.start()
    
    def __Authorize(self, client):
        is_success = False
        while not is_success:
            try:
                type_of_join = client.recv(1024)
                type_of_join =  self.__DeSerialize(type_of_join) # {"Type": "Create", "RoomCode":""} or {"Type": "Join", "RoomCode":"room_code"}
            except:break
            try:
                # send_arg --> {"Error":true/false, "ErrorDesc":"error_explained", "RoomCode":"room_code"}
                if type_of_join["Type"] == "Create":
                    room_code = self.__RoomCodeGenerator()
                    send_arg = {"Error":"false", "ErrorDesc":"", "RoomCode":room_code}

                else:
                    room_code = type_of_join["RoomCode"]
                    if room_code in self.__all_rooms:
                        send_arg = {"Error":"false", "ErrorDesc":"", "RoomCode":""}
                    else:
                        send_arg = {"Error":"true", "ErrorDesc":"Room Doesn't Exist", "RoomCode":""}

            except:send_arg = {"Error":"true", "ErrorDesc":"Unknown Error Occurred", "RoomCode":""}
            
            send_arg_ = self.__Serialize(send_arg)
            client.send(send_arg_)
            if send_arg["Error"] == "false":
                nickname = client.recv(1024).decode()
                user = {"client":client, "Nickname":nickname, "Owner":True if type_of_join["Type"] == "Create" else False}
                if type_of_join["Type"] == "Create":
                    self.__all_rooms[room_code] = [user]
                else:
                    self.__all_rooms[room_code].append(user)


            is_success = not (True if send_arg["Error"] == "true" else False)

        if is_success == True: self.__Receive(room_code, user)

    def __Receive(self, room_code, user):
        while True:
            try:
                req = user["client"].recv(1024) # {"Leave":bool, "Message":""}
                req = self.__DeSerialize(req)
                response = self.__ProcessRequest(req, room_code, user)

                if response:break
            except:
                break

    def __ProcessRequest(self, req, room_code, user):
        ct = datetime.datetime.now()
        if req["Leave"] == "false":
            msg = req["Message"]
            msg = {"Message":msg, "User":user["Nickname"], "RoomClosed":False, "timestamp":int(ct.timestamp())}
            msg = self.__Serialize(msg)
            for client in self.__all_rooms[room_code]:
                if client["Nickname"] != user["Nickname"]:
                    client["client"].send(msg)
            return False
        else:
            if user["Owner"] == True:
                if req["Close"] == "Yes":
                    msg = {"Message":"", "User":"", "RoomClosed":True, "timestamp":int(ct.timestamp())}
                    msg = self.__Serialize(msg)
                    for client in self.__all_rooms[room_code]:
                        if client["Nickname"] != user["Nickname"]:
                            client["client"].send(msg)

                    del self.__all_rooms[room_code]
            else:
                self.__all_rooms[room_code].Remove(user)
            return True

    def run(self):
        self.__connect()


if __name__ == "__main__":
    server = Server(4545)
    server.run()
    
    