/**
 * Get Client system time
 * @return strdatnow
 */
function getCurrentDate (){
   var datnow=new Date();
   var strmon=datnow.getMonth()+1;
   var strdate=datnow.getDate();
   if(strmon < 10){
      strmon="0"+strmon;
   }
   if(strdate < 10){
      strdate="0"+strdate;
   }
   var strdatnow=datnow.getFullYear()+"-"+strmon+"-"+strdate ;
   return strdatnow;
}

function getCurrentDateByPattern(pattern){
   var datnow=new Date();
   var strmon=datnow.getMonth()+1;
   var strdate=datnow.getDate(); 
   var stryear=datnow.getFullYear();
   var pattern1="yyyy-MM-dd";
   var pattern2="yyyy-MM-dd hh:mm:ss";
   var pattern3="dd-MM-yyyy";
   var pattern4="dd-MM-yyyy hh:mm:ss";
   var dateStr="";
   if(strmon < 10){
      strmon="0"+strmon;
   }
   if(strdate < 10){
      strdate="0"+strdate;
   }
   if(pattern==pattern1){
       dateStr=stryear+"-"+strmon+"-"+strdate;
   }
   else if(pattern==pattern2){
   	dateStr=stryear+"-"+strmon+"-"+strdate+" 00:00:00";
   }
   else if(pattern==pattern3){
   	dateStr=strdate+"-"+strmon+"-"+stryear;
   }
   else if(pattern==pattern4){
   	dateStr=strdate+"-"+strmon+"-"+stryear+" 00:00:00";
   }
   return dateStr;
}

/**
 * DateTime valid check
 *theobj DateTime string
 *pattern DateTime format,Exp:yyyy-MM-dd hh:mm:ss
 *@return true/false
 */
function isDateTimeValid(val){    
    if(val.length == 0){
       return true;
    }
    var pattern = getDatePattern(val);
    if(pattern ==""){
      return false;
    }
    var dateStr=val.substring(0,10);
    var datePattern=pattern.substring(0,10);
    var timeStr=val.substr(11);
    var timepattern=/^\d{2}:\d{2}(:\d{2})?$/g;

    if (!isDateValid(dateStr,datePattern))
    {
        return false;
    }
    if(timeStr.match(timepattern)==null){	        
	        return false;
	  }
    var arrTimeStr = timeStr.split(":");
    if(arrTimeStr[0] > 24){
       return false;
    }
    if(arrTimeStr[1] != null && arrTimeStr[1] > 59){
       return false;
    }
    if(arrTimeStr[2] != null && arrTimeStr[2] > 59){
       return false;
    }
    return true;
}
/**
 *Date valid check
 *theobj Date string
 *pattern Date format,Exp:yyyy-MM-dd,dd-MM-yyyy
 *@return true/false
 */
function isDateValid(val)
{
	var tmpdateStr = val;

	if (val.length == 0)
	{
		return true;
	}
	var pattern = getDatePattern(val);
	if(pattern == ""){
	   return false;
  }
	var arrdateStr=tmpdateStr.split("-");
	var dateStr="";
	for(var i=0;i<arrdateStr.length;i++){
	    dateStr+=arrdateStr[i];
	}
	if (dateStr.length != pattern.length-2)
	{		
		return false;
	}

	var patternArray = pattern.split("-");

	var yearOfDate, monthOfDate, dayOfDate;

	for (var y=0; y<patternArray.length; y++)
	{
	  	if(patternArray[y] == "yyyy")
	  	{
	  		yearOfDate = dateStr.substring(0,4);	
	  		
	  		if(yearOfDate * 1 < 1900 || yearOfDate * 1 > 2999){	  		    
	  		    return false;
	  		}
	  		dateStr = dateStr.substring(4);
	  	}
	  	if(patternArray[y] == "MM")
	  	{
	  		monthOfDate = dateStr.substring(0,2);
	  		dateStr = dateStr.substring(2);
	  	}
	  	if(patternArray[y] == "MMM")
	  	{	  		
	  	}
	  	if(patternArray[y] == "dd")
	  	{
	  		dayOfDate = dateStr.substring(0,2);
	  		dateStr = dateStr.substring(2);
	  	}
	}


		if (!date_isDate(yearOfDate, monthOfDate, dayOfDate))
		{			
			return false;
		}

		return true;
}

//if firstDateStr < secondDateStr then return true
function isValidDateScope(sdateStr,edateStr){
    var pattern=getDatePattern(sdateStr);
    if(pattern!=""){
        return compareDate(sdateStr,edateStr,pattern);
    }
    return true;
}

//if dateStr > currentDate then return true
function compareDate2CurDate(dateStr){
    var pattern=getDatePattern(dateStr);
    var currentDate=getCurrentDate(pattern);    
    if(pattern!=""){
    	return compareDate(currentDate,dateStr,pattern);
    }
    return true;
}

//if firstDateStr <= secondDateStr then return true
function compareDate(firstDateStr,secondDateStr,pattern)
{
  if (firstDateStr != "" && secondDateStr != "")
  {    
	  var firstDateTimeArray = firstDateStr.split(" ");
	  var secondDateTimeArray = secondDateStr.split(" ");
	  var pattern=pattern.split(" ")[0];
	  if (firstDateTimeArray.length == 1)
	  {
	  	firstDateArray = firstDateTimeArray[0].split("-");
	  	secondDateArray = secondDateTimeArray[0].split("-");
	  	patternArray = pattern.split("-");
  
	  	for(i=0;i<patternArray.length;i++)
	  	{

	  		if(patternArray[i] == "yyyy")
	  		{
	  			yearOfFirstDate = firstDateArray[i];
	  			yearOfSecondDate = secondDateArray[i];
	  		}
	  		if(patternArray[i] == "MM")
	  		{
	  			monthOfFirstDate = firstDateArray[i];
	  			monthOfSecondDate = secondDateArray[i];
	  		}
	  		if(patternArray[i] == "MMM")
	  		{
	  			monthOfFirstDate = firstDateArray[i];
	  			monthOfSecondDate = secondDateArray[i];
	  		}
	  		if(patternArray[i] == "dd")
	  		{
	  			dayOfFirstDate = firstDateArray[i];
	  			dayOfSecondDate = secondDateArray[i];
	  		}
	  	}
      
	  	if(monthOfFirstDate.length == 3)
	  	{
	  	    for(var i=0;i<enMonthInDate.length;i++)
	    	{
		        if(monthOfFirstDate == enMonthInDate[i])
		        {
		        	monthOfFirstDate = cnMonthInDate[i];
		        }
		        if(monthOfSecondDate == enMonthInDate[i])
		        {
					monthOfSecondDate = cnMonthInDate[i];
		        }
	    	}
	  	}

	  	formattedFirstDateStr = yearOfFirstDate + "-" + monthOfFirstDate + "-" + dayOfFirstDate;
	  	formattedSecondDateStr = yearOfSecondDate + "-" + monthOfSecondDate + "-" + dayOfSecondDate;
	  	if(formattedFirstDateStr>formattedSecondDateStr)
	  	{
	  		//alert(errorMessage);
	  		return false;
	  	}
	  	else
	  	{
	  		return true;
	  	}

	  }
	  else
	  {
	  	firstDateArray = firstDateTimeArray[0].split("-");
	  	secondDateArray = secondDateTimeArray[0].split("-");
	  	patternArray = pattern.split("-");

	  	for(i=0;i<patternArray.length;i++)
	  	{
	  		if(patternArray[i] == "yyyy")
	  		{
	  			yearOfFirstDate = firstDateArray[i];
	  			yearOfSecondDate = secondDateArray[i];
	  		}
	  		if(patternArray[i] == "MM")
	  		{
	  			monthOfFirstDate = firstDateArray[i];
	  			monthOfSecondDate = secondDateArray[i];
	  		}
	  		if(patternArray[i] == "MMM")
	  		{
	  			monthOfFirstDate = firstDateArray[i];
	  			monthOfSecondDate = secondDateArray[i];
	  		}
	  		if(patternArray[i] == "dd")
	  		{
	  			dayOfFirstDate = firstDateArray[i];
	  			dayOfSecondDate = secondDateArray[i];
	  		}
	  	}

	  	if(monthOfFirstDate.length == 3)
	  	{
	  	    for(var i=0;i<enMonthInDate.length;i++)
	    	{
		        if(monthOfFirstDate == enMonthInDate[i])
		        {
		        	monthOfFirstDate = cnMonthInDate[i];
		        }
		        if(monthOfSecondDate == enMonthInDate[i])
		        {
					monthOfSecondDate = cnMonthInDate[i];
		        }
	    	}
	  	}

	  	formattedFirstDateStr = yearOfFirstDate + "-" + monthOfFirstDate + "-" + dayOfFirstDate + firstDateTimeArray[1];
	  	formattedSecondDateStr = yearOfSecondDate + "-" + monthOfSecondDate + "-" + dayOfSecondDate + secondDateTimeArray[1];

	  	if(formattedFirstDateStr>formattedSecondDateStr)
	  	{	
	  		return false;
	  	}
	  	else
	  	{
	  		return true;
	  	}
	  }
  }
  else
  {
  	return true;
  }
}

function date_isDate (year, month, day)
{
	if (!date_isYear(year)) return false;
	if (!date_isMonth(month)) return false;
	if (!date_isDay(day)) return false;

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
  var intMonth = month * 1;//parseInt(month);
  var intDay = parseInt(day);  
  // catch invalid days, except for February
  if (intDay > daysInMonth[intMonth]) return false;

	if ((intMonth == 2) && (intDay > date_daysInFebruary(intYear))) return false;

    return true;
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

function getDatePattern(dateStr){
    var pattern="";
    var pattern1=/\d{4}-\d{2}-\d{2}/;
    var pattern2=/\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2}/
    var pattern3=/\d{2}-\d{2}-\d{4}/;
    var pattern4=/\d{2}-\d{2}-\d{4} \d{2}:\d{2}/
    var pattern5=/\d{4}-\d{2}-\d{2} \d{2}:\d{2}/
    var pattern6=/\d{2}-\d{2}-\d{4} \d{2}:\d{2}/
    if(dateStr.match(pattern1)){
       pattern="yyyy-MM-dd";
    }
    else if(dateStr.match(pattern2)){
        pattern="yyyy-MM-dd hh:mm:ss";
    }
    else if(dateStr.match(pattern3)){
        pattern="dd-MM-yyyy";
    }
    else if(dateStr.match(pattern4)){
        pattern="dd-MM-yyyy hh:mm:ss";
    }
    else if(dateStr.match(pattern5)){
        pattern="yyyy-MM-dd hh:mm";
    }
    else if(dateStr.match(pattern6)){
        pattern="dd-MM-yyyy hh:mm";
    }
    return pattern;
}

Date.prototype.Format = function (format) {
    try {
        var year = this.getFullYear();
        var month = (this.getMonth() + 1) < 10 ? "0" + (this.getMonth() + 1) : (this.getMonth() + 1);
        var day = this.getDate() < 10 ? "0" + this.getDate() : this.getDate();
        var hour = this.getHours() < 10 ? "0" + this.getHours() : this.getHours();
        var min = this.getMinutes() < 10 ? "0" + this.getMinutes() : this.getMinutes();
        var sec = this.getSeconds() < 10 ? "0" + this.getSeconds() : this.getSeconds();

        var pattern = "";
        var pattern1 = /\w{4}-\w{2}-\w{2}/;
        var pattern2 = /\w{4}-\w{2}-\w{2} \w{2}:\w{2}:\w{2}/
        var pattern3 = /\w{2}-\w{2}-\w{4}/;
        var pattern4 = /\w{2}-\w{2}-\w{4} \w{2}:\w{2}/
        var pattern5 = /\w{4}-\w{2}-\w{2} \w{2}:\w{2}/
        var pattern6 = /\w{2}-\w{2}-\w{4} \w{2}:\w{2}/
        var pattern7 = /\w{2}-\w{2}/
        if (format.match(pattern2)) { return "" + year + "-" + month + "-" + day + " " + hour + ":" + min + ":" + sec + ""; }
        if (format.match(pattern4)) { return "" + day + "-" + month + "-" + year + " " + hour + ":" + min + ":" + sec + ""; }
        if (format.match(pattern5)) { return "" + year + "-" + month + "-" + day + " " + hour + ":" + min; }
        if (format.match(pattern6)) { return "" + day + "-" + month + "-" + year + " " + hour + ":" + min; }
        if (format.match(pattern3)) { return "" + day + "-" + month + "-" + year + ""; }
        if (format.match(pattern1)) { return "" + year + "-" + month + "-" + day; }
        if (format.match(pattern7)) { return "" + (this.getMonth() + 1) + "月" + this.getDate() + "日" }
    } catch (e) {
        return "";
    }
    return "";
}