Public Class HieraruchyCode
    Inherits System.Web.UI.Page

#Region "プロパティ"
    Public Property tbl As DataTable = Nothing
    Public Property lstDept As Integer = 0
    Public Property rootCdId As Integer = 1
    Public Property idcl As String = "id"
    Public Property pidcl As String = "par_id"
    Public Property lvlcl As String = "level"
    Public Property cdcl As String = "code"
    Public Property cdodrcl As String = "codenum"
    Public Property cdnmcl As String = "codename"
    Public Property lst As New List(Of code)
    ''' <summary>
    ''' 選択されたコードを設定する親ウィンドウのinputタグの名前
    ''' </summary>
    ''' <returns></returns>
    Public Property retTagNm As String = ""
    ''' <summary>
    ''' 選択されたコードを設定する親ウィンドウのカンマ区切りのコードリストを返すinputタグの名前
    ''' </summary>
    ''' <returns></returns>
    Public Property retlstTagNm As String = ""
    ''' <summary>
    ''' codeを選択できる数を制限する（０の場合、無制限、１の場合、ラジオボタンになる）
    ''' </summary>
    ''' <returns></returns>
    Public Property slctCnt As Integer = 0
#End Region
#Region "WebControl"

#End Region
#Region "イベント"
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'url parameter
        If Not Request.Params("ret") Is Nothing Then
            Me.retTagNm = Request.Params("ret").ToString
        End If
        If Not Request.Params("retlst") Is Nothing Then
            Me.retlstTagNm = Request.Params("retlst").ToString
        End If
        If Not Request.Params("rid") Is Nothing Then
            Me.rootCdId = Convert.ToInt32(Request.Params("rid").ToString)
        End If
        If Not Request.Params("cnt") Is Nothing Then
            Me.slctCnt = Convert.ToInt32(Request.Params("cnt").ToString)
        End If
        'テスト用コード
        Dim tbl As New DataTable
        tbl.Columns.Add("id", GetType(Integer))
        tbl.Columns.Add("par_id", GetType(Integer))
        tbl.Columns.Add("level", GetType(Integer))
        tbl.Columns.Add("code", GetType(String))
        tbl.Columns.Add("codenum", GetType(Integer))
        tbl.Columns.Add("codename", GetType(String))

        tbl.Rows.Add(setRow(tbl.NewRow, 1, 0, 0, "10", 0, "root"))
        tbl.Rows.Add(setRow(tbl.NewRow, 2, 1, 1, "1010", 10, "one"))
        tbl.Rows.Add(setRow(tbl.NewRow, 3, 1, 1, "1020", 20, "two"))
        tbl.Rows.Add(setRow(tbl.NewRow, 4, 2, 2, "101010", 10, "one_one"))
        tbl.Rows.Add(setRow(tbl.NewRow, 5, 2, 2, "101020", 20, "one_two"))
        tbl.Rows.Add(setRow(tbl.NewRow, 7, 3, 2, "102020", 20, "two_two"))
        tbl.Rows.Add(setRow(tbl.NewRow, 6, 3, 2, "102010", 10, "two_one"))

        Me.tbl = tbl
    End Sub
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        If tbl Is Nothing Then Exit Sub
        'tblからDataSourceを生成
        Me.lst = Me.CreateDataSource(tbl, Me.rootCdId, 1)   'rootはレベル１から
    End Sub
#End Region
#Region "内部関数"
    Private Function CreateDataSource(ByRef tbl As DataTable, ByVal schPid As Integer, ByVal schLvl As Integer, Optional ByRef lst As List(Of code) = Nothing) As List(Of code)
        If lst Is Nothing Then
            lst = New List(Of code)
        End If
        '親を登録
        Dim curRw() As DataRow = tbl.Select(idcl & " = " & schPid)
        With curRw(0)
            lst.Add(New code(.Item(idcl), .Item(pidcl), schLvl, .Item(cdcl), .Item(cdodrcl), .Item(cdnmcl), tbl.Select(pidcl & " = " & schPid).Length > 0))
        End With
        '子供をあれば登録
        For Each chd As DataRow In tbl.Select(pidcl & " = " & schPid, cdodrcl)
            '子どもたちを再帰的に登録
            CreateDataSource(tbl, chd(idcl), schLvl + 1, lst)
        Next
        '深さを更新
        If Me.lstDept < schLvl Then
            Me.lstDept = schLvl
        End If
        Return lst
    End Function

    Function setRow(ByRef dr As DataRow, ByVal id As Integer, ByVal par_id As Integer, ByVal level As Integer, ByVal code As String, ByVal codenum As Integer, ByVal codename As String) As DataRow
        dr("id") = id
        dr("par_id") = par_id
        dr("level") = level
        dr("code") = code
        dr("codenum") = codenum
        dr("codename") = codename

        Return dr
    End Function
#End Region
#Region "内部クラス"
    Class code
        Public Property id As Integer
        Public Property pid As Integer
        Public Property lvl As Integer
        Public Property cd As String
        Public Property cdodr As Integer
        Public Property cdnm As String
        ''' <summary>
        ''' 子供をもつ場合true
        ''' </summary>
        ''' <returns></returns>
        Public Property pflg As Boolean

        Sub New(ByVal id As Integer, ByVal pid As Integer, ByVal lvl As Integer, ByVal cd As String, ByVal cdodr As Integer, ByVal cdnm As String, ByVal pflg As Boolean)
            Me.id = id
            Me.pid = pid
            Me.lvl = lvl
            Me.cd = cd
            Me.cdodr = cdodr
            Me.cdnm = cdnm
            Me.pflg = pflg
        End Sub

        Public Overrides Function ToString() As String
            Return Me.id & ":" & Me.pid & ":" & Me.lvl & ":" & Me.cd & ":" & Me.cdodr & ":" & Me.cdnm & ":" & Me.pflg
        End Function
    End Class
#End Region
End Class

