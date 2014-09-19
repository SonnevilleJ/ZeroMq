#import time
import zmq
#from zmq.devices.basedevice import ProcessDevice
#from zmq.devices.monitoredqueuedevice import MonitoredQueue
#from zmq.utils.strtypes import asbytes
#from multiprocessing import Process


monitor_messages_port = 9999
monitor_consumers_port = 7777
number_of_workers = 2

def monitor_messages(ip_of_server):
	context = zmq.Context()
	socket = context.socket(zmq.REQ)
	socket.connect ("tcp://%s:%s" % (ip_of_server, monitor_messages_port))
	socket.send_string("something")
	return socket.recv().decode("utf-8")
	
	
def monitor_consumers(ip_of_server):
	context = zmq.Context()
	socket = context.socket(zmq.REQ)
	socket.connect ("tcp://%s:%s" % (ip_of_server, monitor_consumers_port))
	socket.send_string("something")
	return socket.recv().decode("utf-8")