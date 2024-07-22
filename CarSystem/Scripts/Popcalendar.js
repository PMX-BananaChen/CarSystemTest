//by gaocaihui add 2005-06-28
	var	fixedX = -1			// x position (-1 if to appear below control)
	var	fixedY = -1			// y position (-1 if to appear below control)
	var startAt = 0			// 0 - sunday ; 1 - monday
	var showWeekNumber = 1	// 0 - don't show; 1 - show
	var showToday = 1		// 0 - don't show; 1 - show
	
	var imgDir = "../images/"	//By Tony.Gao modify 2005-11-28

	var us_clearString = "Clear"
	var us_todayString = "Today is"
	var us_weekString = "Wk"
	var us_timeString = "Time"
	var us_dateString = "Date"
	var us_monthName = new Array("January","February","March","April","May","June","July","August","September","October","November","December")
	var	us_monthName2 = new Array("Jan","Feb","Mar","Apr","May","Jun","Jul","Aug","Sep","Oct","Nov","Dec")
	var us_dayName_sun = new Array("Sun","Mon","Tue","Wed","Thu","Fri","Sat")
	var us_dayName_mon = new Array("Mon","Tue","Wed","Thu","Fri","Sat","Sun")

   var cn_clearString = "\u6e05\u7a7a"
   var cn_todayString = "\u4eca\u5929\u662f"
   var cn_weekString = "\u5468"
   var cn_timeString = "\u65f6\u95f4"
   var cn_dateString = "\u65e5\u671f"
   var cn_monthName = new Array("\u4e00\u6708","\u4e8c\u6708","\u4e09\u6708","\u56db\u6708","\u4e94\u6708","\u516d\u6708","\u4e03\u6708","\u516b\u6708","\u4e5d\u6708","\u5341\u6708","\u5341\u4e00\u6708","\u5341\u4e8c\u6708")
   var cn_dayName_sun = new Array("\u65e5","\u4e00","\u4e8c","\u4e09","\u56db","\u4e94","\u516d")
   var cn_dayName_mon = new Array("\u4e00","\u4e8c","\u4e09","\u56db","\u4e94","\u516d","\u65e5")

   var hk_clearString = "\u6e05\u7a7a"
   var hk_todayString = "\u4eca\u5929\u662f"
   var hk_weekString = "\u5468"
   var hk_timeString = "\u6642\u9593"
   var hk_dateString = "\u65e5\u671f"
   var hk_monthName = new Array("\u4e00\u6708","\u4e8c\u6708","\u4e09\u6708","\u56db\u6708","\u4e94\u6708","\u516d\u6708","\u4e03\u6708","\u516b\u6708","\u4e5d\u6708","\u5341\u6708","\u5341\u4e00\u6708","\u5341\u4e8c\u6708")
   var hk_dayName_sun = new Array("\u65e5","\u4e00","\u4e8c","\u4e09","\u56db","\u4e94","\u516d")
   var hk_dayName_mon = new Array("\u4e00","\u4e8c","\u4e09","\u56db","\u4e94","\u516d","\u65e5")

	// global locale
	var locale = "cn"

	// global dateTime
	var dateTime ="dateTime"

	var	crossobj,
		crossSecondObj,
		crossMinuteObj,
		crossHourObj,
		crossMonthObj,
		crossYearObj,
		secondSelected,
		minuteSelected,
		hourSelected,
		monthSelected,
		yearSelected,
		dateSelected,
		omonthSelected,
		oyearSelected,
		odateSelected,
		secondConstructed,
		minuteConstructed,
		hourConstructed,
		monthConstructed,
		yearConstructed,
		intervalID1,
		intervalID2,
		timeoutID1,
		timeoutID2,
		ctlToPlaceValue,
		ctlNow,
		dateFormat,
		nStartingYear,
		nStartingHour,
		nStartingMinute,
		nStartingSecond


	var	bPageLoaded=false
	var	ie=document.all
	var	dom=document.getElementById


	var	ns4=document.layers
	var	today =	new	Date()
	var	dateNow	 = today.getDate()
	var	monthNow = today.getMonth()
	var	yearNow	 = today.getYear()
	var	hourNow	 = today.getHours()
	var	minuteNow = today.getMinutes()
	var	secondNow = today.getSeconds()

	var	imgsrc = new Array("caldrop1.gif","caldrop2.gif","calleft1.gif","calleft2.gif","calright1.gif","calright2.gif")
	var	img	= new Array()

	var bShow = false;

    var inputBoxName

    /* hides <select> and <applet> objects (for IE only) */
    function hideElement( elmID, overDiv )
    {
      if( ie )
      {
        for( i = 0; i < document.all.tags( elmID ).length; i++ )
        {
          obj = document.all.tags( elmID )[i];
          if( !obj || !obj.offsetParent )
          {
            continue;
          }

          // Find the element's offsetTop and offsetLeft relative to the BODY tag.
          objLeft   = obj.offsetLeft;
          objTop    = obj.offsetTop;
          objParent = obj.offsetParent;

          while( objParent.tagName.toUpperCase() != "BODY" )
          {
            objLeft  += objParent.offsetLeft;
            objTop   += objParent.offsetTop;
            objParent = objParent.offsetParent;
          }

          objHeight = obj.offsetHeight;
          objWidth = obj.offsetWidth;

          if(( overDiv.offsetLeft + overDiv.offsetWidth ) <= objLeft );
          else if(( overDiv.offsetTop + overDiv.offsetHeight ) <= objTop );
          else if( overDiv.offsetTop >= ( objTop + objHeight ));
          else if( overDiv.offsetLeft >= ( objLeft + objWidth ));
          else
          {
            obj.style.visibility = "hidden";
          }
        }
      }
    }

    /*
    * unhides <select> and <applet> objects (for IE only)
    */
    function showElement( elmID )
    {
      if( ie )
      {
        for( i = 0; i < document.all.tags( elmID ).length; i++ )
        {
          obj = document.all.tags( elmID )[i];

          if( !obj || !obj.offsetParent )
          {
            continue;
          }

          obj.style.visibility = "";
        }
      }
    }

	function HolidayRec (d, m, y, desc)
	{
		this.d = d
		this.m = m
		this.y = y
		this.desc = desc
	}

	var HolidaysCounter = 0
	var Holidays = new Array()

	function addHoliday (d, m, y, desc)
	{
		Holidays[HolidaysCounter++] = new HolidayRec ( d, m, y, desc )
	}

	if (dom)
	{

		for	(i=0;i<imgsrc.length;i++)
		{
			img[i] = new Image
			img[i].src = imgDir + imgsrc[i]
		}

		document.write ("<div onclick='bShow=true' id='calendar' style='z-index:+999;position:absolute;visibility:hidden;'><table width="
						+((showWeekNumber==1)?250:220)
						+" style='font-family:arial;font-size:11px;border-width:1;border-style:solid;border-color:#a0a0a0;font-family:arial; font-size:11px}' bgcolor='#ffffff'><tr bgcolor='#0000aa'><td>"
						+"<table width='"
						+((showWeekNumber==1)?248:218)
						+"'>"
						+"<tr>"
						+"	<td align='center' style='padding:2px;font-family:arial; font-size:11px;'>"
						+"		<font color='#ffffff'><B><span id='caption'></span></B></font>"
						+"	</td>"
						+"	<td align=right>"
						+"		<a href='javascript:hideCalendar()'><IMG SRC='"
						+imgDir
						+"			calclose.gif' WIDTH='15' HEIGHT='13' BORDER='0'></a>"
						+"	</td>"
						+"</tr>"
						+"<tr>"
						+"	<td align='center' style='padding:2px;font-family:arial; font-size:11px;'>"
						+"		<font color='#ffffff'><B><span id='time'></span></B></font>"
						+"	</td>"
						+"</tr>"
						+"</table></td></tr>"
						+"<tr><td style='padding:5px' bgcolor=#ffffff><span id='content'></span></td></tr>")

		if (showToday==1)
		{
			document.write ("<tr bgcolor=#f0f0f0><td style='padding:5px' align=center><span id='lblToday'></span></td></tr>")
		}

		document.write ("</table></div>"+
						"<div id='selectSecond' style='z-index:+999;position:absolute;visibility:hidden;'></div>"+
						"<div id='selectMinute' style='z-index:+999;position:absolute;visibility:hidden;'></div>"+
						"<div id='selectHour' style='z-index:+999;position:absolute;visibility:hidden;'></div>"+
						"<div id='selectMonth' style='z-index:+999;position:absolute;visibility:hidden;'></div>"+
						"<div id='selectYear' style='z-index:+999;position:absolute;visibility:hidden;'></div>")
	}


	var	monthName
	var dayName

	var	styleLink1="text-decoration:none;color:#0000aa;"
	var	styleLink2="text-decoration:none;color:#ffffff;"

	var	styleAnchor="text-decoration:none;color:black;"
	var	styleLightBorder="border-style:solid;border-width:1px;border-color:#a0a0a0;"

	function swapImage(srcImg, destImg){
		if (ie)	{ document.getElementById(srcImg).setAttribute("src",imgDir + destImg) }
	}

	function init()	{

		today =	new	Date()
		dateNow	 = today.getDate()
		monthNow = today.getMonth()
		yearNow	 = today.getYear()
		hourNow	 = today.getHours()
		minuteNow = today.getMinutes()
		secondNow = today.getSeconds()

		if (startAt==0)
		{
			if (locale=="us")
			{
				monthName = us_monthName
				dayName = us_dayName_sun
			}
			else if (locale=="cn")
			{
				monthName = cn_monthName
				dayName = cn_dayName_sun
			}
			else if (locale=="hk")
			{
				monthName = hk_monthName
				dayName = hk_dayName_sun
			}

		}
		else
		{
			if (locale=="us")
			{
				monthName = us_monthName
				dayName = us_dayName_mon
			}
			else if (locale=="cn")
			{
				monthName = cn_monthName
				dayName = cn_dayName_mon
			}
			else if (locale=="hk")
			{
				monthName = hk_monthName
				dayName = hk_dayName_mon
			}

		}

		if (!ns4)
		{
			if (!ie) { yearNow += 1900	}

			crossobj=(dom)?document.getElementById("calendar").style : ie? document.all.calendar : document.calendar

			hideCalendar()

			crossSecondObj=(dom)?document.getElementById("selectSecond").style : ie? document.all.selectSecond : document.selectSecond

			crossMinuteObj=(dom)?document.getElementById("selectMinute").style : ie? document.all.selectMinute : document.selectMinute

			crossHourObj=(dom)?document.getElementById("selectHour").style : ie? document.all.selectHour : document.selectHour

			crossMonthObj=(dom)?document.getElementById("selectMonth").style : ie? document.all.selectMonth	: document.selectMonth

			crossYearObj=(dom)?document.getElementById("selectYear").style : ie? document.all.selectYear : document.selectYear

			secondConstructed=false
			minuteConstructed=false
			hourConstructed=false
			monthConstructed=false
			yearConstructed=false

			if (showToday==1)
			{
				document.getElementById("lblToday").innerHTML =	getTodayString()
						+ " <a style='"
						+styleLink1
						+"' href='javascript:monthSelected=monthNow;yearSelected=yearNow;constructCalendar();'>"
						+getFullToday()
						+"</a>"
						+"&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a style='"
						+styleLink1
						+"' href='javascript:clearDate()'>"
						+getClearString()
						+"</a>"
			}

			sHTML1=getDateString()+":&nbsp;&nbsp;"
			sHTML1+="<span id='spanLeft'	style='border-style:solid;border-width:1;border-color:#3366FF;cursor:pointer' onmouseover='swapImage(\"changeLeft\",\"calleft2.gif\");this.style.borderColor=\"#88AAFF\";' onclick='javascript:decMonth()' onmouseout='clearInterval(intervalID1);swapImage(\"changeLeft\",\"calleft1.gif\");this.style.borderColor=\"#3366FF\";window.status=\"\"' onmousedown='clearTimeout(timeoutID1);timeoutID1=setTimeout(\"StartDecMonth()\",500)'	onmouseup='clearTimeout(timeoutID1);clearInterval(intervalID1)'>&nbsp<IMG id='changeLeft' SRC='"+imgDir+"calleft1.gif' width=10 height=11 BORDER=0>&nbsp</span>&nbsp;"
			sHTML1+="<span id='spanRight' style='border-style:solid;border-width:1;border-color:#3366FF;cursor:pointer'	onmouseover='swapImage(\"changeRight\",\"calright2.gif\");this.style.borderColor=\"#88AAFF\";' onmouseout='clearInterval(intervalID1);swapImage(\"changeRight\",\"calright1.gif\");this.style.borderColor=\"#3366FF\";window.status=\"\"' onclick='incMonth()' onmousedown='clearTimeout(timeoutID1);timeoutID1=setTimeout(\"StartIncMonth()\",500)'	onmouseup='clearTimeout(timeoutID1);clearInterval(intervalID1)'>&nbsp<IMG id='changeRight' SRC='"+imgDir+"calright1.gif'	width=10 height=11 BORDER=0>&nbsp</span>&nbsp"
			sHTML1+="<span id='spanMonth' style='border-style:solid;border-width:1;border-color:#3366FF;cursor:pointer'	onmouseover='swapImage(\"changeMonth\",\"caldrop2.gif\");this.style.borderColor=\"#88AAFF\";' onmouseout='swapImage(\"changeMonth\",\"caldrop1.gif\");this.style.borderColor=\"#3366FF\";window.status=\"\"' onclick='popUpMonth()'></span>&nbsp;"
			sHTML1+="<span id='spanYear' style='border-style:solid;border-width:1;border-color:#3366FF;cursor:pointer' onmouseover='swapImage(\"changeYear\",\"caldrop2.gif\");this.style.borderColor=\"#88AAFF\";'	onmouseout='swapImage(\"changeYear\",\"caldrop1.gif\");this.style.borderColor=\"#3366FF\";window.status=\"\"'	onclick='popUpYear()'></span>&nbsp;"

			sHTML2=getTimeString()+":&nbsp;&nbsp;"
	        sHTML2+="<span id='spanHour' style='border-style:solid;border-width:1;border-color:#3366FF;cursor:pointer' onmouseover='swapImage(\"changeHour\",\"caldrop2.gif\");this.style.borderColor=\"#88AAFF\";' onmouseout='swapImage(\"changeHour\",\"caldrop1.gif\");this.style.borderColor=\"#3366FF\";window.status=\"\"' onclick='popUpHour()'></span>&nbsp:"
		    sHTML2+="<span id='spanMinute' style='border-style:solid;border-width:1;border-color:#3366FF;cursor:pointer'	onmouseover='swapImage(\"changeMinute\",\"caldrop2.gif\");this.style.borderColor=\"#88AAFF\";' onmouseout='swapImage(\"changeMinute\",\"caldrop1.gif\");this.style.borderColor=\"#3366FF\";window.status=\"\"' onclick='popUpMinute()'></span>&nbsp:"
		    sHTML2+="<span id='spanSecond' style='border-style:solid;border-width:1;border-color:#3366FF;cursor:pointer'	onmouseover='swapImage(\"changeSecond\",\"caldrop2.gif\");this.style.borderColor=\"#88AAFF\";' onmouseout='swapImage(\"changeSecond\",\"caldrop1.gif\");this.style.borderColor=\"#3366FF\";window.status=\"\"' onclick='popUpSecond()'></span>&nbsp"

			document.getElementById("caption").innerHTML  =	sHTML1

			document.getElementById("time").innerHTML = sHTML2

			bPageLoaded=true
		}

	}

	function hideCalendar()	{
	    if (crossobj != null){crossobj.visibility="hidden"}
		if (crossSecondObj != null){crossSecondObj.visibility="hidden"}
		if (crossMinuteObj != null){crossMinuteObj.visibility="hidden"}
		if (crossHourObj != null){crossHourObj.visibility="hidden"}
		if (crossMonthObj != null){crossMonthObj.visibility="hidden"}
		if (crossYearObj !=	null){crossYearObj.visibility="hidden"}

	    showElement( 'SELECT' );
		showElement( 'APPLET' );
	}

	function padZero(num) {
		return (num	< 10)? '0' + num : num ;
	}

	function constructDateTime(d,m,y,h,min,s)
	{
		sTmp = dateFormat
		sTmp = sTmp.replace	("dd","<e>")
		sTmp = sTmp.replace	("d","<d>")
		sTmp = sTmp.replace	("<e>",padZero(d))
		sTmp = sTmp.replace	("<d>",d)
		sTmp = sTmp.replace	("MMM","<p>")
		sTmp = sTmp.replace	("MMMM","<o>")
		sTmp = sTmp.replace	("MM","<n>")
		sTmp = sTmp.replace	("M","<m>")
		sTmp = sTmp.replace	("mmmm","<p>")
		sTmp = sTmp.replace	("mmm","<o>")
		sTmp = sTmp.replace	("mm","<n>")
		sTmp = sTmp.replace	("m","<m>")
		sTmp = sTmp.replace	("<m>",m+1)
		sTmp = sTmp.replace	("<n>",padZero(m+1))
		sTmp = sTmp.replace	("<o>",monthName[m])
		sTmp = sTmp.replace	("<p>",us_monthName2[m])
		sTmp = sTmp.replace	("yyyy",y)
		sTmp += " "
		sTmp += padZero(h)
		sTmp += ":"
		sTmp += padZero(min)
		sTmp += ":"
		sTmp += padZero(s)

		return sTmp.replace ("yy",padZero(y%100))
	}

	function constructDate(d,m,y)
	{
		sTmp = dateFormat
		sTmp = sTmp.replace	("dd","<e>")
		sTmp = sTmp.replace	("d","<d>")
		sTmp = sTmp.replace	("<e>",padZero(d))
		sTmp = sTmp.replace	("<d>",d)
		sTmp = sTmp.replace	("MMM","<p>")
		sTmp = sTmp.replace	("MMMM","<o>")
		sTmp = sTmp.replace	("MM","<n>")
		sTmp = sTmp.replace	("M","<m>")
		sTmp = sTmp.replace	("mmmm","<p>")
		sTmp = sTmp.replace	("mmm","<o>")
		sTmp = sTmp.replace	("mm","<n>")
		sTmp = sTmp.replace	("m","<m>")
		sTmp = sTmp.replace	("<m>",m+1)
		sTmp = sTmp.replace	("<n>",padZero(m+1))
		sTmp = sTmp.replace	("<o>",monthName[m])
		sTmp = sTmp.replace	("<p>",us_monthName2[m])
		sTmp = sTmp.replace	("yyyy",y)

		return sTmp.replace ("yy",padZero(y%100))
	}

	function constructTime(h,min,s)
	{
		sTmp = padZero(h)
		sTmp += ":"
		sTmp += padZero(min)
		sTmp += ":"
		sTmp += padZero(s)

		return sTmp
	}

	function closeCalendar() {
		var	sTmp

		hideCalendar();

		if (dateTime=="dateTime")
		{

			ctlToPlaceValue.value =	constructDateTime(dateSelected,monthSelected,yearSelected,hourSelected,minuteSelected,secondSelected)
		}
		else if (dateTime=="date")
		{
			ctlToPlaceValue.value =	constructDate(dateSelected,monthSelected,yearSelected)
		}
		else if (dateTime=="time")
		{
			ctlToPlaceValue.value =	constructTime(hourSelected,minuteSelected,secondSelected)
		}

	}

	/*** Second Pulldown	***/

	function incSecond() {
		if (nStartingSecond<53)
		{
			for	(i=0; i<7; i++){
				newSecond	= (i+nStartingSecond)+1
				if (newSecond==secondSelected)
				{ txtSecond = "&nbsp;<B>"	+ newSecond +	"</B>&nbsp;" }
				else
				{ txtSecond =	"&nbsp;" + newSecond + "&nbsp;" }
				document.getElementById("s"+i).innerHTML = txtSecond
			}
			nStartingSecond ++;
		}
		bShow=true
	}

	function decSecond() {
		if (nStartingSecond>0)
		{
			for	(i=0; i<7; i++){
				newSecond	= (i+nStartingSecond)-1
				if (newSecond==secondSelected)
				{ txtSecond =	"&nbsp;<B>"	+ newSecond +	"</B>&nbsp;" }
				else
				{ txtSecond =	"&nbsp;" + newSecond + "&nbsp;" }
				document.getElementById("s"+i).innerHTML = txtSecond
			}
			nStartingSecond --;
		}
		bShow=true
	}

	function selectSecond(nSecond) {
		secondSelected=parseInt(nSecond+nStartingSecond);
		secondConstructed=false;
		constructCalendar();
		popDownSecond();
	}

	function constructSecond() {

		popDownMinute()
		popDownHour()
		popDownMonth()
		popDownYear()

		if (!secondConstructed) {

			sHTML =	""

			sHTML =	"<tr><td align='center'	onmouseover='this.style.backgroundColor=\"#FFCC99\"' onmouseout='clearInterval(intervalID1);this.style.backgroundColor=\"\"' style='cursor:pointer'	onmousedown='clearInterval(intervalID1);intervalID1=setInterval(\"decSecond()\",30)' onmouseup='clearInterval(intervalID1)'>-</td></tr>"

			if (secondSelected<=3)
			{
				nStartingSecond =	0
			}
			else if (secondSelected>=57)
			{
				nStartingSecond =	53
			}
			else
			{
				nStartingSecond =	secondSelected-3
			}

			j =	0

			for	(i=(nStartingSecond); i<=(nStartingSecond+6); i++) {

				sName =	i;

				if (i==secondSelected){
					sName =	"<B>" +	sName +	"</B>"
				}
				sHTML += "<tr><td id='s" + j + "' align='center' onmouseover='this.style.backgroundColor=\"#FFCC99\"' onmouseout='this.style.backgroundColor=\"\"' style='cursor:pointer' onclick='selectSecond("+j+");event.cancelBubble=true'>&nbsp;" + sName + "&nbsp;</td></tr>"

				j++
			}

			sHTML += "<tr><td align='center' onmouseover='this.style.backgroundColor=\"#FFCC99\"' onmouseout='clearInterval(intervalID2);this.style.backgroundColor=\"\"' style='cursor:pointer' onmousedown='clearInterval(intervalID2);intervalID2=setInterval(\"incSecond()\",30)'	onmouseup='clearInterval(intervalID2)'>+</td></tr>"

			document.getElementById("selectSecond").innerHTML = "<table width=30	style='font-family:arial; font-size:11px; border-width:1; border-style:solid; border-color:#a0a0a0;' bgcolor='#FFFFDD' cellspacing=0 onmouseover='clearTimeout(timeoutID1)'	onmouseout='clearTimeout(timeoutID1);timeoutID1=setTimeout(\"popDownSecond()\",100);event.cancelBubble=true'>" +	sHTML +	"</table>"

			secondConstructed=true
		}
	}

	function popUpSecond() {

		var	leftOffset

		constructSecond()

		crossSecondObj.visibility = (dom||ie)? "visible"	: "show"

		leftOffset = parseInt(crossobj.left) + document.getElementById("spanSecond").offsetLeft
		if (ie)
		{
			leftOffset += 6
		}

		crossSecondObj.left = leftOffset
		crossSecondObj.top = parseInt(crossobj.top) + 48

	}

	function popDownSecond()	{
		clearInterval(intervalID1)
		clearTimeout(timeoutID1)
		clearInterval(intervalID2)
		clearTimeout(timeoutID2)
		crossSecondObj.visibility= "hidden"
	}

	/*** Minute Pulldown	***/

	function incMinute() {
		if (nStartingMinute<53)
		{
			for	(i=0; i<7; i++){
				newMinute	= (i+nStartingMinute)+1
				if (newMinute==minuteSelected)
				{ txtMinute = "&nbsp;<B>"	+ newMinute +	"</B>&nbsp;" }
				else
				{ txtMinute =	"&nbsp;" + newMinute + "&nbsp;" }
				document.getElementById("min"+i).innerHTML = txtMinute
			}
			nStartingMinute ++;
		}
		bShow=true
	}

	function decMinute() {
		if (nStartingMinute>0)
		{
			for	(i=0; i<7; i++){
				newMinute	= (i+nStartingMinute)-1
				if (newMinute==minuteSelected)
				{ txtMinute =	"&nbsp;<B>"	+ newMinute +	"</B>&nbsp;" }
				else
				{ txtMinute =	"&nbsp;" + newMinute + "&nbsp;" }
				document.getElementById("min"+i).innerHTML = txtMinute
			}
			nStartingMinute --;
		}
		bShow=true
	}

	function selectMinute(nMinute) {
		minuteSelected=parseInt(nMinute+nStartingMinute);
		minuteConstructed=false;
		constructCalendar();
		popDownMinute();
	}

	function constructMinute() {

		popDownSecond()
		popDownHour()
		popDownMonth()
		popDownYear()

		if (!minuteConstructed) {

			sHTML =	""

			sHTML =	"<tr><td align='center'	onmouseover='this.style.backgroundColor=\"#FFCC99\"' onmouseout='clearInterval(intervalID1);this.style.backgroundColor=\"\"' style='cursor:pointer'	onmousedown='clearInterval(intervalID1);intervalID1=setInterval(\"decMinute()\",30)' onmouseup='clearInterval(intervalID1)'>-</td></tr>"

			if (minuteSelected<=3)
			{
				nStartingMinute =	0
			}
			else if (minuteSelected>=57)
			{
				nStartingMinute =	53
			}
			else
			{
				nStartingMinute =	minuteSelected-3
			}

			j =	0

			for	(i=(nStartingMinute); i<=(nStartingMinute+6); i++) {

				sName =	i;

				if (i==minuteSelected){
					sName =	"<B>" +	sName +	"</B>"
				}
				sHTML += "<tr><td id='min" + j + "' align='center' onmouseover='this.style.backgroundColor=\"#FFCC99\"' onmouseout='this.style.backgroundColor=\"\"' style='cursor:pointer' onclick='selectMinute("+j+");event.cancelBubble=true'>&nbsp;" + sName + "&nbsp;</td></tr>"

				j++
			}

			sHTML += "<tr><td align='center' onmouseover='this.style.backgroundColor=\"#FFCC99\"' onmouseout='clearInterval(intervalID2);this.style.backgroundColor=\"\"' style='cursor:pointer' onmousedown='clearInterval(intervalID2);intervalID2=setInterval(\"incMinute()\",30)'	onmouseup='clearInterval(intervalID2)'>+</td></tr>"

			document.getElementById("selectMinute").innerHTML = "<table width=30	style='font-family:arial; font-size:11px; border-width:1; border-style:solid; border-color:#a0a0a0;' bgcolor='#FFFFDD' cellspacing=0 onmouseover='clearTimeout(timeoutID1)'	onmouseout='clearTimeout(timeoutID1);timeoutID1=setTimeout(\"popDownMinute()\",100);event.cancelBubble=true'>" +	sHTML +	"</table>"

			minuteConstructed=true
		}
	}

	function popUpMinute() {

		var	leftOffset

		constructMinute()

		crossMinuteObj.visibility = (dom||ie)? "visible"	: "show"

		leftOffset = parseInt(crossobj.left) + document.getElementById("spanMinute").offsetLeft
		if (ie)
		{
			leftOffset += 6
		}

		crossMinuteObj.left = leftOffset
		crossMinuteObj.top = parseInt(crossobj.top) + 48

	}

	function popDownMinute()	{
		clearInterval(intervalID1)
		clearTimeout(timeoutID1)
		clearInterval(intervalID2)
		clearTimeout(timeoutID2)
		crossMinuteObj.visibility= "hidden"
	}

	/*** Hour Pulldown	***/

	function incHour() {
		if (nStartingHour<17)
		{
			for	(i=0; i<7; i++){
				newHour	= (i+nStartingHour)+1
				if (newHour==hourSelected)
				{ txtHour =	"&nbsp;<B>"	+ newHour +	"</B>&nbsp;" }
				else
				{ txtHour =	"&nbsp;" + newHour + "&nbsp;" }
				document.getElementById("h"+i).innerHTML = txtHour
			}
			nStartingHour ++;
		}
		bShow=true
	}

	function decHour() {
		if (nStartingHour>0)
		{
			for	(i=0; i<7; i++){
				newHour	= (i+nStartingHour)-1
				if (newHour==hourSelected)
				{ txtHour =	"&nbsp;<B>"	+ newHour +	"</B>&nbsp;" }
				else
				{ txtHour =	"&nbsp;" + newHour + "&nbsp;" }
				document.getElementById("h"+i).innerHTML = txtHour
			}
			nStartingHour --;
		}
		bShow=true
	}

	function selectHour(nHour) {
		hourSelected=parseInt(nHour+nStartingHour);
		hourConstructed=false;
		constructCalendar();
		popDownHour();
	}

	function constructHour() {

		popDownSecond()
		popDownMinute()
		popDownMonth()
		popDownYear()

		if (!hourConstructed) {

			sHTML =	""

			sHTML =	"<tr><td align='center'	onmouseover='this.style.backgroundColor=\"#FFCC99\"' onmouseout='clearInterval(intervalID1);this.style.backgroundColor=\"\"' style='cursor:pointer'	onmousedown='clearInterval(intervalID1);intervalID1=setInterval(\"decHour()\",30)' onmouseup='clearInterval(intervalID1)'>-</td></tr>"

			if (hourSelected<=3)
			{
				nStartingHour =	0
			}
			else if (hourSelected>=20)
			{
				nStartingHour =	17
			}
			else
			{
				nStartingHour =	hourSelected-3
			}

			j =	0

			for	(i=(nStartingHour); i<=(nStartingHour+6); i++) {

				sName =	i;

				if (i==hourSelected){
					sName =	"<B>" +	sName +	"</B>"
				}
				sHTML += "<tr><td id='h" + j + "' align='center' onmouseover='this.style.backgroundColor=\"#FFCC99\"' onmouseout='this.style.backgroundColor=\"\"' style='cursor:pointer' onclick='selectHour("+j+");event.cancelBubble=true'>&nbsp;" + sName + "&nbsp;</td></tr>"

				j++
			}

			sHTML += "<tr><td align='center' onmouseover='this.style.backgroundColor=\"#FFCC99\"' onmouseout='clearInterval(intervalID2);this.style.backgroundColor=\"\"' style='cursor:pointer' onmousedown='clearInterval(intervalID2);intervalID2=setInterval(\"incHour()\",30)'	onmouseup='clearInterval(intervalID2)'>+</td></tr>"

			document.getElementById("selectHour").innerHTML = "<table width=30	style='font-family:arial; font-size:11px; border-width:1; border-style:solid; border-color:#a0a0a0;' bgcolor='#FFFFDD' cellspacing=0 onmouseover='clearTimeout(timeoutID1)'	onmouseout='clearTimeout(timeoutID1);timeoutID1=setTimeout(\"popDownHour()\",100);event.cancelBubble=true'>" +	sHTML +	"</table>"

			hourConstructed=true
		}
	}

	function popUpHour() {

		var	leftOffset

		constructHour()

		crossHourObj.visibility = (dom||ie)? "visible"	: "show"

		leftOffset = parseInt(crossobj.left) + document.getElementById("spanHour").offsetLeft
		if (ie)
		{
			leftOffset += 6
		}

		crossHourObj.left =	leftOffset
		crossHourObj.top = parseInt(crossobj.top) +	48
	}

	function popDownHour()	{
		clearInterval(intervalID1)
		clearTimeout(timeoutID1)
		clearInterval(intervalID2)
		clearTimeout(timeoutID2)
		crossHourObj.visibility= "hidden"
	}

	/*** Month Pulldown	***/

	function StartDecMonth()
	{
		intervalID1=setInterval("decMonth()",80)
	}

	function StartIncMonth()
	{
		intervalID1=setInterval("incMonth()",80)
	}

	function incMonth () {
		monthSelected++
		if (monthSelected>11) {
			monthSelected=0
			yearSelected++
		}
		constructCalendar()
	}

	function decMonth () {
		monthSelected--
		if (monthSelected<0) {
			monthSelected=11
			yearSelected--
		}
		constructCalendar()
	}

	function constructMonth() {

		popDownSecond()
		popDownMinute()
		popDownHour()
		popDownYear()

		if (!monthConstructed) {
			sHTML =	""
			for	(i=0; i<12;	i++) {
				sName =	monthName[i];
				if (i==monthSelected){
					sName =	"<B>" +	sName +	"</B>"
				}
				sHTML += "<tr align='center'><td id='m" + i + "' onmouseover='this.style.backgroundColor=\"#FFCC99\"' onmouseout='this.style.backgroundColor=\"\"' style='cursor:pointer' onclick='monthConstructed=false;monthSelected=" + i + ";constructCalendar();popDownMonth();event.cancelBubble=true'>&nbsp;" + sName + "&nbsp;</td></tr>"
			}

			document.getElementById("selectMonth").innerHTML = "<table width=70	style='font-family:arial; font-size:11px; border-width:1; border-style:solid; border-color:#a0a0a0;' bgcolor='#FFFFDD' cellspacing=0 onmouseover='clearTimeout(timeoutID1)'	onmouseout='clearTimeout(timeoutID1);timeoutID1=setTimeout(\"popDownMonth()\",100);event.cancelBubble=true'>" +	sHTML +	"</table>"

			monthConstructed=true
		}
	}

	function popUpMonth() {

		var	leftOffset

		constructMonth()
		crossMonthObj.visibility	= (dom||ie)? "visible" : "show"
		leftOffset = parseInt(crossobj.left) + document.getElementById("spanMonth").offsetLeft
		if (ie)
		{
			leftOffset += 6
		}
		crossMonthObj.left =	leftOffset
		crossMonthObj.top = parseInt(crossobj.top) + 26

	}

	function popDownMonth()	{
		crossMonthObj.visibility= "hidden"
	}

	/*** Year Pulldown ***/

	function incYear() {
		for	(i=0; i<7; i++){
			newYear	= (i+nStartingYear)+1
			if (newYear==yearSelected)
			{ txtYear =	"&nbsp;<B>"	+ newYear +	"</B>&nbsp;" }
			else
			{ txtYear =	"&nbsp;" + newYear + "&nbsp;" }
			document.getElementById("y"+i).innerHTML = txtYear
		}
		nStartingYear ++;
		bShow=true
	}

	function decYear() {
		for	(i=0; i<7; i++){
			newYear	= (i+nStartingYear)-1
			if (newYear==yearSelected)
			{ txtYear =	"&nbsp;<B>"	+ newYear +	"</B>&nbsp;" }
			else
			{ txtYear =	"&nbsp;" + newYear + "&nbsp;" }
			document.getElementById("y"+i).innerHTML = txtYear
		}
		nStartingYear --;
		bShow=true
	}

	function selectYear(nYear) {
		yearSelected=parseInt(nYear+nStartingYear);
		yearConstructed=false;
		constructCalendar();
		popDownYear();
	}

	function constructYear() {
		popDownSecond()
		popDownMinute()
		popDownHour()
		popDownMonth()
		sHTML =	""
		if (!yearConstructed) {

			sHTML =	"<tr><td align='center'	onmouseover='this.style.backgroundColor=\"#FFCC99\"' onmouseout='clearInterval(intervalID1);this.style.backgroundColor=\"\"' style='cursor:pointer'	onmousedown='clearInterval(intervalID1);intervalID1=setInterval(\"decYear()\",30)' onmouseup='clearInterval(intervalID1)'>-</td></tr>"

			j =	0
			nStartingYear =	yearSelected-3

			for	(i=(yearSelected-3); i<=(yearSelected+3); i++) {
				sName =	i;
				if (i==yearSelected){
					sName =	"<B>" +	sName +	"</B>"
				}

				sHTML += "<tr><td id='y" + j + "' onmouseover='this.style.backgroundColor=\"#FFCC99\"' onmouseout='this.style.backgroundColor=\"\"' style='cursor:pointer' onclick='selectYear("+j+");event.cancelBubble=true'>&nbsp;" + sName + "&nbsp;</td></tr>"
				j ++;
			}

			sHTML += "<tr><td align='center' onmouseover='this.style.backgroundColor=\"#FFCC99\"' onmouseout='clearInterval(intervalID2);this.style.backgroundColor=\"\"' style='cursor:pointer' onmousedown='clearInterval(intervalID2);intervalID2=setInterval(\"incYear()\",30)'	onmouseup='clearInterval(intervalID2)'>+</td></tr>"

			document.getElementById("selectYear").innerHTML	= "<table width=44 style='font-family:arial; font-size:11px; border-width:1; border-style:solid; border-color:#a0a0a0;'	bgcolor='#FFFFDD' onmouseover='clearTimeout(timeoutID2)' onmouseout='clearTimeout(timeoutID2);timeoutID2=setTimeout(\"popDownYear()\",100)' cellspacing=0>"	+ sHTML	+ "</table>"

			yearConstructed	= true
		}
	}

	function popDownYear() {
		clearInterval(intervalID1)
		clearTimeout(timeoutID1)
		clearInterval(intervalID2)
		clearTimeout(timeoutID2)
		crossYearObj.visibility= "hidden"
	}

	function popUpYear() {
		var	leftOffset

		constructYear()
		crossYearObj.visibility	= (dom||ie)? "visible" : "show"
		leftOffset = parseInt(crossobj.left) + document.getElementById("spanYear").offsetLeft
		if (ie)
		{
			leftOffset += 6
		}
		crossYearObj.left =	leftOffset
		crossYearObj.top = parseInt(crossobj.top) +	26
	}

	/*** calendar ***/
   function WeekNbr(n) {
      // Algorithm used:
      // From Klaus Tondering's Calendar document (The Authority/Guru)
      // hhtp://www.tondering.dk/claus/calendar.html
      // a = (14-month) / 12
      // y = year + 4800 - a
      // m = month + 12a - 3
      // J = day + (153m + 2) / 5 + 365y + y / 4 - y / 100 + y / 400 - 32045
      // d4 = (J + 31741 - (J mod 7)) mod 146097 mod 36524 mod 1461
      // L = d4 / 1460
      // d1 = ((d4 - L) mod 365) + L
      // WeekNumber = d1 / 7 + 1

      year = n.getFullYear();
      month = n.getMonth() + 1;
      if (startAt == 0) {
         day = n.getDate() + 1;
      }
      else {
         day = n.getDate();
      }

      a = Math.floor((14-month) / 12);
      y = year + 4800 - a;
      m = month + 12 * a - 3;
      b = Math.floor(y/4) - Math.floor(y/100) + Math.floor(y/400);
      J = day + Math.floor((153 * m + 2) / 5) + 365 * y + b - 32045;
      d4 = (((J + 31741 - (J % 7)) % 146097) % 36524) % 1461;
      L = Math.floor(d4 / 1460);
      d1 = ((d4 - L) % 365) + L;
      week = Math.floor(d1/7) + 1;

      return week;
   }

	function constructCalendar () {



		var aNumDays = Array (31,0,31,30,31,30,31,31,30,31,30,31)

		var	startDate =	new	Date (yearSelected,monthSelected,1)
		var endDate

		if (monthSelected==1)
		{
			endDate	= new Date (yearSelected,monthSelected+1,1);
			endDate	= new Date (endDate	- (24*60*60*1000));
			numDaysInMonth = endDate.getDate()
		}
		else
		{
			numDaysInMonth = aNumDays[monthSelected];
		}

		datePointer	= 0
		dayPointer = startDate.getDay() - startAt

		if (dayPointer<0)
		{
			dayPointer = 6
		}

		sHTML =	"<table	 border=0 style='font-family:verdana;font-size:10px;'><tr>"

		if (showWeekNumber==1)
		{
			sHTML += "<td width=27><b>" + getWeekString() + "</b></td><td width=1 rowspan=7 bgcolor='#d0d0d0' style='padding:0px'><img src='"+imgDir+"caldivider.gif' width=1></td>"
		}

		for	(i=0; i<7; i++)	{
			sHTML += "<td width='27' align='right'><B>"+ dayName[i]+"</B></td>"
		}
		sHTML +="</tr><tr>"

		if (showWeekNumber==1)
		{
			sHTML += "<td align=right>" + WeekNbr(startDate) + "&nbsp;</td>"
		}

		for	( var i=1; i<=dayPointer;i++ )
		{
			sHTML += "<td>&nbsp;</td>"
		}

		for	( datePointer=1; datePointer<=numDaysInMonth; datePointer++ )
		{
			dayPointer++;
			sHTML += "<td align=right>"
			sStyle=styleAnchor
			if ((datePointer==odateSelected) &&	(monthSelected==omonthSelected)	&& (yearSelected==oyearSelected))
			{ sStyle+=styleLightBorder }

			sHint = ""
			for (k=0;k<HolidaysCounter;k++)
			{
				if ((parseInt(Holidays[k].d)==datePointer)&&(parseInt(Holidays[k].m)==(monthSelected+1)))
				{
					if ((parseInt(Holidays[k].y)==0)||((parseInt(Holidays[k].y)==yearSelected)&&(parseInt(Holidays[k].y)!=0)))
					{
						sStyle+="background-color:#FFDDDD;"
						sHint+=sHint==""?Holidays[k].desc:"\n"+Holidays[k].desc
					}
				}
			}

			var regexp= /\"/g
			sHint=sHint.replace(regexp,"&quot;")

			if ((datePointer==dateNow)&&(monthSelected==monthNow)&&(yearSelected==yearNow))
			{ sHTML += "<b><a title=\"" + sHint + "\" style='"+sStyle+"' href='javascript:dateSelected="+datePointer+";closeCalendar();'><font color=#ff0000>&nbsp;" + datePointer + "</font>&nbsp;</a></b>"}
			else if	(dayPointer % 7 == (startAt * -1)+1)
			{ sHTML += "<a title=\"" + sHint + "\" style='"+sStyle+"' href='javascript:dateSelected="+datePointer + ";closeCalendar();'>&nbsp;<font color=#909090>" + datePointer + "</font>&nbsp;</a>" }
			else
			{ sHTML += "<a title=\"" + sHint + "\" style='"+sStyle+"' href='javascript:dateSelected="+datePointer + ";closeCalendar();'>&nbsp;" + datePointer + "&nbsp;</a>" }

			sHTML += ""
			if ((dayPointer+startAt) % 7 == startAt) {
				sHTML += "</tr><tr>"
				if ((showWeekNumber==1)&&(datePointer<numDaysInMonth))
				{
					sHTML += "<td align=right>" + (WeekNbr(new Date(yearSelected,monthSelected,datePointer+1))) + "&nbsp;</td>"
				}
			}
		}

		document.getElementById("spanSecond").innerHTML = "&nbsp;" + secondSelected + "&nbsp;<IMG id='changeSecond' SRC='"+imgDir+"caldrop1.gif' WIDTH='12' HEIGHT='10' BORDER=0>"
		document.getElementById("spanMinute").innerHTML = "&nbsp;" + minuteSelected + "&nbsp;<IMG id='changeMinute' SRC='"+imgDir+"caldrop1.gif' WIDTH='12' HEIGHT='10' BORDER=0>"
		document.getElementById("spanHour").innerHTML = "&nbsp;" +	hourSelected + "&nbsp;<IMG id='changeHour' SRC='"+imgDir+"caldrop1.gif' WIDTH='12' HEIGHT='10' BORDER=0>"

		document.getElementById("content").innerHTML   = sHTML
		document.getElementById("spanMonth").innerHTML = "&nbsp;" +	monthName[monthSelected] + "&nbsp;<IMG id='changeMonth' SRC='"+imgDir+"caldrop1.gif' WIDTH='12' HEIGHT='10' BORDER=0>"
		document.getElementById("spanYear").innerHTML =	"&nbsp;" + yearSelected	+ "&nbsp;<IMG id='changeYear' SRC='"+imgDir+"caldrop1.gif' WIDTH='12' HEIGHT='10' BORDER=0>"
	}
    
	function popUpCalendar(ctl,	ctl23, userLocale, userDateTime, userDateFormat, xPosition, yPosition) {

	    //var ctl2 = document.getElementsByName(ctl23)[0]
        var ctl2 = document.getElementById(ctl23);
        if (ctl.tagName.toUpperCase() == "INPUT" && ctl.type.toUpperCase() == "TEXT") {
            ctl2 = ctl;
        }
		var	leftpos
		var	toppos

		if (xPosition == "left")
		{
			leftpos = -240
		}
		else
		{
			leftpos = 0
		}

		if (yPosition == "up")
		{
			toppos = -235
		}
		else
		{
			toppos = 0
		}

		// obtain input
		inputBoxName = ctl2;

		// obtain user's locale
		locale = userLocale

		dateTime = userDateTime

		init()

		if (bPageLoaded)
		{
			if ( crossobj.visibility ==	"hidden" ) {

				ctlToPlaceValue	= ctl2

				// obtain format
				dateFormat = userDateFormat

				formatChar = " "
				aFormat	= dateFormat.split(formatChar)


				if (aFormat.length<3)
				{
					formatChar = "/"
					aFormat	= dateFormat.split(formatChar)
					if (aFormat.length<3)
					{
						formatChar = "."
						aFormat	= dateFormat.split(formatChar)
						if (aFormat.length<3)
						{
							formatChar = "-"
							aFormat	= dateFormat.split(formatChar)
							if (aFormat.length<3)
							{
								// invalid date	format
								formatChar=""
							}
						}
					}
				}

				tokensChanged =	0
				if ( formatChar	!= "" )
				{
					// use user's date
					splitDateTimeChar = " "
					splitTimeChar = ":"

					if (dateTime=="dateTime")
					{
						if (ctl2.value == "")
						{
							aDate = [yearNow,monthNow+1,dateNow]
							aTime = [hourNow,minuteNow,secondNow]

						}
						else
						{
							aDataTime = ctl2.value.split(splitDateTimeChar)

							if (aDataTime.length == 2)
							{
								aDate =	aDataTime[0].split(formatChar)
								aTime = aDataTime[1].split(splitTimeChar)
							}
							else
							{
								aDate = [yearNow,monthNow+1,dateNow]
								aTime = [hourNow,minuteNow,secondNow]
							}
						}
					}
					else if (dateTime=="date")
					{
						if (ctl2.value == "")
						{
							aDate = [yearNow,monthNow+1,dateNow]
						}
						else
						{
							aDate =	ctl2.value.split(formatChar)
						}

						aTime = [hourNow,minuteNow,secondNow]
					}
					else if (dateTime=="time")
					{

						if (ctl2.value == "")
						{
							aTime = [hourNow,minuteNow,secondNow]
						}
						else
						{
							aTime = ctl2.value.split(splitTimeChar)
						}

						aDate = [yearNow,monthNow+1,dateNow]
					}

					if (ctl2.value == "")
					{
						dateSelected = dateNow
						monthSelected = monthNow+1
						yearSelected = yearNow
					}
					else
					{
						for	(i=0;i<3;i++)
						{
							if ((aFormat[i]=="d") || (aFormat[i]=="dd"))
							{
								dateSelected = parseInt(aDate[i], 10)
								tokensChanged ++
							}
							else if	((aFormat[i]=="m") || (aFormat[i]=="mm") || (aFormat[i]=="M") || (aFormat[i]=="MM"))
							{
								monthSelected =	parseInt(aDate[i], 10) - 1
								tokensChanged ++
							}
							else if	(aFormat[i]=="yyyy")
							{
								yearSelected = parseInt(aDate[i], 10)
								tokensChanged ++
							}
							else if	((aFormat[i]=="mmmm") || (aFormat[i]=="MMMM"))
							{
								for	(j=0; j<12;	j++)
								{
									if (aDate[i]==monthName[j])
									{
										monthSelected=j
										tokensChanged ++
									}
								}
							}
							else if	((aFormat[i]=="mmm") || (aFormat[i]=="MMM"))
							{
								for	(j=0; j<12;	j++)
								{
									if (aDate[i]==us_monthName2[j])
									{
										monthSelected=j
										tokensChanged ++
									}
								}
							}
						}
					}


					hourSelected = parseInt(aTime[0], 10)
					minuteSelected = parseInt(aTime[1], 10)
					secondSelected = parseInt(aTime[2], 10)

				}

				if ((tokensChanged!=3)||isNaN(dateSelected)||isNaN(monthSelected)||isNaN(yearSelected))
				{
					dateSelected = dateNow
					monthSelected =	monthNow
					yearSelected = yearNow
				}


				odateSelected=dateSelected
				omonthSelected=monthSelected
				oyearSelected=yearSelected

				aTag = ctl
				do {
					aTag = aTag.offsetParent;
					leftpos	+= aTag.offsetLeft;
					toppos += aTag.offsetTop;
				//} while(aTag.tagName!="BODY");
				} while(aTag.tagName!="BODY"&&aTag.style.position!="absolute");  //by gaocaihui modify 2008-12-31

				crossobj.left =	fixedX==-1 ? ctl.offsetLeft	+ leftpos :	fixedX
				crossobj.top = fixedY==-1 ?	ctl.offsetTop +	toppos + ctl.offsetHeight +	2 :	fixedY



				constructCalendar (1, monthSelected, yearSelected);

				crossobj.visibility=(dom||ie)? "visible" : "show"

				hideElement( 'SELECT', document.getElementById("calendar") );
				hideElement( 'APPLET', document.getElementById("calendar") );

				bShow = true;
			}
			else
			{
				hideCalendar()
				if (ctlNow!=ctl) {popUpCalendar(ctl, ctl2, userLocale, userDateTime)}
			}
			ctlNow = ctl
		}
	}

	/*** clear the date inputbox ***/
	function clearDate()
	{
		inputBoxName.value = "";
	}

	/*** populate the full today string for different locale ***/
	function getFullToday()
	{
		var todayStr = ""

		if (locale=="us")
		{
			todayStr=dayName[(today.getDay()-startAt==-1)?6:(today.getDay()-startAt)]
					+", "
					+dateNow
					+" "
					+monthName[monthNow].substring(0,3)
					+"	"
					+yearNow
		}
		else if (locale=="cn")
		{
			todayStr=yearNow
					+"-"
					+(monthNow+1)
					+"-"
					+dateNow
					+" "
					+dayName[(today.getDay()-startAt==-1)?6:(today.getDay()-startAt)]
		}
		else if (locale=="hk")
		{
			todayStr=dateNow
					+"-"
					+(monthNow+1)
					+"-"
					+yearNow
					+" "
					+dayName[(today.getDay()-startAt==-1)?6:(today.getDay()-startAt)]
		}

		return todayStr
	}

	/*** display "date" string for different locale ***/
	function getDateString()
	{
		var dateString = ""

		if (locale=="us")
		{
			dateString = us_dateString
		}
		else if (locale=="cn")
		{
			dateString = cn_dateString
		}
		else if (locale=="hk")
		{
			dateString = hk_dateString
		}

		return dateString
	}

	/*** display "time" string for different locale ***/
	function getTimeString()
	{
		var timeString = ""

		if (locale=="us")
		{
			timeString = us_timeString
		}
		else if (locale=="cn")
		{
			timeString = cn_timeString
		}
		else if (locale=="hk")
		{
			timeString = hk_timeString
		}

		return timeString
	}

	/*** display "clear" string for different locale ***/
	function getClearString()
	{
		var clearString = ""

		if (locale=="us")
		{
			clearString = us_clearString
		}
		else if (locale=="cn")
		{
			clearString = cn_clearString
		}
		else if (locale=="hk")
		{
			clearString = hk_clearString
		}

		return clearString
	}

	/*** display "today" string for different locale ***/
	function getTodayString()
	{
		var todayString = ""

		if (locale=="us")
		{
			todayString = us_todayString
		}
		else if (locale=="cn")
		{
			todayString = cn_todayString
		}
		else if (locale=="hk")
		{
			todayString = hk_todayString
		}

		return todayString
	}

	/*** display "week" string for different locale ***/
	function getWeekString()
	{
		var weekString = ""

		if (locale=="us")
		{
			weekString = us_weekString
		}
		else if (locale=="cn")
		{
			weekString = cn_weekString
		}
		else if (locale=="hk")
		{
			weekString = hk_weekString
		}

		return weekString
	}

//add by xujianpeng 20356
	// Returns true if character c is a digit
	// (0 .. 9).
	function date_isDigit (c)
	{   return ((c >= "0") && (c <= "9"));
	}

	// isInteger (STRING s [, BOOLEAN emptyOK])
	// Returns true if all characters in string s are numbers.
	function date_isInteger (s)
	{
	    // Search through string's characters one by one
	    // until we find a non-numeric character.
	    // When we do, return false; if we don't, return true.

	    for (i = 0; i < s.length; i++)
	    {
	        // Check that current character is number.
	        var c = s.charAt(i);

	        if (!date_isDigit(c)) return false;
	    }

	    // All characters are numbers.
	    return true;
	}

	// isIntegerInRange (STRING s, INTEGER a, INTEGER b )
	// isIntegerInRange returns true if string s is an integer
	// within the range of integer arguments a and b, inclusive.
	function date_isIntegerInRange (s, a, b)
	{
	    // Catch non-integer strings to avoid creating a NaN below,
	    // which isn't available on JavaScript 1.0 for Windows.
	    if (!date_isInteger(s)) return false;

	    // Now, explicitly change the type to integer via parseInt
	    // so that the comparison code below will work both on
	    // JavaScript 1.2 (which typechecks in equality comparisons)
	    // and JavaScript 1.1 and before (which doesn't).
	     var nums = parseFloat (s);
		//alert("s:"+s+",nums:"+nums);
		return ((nums >= a) && (nums <= b));
	}

	// isYear (STRING s )
	// date_isYear returns true if string s is a valid
	// Year number.  Must be 2 or 4 digits only.
	function date_isYear (s)
	{
		if (!date_isInteger(s)) return false;
	    return ((s.length == 2) || (s.length == 4));
	}

	// date_isMonth (STRING s [, BOOLEAN emptyOK])
	// date_isMonth returns true if string s is a valid
	// month number between 1 and 12.
	function date_isMonth (s)
	{
	    return date_isIntegerInRange (s, 1, 12);
	}

	// date_isDay (STRING s [, BOOLEAN emptyOK])
	// date_isDay returns true if string s is a valid
	// day number between 1 and 31.
	function date_isDay (s)
	{   return date_isIntegerInRange (s, 1, 31);
	}

	// date_daysInFebruary (INTEGER year)
	// Given integer argument year,
	// returns number of days in February of that year.
	function date_daysInFebruary (year)
	{   // February has 29 days in any year evenly divisible by four,
	    // EXCEPT for centurial years which are not also divisible by 400.
	    return (  ((year % 4 == 0) && ( (!(year % 100 == 0)) || (year % 400 == 0) ) ) ? 29 : 28 );
	}

	function date_isDate (year, month, day)
	{
		if (!date_isYear(year)) return false;
		if (!date_isMonth(month)) return false;
		if (!date_isDay(day)) return false;

		//<-----there is point has a modification-----*****
		var daysInMonth = new Array();
		daysInMonth[1] = 31;
		// must programmatically check this
		daysInMonth[2] = 29;
		daysInMonth[3] = 31;
		daysInMonth[4] = 30;
		daysInMonth[5] = 31;
		daysInMonth[6] = 30;
		daysInMonth[7] = 31;
		daysInMonth[8] = 31;
		daysInMonth[9] = 30;
		daysInMonth[10] = 31;
		daysInMonth[11] = 30;
		daysInMonth[12] = 31;

		var intYear = parseInt(year);
	    var intMonth = parseInt(month);
	    var intDay = parseInt(day);

	    // catch invalid days, except for February
	    if (intDay > daysInMonth[intMonth]) return false;

	//	alert("intMonth	:"+intMonth);
	//	alert("Feb" + date_daysInFebruary(intYear));
		if ((intMonth == 2) && (intDay > date_daysInFebruary(intYear))) return false;

	    return true;
	}


	function isDateStrValid(val)
	{
		var pattern = document.forms[0].hiddenDatePattern.value;
		var dateStr = val.value;
		if (val.value.length == 0)
		{
			return true;
		}

		if (dateStr.length != pattern.length - 2)
		{
			return false;
		}

		var patternArray = pattern.split("-");

		var yearOfDate, monthOfDate, dayOfDate;

		for (i=0; i<patternArray.length; i++)
		{
		  	if(patternArray[i] == "yyyy")
		  	{
		  		yearOfDate = dateStr.substring(0,4);
		  		dateStr = dateStr.substring(4);
		  	}
		  	if(patternArray[i] == "MM")
		  	{
		  		monthOfDate = dateStr.substring(0,2);
		  		dateStr = dateStr.substring(2);
		  	}
		  	if(patternArray[i] == "MMM")
		  	{
		  		alert("MMM error");
		  	}
		  	if(patternArray[i] == "dd")
		  	{
		  		dayOfDate = dateStr.substring(0,2);
		  		dateStr = dateStr.substring(2);
		  	}
		}


		if (!date_isDate(yearOfDate, monthOfDate, dayOfDate))
		{
			return false;
		}

		var sTmp = pattern;
		sTmp = sTmp.replace	("dd",dayOfDate)
		sTmp = sTmp.replace ("MM",monthOfDate);
		sTmp = sTmp.replace	("yyyy",yearOfDate);
		val.value = sTmp;

		return true;

	}



	function checkDateInput(val)
	{

		if (!isDateStrValid(val))
		{
			var inputPattern = document.forms[0].hiddenDatePattern.value;
			inputPattern = inputPattern.replace("-","");
			inputPattern = inputPattern.replace("-","");
			alert("Input is invalid! the date format is: " + inputPattern );
			val.focus();
		}
	}

	function clearDateInput(val)
	{
		var strTmp = val.value;
		if (strTmp.length > 0)
		{
			strTmp = strTmp.replace("-","");
			strTmp = strTmp.replace("-","");
		}
		val.value = strTmp;
	}



	//获取IE和Js兼容的Js事件
	function GetEvent() {
	    //同时兼容ie和ff的写法
	    if (document.all) {
	        return window.event;
	    }
	    var func = GetEvent.caller;
	    while (func != null) {
	        var arg0 = func.arguments[0];
	        if (arg0) {
	            if ((arg0.constructor == Event || arg0.constructor == MouseEvent) || (typeof (arg0) == "object" && arg0.preventDefault && arg0.stopPropagation)) {
	                return arg0;
	            }
	        }
	        func = func.caller;
	    }
	    return null;
	}


	document.onkeypress = function hidecal1() {
	    var event = GetEvent();
	    if (event.keyCode == 27) {
	        hideCalendar()
	    } 
	}
	document.onclick = function hidecal2 () {
		if (!bShow)
		{
			hideCalendar()
		}
		bShow = false
	}

	if(ie)
	{
		init()
	}
	else
	{
		window.onload=init
	}




