#import time
import zmq
#from zmq.devices.basedevice import ProcessDevice
#from zmq.devices.monitoredqueuedevice import MonitoredQueue
#from zmq.utils.strtypes import asbytes
#from multiprocessing import Process


monitor_port = 9999
number_of_workers = 2

def monitor():
	context = zmq.Context()
	socket = context.socket(zmq.REQ)
	socket.connect ("tcp://127.0.0.1:%s" % monitor_port)
	socket.send_string("something")
	return int(socket.recv().decode("utf-8"))