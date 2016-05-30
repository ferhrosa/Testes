Imports System.Windows.Forms

Public Class Formulario(Of T) : Inherits Control


    Public Property Dados As IEnumerable(Of T)




    Public Sub CarregarControles()
        For Each Item In Dados

        Next
    End Sub

End Class
