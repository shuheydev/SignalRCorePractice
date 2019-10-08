using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SignalRClientWpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        HubConnection connection;

        public MainWindow()
        {
            InitializeComponent();

            //接続のためのHubConnectionのインスタンスを作成する.
            connection = new HubConnectionBuilder()
                .WithUrl(@"https://localhost:44350/chathub")//サーバー側でMapHubで指定したURLを指定する.
                .Build();
        }

        //接続ボタンが押されたときの処理
        private async void connectButton_Click(object sender, RoutedEventArgs e)
        {
            //サーバー側から呼び出される処理を定義
            //サーバー側からReceiveを指定して呼び出しがあったときに,
            //登録したDelegateが実行される.
            connection.On<string>("Receive", (message) =>
            {
                //Dispatcher使っている理由は?
                //UIに直接書き込むのであればUIスレッドで行う必要があるため
                this.Dispatcher.Invoke(() =>
                {
                    var newMessage = $"{message}";
                    messagesList.Items.Add(newMessage);
                });
            });

            try
            {
                //接続
                await connection.StartAsync();
                messagesList.Items.Add("Connection started");
            }
            catch (Exception ex)
            {
                messagesList.Items.Add(ex.Message);
            }
        }

        //送信ボタンが押されたときの処理
        private async void sendButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //メッセージを送信
                //サーバー側のSendメソッドを呼び出し,TextBoxの文字列を送る.
                //サーバー側のSendメソッドは2つのstringを受け取る
                await connection.InvokeAsync("send", messageTextBox.Text, "WPF");
            }
            catch (Exception ex)
            {
                messagesList.Items.Add(ex.Message);
            }
        }
    }
}
