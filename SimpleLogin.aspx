<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SimpleLogin.aspx.cs" Inherits="first.Login.SimpleLogin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    Статус: <asp:Literal ID="LStatus" runat="server"></asp:Literal>
    <table>
        <thead>
            <tr>
                <th colspan="2">Войти</th>
            </tr>
        </thead>
        <tbody>
            <tr>
                <td>Логин</td>
                <td><asp:TextBox runat="server" ID="TBLogin"></asp:TextBox></td>
            </tr>
            <tr>
                <td>Пароль</td>
                <td><asp:TextBox runat="server" ID="TBPassword" TextMode="Password"></asp:TextBox></td>
            </tr>
            <tr>
                <td colspan="2"><asp:Button runat="server" ID="BEnter" OnClick="BEnter_Click" Text="Войти" /></td>
            </tr>
        </tbody>
    </table>

    <table>
        <thead>
            <tr>
                <th colspan="2">Зарегистрироваться</th>
            </tr>
        </thead>
        <tbody>
            <tr>
                <td>Логин</td>
                <td><asp:TextBox id="TBRegLogin" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td>Пароль</td>
                <td><asp:TextBox ID="TBRegPassword" runat="server" TextMode="Password"></asp:TextBox></td>
            </tr>
            <tr>
                <td>Email</td>
                <td><asp:TextBox ID="TBEmail" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td colspan="2"><asp:Button runat="server" ID="BRegistrate" OnClick="BRegistrate_Click" Text="Зарегистрироваться" /></td>
            </tr>
        </tbody>
    </table>

    <asp:Button ID="BExit" runat="server" Text="Выйти" OnClick="BExit_Click" /><br />

    Ошибки: <asp:Literal id="LErrs" runat="server"></asp:Literal><br />
    
    Расшифрованный токен
    <div>
        <asp:Literal ID="LOpenToken" runat="server"></asp:Literal>
    </div>
    Валидация по токену
    <div>
        <asp:Literal ID="LValid" runat="server"></asp:Literal>
    </div>
</asp:Content>
