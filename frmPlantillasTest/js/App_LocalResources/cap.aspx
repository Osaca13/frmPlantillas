<asp:placeholder runat="server" id="pnlCap" visible="true">
<table width="100%" border="0" cellspacing="0" cellpadding="0">
  <tr>
    <td> <form method="post" name="formbusca" action="/html/cgis/cercar.asp" onSubmit="document.formbusca.novacerca.value=1;"> 
      <table width="759" border="0" cellspacing="0" cellpadding="0" align="center" background="/img/common/fg_barra_top05.jpg" height="79">
        <tr> 
          <td height="55" width="126">&nbsp;</td>
          <td width="295">&nbsp;</td>
          <td width="83">
              <div align="center"></div>
            </td>
          <td width="252">&nbsp;</td>
          <td width="4">&nbsp;</td> 
        </tr>
        <tr> 
          <td colspan="5">
              <table border="0" cellspacing="0" cellpadding="0" height="9" width="759">
                <tr> 
                  <td class="txtGrisCL_MenuTop10px" width="77" height="12">&nbsp;</td>
                  <td width="46" height="12"><a href="/home.asp" class="txtGrisCL_MenuTop10px">INICI</a></td>
                  <td width="30" height="12"><img src="/img/common/g_separador_menutop.gif" width="30" height="24"></td>
                  <td width="129" height="12"><a href="http://www.l-h.cat" class="txtGrisCL_MenuTop10px">AJUNTAMENT 
                    ON-LINE</a></td>
                  <td width="22" height="12"><img src="/img/common/g_separador_menutop.gif" width="30" height="24"></td>
                  <td valign="middle" width="258" height="12"> 
                    <table width="84%" border="0" cellspacing="0" cellpadding="0">
                      <tr> 
                        <td width="15%"><a href="/html/cgis/llisti/llistialfa.asp"class="txtGrisCL_MenuTop10px" onMouseOver="document.telef.src='/img/common/iconografia/ico_llisti.png'" onMouseOut="document.telef.src='/img/common/iconografia/ico_llisti.png'"> 
                          <img src="/img/common/iconografia/ic_telefono.gif" name="telef" border="0" onMouseOver="this.src='/img/common/iconografia/ico_llisti.png'" onMouseOut="this.src='/img/common/iconografia/ico_llisti.png'"></a></td>
                        <td width="85%"> 
                          <div align="left"><a href="/html/cgis/llisti/llistialfa.asp"class="txtGrisCL_MenuTop10px" onMouseOver="document.telef.src='/img/common/iconografia/ico_llisti.png'" onMouseOut="document.telef.src='/img/common/iconografia/ico_llisti.png'"> 
                            &nbsp;LLIST&Iacute; TELEF&Ograve;NIC</a></div>
                        </td>
                      </tr>
                    </table>
                  </td>
                  <td width="30" height="12"><img src="../../img/g_separador_menutop.gif" width="30" height="24"></td>
                  <td width="148" valign="top" height="12"> 
                    <div align="right"> 
                      <table border="0" cellspacing="0" cellpadding="0" height="24">
                        <tr> 
                          <td> 
                            <input type="text" name="buscar" class="txtGrisCL_TextoBuscar10px" value="">
                          </td>
                          <td width="20"><a href="#"  onClick="window.event.returnValue = false;document.formbusca.novacerca.value=1;document.formbusca.submit();"><img src="/img/common/b_buscar_off.gif" width="18" height="18" hspace="5" border="0"></a> 
                            <input type="hidden" name="novacerca" value="FALSE">
                          </td>
                        </tr>
                      </table>
                    </div>
                  </td>
                  <td width="39" height="12">&nbsp;</td>
                </tr>
                <tr> 
                  <td class="txtGrisCL_MenuTop10px" colspan="9" height="12"><table width="759" border="0" cellspacing="0" cellpadding="0" height="20">
              <tr align="left"> 
                <td colspan="2" valign="top" > 
                    
                <!-- #INCLUDE VIRTUAL="ticker.asp" -->
                </td>
                <td width="271" valign="top">
                          <div align="right"><img src="../../img/g_bullet_flecha_areapersonal.gif" width="7" height="6" hspace="3"><a href="/asp/areaPersonal.asp" class="txtRojo_AreaPersonal9px">Espai 
                            personal</a> 
							
                            <% IF Session("login")=TRUE THEN%>
                            &nbsp;<img src="../../img/g_bullet_flecha_areapersonal.gif" width="7" height="6" hspace="3"><a href="/asp/areaPersonal.asp?desconectar=1" class="txtRojo_AreaPersonal9px">Desconnectar</a> 
                            <%END IF%>
                          </div>
                </td>
              </tr>
            
            </table></td>
                </tr>
              </table>
          </td>
        </tr>	
      </table>
    </form>
    </td>
  </tr>
  
</table>
<table width="100%" border="0" cellspacing="0" cellpadding="0">
  <tr>
    <td> 
      
      <table width="759" border="0" cellspacing="0" cellpadding="0" align="center">
        <tr> 
          <td valign="top" height="250">
</asp:placeholder>          

<script runat="server" language="vb">

    Private Sub Page_init(ByVal sender As System.Object, ByVal e As System.EventArgs)
        IF session("usuariXarxa") is nothing THEN
            If HttpContext.Current.User.Identity.Name.Length > 0 Then
                session("usuariXarxa") = GAIA.formatejaUsuari(HttpContext.Current.User.Identity.Name)
            End If
        END IF

        IF NOT  clsPermisos.GetGroupsAD(session("usuariXarxa")).contains("noIntranet") THEN

            try  'ho faig per si té hi son els pnl 
                pnlCap.visible=True
                'pnlPeu.visible = True
            Catch
            end try
        END IF

    End Sub


</script>