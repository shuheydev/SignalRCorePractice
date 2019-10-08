using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;
using System.Windows;

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
                .WithAutomaticReconnect()
                .Build();

            //サーバー側から呼び出される処理を定義
            //サーバー側からReceiveを指定して呼び出しがあったときに,
            //登録したDelegateが実行される.
            //複数回これを実行すると複数登録されるので注意.
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


            //接続状態のイベント
            connection.Closed += (message) =>
            {
                this.Dispatcher.Invoke(() =>
                {
                    messagesList.Items.Add($"Closed::");
                });

                return Task.CompletedTask;
            };

            connection.Reconnecting += (message) =>
            {
                this.Dispatcher.Invoke(() =>
                {
                    messagesList.Items.Add($"Reconnecting::{message.Message}");
                });

                return Task.CompletedTask;
            };

            connection.Reconnected += (message) => {
                this.Dispatcher.Invoke(() => {
                    messagesList.Items.Add($"Reconnected::{message}");
                });

                return Task.CompletedTask;
            };
        }

        //接続ボタンが押されたときの処理
        private async void connectButton_Click(object sender, RoutedEventArgs e)
        {
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

        private async void disconnectButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await connection.StopAsync();
                connection.Remove("Receive");
            }
            catch (Exception ex)
            {
                messagesList.Items.Add(ex.Message);
            }
        }
    }
}
