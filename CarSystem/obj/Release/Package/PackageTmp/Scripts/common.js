//Int regular expression.
var reInt = /^([0-9]|[1-9]\d*)$/;
//Date regular expression.
var reDate = /^(\d{4})\-(\d{2})\-(\d{2})$/;
//Float type data.
var reFloat = /^\d{0,10}(\.\d{2})?$/g;
//Chinese character regular expression.
var reZHCN = /^[u4E00-u9FA5]+$/;
//Fax number.
var reFaxNumber = /^(\d*[-])?\d*$/;

var reTime = /([0-1][0-9]|2[0-3]):([0-5][0-9])/;
//Javascript request.QueryString() method.
var request = {
QueryString : function(val) {
var uri = window.location.search;
var re = new RegExp("" +val+ "\=([^\&\?]*)", "ig");
return ((uri.match(re))?(uri.match(re)[0].substr(val.length+1)):null);
},
QueryStrings : function() {
var uri = window.location.search;
var re = /\w*\=([^\&\?]*)/ig;
var retval=[];
while ((arr = re.exec(uri)) != null)
retval.push(arr[0]);
return retval;
},
setQuery : function(val1, val2) {
var a = this.QueryStrings();
var retval = "";
var seted = false;
var re = new RegExp("^" +val1+ "\=([^\&\?]*)$", "ig");
for(var i=0; i<a.length; i++) {
if (re.test(a[i])) {
seted = true;
a[i] = val1 +"="+ val2;
}
}
retval = a.join("&");
return "?" +retval+ (seted ? "" : (retval ? "&" : "") +val1+ "=" +val2);
}
}
//End


//Show div Pane under doc element which mouse clicked.
function   showLayer(o,y){   
document.getElementById(o).style.left=getDimX(y) + "px";//For firefox browser.
document.getElementById(o).style.top=getDimY(y) + "px";
document.getElementById(o).style.display='block';
}
function hiddenLayer(o)
{
document.getElementById(o).style.display='none';   
}
function   getDimX(el){   
for   (var   lx=0;el!=null;   
lx+=el.offsetLeft,el=el.offsetParent);   
return (lx);
}   
function   getDimY(el){   
for   (var   ly=0;el!=null;   
ly+=el.offsetTop,el=el.offsetParent);   
return (ly + 20);
}   

//Show current year,month,day,week and hour
function showTime()
{ 
	AllMyDate=new Date();
	MyYear=AllMyDate.getYear();
	MyMonth=AllMyDate.getMonth();
	MyDate=AllMyDate.getDate();
	MyHours=AllMyDate.getHours();
	MyMinutes=AllMyDate.getMinutes();
	MySeconds=AllMyDate.getSeconds();
	
	if (AllMyDate.getDay() == 1){weekday="Mon.";}
	if (AllMyDate.getDay() == 2){weekday="Tues.";}
	if (AllMyDate.getDay() == 3){weekday="Wed.";}
	if (AllMyDate.getDay() == 4){weekday="Thurs.";}
	if (AllMyDate.getDay() == 5){weekday="Friday";}
	if (AllMyDate.getDay() == 6){weekday="Sat.";}
	if (AllMyDate.getDay() == 0){weekday="Sun.";}
	
	var timeshow = document.getElementById("divTimeShow");
	timeshow.innerHTML=MyYear+"-"+MyMonth+"-"+ MyDate+" "+MyHours+":" + MyMinutes +" " + weekday;
	//setTimeout("datetime()",1000);
}



//Select all enabled check box
function setEnabledAll()
{
 var ele=document.getElementById("chkAll");
 var elements = document.getElementsByTagName("INPUT");
    for(i=0; i<elements.length;i++)
    {
        if( elements[i].type=="checkbox" && elements[i].disabled==false)
        {
            elements[i].checked = ele.checked;
        }
    }
}
 

 //Open maximize window._blank
function openDetailPage(strURL)
{
    if(strURL != "")
    window.open(strURL, "_blank", 'top=0, left=0,width='+ (screen.availWidth - 10) +',height='+ (screen.availHeight-40) +' toolbar=no, menubar=no, scrollbars=yes, resizable=yes,location=no, status=yes');
    else
    return false;
}

//獲取指定高度和寬度的窗口
function openDetailPage(url,width,height)
{
    var top = (window.screen.availHeight - 30 - height) / 2;
    var left = (window.screen.availWidth - 10 - width) / 2;
    window.open(url, 'win', 'height=' + height + ',width=' + width + ',top=' + top + ',left=' + left + ',toolbar=no,menubar=no,scrollbars=yes, resizable=no,location=no, status=no,depended=no')
}

//Open maximize window._blank,Open saveAs
function openDetailPageForSaveAs(strURL)
{
    if(strURL != "")
    window.open(strURL, "_blank", 'top=0, left=0,width='+ (screen.availWidth - 10) +',height='+ (screen.availHeight-40) +' toolbar=no, menubar=yes, scrollbars=yes, resizable=yes,location=no, status=yes');
    else
    return false;
}

function isValidBU(strBU)
{
  var pattern = /^[a-z A-Z 0-9]{0,10}$/g;
  if(strBU.match(pattern) == null){
     return false;
  }
  return true;
}

function isValidPlantNo(strPlantNo)
{
  var pattern = /^[a-z A-Z 0-9]{4}$/g;
  if(strPlantNo.match(pattern) == null){
     return false;
  }
  return true;
}

function isValidYear(strYear){
  var pattern = /^2[0-9]{3}$/g;
  if(strYear.match(pattern) == null){
     return false;
  }
  return true;
}

function isValidInt(strInt)
{
    if(strInt.match(reInt) == null){
     return false;
  }
  return true;
}
/*************************************/
//get appoint decimal length's value. by gaocaihui and 2008-11-24
function getDecimalRound(strValue,length)
{
    return  Math.round(strValue*Math.pow(10,length))/Math.pow(10,length); 
}
//valid int or decimal data type. by gaocaihui add 2008-11-18
function isValidIntOrDecimal(strValue)
{
      var pattern = /^[+]?\d+(\.\d+)?$/;
      if(strValue.match(pattern) == null)
      {
         return false;
      }
      return true;
}
//valid integer or three digit decimal data type. by gaocaihui add 2008-11-18
function isValidIntOrThreeDigitDecimal(strValue)
{
      var pattern = /^[+]?\d+(\.?[0-9]{0,3})?$/;
      if(strValue.match(pattern) == null)
      {
         return false;
      }
      return true;
}
//valid integer or double digit decimal data type. by gaocaihui add 2008-11-18
function isValidIntOrDoubleDigitDecimal(strValue)
{
      var pattern = /^[+|-]?\d+(\.?[0-9]{0,2})?$/;
      if(strValue.match(pattern) == null)
      {
         return false;
      }
      return true;
}
/**************************************************/

function isValidDate(strDate)
{
    if(strDate.match(reDate) == null){
     return false;
  }
  return true;
}


function isValidQuantity(strQty){
     if(strQty =="0"){
        return true;
     }
     
     var tmpstr = strQty;    
     var pattern = /^[1-9]\d{0,9}(.\d{1,3})?$/g;
     if(strQty.indexOf(",") != -1){
       tmpstr = strQty.replace(/,/g,"");
       pattern =/^[1-9]\d{0,2}(,\d{3}){0,3}(.\d{1,3})?$/g;
     }          
    
     var arrstr = tmpstr.split(".");
     if(arrstr[0].length > 7){
         return false;
     }
     
     if(strQty.match(pattern) == null){
         return false;
     }
     return true;
}

function isValidTotalPrice(strTPrice){
     var tmpstr = strTPrice;    
     var pattern = /^[1-9]\d{0,10}(.\d{1,2})?$/g;
     if(strTPrice.indexOf(",") != -1){
       tmpstr = strTPrice.replace(/,/g,"");
       pattern =/^[1-9]\d{0,2}(,\d{3}){0,3}(.\d{1,2})?$/g;
     }          
    
     var arrstr = tmpstr.split(".");
     if(arrstr[0].length > 7){
         return false;
     }
     
     if(strTPrice.match(pattern) == null){
         return false;
     }
     return true; 
}

function isValidTime(strTime)
{
    if(strTime.match(reTime) == null){
     return false;
  }
  return true;
}

//Remove spaces
String.prototype.Trim = function() 
{ 
     return this.replace(/(^\s*)|(\s*$)/g, ""); 
}

 function isDigital(s)
 { 
    var i,c;
    n=s.length;
    for(i=0;i<n;i++)
        { c=s.charAt(i);
        if(c<"0" || c>"9") 
	        return false;
        }
    if(n>=1) 
        return true;
    else 
        return false;
}
//is hour and minute.by gaocaihui add
function isHour(ctime)
{
     var flag = false;
     var re=/^(([0-1][0-9])|(2[0-3])):([0-5][0-9])$/g;
     flag = re.test(ctime);
     return flag;
}


//Change data format into MM/dd/YYYY
function changeDateFormat(strDate)
{
    var arrDate = strDate.split("-");
    return arrDate[1] + "/" + arrDate[2] + "/" + arrDate[0];
}



//Add by gaocaihui.2008-09-26 
//Only input the number character. 
function NumericOnKeyUp(obj)
{
	//Replace the value but number.
	obj.value = obj.value.replace(/[^\d.]/g,"");
	//The first character is number.
	obj.value = obj.value.replace(/^\./g,"");
	//Has only one decimal
	obj.value = obj.value.replace(/\.{2,}/g,".");
	//Has only one decimal
	obj.value = obj.value.replace(".","$#$").replace(/\./g,"").replace("$#$",".");
	if(obj.value == "")
	{
	    obj.value = 0;
	}
}

//add allow to input float.by gaocaihui modify 2009-2-11
function FloatOnkeyDown()
{
    func = FloatOnkeyDown.caller;  // supports firefox
    var evt = func.arguments[0] || window.event;     
    var keyCode = evt.charCode || evt.keyCode;
    
    if(!(keyCode==46)&&!(keyCode==8)&&!(keyCode==9)&&!(keyCode==37)&&!(keyCode==39)&&!(keyCode==190)) 
    {
       if(!((keyCode>=48&&keyCode<=57)||(keyCode>=96&&keyCode<=105)||(evt.ctrlKey&&(keyCode==86||keyCode==67)))) 
       {   
          if (window.event) //IE
          {
               evt.returnValue = false;   //event.returnValue=false.
          }
          else //Firefox
          {
              evt.preventDefault();
          }
       }
    }
}

//by  modify by gaocaihui on 20081115 supports press tab key to move.
function NumericOnkeydown() 
{ 
    func = NumericOnkeydown.caller;  //add on 2008-12-12 supports firefox
    var evt = func.arguments[0] || window.event;     
    var keyCode = evt.charCode || evt.keyCode;
    var value=evt.srcElement.value;
     //允許輸入負號(109,189)
     if((keyCode==109||keyCode==189)&&value==""){
       return true;        
     }
    if(!(keyCode==46)&&!(keyCode==8)&&!(keyCode==9)&&!(keyCode==37)&&!(keyCode==39)) 
    {      
     if(!((keyCode>=48&&keyCode<=57)||(keyCode>=96&&keyCode<=105)||(evt.ctrlKey&&(keyCode==86||keyCode==67))))   //allow ctrl+v.by gaocaihui modify 2009-2-11
       {            
          if (window.event) //IE
            {
                evt.returnValue = false;   //event.returnValue=false.
            }
            else //Firefox
            {
                evt.preventDefault();
            }

       }
    }
    
} 

function NumericOnchange(obj)
{
    if(obj != null)
    {
         if(!isValidIntOrThreeDigitDecimal(obj.value))
         {
            obj.value = 0;
         }
        
       // obj.value = parseFloat(obj.value);
       // if(obj.value == '' || obj.value == 'NaN')
        //{
            
        //}
    }
}


//ADDED BY gaocaihui ON 2008-10-25.
//Discription:Validate input.
function   regInput(obj,   reg,   inputStr)   
{   
  var   docSel =   document.selection.createRange()   
  if   (docSel.parentElement().tagName   !=   "INPUT")
     return   false   
  oSel   =   docSel.duplicate()   
  oSel.text   =   ""   
  var   srcRange =   obj.createTextRange()   
    oSel.setEndPoint("StartToStart",   srcRange)   
  var   str   =   oSel.text   +   inputStr   +   srcRange.text.substr(oSel.text.length)   
  return   reg.test(str)   
}



/**
 * Email address input param judge Format
 * @return true/false
 */
function isValidEmail(s)
 {  var n,i,ErrorChar ,j;
	ErrorChar=" ~!`$%^&*()+=?<>,{}[]\\/|'\"";
	n=s.length;
	if(n<5) return(false);
	for(j=0;j<ErrorChar.length;j++)
	    if(s.indexOf(ErrorChar.charAt(j))>=0) return(false);
    if(s.indexOf("#")>0) return (false);
	if(s.indexOf(".")<0||s.indexOf("@")<0 || s.charAt(0)=="@" || s.charAt(n-1)==".")
	return(false);
	else
	return(true);
 }

//Add '0' if float decimal less than 6 bit.
function addDecimalDigit(str)
{
    if(str.indexOf(".") < 0)
        return str + ".000000";
    else
    {
        var arr = str.split(".");
        var dec = arr[1];
        
        if(dec.length == 6)
            return str;
        else
        {
            for(var i = 0; i < 6 - dec.length; i ++)
            {
                str += "0";
            }
            return str;
        }
    }
}


//Get checked radio value from a group radio control with same name.
function getCheckedRadioVaule(radioName){
    var objRadio = document.getElementsByName(radioName);
    var count = objRadio != null ? objRadio.length : 0;
    var returnValue = "";
    for(var i = 0; i < count; i ++){
      var tmpRadio = objRadio[i];
      if(tmpRadio.checked){
         returnValue = tmpRadio.value;
         break;
      }
    }
    return returnValue;
}


//Check security of the password
function isSafePwd(pwdStr) {
  var level1 = false;
  var level2 = false;
  var level3 = false;
  var re1 = /\d/g;
  var re2 = /[a-zA-Z]/g;
  var re3 = /[!"#$%&'()*+,-./:;<=>?@[\]^_`{|}~]/g;
  
  if(pwdStr.length < 10) {        
    return false;
  } 
  else {
    for(var i=0; i<pwdStr.length; i++) {
      if(pwdStr.substring(i, i+1).match(re1) && !level1) {
        level1 = !level1;
      }
      if(pwdStr.substring(i, i+1).match(re2) && !level2) {
        level2 = !level2;
      }
      if(pwdStr.substring(i, i+1).match(re3) && !level3) {
        level3 = !level3;
      }
    }
    
    if(level1 && level2 && level3) {
        return true;
    }
  }     
  return false;
}

//thousand bit separator.
function SeparatorSplit(num)
{
    if(num==null)
    {
        return "0";
    }
    num  =  num+"";
    if(num.Trim()=="")
    {
        return "0";
    }
    var  re=/(-?\d+)(\d{3})/
    while(re.test(num))
    {
        num=num.replace(re,"$1,$2")  
    }
    return  num;
}

function CommitReflashPage()
{
    try{
        if(window.opener!=null)
        {
            //window.opener.location.href=window.opener.location.href
            if(!window.opener.closed){
                    window.opener.location.href=window.opener.location.href
                }
                //Modify by Jason on 20090409, may be the current was opened from todolist page(not main page).
                if(window.opener.opener != null && !window.opener.opener.closed){
                    window.opener.opener.location.href=window.opener.opener.location.href
                }
        }
        window.opener=null;window.open('','_top');window.top.close();
    }catch(e){
        window.opener=null;window.open('','_top');window.top.close();
        //nothing todo
    }
}
function ReflashPage()
{
    try{
        if(window.opener!=null)
        {
            //window.opener.location.href=window.opener.location.href
            if(!window.opener.closed){
                    window.opener.location.href=window.opener.location.href
                }
                //Modify by Jason on 20090409, may be the current was opened from todolist page(not main page).
                if(window.opener.opener != null && !window.opener.opener.closed){
                    window.opener.opener.location.href=window.opener.opener.location.href
                }
        }
        
    }catch(e){
        
        //nothing todo
    }
}

function SeparatorAssemble(num)
{
    if(num==null||num.Trim()=="")
    {
        return 0;
    }
    var x = num.split(',');
    return parseFloat(x.join(""));
}

//return char
function SeparatorAssemble2(num)
{
    if(num==null||num.Trim()=="")
    {
        return 0;
    }
    var x = num.split(',');
    return x.join("");
}


//Refresh parent page after updating
function refreshOpener(searchButtonId)
{
    if(window.opener!=null)
    {
        var btnSearch=window.opener.document.getElementById(searchButtonId);
        btnSearch.click();
    }
}

function DateTimeString(datess)
{
    var str="";
    if(datess==null || datess=="")
    {
        return str;
    }
    var vYear = datess.getFullYear();
    var vMon = datess.getMonth() + 1;
    var vDay = datess.getDate();
    var vHour = datess.getHours();
    var vMin = datess.getMinutes();
    var vSec = datess.getSeconds();

    str+=vYear+"-";
    str+=(vMon<10 ? "0" + vMon : vMon)+"-";
    str+=(vDay<10 ?  "0"+ vDay : vDay )+" ";
    str+=(vHour<10 ? "0" + vHour : vHour)+":";
    str+=(vMin<10 ? "0" + vMin : vMin)+":";
    str+=(vSec<10 ?  "0"+ vSec : vSec );

    return str;
}



function printPage(elementId)
{
    try
    {
        var gvTbl=document.getElementById(elementId);
        //gvTbl.parentNode.style.pageBreakAfter='always'; //commented out by gaocaihui on 2009-05-06

        var element=document.createElement("thead");
        var node =gvTbl.rows[0].cloneNode(true);
        element.appendChild(node);        
        element.style.display="table-header-group";	
            
        if(gvTbl.hasChildNodes)
        {
	        //gvTbl.childNodes[0].removeChild(gvTbl.childNodes[0].firstChild);
	        gvTbl.insertBefore(element,gvTbl.childNodes[0]);
        }
        
        document.getElementById('btnPrintTop').style.display='none';
		document.getElementById('btnPrintBottom').style.display='none';		
		gvTbl.childNodes[1].firstChild.style.display='none';
           
    }
    catch(e) //indicating FireFox, in FireFox gvTbl.childNodes.length=3
	{
		gvTbl.childNodes[2].firstChild.style.display='none';	
	}
	
	window.print();                      
}



//start by gaocaihui add.2009-3-10
function   isIE(){ 
      if   (window.navigator.userAgent.toString().toLowerCase().indexOf("msie") >=1)
        return   true;
      else
        return   false;
}

if(!isIE()){  
    try{
     HTMLElement.prototype.__defineGetter__("children", 
     function () { 
         var returnValue = new Object(); 
         var number = 0; 
         for (var i=0; i<this.childNodes.length; i++) { 
             if (this.childNodes[i].nodeType == 1) { 
                 returnValue[number] = this.childNodes[i]; 
                 number++; 
             } 
         } 
         returnValue.length = number; 
         return returnValue; 
     } 
     );
     
      
      HTMLElement.prototype.__defineGetter__(           "innerText",
        function(){
          var   anyString   =   "";
          var   childS   =   this.childNodes;
          for(var   i=0;   i <childS.length;   i++)   {
            if(childS[i].nodeType==1)
              anyString   +=   childS[i].tagName=="BR"   ?   '\n'   :   childS[i].innerText;
            else   if(childS[i].nodeType==3)
              anyString   +=   childS[i].nodeValue;
          }
          return   anyString;
        }
      );
      HTMLElement.prototype.__defineSetter__(           "innerText",
        function(sText){
          this.textContent=sText;
        }
      ); 
      
   }
   catch(e)
   { }
} 

//end add.
 
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


//=============只能输入小数=============//anzhiqiang
function DecimalOnkeyDown(_textBox,defaultValue){
    var event = GetEvent();
    var keyChar = String.fromCharCode(event.keyCode);
    var value = _textBox.value.toLowerCase;  
    var a=(event.keyCode>105||event.keyCode<96);    //小鍵盤
    var b=(event.keyCode>57||event.keyCode<48);     //大鍵盤
    var c=(event.keyCode!=190&&event.keyCode!=110); //小數點
    var d=(event.keyCode!=109&&event.keyCode!=189); //減號
    var e=(event.keyCode!=8);                       //退格鍵
    var f=(event.keyCode!=37);                      //←
    var g=(event.keyCode!=39);                      //→  
    var h=(event.keyCode!=9);                       //Tab  
    var k=(event.keyCode!=46);                      //刪除鍵
    var o=(!(event.ctrlKey&&event.keyCode==86));       //Ctrl+V

    if(a&&b&&c&&d&&e&&f&&g&&h&&k&&o)
    {
       return false;
   }
    var pointNum=0;
     var _Num=0;
     for (var i = 0; i < _textBox.value.length; i++) {
        var ch = _textBox.value.charAt(i);
        if(ch=="-"){_Num=_Num+1;}
        if(ch=="."){pointNum=pointNum+1;}
    }
    if(pointNum>0&&(event.keyCode==190||event.keyCode==110)){return false; }
    if(_Num>0&&(event.keyCode==109||event.keyCode==189)){return false; }
    if(
       ((event.keyCode==190||event.keyCode==110)&&( _textBox.value.length==0)) ||
       ((event.keyCode==190||event.keyCode==110)&&( _textBox.value.length==1&&_textBox.value=="-"))       
    ){return false; }

    if((event.keyCode==109||event.keyCode==189)&&_textBox.value.length>0)
    {  
        return false;  
    }
    _textBox.onblur=function(){
        if(this.value==""||(isNaN(this.value))){
          this.value=defaultValue;
        }
    }
}