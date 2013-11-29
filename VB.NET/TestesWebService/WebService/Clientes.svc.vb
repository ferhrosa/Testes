' NOTE: You can use the "Rename" command on the context menu to change the class name "Clientes" in code, svc and config file together.
' NOTE: In order to launch WCF Test Client for testing this service, please select Clientes.svc or Clientes.svc.vb at the Solution Explorer and start debugging.
Public Class Clientes
    Implements IClientes

    Public Function ListarClientes() As List(Of Cliente) Implements IClientes.ListarClientes
        Dim clientes As New List(Of Cliente)
        clientes.Add(New Cliente() With {.Nome = "Fernando", .Endereco = "Endereço teste"})
        clientes.Add(New Cliente() With {.Nome = "Patrícia", .Endereco = "Avenida 14 de dezembro"})

        Return clientes
    End Function

End Class
