<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebAspUpdatePanel.Default" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    
    <div class="row">
        <asp:Label runat="server" Text="Fora: " ID="lblTempoGeral"></asp:Label>
        <asp:Button ID="btn" runat="server" CssClass="btn-primary" Text="Atualizar" />
    </div>
    <div class="row jumbotron">
      <asp:Timer ID="Timer1" runat="server" Interval="1000" OnTick="Timer1_Tick"  ViewStateMode=Enabled></asp:Timer>

        <asp:UpdatePanel ID="upd1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                  
                <div class="row">
                    <asp:Label runat="server" Text="Dentro: " ID="lblTempo"></asp:Label>
                </div>

                <div class="row">
                    <asp:Chart ID="Chart1" runat="server">                       
                    </asp:Chart>
                </div>

            </ContentTemplate>

            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="Timer1" EventName="Tick"   />
            </Triggers>
        </asp:UpdatePanel>

    </div>

</asp:Content>
