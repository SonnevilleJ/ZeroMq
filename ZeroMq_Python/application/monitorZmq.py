#import time
import zmq
#from zmq.devices.basedevice import ProcessDevice
#from zmq.devices.monitoredqueuedevice import MonitoredQueue
#from zmq.utils.strtypes import asbytes
#from multiprocessing import Process


monitor_port = 9999
number_of_workers = 2

def monitor(ip_of_server):
	context = zmq.Context()
	socket = context.socket(zmq.REQ)
	socket.connect ("tcp://%s:%s" % (ip_of_server, monitor_port))
	socket.send_string("something")
	return int(socket.recv().decode("utf-8"))