from flask import Flask
from flask import render_template
from monitorZmq import *
from autoDiscovery import *
app = Flask(__name__)


@app.route('/monitor/')
def queueMonitor():
  model = {}
  ip_of_server = auto_discover_server();
  model["queueDepth"] = monitor(ip_of_server)
  return render_template('monitor.html', model=model)

if __name__ == '__main__':
  app.run(debug=True)
