from flask import Flask
from flask import render_template
from monitorZmq import *
from autoDiscovery import *
app = Flask(__name__)

ip_of_server = auto_discover_server();

@app.route('/monitor/')
def queue_monitor():
  model = {}
  model["queueDepth"] = int(monitor_messages(ip_of_server))
  return render_template('monitor.html', model=model)
  
@app.route('/currentqueuedepth/')
def current_queue_depth():
  return monitor_messages(ip_of_server)
  
@app.route('/currentconsumercount/')
def current_consumer_count():
  return monitor_consumers(ip_of_server)

if __name__ == '__main__':
  app.run(debug=True)
