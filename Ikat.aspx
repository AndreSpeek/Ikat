﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Ikat.aspx.cs" Inherits="Ikat" %>
<%@ Register Src="Controls/Mobrol.ascx" TagPrefix="ikat" TagName="Mobrol" %>
<ikat:Mobrol runat="server" id="Mobrol" />
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Ikat Translations</title>
    <link href="Ikat.css" rel="stylesheet" />
    <script type="text/javascript">
        function scroller()
        {
            var divTop = document.getElementById('container').scrollTop;
            document.getElementById('hfScrollPosition').value = divTop;
        }

        function setScrollposition()
        {
            document.getElementById('container').scrollTop = document.getElementById('hfScrollPosition').value;
        }

        function copyClipBoard(textbox)
        {
            var tb = document.getElementById(textbox);
            if (tb.value.length == 0) {
                tb.focus();
                paste(tb);
                
            }
        }

        function paste(tb) {
            navigator.clipboard.readText()
              .then(text => {
                  // `text` contains the text read from the clipboard
                  tb.value = text;
              })
              .catch(err => {
                  // maybe user didn't grant access to read from clipboard
                  console.log('Something went wrong', err);
                    });
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="scriptManager" runat="server" />
    <div class="topBar">
        IKAT
    </div>
    <asp:HiddenField ID="hfScrollPosition" runat="server" ClientIDMode="Static" />
    <div id="divLogIn" runat="server" style="width: 95vw; height: 70vh; display: flex; align-items: center; justify-content: center;">
        <div class="login">
        <table>
            <tr>
                <th>
                    Enter password to continue:
                </th>
            </tr>
            <tr>
                <td style="padding-top: 11px;">
                    <asp:TextBox ID="tbPassKey" runat="server"  TextMode="Password" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Button ID="btnLogin" runat="server" Text="Login" OnClick="btnLogin_Click" CssClass="btnSmall" />
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td>
                    <asp:LinkButton ID="btnGoWebsite" runat="server" OnClick="btnGoWebsite_Click">
                        Or go to website
                    </asp:LinkButton>
                </td>
            </tr>
        </table>
        </div>
    </div>
    <div id="divIkat" runat="server" align="center" visible="false">
        <asp:UpdatePanel ID="pnlIkat" runat="server">
            <ContentTemplate>
                <div style="width: 95%; text-align: left;">
                    <table class="buttonBar">
                        <tr>
                            <td>
                                <h1><asp:Label ID="lblHeader" runat="server" Text="--" /></h1>
                            </td>
                            <td style="text-align: right">
                                <asp:Button ID="btnLanguages" runat="server" Text="Languages" OnClick="btnLanguages_Click" CssClass="btnLarge" />
                                <asp:Button ID="btnTranslator" runat="server" Text="Terms & Translations" OnClick="btnTranslator_Click" CssClass="btnLarge" />
                                <asp:LinkButton ID="btnLogOff" runat="server" OnClick="btnLogOff_Click">
                                    Exit
                                </asp:LinkButton>
                            </td>
                        </tr>
                    </table>
                    <div id="divLanguages" runat="server" visible="false">
                        <table>
                            <asp:Repeater ID="rptLanguages" runat="server" OnItemDataBound="rptLanguages_ItemDataBound">
                                <ItemTemplate>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="tbCode" runat="server" Width="50px" Enabled="false" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="tbDescription" runat="server" Width="250px" />
                                        </td>
                                        <td>
                                            <asp:Button ID="btnUpdateLanguage" runat="server" Text="Update" OnClick="btnUpdateLanguage_Click" CssClass="btnSmall" />
                                        </td>
                                        <td>
                                            <asp:Button ID="btnRemoveLanguage" runat="server" Text="Remove" OnClick="btnRemoveLanguage_Click" CssClass="btnSmall" />
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                            <tr>
                                <td>
                                    <asp:TextBox ID="tbNewCode" runat="server" Width="50px" />
                                </td>
                                <td>
                                    <asp:TextBox ID="tbNewDescription" runat="server" Width="250px" />
                                </td>
                                <td colspan="2">
                                    <asp:Button ID="btnAddLanguage" runat="server" Text="Add" OnClick="btnAddLanguage_Click" CssClass="btnSmall" />
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div id="divTerms" runat="server">
                        <table style="width: 100%;">
                            <tr>
                                <td style="width: 500px; vertical-align: top;">
                                    <div id="container" style="position: relative; height: 81vh; background-color: #FAFAFA; overflow-y: scroll;" onscroll="scroller();">
                                        <div id="x" style="position: absolute;"></div>
                                        <table id="tblTerms" style="width: 100%;" class="terms">
                                            <tr>
                                                <th>
                                                    Terms
                                                </th>
                                                <th>
                                                    <span style="float: right;">
                                                        <asp:LinkButton ID="btnRefreshTerms" runat="server" OnClick="btnRefreshTerms_Click">
                                                            <span style="color: var(--accent);">Refresh</span>
                                                        </asp:LinkButton>
                                                    </span>
                                                </th>
                                            </tr>
                                            <asp:Repeater ID="rptTerms" runat="server" OnItemDataBound="rptTerms_ItemDataBound">
                                                <ItemTemplate>
                                                    <tr>
                                                        <td style="width: 95%;">
                                                            <asp:LinkButton ID="btnTermID" runat="server" OnClick="btnTermID_Click">
                                                                <div style="width: 100%;"><asp:Label ID="lblTermID" runat="server" /></div>
                                                            </asp:LinkButton>
                                                        </td>
                                                        <td style="width: 5%; text-align: center;">
                                                            <asp:LinkButton ID="btnDeleteTerm" runat="server" OnClick="btnDeleteTerm_Click">
                                                                X
                                                            </asp:LinkButton>
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </table>
                                    </div>
                                </td>
                                <td style="vertical-align: top;">
                                    <asp:HiddenField ID="hfTermID" runat="server" />
                                    <table style="width: 95%;" class="translator">
                                        <tr>
                                            <th>
                                                <asp:CheckBox ID="cbForceMultiLine" runat="server" Text="Force Multi-line" AutoPostBack="true" OnCheckedChanged="cbForceMultiLine_CheckedChanged" />
                                            </th>
                                            <th colspan="2" style="text-align: right;">
                                                <asp:Button ID="btnAddTerm" runat="server" Text="Add Term" OnClick="btnAddTerm_Click" CssClass="btnSmall" />
                                            </th>
                                        </tr>
                                        <tr>
                                            <td style="width: 200px;">
                                                Referer
                                            </td>
                                            <td>
                                                <asp:TextBox ID="tbReferer" runat="server" Width="200px" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Default
                                            </td>
                                            <td>
                                                <asp:TextBox ID="tbDefaultText" runat="server" Width="100%" TextMode="MultiLine" Rows="1" />
                                            </td>
                                            <td>

                                            </td>
                                        </tr>
                                        <asp:Repeater ID="rptTranslations" runat="server" OnItemDataBound="rptTranslations_ItemDataBound">
                                            <ItemTemplate>
                                                <tr>
                                                    <td>
                                                        <asp:HiddenField ID="hfLanguageCode" runat="server" />
                                                        <asp:Label ID="lblDescription" runat="server" />
                                                    </td>
                                                    <td style="vertical-align: top;">
                                                        <asp:TextBox ID="tbTranslation" runat="server" Width="100%" TextMode="MultiLine" Rows="1" />
                                                    </td>
                                                    <td style="width: 30px; vertical-align: top; padding-left: 9px;">
                                                        <img id="imgTranslate" runat="server" src="images/google_translate.png" style="height: 24px;" />
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                        <tr>
                                            <td>

                                            </td>
                                            <td>
                                                <asp:Button ID="btnSaveTerm" runat="server" Text="Save" OnClick="btnSaveTerm_Click" CssClass="btnSmall" AccessKey="S" />
                                                <asp:Button ID="btnCancelTerm" runat="server" Text="Cancel" OnClick="btnCancelTerm_Click" CssClass="btnSmall" Visible="false" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
