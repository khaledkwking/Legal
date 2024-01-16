Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.Data
Imports System.Data.SqlClient

<WebService(Namespace:="http://tempuri.org/")> _
<WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
<System.Web.Script.Services.ScriptService()> _
Public Class ItemDescComplete
    Inherits System.Web.Services.WebService

    <WebMethod()> _
    Public Function AutoSuggest(ByVal prefixText As String, ByVal count As Integer) As String()
        Try

        
            Dim ss As New SuperEngine
            Dim log As String = ""
            log += vbNewLine + "VENDORRRR SERVICE ACCESS -- PRE: " + prefixText + " COUNT: " + CStr(count)

            Dim dr As SqlDataReader = ItemsData.ins.getNamesReader(prefixText)

            Dim items As New System.Collections.Generic.List(Of String)(count)

            While dr.Read()
                items.Add(gets(dr("name")))
            End While
            dr.Close()

            'log += vbNewLine + "ITEMS COUNT: " + items.Count.ToString()
            'ss.SaveTextToFile(log, "C:\log.txt")
            Return items.ToArray()
        Catch ex As Exception
            'Dim ss As New SuperEngine

            'ss.SaveTextToFile(vbNewLine + "ERROR: " + ex.Message, "C:\log.txt")
        End Try
    End Function

    Private Function gets(ByVal s As Object) As String
        If s Is DBNull.Value Then
            Return ""
        Else
            Return CStr(s)
        End If
    End Function

End Class
