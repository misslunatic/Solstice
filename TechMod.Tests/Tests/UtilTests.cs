using Discord;
using Discord.WebSocket;
using Moq;
using TechMod.Classes.Util;

namespace TechMod.Tests.Tests;

public class UtilTests
{


    [Fact]
    public void ProgressBarTest()
    {
        var dt = DateTime.Now;
        for (var i = 0; i < Config.BarLength; i++)
        {
            var barText = Utils.GetProgressBar(dt, dt.AddMinutes(Config.BarLength), dt.AddMinutes(i), "X", "-");
            Assert.True(barText.Where(p => p == 'X').Count() == i, $"Progress bar {i}/{Config.BarLength}, filled ({barText})");
            Assert.True(barText.Where(p => p == '-').Count() == Config.BarLength - i, $"Progress bar {i}/{Config.BarLength}, empty ({barText})");

        }
    }
}