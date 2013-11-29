Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports System.Web.Script.Services

' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
' <System.Web.Script.Services.ScriptService()> _
<System.Web.Services.WebService(Namespace:="http://tempuri.org/")>
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)>
<ToolboxItem(False)>
<ScriptService()>
Public Class Contatos
    'Inherits System.Web.Services.WebService

    <WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Public Function ListarContatos() As List(Of Contato)

        Dim contatos As New List(Of Contato)

        For i = 1 To 1000
            contatos.Add(New Contato() With {.Nome = "Teste " & i, .Endereco = "Endereço " & i})
        Next

        'contatos.Add(New Contato() With {.Nome = "Fernando", .Endereco = "Endereço teste"})
        'contatos.Add(New Contato() With {.Nome = "Patrícia", .Endereco = "Avenida 14 de dezembro"})


        Return contatos

    End Function

End Class