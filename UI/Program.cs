using System;
using Airbnb.Services;

class Program
{
    static void Main(string[] args)
    {
        var service = new BookingService();
        service.Start();
    }
}