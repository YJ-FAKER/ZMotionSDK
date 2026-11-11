using System;
using ZMotionSDK;
using ZMotionSDK.Models;

internal static class Program
{
    private static int _passed;

    private static void Assert(bool condition, string description)
    {
        if (!condition) throw new Exception("FAIL: " + description);
        _passed++;
    }

    private static void Throws<T>(Action action, string description) where T : Exception
    {
        try { action(); }
        catch (T) { _passed++; return; }
        throw new Exception("FAIL: " + description);
    }

    private static void Main()
    {
        // No controller connection or motion commands in this test.
        var home = AxisStatus.Parse(1 << 6);
        Assert(home.IsFindingHome && !home.HasFault && !home.HasMotionFailure,
            "Homing must not trigger a servo alarm.");

        var alarm = AxisStatus.Parse(1 << 3);
        Assert(alarm.Alarm_Axis && alarm.HasFault && alarm.HasMotionFailure,
            "Servo alarm should be classified as a fault.");
        Assert(AxisStatus.Parse(1 << 11).HasMotionFailure,
            "Cancelled motion cannot be reported as successful.");

        Assert(0xFFFFFFFFu.SetBit(31, false) == 0x7FFFFFFFu,
            "Unsigned bit 31 should clear without sign extension.");
        Assert(0u.SetBit(31, true) == 0x80000000u, "Unsigned bit 31 should set.");
        Throws<ArgumentOutOfRangeException>(() => 1u.SetBit(32, true),
            "Out-of-range bit index must fail.");

        var motion = new ZMotion();
        Assert(motion.Handle == IntPtr.Zero, "New instance should be disconnected.");
        motion.Close();
        motion.Close();
        Assert(motion.Handle == IntPtr.Zero, "Repeated Close should be safe.");
        Throws<ArgumentOutOfRangeException>(() => motion.GetDI_Multi(-1, 3),
            "Negative I/O addresses must be rejected.");
        Throws<ArgumentOutOfRangeException>(() => motion.GetDO_Multi(8, 7),
            "Reversed I/O range must be rejected.");
        Throws<ArgumentException>(() => motion.SetDO_Multi(0, new bool[0]),
            "Empty output write must be rejected.");
        Throws<ArgumentOutOfRangeException>(() => motion.SetDO_Multi(65535, new bool[2]),
            "Overflowing output address range must be rejected.");
        motion.Dispose();
        motion.Dispose();
        Throws<ObjectDisposedException>(() => motion.Open_Eth("127.0.0.1"),
            "Cannot reopen a disposed instance.");
        Console.WriteLine("PASS: " + _passed + " checks on " + System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription);
    }
}