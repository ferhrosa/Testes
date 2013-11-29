<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CheckBoxListValidation.aspx.cs" Inherits="WebFormsTestes.CheckBoxListValidation" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN" "http://www.w3.org/TR/html4/loose.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>

            <asp:CheckBoxList ID="cblPais" runat="server">
                <asp:ListItem>Alemanha</asp:ListItem>
                <asp:ListItem>Argentina</asp:ListItem>
                <asp:ListItem>Brasil</asp:ListItem>
                <asp:ListItem>Inglaterra</asp:ListItem>
                <asp:ListItem>Itália</asp:ListItem>
                <asp:ListItem>Japão</asp:ListItem>
            </asp:CheckBoxList>

        </div>
    </form>

    <script type="text/javascript">

        var cblPais = document.getElementById("<%= cblPais.ClientID %>");

        var checkBoxes = cblPais.getElementsByTagName("input");

        for (var i = 0; i < checkBoxes.length; i++)
        {
            var checkBox = checkBoxes[i];

            if (checkBox.attachEvent)
                checkBox.attachEvent('onclick', limitaSelecao);
            else
                checkBox.addEventListener("click", limitaSelecao);
        }

        function limitaSelecao(e)
        {
            var checkBox = e.srcElement;

            if (checkBox.checked)
            {
                var qtde = 0;

                for (var i = 0; i < checkBoxes.length; i++)
                {
                    if (checkBoxes[i].checked)
                        qtde++;
                }

                if (qtde > 3)
                {
                    checkBox.removeAttribute("checked");
                    checkBox.checked = false;
                    return false;
                }
            }
        }

    </script>

</body>
</html>
