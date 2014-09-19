(function() {
	var dataPoints = [];
	
	function getDataPoint(){
		$.get('/currentqueuedepth', function(data) {
			dataPoints.push(data);
			
			if(dataPoints.length > 300)
				dataPoints.shift(); // pop one of the old points off;
			
			updateSummary(data);
			updateGraph(data);
		});
	}
	
	function updateSummary(data) {
		var newSummary = "The Queue is currently empty.";
		if(data) {
			newSummary = "The current queue depth is: <b>" + data + "</b>";
		}
		
		$('#summary').html(newSummary);
	}
	
	var isGraphReady = false;
	function updateGraph() {
		var maxPoint = 5;
		
		for(var i = 0, ii = dataPoints.length; i < ii; i++) {
			var point = dataPoints[i];
			if(point > maxPoint)
				maxPoint = point;
		}
	
	
		var chartUrl = "https://chart.googleapis.com/chart?cht=lc&chs=600x400&chxt=x,y";
		chartUrl += "&chxr=1,0," + maxPoint + "|0,0," + dataPoints.length;
		chartUrl += "&chd=t:";
		chartUrl += dataPoints.join(',');
		
		$('#chart').attr('src', chartUrl);
	}

	function start() {
		getDataPoint();
		setInterval(getDataPoint, 2000);
	}

	$(start);
})()
