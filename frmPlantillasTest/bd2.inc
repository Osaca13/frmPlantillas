<%
	Function ComprovarError(rc)
		if rc <> lhNoError then
			url = "Error.asp?missatge=aa" & bd.UltimErrorString
			response.write url
		end if
	End Function

	'Variables globals
	Dim bd 'Objecte LHBDIX_ASP

	Sub Inicialitzar()
		Set bd = Server.CreateObject("LHBDIX_ASP.LHBDASP")
		rc = bd.ConnectarBD("DBPERS", "PE", "4t./sR2C")
		ComprovarError(rc)

		bd.MostrarMsgErrors = 0
	End Sub

	Sub InicialitzarDB2E()
		Set bd = Server.CreateObject("LHBDIX_ASP.LHBDASP")
		rc = bd.ConnectarBD("DB2E", "TEPWWW", "t59$JTwA")
		ComprovarError(rc)
		bd.MostrarMsgErrors = 0
	End Sub


	Sub Finalitzar()
		rc = bd.TancarBD()
		ComprovarError(rc)
	End Sub
	


	Sub InicialitzarP()
		Set bd = Server.CreateObject("LHBDIX_ASP.LHBDASP")
		rc = bd.ConnectarBD("DB2P", "TANWWW", "m3h4$1xS")
		ComprovarError(rc)
		
		bd.MostrarMsgErrors = 0
	End Sub

	Sub FinalitzarP()
		rc = bd.TancarBD()
		ComprovarError(rc)
	End Sub
	
	
	
	
%>