using System;

public class Class1
{
	public Class1()
	{
        using Airbnb.Services;

        var service = new BookingService();
        service.Start();
    }
}
