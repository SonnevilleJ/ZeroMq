from flask import Flask
from flask import render_template
from monitorZmq import *
app = Flask(__name__)


@app.route('/monitor/')
def queueMonitor():
  model = {}
  model["queueDepth"] = monitor()
  return render_template('monitor.html', model=model)

if __name__ == '__main__':
  app.run(debug=True)
