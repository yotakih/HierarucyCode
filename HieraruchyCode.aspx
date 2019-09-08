<%@ Page Language="vb" AutoEventWireup="false" Inherits="TestWebApp1.HieraruchyCode" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <script type="text/javascript" src="//ajax.googleapis.com/ajax/libs/jquery/1.11.0/jquery.min.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            let opener = window.opener;
            opener.$('#<%= Me.retlstTagNm %>').val().split(',').forEach(function (v) {
                $('[name=cdchk][value=' + v + ']').prop('checked', true);
            });
        });
        function clearcode() {
            let opener = window.opener;
            opener.$('#<%= Me.retTagNm %>').val('');
            opener.$('#<%= Me.retlstTagNm %>').val('');
            window.close();
        }
        function selectcode() {
            let opener = window.opener;
            let rettxt = '';
            let retlst = new Array();
            let slctCnt = 0;
            $('[name=cdchk]').each(function (idx, ele) {
                if ($(ele).prop('checked')) {
                    rettxt += $('#lb' + $(ele).val()).text() + '[' + $(ele).val() + ']' + '\n';
                    retlst.push($(ele).val());
                    slctCnt = slctCnt + 1;
                }
            });
            if (<%=Me.slctCnt%> != 0 && <%=Me.slctCnt%> < slctCnt) {
                alert('選択可能な件数を超えています。\n<%=Me.slctCnt%>以内にしてください。');
            }
            opener.$('#<%= Me.retTagNm %>').val(rettxt);
            opener.$('#<%= Me.retlstTagNm %>').val(retlst.join(','));
            window.close();
        }
    </script>
    <style type="text/css">
        .floatButton {
            top: 200px;
            right: 20px;
            position: fixed;
            width: 120px;
            padding: 10px;
            z-index: 1;
            background-color: #FFFFCC;
            box-shadow: 5px 5px 5px 0px lightgray;
        }
        ul {
            list-style-type: none;
            padding-left: 10px;
        }
        ul.lvl_1 {
            background-color: #00F;
            color: white;
        }
        ul.lvl_2 {
            background-color: #77F;
            padding-top: 3px;
            color: white;
        }
        ul.lvl_3 {
            background-color: #EEF;
            padding-top: 3px;
            color: white;
        }
        li.notpar {
            background-color: white;
            color: black;
        }
    </style>
</head>
<body>
    <div class="floatButton">
        <input type="button" value="選択" onclick="selectcode();" />
        <input type="button" value="クリア" onclick="clearcode();" />
    </div>
    
    <% 
        Dim curLvl As Integer = 0
        For Each code As code In Me.lst
    %>
        <%-- 現在の階層と自分の階層が異なる場合、階層を調整する --%>
        <% If curLvl > code.lvl Then %>
            <% For i As Integer = 0 To curLvl - code.lvl - 1 %>
                </ul>
            <% next %>
            <% curLvl = code.lvl %>
        <% End If %>
        <% If curLvl < code.lvl Then %>
            <% For i As Integer = 0 To code.lvl - curLvl - 1 %>
                <ul class="lvl_<%= code.lvl %>">
            <% Next %>
            <% curLvl = code.lvl %>
        <% End If%>
        <%-- 自分を描画 --%>
        <li <% If Not code.pflg Then %>class="notpar"<% End If %>>
            <label id="lb<%= code.cd %>"><%= code.cdnm %></label>[<%= code.cd %>]
            <% if Not code.pflg Then %>
                <input type="<%= IIf(Me.slctCnt = 1, "radio", "checkbox").ToString %>" id="chk_<%= code.id %>" name="cdchk" value="<%= code.cd %>" />
            <% End If %>
        </li>
    <% Next %>
    <%-- 最後の閉じul --%>
    <% For i = 0 To curLvl - 1 %>
        </ul>
    <% Next %>
</body>
</html>
