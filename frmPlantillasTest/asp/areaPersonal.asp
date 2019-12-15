<% 

response.redirect("../inici.aspx")
		
Response.Expires = -1 
IF Request.QueryString("desconectar")=1 THEN
	Session("login")=FALSE
	END IF
%>
<html>
<head>

<title>Intranet de l'Ajuntament de L'Hospitalet</title>

<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1">
<link rel="stylesheet" href="Styles/intranet.css" type="text/css">
<link rel="stylesheet" href="Styles/gaiaIntranet.css" type="text/css">
<script type="text/javascript">
<!--//
function sizeFrame() {
var mG = document.getElementById("menuGAIA");
if(mG.contentDocument) {
mG.height = mG.contentDocument.documentElement.scrollHeight; //FF 3.0.11, Opera 9.63, and Chrome
} else {
mG.height = mG.contentWindow.document.body.scrollHeight+20; //IE6, IE7 and Chrome
}

var mE = document.getElementById("menuEleccions");
if(mE.contentDocument) {
mE.height = mE.contentDocument.documentElement.scrollHeight; //FF 3.0.11, Opera 9.63, and Chrome
} else {
mE.height = mE.contentWindow.document.body.scrollHeight+20; //IE6, IE7 and Chrome
}



}
window.onload=sizeFrame; 
//-->
</script>
</head>
<body bgcolor="#FFFFFF" text="#000000" leftmargin="10" topmargin="10" >
   
<!--#INCLUDE VIRTUAL="../js/App_LocalResources/cap.aspx" -->
<p> 
 <script language="JavaScript">

//<% 'function SacarAlert()
//' {
//'alert('ATENCIÓ: Els marcatges que es mostren en aquesta opció són correctes, però el càlcul del saldo horari pot ser incorrecte ja que està en fase de prova');
//'}%>
  function valida() {
	var error, cont;
	error="";
	cont=0;

	if (document.form1.primerPin.value=="") {
		error+="\n+ PIN";
		cont++;
	}
	if (document.form1.pin2.value=="") {
		error+="\n+ PIN2";
		cont++;
	}
	if (document.form1.any.value=="") {
		error+="\n+ any";
		cont++;
	}
	if (document.form1.dni.value=="") {
		error+="\n+ NIF";
		cont++;
	}
	if (error!="") {		
		if (cont==1){
			window.alert("El següent camp és obligatori:" + error);
		}
		else {
			
			
			window.alert("Els següents camps són obligatoris:" + error);
		}
		return false;
	}
	else {
		if (document.form1.primerPin.value !=  document.form1.pin2.value) {
			window.alert("El segon PIN no coincideix amb el primer. Si us plau, torni a introduïr el número secret personal. ");
			return false;
		}
		if (document.form1.primerPin.value.length<4) {
			window.alert("El PIN ha de tenir una longitud de 4 xifres.");
			return false;
		}
		primerPin = document.form1.primerPin.value;
	
		if ( (primerPin.charAt(0)==primerPin.charAt(1)) & (primerPin.charAt(1)==primerPin.charAt(2)) & (primerPin.charAt(2)==primerPin.charAt(3)) ) {
			window.alert("El PIN no pot tenir totes les xifres iguals.");			
			return false;
		}
		return true;
	}
}
//-->
</script>
<!--#include VIRTUAL="~/constants.inc"-->
<!--#include VIRTUAL="~/bd2.inc"-->
<%
'***********************************************************************
' Comprovo si l'usuari ha fet login => session("login")=TRUE
'***********************************************************************

IF len(Request("primerPin"))>0 and len(Request("dni"))>0  THEN	
	' Comprovo  si és una sol·licitud de primer pin	

		Call Inicialitzar
		iexpr="SELECT PERCDNIF,PERDTNAI FROM PE.PETLPER WHERE PERCDNIF='"& Trim(UCase(Request("dni"))) &"' AND PERDSUSU like '"& Session("usuw2k") &"' WITH UR "
		rc = bd.Consultar (iexpr,idCons)
		IF  idCons.EOF THEN%>
	<table width="100%" border="0" cellspacing="0" cellpadding="0">
              <tr> 
                <td bgcolor="#990000" class="txtBlanco14px">
				<b>Error: Les dades de comprovació són errònies</b></td>
              </tr>
            </table>
	<% 	ELSE
			IF Left(idCons("PERDTNAI"),4)<>Request("any") THEN
	%>
<table width="100%" border="0" cellspacing="0" cellpadding="0">
   	         <tr> 
                <td bgcolor="#990000" class="txtBlanco14px">
				<b>Error: Les dades de comprovació són errònies</b></td>
              </tr>
            </table>
             
<%		    ELSE 
				Set DataConn = Server.CreateObject("ADODB.Connection")
				DataConn.ConnectionTimeout = 15
				DataConn.CommandTimeout = 30
				DataConn.Open "CpdLocalServer", "CPDsa", "sa12roKa"	
				Set cmdDC = Server.CreateObject("ADODB.Command")	
				cmdDC.ActiveConnection = DataConn						
				cmdDC.CommandText = "SELECT * FROM INTTLPIN WHERE INTINNIF ='" & Session("dni") & "' "
				cmdDC.CommandType = 1
				Set rsDC = Server.CreateObject("ADODB.Recordset")
				rsDC.Open cmdDC, , 3, 3
				IF rsDC.EOF THEN	
					cmdDC.CommandText = "INSERT INTO INTTLPIN VALUES ('" & Session("dni") & "','" & Request("primerPin") & "','" & Now & "','"  & Now & "',0,'"&Request.ServerVariables("REMOTE_ADDR")&"')"
					cmdDC.CommandType = 1
					Set rsDC = Server.CreateObject("ADODB.Recordset")
					rsDC.Open cmdDC, , 3, 3
				END IF
			'Response.Write cmdDC.CommandText
				Session("ultimAccess")=now
				Session("ultimcanviPIN")=now
				Session("login")=TRUE
		END IF
 	END IF

END IF

'Comprovo si el PIN introduït és correcte
IF len(Request("pin"))>0 THEN
		Set DataConn = Server.CreateObject("ADODB.Connection")
		DataConn.ConnectionTimeout = 15
		DataConn.CommandTimeout = 30
		DataConn.Open "CpdLocalServer", "CPDsa", "sa12roKa"
		Set cmdDC = Server.CreateObject("ADODB.Command")

		cmdDC.ActiveConnection = DataConn
		cmdDC.CommandText = "SELECT * FROM INTTLPIN WHERE INTINNIF ='" & Session("dni") & "' "
		cmdDC.CommandType = 1
		Set rsDC = Server.CreateObject("ADODB.Recordset")
		rsDC.Open cmdDC, , 3, 3
    	IF  NOT rsDC.EOF THEN
			IF rsDC.Fields("INTCDBLO")>=3 THEN
			%>
			<table width="100%" border="0" cellspacing="0" cellpadding="0">
              <tr> 
                <td bgcolor="#990000" class="txtBlanco14px">
				<b>Error. Hi ha massa intents erronis. Si us plau, truqueu al 9555 per resoldre la incidència.</b></td>
              </tr>
            </table>
			
<%				
			ELSE
				IF rsDC.Fields("INTCDPIN")=Request("pin") THEN
					Session("login")=True
					Session("ultimAccess")=rsDC.Fields("INTDTACC")
					Session("ultimcanviPIN")=rsDC.Fields("INTDTMOD")
					cmdDC.CommandText = "UPDATE INTTLPIN SET INTDTACC ='" & Now & "', INTCDBLO=0, INTDSADR='"&Request.ServerVariables("REMOTE_ADDR")&"' WHERE INTINNIF ='" & Session("dni") & "'"
					cmdDC.CommandType = 1
					Set rsDC = Server.CreateObject("ADODB.Recordset")
					rsDC.Open cmdDC, , 3, 3
				ELSE
					cmdDC.CommandText = "UPDATE INTTLPIN SET INTCDBLO =" & CInt(rsDC.Fields("INTCDBLO"))+1 & ", INTDSADR='"&Request.ServerVariables("REMOTE_ADDR")&"' WHERE INTINNIF ='" & Session("dni") & "'"
					cmdDC.CommandType = 1
					Set rsDC = Server.CreateObject("ADODB.Recordset")
					rsDC.Open cmdDC, , 3, 3
%>
	<table width="100%" border="0" cellspacing="0" cellpadding="0">
              <tr> 
                <td bgcolor="#990000" class="txtBlanco14px">
				<b>Error. El PIN no és vàlid. Si us plau, torni a introduir el PIN.</b></td>
              </tr>
            </table>
					
			<%
				END IF
			END IF
		END IF
END IF
'Response.Write Request.ServerVariables("LOGON_USER")

IF Session("login")<>True THEN
	
	'no ha fet login 
'***********************************************************************
' Vaig a buscar l'usuari a personal amb les dades de w2k.
'***********************************************************************
	IF len(Session("usuw2k"))=0 THEN
		Dim nom, descNom, diesCaducitat
		Session.TimeOut = 1000

		IF len(Request.ServerVariables("LOGON_USER"))<>0 THEN
			
			IF UCase(left(Request.ServerVariables("LOGON_USER"),2)) = "LH"   THEN			
				ADpath = "WinNT://" & Replace(Request.ServerVariables("LOGON_USER"),"\","/")& ",user"
			ELSE	
				ADpath = "WinNT://LH/" & Replace(Request.ServerVariables("LOGON_USER"),"\","/")& ",user"			
			END IF
			
			Set myDomain = GetObject(ADpath)
			diesCaducitat=datediff("d",now,myDomain.PasswordExpirationDate)
			usuari =  Request.ServerVariables("LOGON_USER")
			nom= myDomain.FullName
			descNom = myDomain.Description
			pos = InStr(Trim(usuari),"\")+1
			usuw2k = lcase(Mid(Trim(usuari),pos,len(Trim(usuari))-pos+1))
			
			Session("usuw2k")=usuw2k
			Session("usuari")=nom
			Session("diesCaducitat")=diesCaducitat			
			Session("dni")=""
			Session("empresa")=""
			'Response.Write "NOM="&nom&"USUW2k="&usuw2k	
		ELSE	

			Session("usuw2k")=""
			Session("usuari")=""
			Session("diesCaducitat")=""
			Session("dni")=""
			Session("empresa")=""
			%>
			<table width="100%" border="0" cellspacing="0" cellpadding="0">
              <tr> 
                <td bgcolor="#990000" class="txtBlanco14px"> <b> Error : Usuari 
                  no trobat. Si us plau, truqueu al 9555 per resoldre la incid&egrave;ncia.</b><%=Request.ServerVariables("LOGON_USER")%></td>
              </tr>
            </table>
<%
		END IF		  
	END IF
	IF len(Session("dni"))=0 and len(Session("usuw2k"))>0 THEN
		Call Inicialitzar
		usuari=Replace(session("usuw2k"),"/","\")
		pos = InStr(Trim(usuari),"\")+1
		usuw2k = Mid(Trim(usuari),pos,len(Trim(usuari))-pos+1)

		iexpr="SELECT PERCDPSN, PERINPER, PERCDEMP, PERDSEML,PERCDNIF,PERDSNOM,PERCDFIT FROM PE.PETLPER WHERE PERDSUSU like '"& usuw2k &"' ORDER BY PERCDEMP ASC WITH UR "
		rc = bd.Consultar(iexpr,idCons)
		dni=""
		IF  idCons.EOF THEN					
%>
		<table width="100%" border="0" cellspacing="0" cellpadding="0">
              <tr> 
                <td bgcolor="#990000" class="txtBlanco14px"> <b> Error : Usuari 
                  no trobat. Si us plau, truqueu al 9555 per resoldre la incid&egrave;ncia.<%=Request.ServerVariables("LOGON_USER")%></b></td>
              </tr>
            </table>
    	         
<%		ELSE
			Session("email")=Trim(idcons("PERDSEML"))
			Session("PERDSNOM")= Trim(idcons("PERDSNOM"))
			Session("dni") = Trim(idcons("PERCDNIF"))		
			Session("empresa")=Trim(idcons("PERCDEMP"))
			Session("PERINPER")=Trim(idcons("PERINPER"))
			Session("PERCDPSN")=Trim(idcons("PERCDPSN"))
			Session("PERCDFIT")=Trim(idcons("PERCDFIT"))
			'Response.Write(Session("empresa"))
		END IF
	END IF
'***********************************************************************
'  Tinc el usuari identificat. Ara vaig a buscar el pin	
'***********************************************************************

	IF len(Session("dni"))>0 THEN
'***********************************************************************
' Comprovo si hi ha pin per aquest usuari
'***********************************************************************		
		Set DataConn = Server.CreateObject("ADODB.Connection")
		DataConn.ConnectionTimeout = 15
		DataConn.CommandTimeout = 30
		DataConn.Open "CpdLocalServer", "CPDsa", "sa12roKa"
		Set cmdDC = Server.CreateObject("ADODB.Command")
		cmdDC.ActiveConnection = DataConn
		cmdDC.CommandText = "SELECT * FROM INTTLPIN WHERE INTINNIF ='" & Session("dni") & "'"
		cmdDC.CommandType = 1
		Set rsDC = Server.CreateObject("ADODB.Recordset")
		rsDC.Open cmdDC, , 3, 3
    	IF  rsDC.EOF THEN
			'demano nou pin+any naixement + dni
			%>
              <br>
              <span class="txtNeg14px">Hola&nbsp;<span class="txtRojo14px"><%IF len(Session("PERDSNOM"))>0 THEN
			  Response.Write UCase(Left(Trim(Session("PERDSNOM")),1))  & LCase(Mid(Session("PERDSNOM"),2,len(Session("PERDSNOM"))-1))
			  ' LCase(Mid(Session("PERDSNOM"),2,len(Session("PERDSNOM"))-1))
			  END IF%>. 
              </span> Aquest &eacute;s el teu</span>&nbsp;<span class="txtRojo14px"><b>Espai 
              personal</b></span><br>
              <span class="txtNeg12px"><br>
            Necessitem que introdueixis un nou número secret personal (PIN) de 
            quatre xifres.<br>
            Aquest número s'ha d'introduir ara per primer cop i serà necessari 
            que el recordis pels propers accessos al teu Espai Personal.<br>
            A l'espai personal trobar&agrave;s dades privades i per tant &eacute;s 
            molt important que ning&uacute; m&eacute;s conegui el teu PIN.</span><span class="txtNeg12px"><br>
            Igualment has d'introduir el teu NIF i l'any de naixement per validar 
            la teva identitat i poder assignar-li el PIN escollit.</span><span class="txtNeg12px"><br>
              <br>
              Nota: el PIN no pot tenir les quatre xifres iguals.</span>
<form name="form1" method="post" action="">
              <div align="right"> 
                <table width="300" border="0" cellspacing="0" cellpadding="0" align="center">
                  <tr bgcolor="#CCCCCC"> 
                    <td height="1"  width="196"></td>
                    <td height="1"  width="104"></td>
                  </tr>
                  <tr bgcolor="#FDE3D7"> 
                    <td height="15" class="txtRojo24px" width="196"> 
                      <div align="right" class="txtNeg12px"> N&uacute;mero secret 
                        personal (PIN)&nbsp;&nbsp;</div>
                    </td>
                    <td height="15" class="txtRojo24px" width="104"> 
                      <input type="password" name="primerPin" maxlength="4" size="4" class="inputTextrojo12px">
                    </td>
                  </tr>
                  <tr bgcolor="#FDE3D7"> 
                    <td width="196" bgcolor="#FDE3D7"> 
                      <div align="right" class="txtNeg12px">Repetir n&uacute;mero&nbsp;(PIN2)&nbsp; 
                      </div>
                    </td>
                    <td width="104"> 
                      <input type="password" name="pin2" maxlength="4" size="4" class="inputTextrojo12px">
                    </td>
                  </tr>
                  <tr bgcolor="#CCCCCC"> 
                    <td height="1"  width="196"></td>
                    <td height="1"  width="104"></td>
                  </tr>
                  <tr bgcolor="#FEF2ED"> 
                    <td width="196" bgcolor="#FEF2ED"> 
                      <div align="right" class="txtNeg12px">NIF&nbsp;&nbsp;</div>
                    </td>
                    <td width="104"> 
                      <input type="text" name="dni" maxlength="9" size="9" class="inputTextrojo12px">
                    </td>
                  </tr>
                  <tr bgcolor="#FEF2ED"> 
                    <td width="196"> 
                      <div align="right" class="txtNeg12px">Any naixement&nbsp;&nbsp;</div>
                    </td>
                    <td width="104"> 
                      <input type="text" name="any" maxlength="4" size="4" class="inputTextrojo12px">
                    </td>
                  </tr>
                  <tr bgcolor="#CCCCCC"> 
                    <td height="1"  width="196"></td>
                    <td height="1"  width="104"></td>
                  </tr>
                  <tr> 
                    <td colspan="2"> 
                      <div align="center"> 
                        <input type="submit" name="Acceptar2" value="Acceptar" onClick="if (valida()==false) return false;">
                      </div>
                    </td>
                  </tr>
                  <tr> 
                    <td height="37" width="196">&nbsp; </td>
                    <td height="37" width="104">&nbsp;</td>
                  </tr>
                </table>
              </div>
            </form>
            <%
			
		ELSE
			'demano pin actual	
					
			
%>
            <form name="form1" method="post" action="">
              <br>
              <br>
			  
              <table width="300" border="0" cellspacing="0" cellpadding="0" align="center" height="129">
                <tr bgcolor="#FDE3D7"> 
                  <td height="15" class="txtRojo24px" colspan="3"> 
                    <div align="center"> Introduir PIN</div>
                  </td>
                </tr>
                <tr> 
                  <td height="1" bgcolor="#FDE3D7" width="85"></td>
                  <td bgcolor="#CCCCCC" height="1" width="1"></td>
                  <td bgcolor="#CCCCCC" height="1" width="210"></td>
                </tr>
                <tr> 
                  <td bgcolor="#FDE3D7" class="txtRojo14px" height="26" width="85"> 
                    <div align="right">Usuari: </div>
                  </td>
                  <td bgcolor="#CCCCCC" class="txtRojo14px" height="26" width="1"></td>
                  <td bgcolor="#FEF2ED" class="txtRojo14px" height="26" width="210">&nbsp;&nbsp;&nbsp;<%=Session("usuari")%> 
                  </td>
                </tr>
                <tr> 
                  <td bgcolor="#FDE3D7" class="txtRojo14px" width="85"> 
                    <div align="right">PIN: </div>
                  </td>
                    
                  <td bgcolor="#CCCCCC" class="txtRojo14px" height="28" width="1"></td>
                  <td bgcolor="#FEF2ED" width="210"> &nbsp;&nbsp; 
                    <input type="password" name="pin" maxlength="4" size="4" class="inputTextrojo12px">
                    <input type="submit" name="Acceptar" value="Acceptar">
                  </td>
                </tr>
                <tr> 
                  <td height="37" colspan="3"><span class="txtNeg12px">+ <a href="/areaPersonal/enviaPIN.asp" class="txtRojo12px">Recordar 
                    PIN</a></span><br>
                    <div align="right"></div>
                  </td>
                </tr>
              </table><script>
			  document.form1.pin.focus();
			  </script>
			  <% if trim(Session("diesCaducitat")) <> "" then %>

			  	<br>
			  	<% if CInt(Session("diesCaducitat")) > 10 then %>
					<span class="txtGris12px">
				<% else %>
					<span class="txtRojo14px">		
				<% end if %>
                Avís: La teva contrasenya de la xarxa caducar&agrave; dintre de <%=Session("diesCaducitat")%> dies.<br>
                <a href="/iisadmpwd/aexp3.asp?" title="Canvi de contrasenya">Prem aqu&iacute; per canviar-la</a></span>
				<br>
			  <% end if %>
              <br>
              <span class="txtGris12px">Nota: Si desitges canviar d'usuari/a has 
              de canviar l'usuari/a amb el/la que has entrat a la xarxa.</span> <span class="txtGris12px"> 
              Per això cal anar a l'opció de Windows: <i>Inicio -> Apagar -> Cerrar 
              sesion.</i></span> 
            </form>
            <%			
		END IF
	END IF	
ELSE

'     UPDATE en PERSONAL del campo PERDTUCI con fecha actual
'     ====================================================== 

'maguilar = "06969214F"

'if UCase(Session("dni")) = maguilar then 
	Call Inicialitzar
	iexpr="UPDATE PE.PETLPER  SET PERDTUCI = CURRENT_DATE  WHERE PERCDNIF  ='"& Trim(UCase(Session("dni"))) &"' "
	'Response.write("UPDATE PE.PETLPER  SET PERDTUCI = CURRENT_DATE  WHERE PERCDNIF  ='"& UCase(Session("dni")) &"'")
	rc = bd.Consultar (iexpr,idCons)
	
'end if
%>
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
              <tr> 
                <td width="10" valign="top" height="2"> 
                  <table width="100%" border="0" cellspacing="0" cellpadding="0" bgcolor="ededed">
                    <tr> 
                      <td rowspan="2" width="4" height="44">&nbsp;</td>
                      <td height="15" colspan="2"> 
                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                          <tr> 
                            <td><a href="../home.asp" class="txtGris_Path10px">Home</a><img src="../img/common/g_bullet_path.gif" width="11" height="8" align="absbottom"><span class="txtGris_Path10px">Espai 
                              Personal de <%=Session("usuari")%></span> </td>
                          </tr>
                        </table>                      </td>
                      <td height="30" valign="bottom" rowspan="3" <% IF dni<>"" then%>bgcolor="#FFFFFF"<% END IF%> width="60" > 
                        <% IF Session("dni")<>"" THEN%>
                        <div align="right"><a href="/asp/obrefotopersonal.asp?f=<%=Session("dni")%>" target="_blank"	><img src="/asp/obrefotopersonal.asp?f=<%=Session("dni")%>" height="60" border="0"> 
                          </a> 
                          <%END IF%>
                        </div>                      </td>
                    </tr>
                    <tr> 
                      <td class="txtRojo_Titulo_PagInterior18px" height="1" width="513">Espai 
                        Personal de <%=Session("usuari")%> </td>
                      <td class="txtRojo14px" width="187">
					  Codi CIPI: <%=Session("PERINPER")%>
					  <% IF Session("PERCDPSN")>=51100 AND Session("PERCDPSN")<=51197  THEN %>Codi CIPI: <%=Session("PERINPER")%><% END IF %></td>
                    </tr>
                    <tr bgcolor="#FEF2ED"><td></td>

                      <td  height="16" colspan="2" > 
                        <table width="700" border="0" cellspacing="0" cellpadding="0">
  <tr>
                            <td width="195"><span class="txtNegro_Anclas11px">&Uacute;ltim acc&eacute;s:&nbsp;<%=Trim(Session("UltimAccess"))%></span></td>
								  <td width="11">&nbsp;</td>
                            <td width="221"><span class="txtNegro_Anclas11px">&nbsp;&nbsp;&nbsp;Últim 
                                canvi PIN: <%=Session("UltimCanviPIN")%></span></td> <td width="10">&nbsp;</td>
    <td width="179" class="txtNegro_Anclas11px"><%
	
	Set DataConn = Server.CreateObject("ADODB.Connection")
				DataConn.ConnectionTimeout = 15
				DataConn.CommandTimeout = 30
				DataConn.Open "CpdLocalServer", "CPDsa", "sa12roKa"	
				Set cmdDC = Server.CreateObject("ADODB.Command")	
				cmdDC.ActiveConnection = DataConn						
				cmdDC.CommandText = "SELECT count(*) as total FROM INTTLPIN WHERE INTDTACC>'" & Day(now)&"/"&Month(now)&"/"&Year(now)&"'"
				cmdDC.CommandType = 1
				Set rsDC = Server.CreateObject("ADODB.Recordset")
				rsDC.Open cmdDC, , 3, 3
				IF NOT rsDC.EOF THEN
				%>
    Persones connectades avui: <span class="txtRojo_Titulo_PagInterior12px"><%=rsDC.Fields("total")%></span>
<%				END IF%>	

    </td>
   <td width="84" valign="top"><a href="/areaPersonal/canviaPin.asp" class="txtNegro_Anclas11px">Canviar PIN</a></td>
  </tr>
</table>  </td>
                    </tr>
                  </table>
                </td>
              </tr>
              <tr> 
                <td  valign="top"> 
                  <table width="100%" border="0" cellspacing="0" cellpadding="0">
				  
                    <tr> 
                      <td width="4">&nbsp;</td>
                      <td  class="txtGrisCL_Copy_Interior11px">                 
					  <!-- #INCLUDE VIRTUAL="/asp/menuaplicacions2.asp" -->
					  <% 
					   Call InicialitzarDB2E
					   dni=Trim(UCase(Session("dni")))
					   if len(dni)=10 THEN
							dni="0"&dni
END IF

if len(dni)=9 THEN
	dni="00"&dni
END IF
if len(dni)=8 THEN
	dni="000"&dni
END IF
 
 		iexpr="SELECT * FROM SC.SCTLCER WHERE CERINCER ='2SOL2015" &  dni &"' "
		rc = bd.Consultar (iexpr,idCons)
		if rc then
			IF  not idCons.EOF THEN %>
							<table width="700" border="0" cellspacing="0" cellpadding="0">
				<tr>
    <td width="488"><img src="../img/common/g_bullet_flecha_interiores.gif" width="6" height="3" align="absmiddle" border="0"><a href="/areapersonal/duplicatIRPFTSOL.aspx"  class="txtRojo_Indice_Interior11px" target="_blank"> <img src="/img/common/iconografia/ico_pdf.png" width="18" height="18" border="0" align="absmiddle"> Certificat
          Treballadors Públics Solidaris. (2015)</a>
	
</td>                      
						 </tr> 
						 
						 </table>
        <%END IF
		END IF %> <br>
                     <table width="100%"  border="0">
  <tr>
    <td  colspan="2">
			<table width="100%" border="0" cellspacing="0" cellpadding="0">
				<tr> 
					<td><span class="txtRojo_Titulo_PagInterior16px">Recursos Humans</span></td>
				</tr>
				<tr> 
					<td bgcolor="#CCCCCC" height="1"></td>
				</tr>
			</table>
			
		</td>
    </tr>
	<tr>
		<td width="302" valign="top">
        
          	
                     
			 <% IF session("empresa")<>91 THEN%>
                        <table width="100%" border="0" cellspacing="0" cellpadding="2">
                          <tr>
                            <td colspan="2"><img src="../img/common/g_bullet_flecha_interiores.gif" width="6" height="3" align="absmiddle" border="0"><a href="/areapersonal/fitxatreballador.aspx"  class="txtRojo_Indice_Interior11px" target="_blank"><img src="/img/common/iconografia/ico_pdf.png" width="18" height="18" border="0" align="absmiddle"></a><a href="/areapersonal/fitxatreballador.aspx"  class="txtRojo_Indice_Interior11px" target="_blank"> Fitxa del/de la treballador/a</a></td>
                          </tr>
                          <tr> 
						<td><img src="../img/common/g_bullet_flecha_interiores.gif" width="6" height="3" align="absmiddle"><a href="/areapersonal/certSitAct.aspx" class="txtRojo_Indice_Interior11px" target="_blank"><img src="/img/common/iconografia/ico_pdf.png" width="18" height="18" border="0" align="absmiddle"> Certificat de situaci&oacute; actual</a></td>
					</tr>
						<%  if session("dni")= "06969214F"  then%>
                          <tr>
						   	 	<td><img src="../img/common/g_bullet_flecha_interiores.gif" width="6" height="3" align="absmiddle" border="0"><a href="/areapersonal/fitxalaboral.aspx"  class="txtRojo_Indice_Interior11px" target="_blank"><img src="/img/common/iconografia/ico_word.png" width="18" height="18" border="0" align="absmiddle"></a><a href="/areapersonal/fitxaLaboral.aspx" class="txtRojo_Indice_Interior11px" target="_blank"> Fitxa funcions laborals </a></td>
				   	 	  </tr>
						 
						  <%end if%>
                          
                          
                          
                          
                        </table>
					<% END IF%>			
         <table width="100%" border="0" cellspacing="0" cellpadding="2">
					<tr> 
						<td>
			 <img src="../img/common/g_bullet_flecha_interiores.gif" width="6" height="3" align="absmiddle" border="0"><a href="/rrhh/nomines.aspx" class="txtRojo_Indice_Interior11px">N&ograve;mines</a></td></tr></table>
	
                        <table width="100%" border="0" cellspacing="0" cellpadding="2">
                          <tr> 
                            <td><img src="../img/common/g_bullet_flecha_interiores.gif" width="6" height="3" align="absmiddle" border="0"><a href="/areapersonal/controlpresencia/controlpresencia.aspx" <%'onclick="SacarAlert();"%>  class="txtRojo_Indice_Interior11px">Control del saldo horari</a></td>                           
                          </tr>
                  </table>
                          
						<table width="100%" border="0" cellspacing="0" cellpadding="2">
                          <tr> 
                            <td><a href="/areapersonal/absentisme.aspx"  class="txtRojo_Indice_Interior11px"><img src="../img/common/g_bullet_flecha_interiores.gif" width="6" height="3" align="absmiddle" border="0">Llicències i permisos</a></td>
                          </tr>
                        </table>
                    	<% IF session("empresa")<>91 THEN%>											
							
						<table width="100%" border="0" cellspacing="0" cellpadding="2">
				    		<tr> 
								<td><img src="../img/common/g_bullet_flecha_interiores.gif" width="6" height="3" align="absmiddle"><a href="/areapersonal/fitxatreballador/canvidades.aspx" class="txtRojo_Indice_Interior11px">Sol·licitud de canvi de dades del treballador</a></td>
							</tr>
					
							 <tr> 
								<td><img src="../img/common/g_bullet_flecha_interiores.gif" width="6" height="3" align="absmiddle"><a href="/areapersonal/FitxaFormacioTreballador/FormacionAcademica.aspx" class="txtRojo_Indice_Interior11px">Comunicació de dades acadèmiques del treballador</a></td>
							</tr>
							
				</table>
				<%END IF%>
		</td>
		<td width="425" valign="top">
        <iframe src="menurrhh.aspx" scrolling="no" frameborder="0" marginheight="0" marginwidth="0" class="floatleft w100 marginDreta10"></iframe> 
		<script language="javascript1.2">
		//alert("L´Horari d´atenció telefònica del control de presència és de: 13:00 a 14:00 hores");
		</script>
		<% IF session("empresa")<>91 THEN%>		<div class="floatleft relativo fonsGrocffffde">
        	<span class="topEsquerraBlanc"></span><span class="topDretaBlanc"></span>
		    <div class="padding5" style="background-color:#CCCCCC;"><strong class="txtneg12px bold mayusculas">Informaci&oacute; d'inter&egrave;s</strong></div>
			<div class="padding10">
                <div class="txtneg11px paddingBottom10"><img src="../img/common/g_bullet_flecha_interiores.gif" width="6" height="3" align="absmiddle" border="0">L'Horari d'atenci&oacute; telef&ograve;nica del control de pres&egrave;ncia &eacute;s de: 9:00 a 10:00 hores i de 12:00 a 13:00 hores
                </div>
                <div  class="txtneg11px paddingBottom10"><img src="../img/common/g_bullet_flecha_interiores.gif" width="6" height="3" align="absmiddle" border="0">Sempre que se surti de l&rsquo;edifici on es presten els serveis o s&rsquo;hi entri, s&rsquo;ha de fitxar amb el codi corresponent.
                </div> 
                <div class="txtneg11px paddingBottom"><img src="../img/common/g_bullet_flecha_interiores.gif" width="6" height="3" align="absmiddle" border="0">Si no es realitza correctament el marcatge es pot alterar el saldo, a causa de la irregularitat dels marcatges.</div>
             </div>
			<span class="bottomEsquerraBlanc"></span><span class="bottomDretaBlanc"></span>
        </div>
		<% END IF%>
		
		</td>
	</tr>
	<tr>
	  <td colspan="2" valign="top">
	   <% if session("dni")= "06969214F" or session("dni")= "35005179F" or session("dni")= "38545546F" or session("dni")= "37382871M" or session("dni")= "38445868B" or session("dni") = "46608455J" then%>
			&nbsp;<iframe src="menuAbsenciaSindical.aspx" id="menuAbsenciaSindical" scrolling="no" frameborder="0" marginheight="0" marginwidth="0"  width="700" height="50"></iframe> 							
	   <% end if%>
	  </td>
	</tr>
	
	  
				
</table>   
<iframe src="menuUtils.aspx" id="menuTrasllats" scrolling="no" frameborder="0" marginheight="0" marginwidth="0" class="floatleft  marginDreta10" width="757" height="60"></iframe> 

                   
                     
                      </td>
                      <td width="94">&nbsp;</td>
                    </tr>
                    <tr> 
                      <td width="4">&nbsp;</td>
                      <td width="662" valign="top" class="txtGrisCL_Copy_Interior11px"> 
											
                       
                        <br>
												
                      </td>
                      <td width="94" valign="top" class="txtGrisCL_Copy_Interior11px">&nbsp;</td>
                      <td width="10">&nbsp;</td>
                    </tr>
										
                    <tr> 
                      <td width="4">&nbsp;</td>
                      <td colspan="2" valign="top" class="txtGrisCL_Copy_Interior11px">&gt; Nota: 
                        el s&iacute;mbol<img src="/img/common/iconografia/ico_pdf.png" width="18" height="18" border="0" align="absmiddle"> 
                        indica que &eacute;s necessari l'&uacute;s del Acrobat 
                        Reader versi&oacute; 5 per obrir l'enlla&ccedil;. Si no 
                        el teniu el podeu instal&middot;lar fent click <a href="/ar505esp.exe">aqu&iacute;</a>.</td>
                    
                    </tr>
                  </table>
                  
                </td>
              </tr>
            </table>
            <br>
            <%END IF%>
    
<!--#INCLUDE VIRTUAL="../js/App_LocalResources/peu.inc" -->
</body>
<!-- #EndTemplate --></html>
