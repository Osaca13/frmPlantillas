<asp:placeholder runat="server" id="pnlCap">
<table width="100%" border="0" cellspacing="0" cellpadding="0" align="center" style="margin-top:5px;" class="noImprimible">
	<tr> 
		<td colspan="5">
			<table width="100%" border="0" cellspacing="0" cellpadding="0">
				 <tr> 
					<td width="17" height="17"><img src="/img/common/vorera_esq.png" style="border:0"/></td>
					<td colspan="2" height="17" valign="top"><table style="border-top:1px solid #c4c4c4;" cellpadding="0" cellspacing="0" border="0" width="100%"><tr><td></td></tr></table></td>
					<td width="17" height="17"><img src="/img/common/vorera_dre.png" style="border:0"/></td>
				</tr> 
			</table>
		</td>
	</tr>        
	<tr> 
		<td width="1" bgcolor="#c4c4c4"></td>
		<td align="left" width="373"><a href="/home.aspx"><img src="/img/common/logo_hospinet.png" style="border:0; padding-left:20px; padding-right:20px"/></a></td>
		<td align="center" valign="top">
            <!--#INCLUDE VIRTUAL="js/App_LocalResources/eltemps.aspx"-->

		</td>
		<td align="right" width="222"><img src="/img/common/fasana_ajuntament.png" style="border:0"/></td>
		<td width="1" bgcolor="#c4c4c4"></td> 
	</tr>
	<tr> 
		<td colspan="5" bgcolor="#466480"> 
		
			<table width="100%" cellpadding="2" cellspacing="0" border="0">
				<tr>
					<td align="left">					
						<table border="0" cellspacing="3" cellpadding="0" width="100%">
							<td valign="middle" align="left" class="paddingDretaEsquerra10"><img src="/img/common/icona_ajuntament.png" class="valignMiddle border0"/></td>
							<td align="center" nowrap><a href="/home.asp" class="blanc t08 RobotoCondensed valignMiddle nodeco">INICI</a></td>
							<td align="center" class="paddingDretaEsquerra5"><img src="/img/common/separador_linea_blanca.png"></td>
							<td align="center" nowrap><a href="http://www.l-h.cat/" class="blanc t08 RobotoCondensed nodeco" target="_blank">AJUNTAMENT ON-LINE</a></td>
							<td align="center" class="paddingDretaEsquerra5"><img src="/img/common/separador_linea_blanca.png"></td>
							<td align="center" nowrap><a href="/html/cgis/Llistitelefonic/Listintelefonico.aspx" class="blanc t08 RobotoCondensed valignMiddle nodeco"><img src="/img/common/ico_llisti.png" style="border:0; vertical-align:middle;">&nbsp;LLIST&Iacute; TELEF&Ograve;NIC</a></td>                       
							<td align="center" class="paddingDretaEsquerra5"><img src="/img/common/separador_linea_blanca.png"></td>
							<td align="center" nowrap><a href="https://www.l-h.cat/recullpremsa" class="blanc t08 RobotoCondensed valignMiddle nodeco" target="_blank"><img src="/img/common/ico_recull.png"style="border:0; vertical-align:middle;">&nbsp;RECULL DE PREMSA</a></td> 
							<td valign="middle" align="center" nowrap width="40%"><form class="margin0 padding0" name="busqueda" action="/utils/cercador/cercador.aspx" method="post"><input type="text" class="borderRadius15 padding3 arial valignMiddle t075" name="txtCercaCapcalera" id="txtCercaCapcalera" size="30"/>&nbsp;<input id="Cercar" name="Cercar" class="border0 valignMiddle" type="image" src="/img/common/lupa.png"  onclick="document.busqueda.submit()" onkeypress="document.busqueda.submit()"/></form></td> 
							<td align="right" nowrap class="paddingDreta20"><img src="/img/common/ico_espaipersonal.png" style="vertical-align:middle;">&nbsp;<a href="/asp/areapersonal.aspx" class="t08 RobotoCondensed blanc nodeco">Espai personal</a> 
						<% IF Session("login") Then
                                Response.Write("<img src=""/img/common/ico_desconnectar.png"" style=""vertical-align:middle;"">&nbsp;<a href=""/areapersonal.aspx?desconectar=1"" class=""arial blanc bold t075 nodeco"">Desconnectar</a>")
                            End IF%> 
								</td>     
					</table>
					
					</td>
					
				</tr>
			</table>         
				
		</td>
	</tr>
</table>

   
</asp:placeholder>          


<script runat="server" language="vb" debug="false">      
	Private Sub Page_init(ByVal sender As System.Object, ByVal e As System.EventArgs) 
		IF session("usuariXarxa") is nothing THEN
			If HttpContext.Current.User.Identity.Name.Length > 0 Then
				 session("usuariXarxa") = GAIA.formatejaUsuari(HttpContext.Current.User.Identity.Name)
			End If														
		END IF		
		IF   clsPermisos.GetGroupsAD(session("usuariXarxa")).contains("noIntranet") THEN
			try	
				pnlCap.visible=false 
				pnlPeu.visible=false
			catch
			end try
		END IF
	End Sub		
</script>