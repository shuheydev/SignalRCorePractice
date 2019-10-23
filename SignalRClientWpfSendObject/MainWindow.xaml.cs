using Microsoft.AspNetCore.SignalR.Client;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace SignalRClientWpfSendObject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private HubConnection _connection;

        public MainWindow()
        {
            InitializeComponent();

            var context=SynchronizationContext.Current;
            _connection = new HubConnectionBuilder()
                 .WithUrl(@"https://localhost:44350/chathub")
                 .WithAutomaticReconnect()
                 .Build();

            _connection.On<Message>("ReceiveObject", (message) => {
                this.Dispatcher.Invoke(()=> {
                    messagesList.Items.Add($"{message.Body}_{message.SenderName}_{message.SendDateTime.ToString()}");
                });
            });
        }

        private async void connectButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await _connection.StartAsync();
                this.Dispatcher.Invoke(()=> {
                    messagesList.Items.Add("Connection started");
                });
            }
            catch(Exception ex)
            {
                this.Dispatcher.Invoke(()=> {
                    messagesList.Items.Add(ex.Message);
                });
            }
        }

        private async void sendButton_Click(object sender, RoutedEventArgs e)
        {
            //送信するオブジェクトを作成
            var message = new Message {
                SenderName = "WPF",
                Body = messageTextBox.Text,
                SendDateTime = DateTimeOffset.Now,
            };

            //送信
            try
            {
                await _connection.InvokeAsync("SendObject",message);
            }
            catch(Exception ex)
            {
                this.Dispatcher.Invoke(()=> {
                    messagesList.Items.Add(ex.Message);
                });
            }
        }

        private async void disconnectButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await _connection.StopAsync();
                _connection.Remove("ReceiveMessage");
            }
            catch (Exception ex)
            {
                messagesList.Items.Add(ex.Message);
            }
        }
    }
}
