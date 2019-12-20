<%@ Import Namespace="System.Data.OleDb" %>
<%   
    Try
        Dim strPage as String
        Dim objconn As OleDbConnection
        Response.Write(GAIA.GetHTML(objconn, "http://www.l-h.cat/utils/temps/meteocat.html"))
    Catch

    End try
%>