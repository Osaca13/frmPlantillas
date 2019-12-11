<%@ Register TagPrefix="menuG" TagName="menuG" Src="~/js/App_LocalResources/menu.ascx" %>
<%
if  instr(Request.userAgent, "MSIE")=0 THEN 
	pnlAvis.visible=TRUE
	lblAVis.text = "Per raons d’eficiència i estalvi de recursos, GAIA s’ha desenvolupat per a que funcioni correctament en navegadors Microsoft Internet Explorer.<br />El navegador amb el que estàs obrint GAIA no compleix els nostres estàndards i pot ser que la usabilitat no sigui la prevista. "
else
	pnlAvis.visible=FALSE
end if
%>
<menuG:menug ID="menuG" Text="Menú GAIA" runat="server"/>
<asp:panel id="pnlAvis" runat="server" visible="false">
<div class="missatgeAvisIntranet"> <span class="topEsquerraBlanc"></span> <span class="topDretaBlanc"></span>
  <div class="icona">
    <asp:Label ID="lblAvis" runat="server" />
  </div>
  <span class="bottomEsquerraBlanc"></span> <span class="bottomDretaBlanc"></span> </div>
</asp:panel>

    
    