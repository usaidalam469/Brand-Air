from django.contrib import admin
from django.urls import path,include
from . import views

urlpatterns = [
    path('', views.index),
    path('start', views.startserver),
    path('stop', views.stopserver),
]