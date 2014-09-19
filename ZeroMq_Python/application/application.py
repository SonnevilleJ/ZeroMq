from flask import Flask
from flask import render_template
app = Flask(__name__)


@app.route('/monitor/')
@app.route('/monitor/<queueDepth>')
def hello(queueDepth=None):
  model = {}
  model["queueDepth"] = 10
  return render_template('monitor.html', model=model)

if __name__ == '__main__':
  app.run(debug=True)
