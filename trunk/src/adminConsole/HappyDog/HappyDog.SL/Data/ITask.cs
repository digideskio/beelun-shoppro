using System;
using HappyDog.SL.EventArguments;

namespace HappyDog.SL.Data
{
    public interface ITask
    {
        event EventHandler<HDCallBackEventArgs> onComplete;
        void StartAsync(object param, int id);
        string TaskDescription { get; set; }
    }
}