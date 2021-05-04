using System;
using Microsoft.Extensions.DependencyInjection;
using Terminal.Core;
using Terminal.Core.Extensions;

namespace Terminal
{
    public static class Program
    {
        private const string ConsoleName = "Transactor";

        private static Application Init()
        {
            Console.Title = ConsoleName;

            return new ServiceCollection()
                .Configure()
                .BuildServiceProvider()
                .GetService<Application>();
        }

        public static void Main(string[] args) => Init().Run();
    }
}
