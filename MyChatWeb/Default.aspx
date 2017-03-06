<%@ Page Title="MyChat" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="MyChatWeb._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <link href="Content/w3.css" rel="stylesheet" />

    <div class="jumbotron">
        <h2>MyChat</h2>
        <p class="lead">Choose a Chat File to convert to JSON</p>
    </div>

    <%--First Row - Filter Options--%>
    <div class="row">

        <%--file Upload--%>
        <div class="col-md-4" style="width: auto">
            <h3>Choose the File here</h3>
            <br>
            <p>
                <asp:FileUpload ID="FileUpload1" runat="server" Width="100%" ToolTip="Choose File to upload and convert" BorderColor="#CCCCCC" BorderWidth="1px" />
            </p>
        </div>

        <%--Filters to apply--%>
        <div class="col-md-4" style="width: auto">
            <h3>Choose some Filter</h3>
            <br>

            <table class="w3-table" style="width: 100%;">
                <tr>
                    <td>
                        <asp:CheckBox class="w3-check" ID="obfuscateUser" runat="server" Text="Obfuscated User" /></td>
                </tr>
                <tr>
                    <td>
                        <asp:CheckBox class="w3-check" ID="creditCard" runat="server" Text="Hide Credit Card" /></td>
                </tr>
                <tr>
                    <td>
                        <asp:CheckBox class="w3-check" ID="phoneNumber" runat="server" Text="Hide Phonenumber" /></td>
                </tr>
            </table>
        </div>

        <div class="col-md-4" style="width: auto">
            <br>
            <br>
            <br>

            <table style="width: 100%;">
                <tr>
                    <td>User Filter</td>
                    <td>
                        <asp:TextBox ID="userFilter" runat="server" BorderColor="#CCCCCC" BorderWidth="1px" /></td>
                </tr>
                <tr>
                    <td>Including Words</td>
                    <td>
                        <asp:TextBox ID="includingWords" runat="server" BorderColor="#CCCCCC" BorderWidth="1px" /></td>
                </tr>
                <tr>
                    <td>Excluding Words</td>
                    <td>
                        <asp:TextBox ID="excludingWords" runat="server" BorderColor="#CCCCCC" BorderWidth="1px" /></td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>Blacklist</td>
                    <td>
                        <asp:TextBox ID="blackList" runat="server" BorderColor="#CCCCCC" BorderWidth="1px" /></td>
                    <td>&nbsp;</td>
                </tr>
            </table>
        </div>

        <%--Fire the Upload to the Server--%>
        <div class="col-md-4" style="width: auto">
            <h3>...and finally start here</h3>
            <br>
            <p>
                <asp:Button ID="Button2" class="w3-btn w3-blue" runat="server" Text="Upload -->" OnClick="uploadFile" />
            </p>
        </div>
    </div>

    <hr>

    <%--second row - Chats & Pie Chart--%>
    <div class="row">
        <div class="col-md-4" style="width: 50%; height: auto;">
            <%--Chat Table--%>
            <br />
            <h2>Chats</h2>
            <br />
            <asp:Table ID="Table1" runat="server"></asp:Table>
        </div>

        <div class="col-md-4" style="width: 50%; height: auto;">
            <%--Pie Chart--%>
            <br />
            <h2>Chat Activity</h2>
            <br />
            <div id='chart_div2' style='width: 100%; height: 90%;'></div>
        </div>
    </div>

    <hr>
    <br>

    <%--Third Row - Original File & converted File--%>
    <div class="row">

        <%--Original File--%>
        <div class="col-md-4" style="width: 50%; height: auto;">
            <h3>Upload-Content looks like</h3>
            <br>
            <p>
                <asp:Label ID="Label1" runat="server" Text="no File, please select one" Style="width: 100%; height: auto;"></asp:Label>
            </p>
        </div>

        <%--Converted JSON File--%>
        <div class="col-md-4" style="width: 50%; height: auto;">
            <h3>JSON-Content looks like</h3>
            <br>
            <p>
                <asp:Label ID="Label2" runat="server" Text="no File, please select one" Style="width: 100%; height: auto;"></asp:Label>
            </p>
        </div>
    </div>

    <%----------------------------------------------%>
    <%--SCRIPTS                                   --%>
    <%----------------------------------------------%>

    <%--Setup Pie Chart--%>
    <script type="text/javascript" src="https://www.gstatic.com/charts/loader.js"></script>
    <script type='text/javascript'> google.charts.load('current', { 'packages': ['corechart'] });

        google.charts.setOnLoadCallback(drawChatChart);

        function drawChatChart() {
            var data = google.visualization.arrayToDataTable([['User', 'Chats'], [<%=_chartData%>]);
            var options = { title: '', 'is3D':true, 'legend':{position: 'right', alignment: 'labeled'}, 'width':600,'height':300, chartArea:{left:10,top:0,width:'75%',height:'75%'} };
            var chart = new google.visualization.PieChart(document.getElementById('chart_div2'));
            chart.draw(data, options);
        };

    </script>
</asp:Content>
