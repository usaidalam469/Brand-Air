from django.shortcuts import render, HttpResponse
from .Classes import VideoProcessing
# Create your views here.

obj = VideoProcessing.VideoProcessing()


def index(request):
    return HttpResponse("Hello")


def startserver(request):
    obj.ConnectToServer()
    obj.ProcessVideo()
    return HttpResponse("Server Started")


def stopserver(request):
    obj.CloseServer()
    return HttpResponse("Server Stopped")