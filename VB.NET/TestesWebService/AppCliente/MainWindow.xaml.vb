Class MainWindow 

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.



    End Sub

    Private Sub Window_Loaded(sender As Object, e As RoutedEventArgs)

        Try
            Dim proxy As New ContatosReference.ContatosSoapClient

            Dim a As ContatosReference.Contato

            dataGrid.ItemsSource = proxy.ListarContatos()


        Catch ex As Exception

            MsgBox(ex.Message)

        End Try





    End Sub


End Class
