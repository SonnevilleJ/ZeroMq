(function() {
	var queueDepths = [],
		consumerCounts = [],
		MAX_POINTS = 300;
	
	function getDataPoint(){
		$.get('/currentqueuedepth', function(data) {
			queueDepths.push(data);
			
			if(queueDepths.length > MAX_POINTS)
				queueDepths.shift(); // pop one of the old points off;
			
			updateQueueSummary(data);
			updateGraph();
		});
		
		$.get('/currentconsumercount', function(data) {
			queueDepths.push(data);
			
			if(queueDepths.length > MAX_POINTS)
				queueDepths.shift(); // pop one of the old points off;
			
			updateConsumerSummary(data);
			updateGraph(data);
		});
	}
	
	function updateQueueSummary(data) {
		var newSummary = "The Queue is currently empty.";
		if(data) {
			newSummary = "The current queue depth is: <b>" + data + "</b>";
		}
		
		$('#queueSummary').html(newSummary);
	}
	
	function updateConsumerSummary(data) {
		var newSummary = "There are currently no Consumers.";
		if(data) {
			newSummary = "The current consumer count is: <b>" + data + "</b>";
		}
		
		$('#consumerSummary').html(newSummary);
	}
	
	var isGraphReady = false;
	function updateGraph() {
		var maxPoint = 5;
		
		for(var i = 0, ii = queueDepths.length; i < ii; i++) {
			var point = queueDepths[i];
			if(point > maxPoint)
				maxPoint = point;
		}
	
	
		var chartUrl = "https://chart.googleapis.com/chart?cht=lc&chs=600x400&chxt=x,y";
		chartUrl += "&chxr=1,0," + maxPoint + "|0,0," + MAX_POINTS;
		chartUrl += "&chco=FF0000, 0000FF";
		chartUrl += "&chd=t:";
		chartUrl += queueDepths.join(',');
		chartUrl += "|";
		chartUrl += consumerCounts.join(',');
		
		$('#chart').attr('src', chartUrl);
	}

	function start() {
		for(var i = 0; i < MAX_POINTS; i++) {
			queueDepths[i] = 0;
			consumerCounts[i] = 0;
		}
		getDataPoint();
		setInterval(getDataPoint, 1000);
	}

	$(start);
})()
