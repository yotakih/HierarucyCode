<%@ Control Language="vb" AutoEventWireup="false" Inherits="TestWebApp1.HieraruchyCodePopper" %>

    <textarea id="<%=Me.ClientID %>_taCd" cols="<%=Me.TextAreaCols %>" rows="<%=Me.TextAreaRows %>"></textarea>
    <input type="hidden" id="<%=Me.ClientID %>_hdnCd" />
    <input type="button" onclick="openWindow('./HieraruchyCode.aspx?ret=<%=Me.ClientID%>_taCd&retlst=<%=Me.ClientID%>_hdnCd&rid=<%=Me.RootCodeId%>');" value="コードを選択" />
