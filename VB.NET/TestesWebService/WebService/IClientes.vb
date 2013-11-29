Imports System.ServiceModel

' NOTE: You can use the "Rename" command on the context menu to change the interface name "IClientes" in both code and config file together.
<ServiceContract()>
Public Interface IClientes

    <OperationContract()>
    Function ListarClientes() As List(Of Cliente)

End Interface
