using System;
using System.Windows.Forms;
using Microsoft.Toolkit.Uwp.Notifications;

class Program
{
    private static bool hasNotified = false;
    private static double lastBatteryLevel = 0;

    static void Main()
    {
        while (true)
        {
            try
            {
                PowerStatus status = SystemInformation.PowerStatus;
                int batteryLevel = (int)(status.BatteryLifePercent * 100);

                if (batteryLevel == 80 && lastBatteryLevel < 80 && !hasNotified)
                {
                    ShowNotification("Battery Alert", $"Battery level has reached {batteryLevel}%");
                    hasNotified = true;
                }
                else if (batteryLevel < 50)
                {
                    hasNotified = false;
                }

                lastBatteryLevel = batteryLevel;

                Console.Clear();
                Console.WriteLine("=== Battery Information ===");
                Console.WriteLine($"Battery Level: {batteryLevel}%");
                Console.WriteLine($"Battery Status: {GetBatteryStatus(status.BatteryChargeStatus)}");
                Console.WriteLine($"Power Source: {(status.PowerLineStatus == PowerLineStatus.Online ? "AC Power" : "Battery")}");
                Console.WriteLine("========================");
                Console.WriteLine("Notification will appear when battery reaches 50%");
                
                System.Threading.Thread.Sleep(10000); // Update every 10 seconds
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }

    static string GetBatteryStatus(BatteryChargeStatus status)
    {
        if (status.HasFlag(BatteryChargeStatus.Charging))
            return "Charging";
        else if (status.HasFlag(BatteryChargeStatus.Critical))
            return "Critical";
        else if (status.HasFlag(BatteryChargeStatus.Low))
            return "Low";
        else if (status.HasFlag(BatteryChargeStatus.High))
            return "High";
        else
            return "Normal";
    }

    static void ShowNotification(string title, string message)
    {
        new ToastContentBuilder()
            .AddText(title)
            .AddText(message)
            .Show();
    }
}