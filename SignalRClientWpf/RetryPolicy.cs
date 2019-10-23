using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;

namespace SignalRCorePractice
{
    public class RandomRetryPolicy : IRetryPolicy
    {
        private readonly Random _random = new Random();

        public TimeSpan? NextRetryDelay(RetryContext retryContext)
        {
            //2~5秒の間でランダムに再接続を試みる
            return TimeSpan.FromSeconds(_random.Next(2,5));
        }
    }
}
