Imports System.Data.SqlClient
Public Class Inicio
    Public id As Integer
    Public ultimo As Integer
    Public bandera As Integer
    Public cod As Integer

    Private Sub Inicio_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        btnEditar.Enabled = False
        txtNombre.Enabled = False
        txtNumero.Enabled = False
        txtDireccion.Enabled = False
        btnEliminar.Enabled = False
        ListBox1.Items.Clear()
        llenarLista()
    End Sub

    Private Sub llenarLista()
        ListBox1.Items.Clear()
        Dim conexion As New SqlConnection("Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" & "D:\Visual Studio 2019\prueba\Database.mdf" & ";Integrated Security=True")
        Dim cmd As New SqlCommand
        Dim reader As SqlDataReader
        cmd.CommandType = CommandType.Text
        cmd.CommandText = "Select nombre from agenda"
        cmd.Connection = conexion
        Try
            conexion.Open()
            reader = cmd.ExecuteReader
            If reader.HasRows Then
                While reader.Read
                    ListBox1.Items.Add(reader(0))
                End While
            Else
                ListBox1.Items.Add("No hay registros para mostrar")
            End If
            conexion.Close()
        Catch ex As Exception
            MsgBox(ex.ToString, MsgBoxStyle.Critical, "Atención")
        End Try
    End Sub

    Private Sub buscarultireg()
        Dim sql As New SqlConnection("Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" & "D:\Visual Studio 2019\prueba\Database.mdf" & ";Integrated Security=True")
        Dim cmd As New SqlCommand
        cmd.CommandType = CommandType.Text
        cmd.CommandText = "Select top 1 * from agenda order by [id] desc"
        cmd.Connection = sql
        Try
            sql.Open()
            id = Convert.ToInt32(cmd.ExecuteScalar)
            sql.Close()
            ultimo = id + 1
        Catch ex As Exception
            MsgBox(ex.ToString, MsgBoxStyle.Critical, "Atención")
        End Try
    End Sub

    Private Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        txtNombre.Text = ""
        txtNumero.Text = ""
        txtDireccion.Text = ""
        txtNombre.Enabled = False
        txtNumero.Enabled = False
        txtDireccion.Enabled = False
        btnEditar.Enabled = False
        btnNuevo.Enabled = True
        btnEliminar.Enabled = False

    End Sub

    Private Sub btnEditar_Click(sender As Object, e As EventArgs) Handles btnEditar.Click
        txtNombre.Enabled = True
        txtNumero.Enabled = True
        txtDireccion.Enabled = True
        txtNombre.Focus()
        btnEditar.Enabled = False
        btnNuevo.Enabled = False
        btnEliminar.Enabled = False
        bandera = 2
    End Sub

    Private Sub ListBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListBox1.SelectedIndexChanged
        btnEditar.Enabled = True
        btnEliminar.Enabled = True
        Dim a As New SqlConnection("Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" & "D:\Visual Studio 2019\prueba\Database.mdf" & ";Integrated Security=True")
        Dim b As New SqlCommand
        Dim lector As SqlDataReader
        b.CommandType = CommandType.Text
        b.CommandText = "select  id, nombre, numero, direccion from agenda where nombre ='" & ListBox1.SelectedItem & "'"
        b.Connection = a
        Try
            a.Open()
            lector = b.ExecuteReader
            If lector.HasRows Then
                While lector.Read
                    cod = lector(0)
                    txtNombre.Text = lector(1)
                    txtNumero.Text = lector(2)
                    txtDireccion.Text = lector(3)
                End While
            End If
            buscarultireg()
        Catch ex As Exception
            MsgBox(ex.ToString, MsgBoxStyle.Critical, "Atención")
        End Try
    End Sub

    Private Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click
        txtNombre.Enabled = True
        txtNumero.Enabled = True
        txtDireccion.Enabled = True
        txtNombre.Focus()
        bandera = 1
        btnNuevo.Enabled = False
        btnEditar.Enabled = False
        btnEliminar.Enabled = False
        buscarultireg()
    End Sub

    Private Sub btnEliminar_Click(sender As Object, e As EventArgs) Handles btnEliminar.Click
        If MsgBox("¿Desea eliminar el registro " & ListBox1.SelectedItem.ToString & "?", MsgBoxStyle.YesNo, "Pregunta") Then
            Try
                Dim s As New SqlConnection("Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" & "D:\Visual Studio 2019\prueba\Database.mdf" & ";Integrated Security=True")
                Dim c As New SqlCommand
                c.CommandType = CommandType.Text
                c.CommandText = "delete from agenda where nombre ='" & ListBox1.SelectedItem & "'"
                c.Connection = s
                s.Open()
                c.ExecuteNonQuery()
                s.Close()
                llenarLista()
                btnCancelar.PerformClick()
            Catch ex As Exception
                MsgBox(ex.ToString, MsgBoxStyle.Critical, "Atención")
            End Try
        End If
    End Sub

    Private Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        If bandera = 1 Then
            Dim sql As New SqlConnection("Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" & "D:\Visual Studio 2019\prueba\Database.mdf" & ";Integrated Security=True")
            Dim cmd As New SqlCommand
            Try
                cmd.CommandType = CommandType.Text
                cmd.CommandText = "insert into agenda values (@id,@nombre,@numero,@direccion)"
                cmd.Parameters.AddWithValue("@id", ultimo)
                cmd.Parameters.AddWithValue("@nombre", txtNombre.Text)
                cmd.Parameters.AddWithValue("@numero", txtNumero.Text)
                cmd.Parameters.AddWithValue("@direccion", txtDireccion.Text)
                cmd.Connection = sql
                sql.Open()
                cmd.ExecuteNonQuery()
                sql.Close()
                llenarLista()
            Catch ex As Exception
                MsgBox(ex.ToString, MsgBoxStyle.Critical, "Atención")
            End Try
        ElseIf bandera = 2 Then
            Dim conec As New SqlConnection("Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" & "D:\Visual Studio 2019\prueba\Database.mdf" & ";Integrated Security=True")
            Dim comand As New SqlCommand
            Try
                comand.CommandType = CommandType.Text
                comand.CommandText = "update agenda set [nombre]=@nombre, [numero]=@numero, [direccion]=@direccion where id = " & cod
                comand.Parameters.AddWithValue("@nombre", txtNombre.Text)
                comand.Parameters.AddWithValue("@numero", txtNumero.Text)
                comand.Parameters.AddWithValue("@direccion", txtDireccion.Text)
                comand.Connection = conec
                conec.Open()
                comand.ExecuteNonQuery()
                conec.Close()
                llenarLista()
            Catch ex As Exception
                MsgBox(ex.ToString, MsgBoxStyle.Critical, "Atención")
            End Try
        End If
        btnCancelar.PerformClick()
    End Sub
End Class
