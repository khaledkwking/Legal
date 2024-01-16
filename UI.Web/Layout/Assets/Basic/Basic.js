function input_filterAmt (str, dec, bNeg)
{ // auto-correct input - force numeric data based on params.
/********Arguments Description ******************************* 
    1- str: the value of the textbox being validated
    2- dec: the number of decimals you want to keep ex: 2 will return 0.00
    3- bNeg: Is Negative allowed or not ... 1 for yes and 0 for no
 ********************************************************************/
if(str=="")
{
  //  new $.Zebra_Dialog("NOTHING RETURNING NOTHING");
    return ""
}
//else
//    new $.Zebra_Dialog("NOT NOTHING IT's: "+str);
 var cDec = '.'; // decimal point symbol
 var bDec = false; var val = "";
 var strf = ""; var neg = ""; var i = 0;
 if (str == "") return;
 parseFloat ("0").toFixed (dec);
 if (bNeg && str.charAt (i) == '-') { neg = '-'; i++; }
 for (i; i < str.length; i++)
 {
  val = str.charAt (i);
  if (val == cDec)
  {
   if (!bDec) { strf += val; bDec = true; }
  }
  else if (val >= '0' && val <= '9')
   strf += val;
 }
 strf = (strf == "" ? 0 : neg + strf);
 var out =parseFloat (strf).toFixed (dec);
 if(out)
    return out
else
    return 0
} 
function InsertText(input, insTexte)
{
    // inserts a text at the cusror's position on a textbox
    startTag = '';
    endTag = '';
     if (input.createTextRange)
     {
      var text;
      input.focus(input.caretPos);
      input.caretPos = document.selection.createRange().duplicate();
      if(input.caretPos.text.length>0)
      {
       input.caretPos.text = startTag + input.caretPos.text + endTag;
      }
      else
      {
       input.caretPos.text = startTag + " " + insTexte + " " + endTag;
      }
     }
     else input.value += startTag + insTexte + endTag;
}

function replaceAll(strOld, strNew, strSource)
{
    while(strSource.indexOf(strOld)!= -1)
    {
        strSource = strSource.replace(strOld,strNew);
    }
    return strSource;
}
function getObjById(id)
{
    for(var i = 0; i < document.forms[0].elements.length; i++) 
    {
        elm = document.forms[0].elements[i]
        if (elm.id.indexOf(id)!=-1) 
        {
            return elm;
        }
    }
    return null;
}

fmtMoney = function( n, c, d, t ) {
    if(n == "0")
        return "0.000";
      
	var m = ( c = Math.abs( c ) + 1 ? c : 2, d = d || ",", t = t || ".", /(\d+)(?:(\.\d+)|)/.exec( n + "" ) ), x = m[1].length % 3;
	//new $.Zebra_Dialog("M: "+m);
	var out= ( x ? m[1].substr( 0, x ) + t : "" ) + m[1].substr( x ).replace( /(\d{3})(?=\d)/g, "$1" + t ) + ( c ? d + ( +m[2] ).toFixed( c ).substr( 2 ) : "" );
	//alert("OUT: "+out);
	var str =out+"";
	
	var start = str.substr(0,str.indexOf("."));
	var af = str.substr(str.indexOf(".")+1);
	//alert("STRING: "+str+" & START: "+start+" and AFTER: "+af);
	if(start.lastIndexOf(",")==(start.length-1))
	    start=start.substr(0,start.length-1);
	if(af == "N")
	    af="000";
	return start+"."+af;
};
function formatCurrency(strValue)
{
    var ad = ""
    //alert("VALUE: "+strValue+" and - is: "+strValue.indexOf("-"));
    if(strValue.indexOf("-") == 0)
    {
        ad = "-";
    }
    
	strValue = strValue.toString().replace(/\$|\,/g,'');
	
	dblValue = parseFloat(Math.abs(strValue));
	//alert("NEW STRING: "+dblValue+ " and AD: "+ad);
    return ad+fmtMoney(dblValue, 3, '.', ',' );

//	blnSign = (dblValue == (dblValue = Math.abs(dblValue)));
//	dblValue = Math.floor(dblValue*1000+0.50000000001);
//	intCents = dblValue%1000;
//	strCents = intCents.toString();
//	dblValue = Math.floor(dblValue/1000).toString();
//	if(intCents<10)
//		strCents = "0" + strCents;
//	for (var i = 0; i < Math.floor((dblValue.length-(1+i))/3); i++)
//		dblValue = dblValue.substring(0,dblValue.length-(4*i+3))+','+
//		dblValue.substring(dblValue.length-(4*i+3));
//	return (((blnSign)?'':'') + ' ' + dblValue + '.' + strCents);
}

function chkPrice(id)
{
    var txt = document.getElementById(id);
    //alert("TXT: "+txt);
    if(!txt)
        return false;
    //alert("PARSE: "+parseFloat(txt.value));
    if(!parseFloat(txt.value) && parseFloat(txt.value) != 0)
    {
        //alert("GETTING OUT");
        return false;
    }
    var f = parseFloat(txt.value);
    //alert("F: "+f);
    if(f<0)
        return false;
    txt.value=f;
    return true;
}

function chkPriceObj(txt)
{
    //alert("TXT: "+txt);
    if(!txt)
        return false;
    //alert("PARSE: "+parseFloat(txt.value));
    if(!parseFloat(txt.value) && parseFloat(txt.value) != 0)
    {
        //alert("GETTING OUT");
        return false;
    }
    var f = parseFloat(txt.value);
    //alert("F: "+f);
    if(f<0)
        return false;
    txt.value=f;
    return true;
}
function listen(evnt, elem, func) 
{
    if (elem.addEventListener) // W3C DOM
        elem.addEventListener(evnt,func,false);

    else if (elem.attachEvent) 
    { // IE DOM
        var r = elem.attachEvent("on"+evnt, func);
        return r;
    }
    else 
        window.alert('I\'m sorry Dave, I\'m afraid I can\'t do that.');
}
 function ShowInternalPopup()
    {
        //alert("Show Pop Up");
        if(document.getElementById("ctl00_ShowInternalMessage1_btnTriger"))
        {
        //var btn =document.getElementById("ctl00_ShowInternalMessage1_btnTriger");
            //document.getElementById("ctl00_ShowInternalMessage1_btnTriger").click();
            setTimeout("CallPopup();",3000);
        }
        //alert("BUTTON: "+btn);
        //btn.click();
        //$get('btnTriger').click();
        
    }
    function CallPopup()
    {
        document.getElementById("ctl00_ShowInternalMessage1_btnTriger").click();
    }
listen("load",window,ShowInternalPopup);

var plus = new Image();
var minus = new Image();
var tab1 = new Image();
var tab2 = new Image();
var tab3 = new Image();
var tab4 = new Image();
function preloadImages()
{
    //alert("PRE");
    try
    {
        LoadShow();
    }
    catch(ex)
    {}
    plus.src = "/Layout/Assets/Basic/plus.gif";
    minus.src = "/Layout/Assets/Basic/minus.gif";
    tab1.src="styles/tableft10.gif";
    tab2.src="styles/tableftJ.gif";
    tab3.src="styles/tabright10.gif";
    tab4.src="styles/tabrightJ.gif";
}
//listen("load",window,);
preloadImages();
function ToggleWindow(img,pnl)
{
    var p = document.getElementById(pnl);
    var i = document.getElementById(img);
    if( p.style.display == "none")
    {
        i.src=minus.src;
        p.style.display="";
    }
    else
    {
        i.src=plus.src;
        p.style.display="none";
    }
   
}

function isNumberKey(evt)
{
    var charCode = (evt.which) ? evt.which : event.keyCode
        if(charCode=46)
         return true;
    
    
    if (charCode > 31 && (charCode < 48 || charCode > 57))
    return false;

    return true;
}
function toggleDiv(divid,img)
{
    //var img = document.getElementById(imgid);
    var div = document.getElementById(divid);
    if( div.style.display=="" )
    {
        div.style.display="none";
        img.src=plus.src;
    }
    else
    {
        div.style.display="";
        img.src=minus.src;
    }
}