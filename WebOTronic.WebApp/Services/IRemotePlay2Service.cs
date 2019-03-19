using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;
using WebOTronic.WebApp.Models.RemotePlay2;

namespace WebOTronic.WebApp.Services
{
    public interface IRemotePlay2Service
    {
        void LeftPaddle(string direction);
        void RightPaddle(string direction);
        void Run();
        World Register(string leftPaddleId, string rightPaddleId);
  }
}