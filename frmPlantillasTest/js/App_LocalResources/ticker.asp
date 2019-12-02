<style type="text/css">
.tickerFX1{font-family:arial,verdana,helvetica; font-size:7pt; background:#EFEFEF; width:50; cursor:hand; border:1px solid #808080}
.tickerFX2{font-family:arial,verdana,helvetica; font-size:7pt; background:#EFEFEF; width:50; cursor:hand; border:0px solid #FFFFFF}
</style>
<script language="javascript">

window.onerror = null;
 var bName = navigator.appName;
 var bVer = parseInt(navigator.appVersion);
 var NS4 = (bName == "Netscape" && bVer >= 4);
 var IE4 = (bName == "Microsoft Internet Explorer" 
 && bVer >= 4);
 var NS3 = (bName == "Netscape" && bVer < 4);
 var IE3 = (bName == "Microsoft Internet Explorer" 
 && bVer < 4);
 var ii=0;
 var top_pos = 0;
 var left_pos = 0;
 var time_length = 5000;
 var div_name = "qiksearch";
 data_act=new Date ;
 
 var ticker_msg = new Array(
 "22/12/2011 22/12/2011 Dades acadèmiques 2011",
 "22/12/2011 22/12/2011 Actualització de dades",
 "31/12/2011 24/11/2011 Circular fin de protesis 2011",
 "31/12/2011 3/12/2010 Circular Calendari laboral 2011",
 "31/12/2011 3/12/2010 Calendari  any 2011",
 "31/12/2011 3/12/2010 Calendari Horari i Abonament Nomina any 2011",
 "31/12/2011 3/12/2010 Vacances 2011",
 "31/12/2011 3/12/2010 Assumptes personals 2011", 
 "31/12/2011 3/12/2010 Dies de lliure disposició per venciments de triennis");

 var ticker_url = new Array(
 "/RHO/Circulars/DADES ACADEMIQUES 2011.pdf",
 "/RHO/Circulars/Actualització de dades Intranet 2011.pdf",
 "/RHO/Circulars/Circular fin de protesis 2011.pdf",
 "/RHO/Circulars/CircularCALENDARI_LABORAL 2011.pdf",
 "/RHO/Circulars/Calendari Laboral any 2011.pdf",
 "/RHO/Circulars/Calendari Horari i Ab.Nòmina2011.pdf", 
 "/RHO/Circulars/Vacances 2011.pdf", 
 "/RHO/Circulars/ASSUMPTES PERSONALS 2011.pdf", 
 "/RHO/Circulars/Dies lliure disposició2011.pdf");
	 
	
var ticker_len = ticker_msg.length;

for(var l=0; l<ticker_len; l++)
{ 
data_cad=new Date(ticker_msg[l].substr(6,4),(ticker_msg[l].substr(3,2))-1,ticker_msg[l].substr(0,2));
document.write('<div id="' + div_name + l + '" style="position:absolute; visibility:hidden;">' + '<table  cellspacing="0" cellpadding="1" width="500" ><tr><td align="left"><table width="100%" id="ticker_container"><tr>');
document.write('<td align="left"><table  cellspacing="0" cellpadding="0" onclick="tickerFX_listAll(&#39;all_news_div&#39;); " title="Veure tots"><tr><td ><img src="/img/home/b_veure_tots.gif"></td></tr></table></td>');
if (data_cad > data_act) {
	document.write('<td align="left"><a href="' + ticker_url[l] + '" class="txtGris9px" >' + "[ " + ticker_msg[l].substr(11) + " ]"+ '</a></td>');
}
document.write('</tr></table></td></tr></table>' + '</div>');
}

 document.write('<div id="all_news_div" style="position:absolute;  visibility:hidden" >');
 document.write('<table width="400" align="center" style="background:#FFFFDD; border:1px solid #000000; border-top-width:0" ><tr ><td>');
 for(var l=0; l<ticker_len; l++)
 {
  	data_cad=new Date(ticker_msg[l].substr(6,4),(ticker_msg[l].substr(3,2))-1,ticker_msg[l].substr(0,2));
	if (data_cad > data_act){

  document.write('<b><font face="arial,helvetica">&raquo;</font></b> <a href="' + ticker_url[l] + '" class="txtGris9px">' + ticker_msg[l].substr(11) + '</a><br>'); 
 }
 }
  document.write('<center><table  cellspacing="0" cellpadding="0" onclick="tickerFX_listAll(&#39;all_news_div&#39;); " title="Tancar"><tr><td align="middle"><img src="/img/home/b_tancar.gif"></td></tr></table></center>');

 document.write('</td></tr></table>');
 document.write('</div>');

if (NS4 || IE4) {
 if (navigator.appName == "Netscape") {
 layerStyleRef="layer.";
 layerRef="document.layers";
 styleSwitch="";
 }else{
 layerStyleRef="layer.style.";
 layerRef="document.all";
 styleSwitch=".style";
 }
}

//SCROLL
function tick(){
if (NS4 || IE4) {
 data_cad=new Date(ticker_msg[ii].substr(6,4),(ticker_msg[ii].substr(3,2))-1,ticker_msg[ii].substr(0,2));

 if(ii<ticker_len)
 {
  if(ii==0)
  { 
   eval(layerRef+'["'+div_name+(ticker_len-1)+'"]'+
   styleSwitch+'.visibility="hidden"');
  }
  if(ii>0)
  { 
   eval(layerRef+'["'+div_name+(ii-1)+'"]'+
   styleSwitch+'.visibility="hidden"');
  }
 eval(layerRef+'["'+div_name+ii+'"]'+
 styleSwitch+'.visibility="visible"');
 }
 if(ii<ticker_len-1)
 {
  ii++;
 }
 else
 {
  ii=0;
 }
 if (data_cad > data_act)
 	setTimeout("tick()",time_length);
 else
  	setTimeout("tick()",0);
 }
}

var list_news_flag=0;
var change_speed_flag=0;

function tickerFX_listAll(layerName)
{
 if(list_news_flag==0)
 {
  list_news_flag=1;
  if(NS4 || IE4)
  {
   eval(layerRef+'["'+layerName+'"]'+
   styleSwitch+'.visibility="visible"');
  }
 }
 else
 {
  list_news_flag=0;
  if(NS4 || IE4)
  {	
   eval(layerRef+'["'+layerName+'"]'+
   styleSwitch+'.visibility="hidden"');
  }
 }
}
tick();
</script>