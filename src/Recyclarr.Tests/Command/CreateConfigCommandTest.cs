﻿using System.IO.Abstractions;
using CliFx.Infrastructure;
using NSubstitute;
using NUnit.Framework;
using Recyclarr.Command;
using Serilog;

// ReSharper disable MethodHasAsyncOverload

namespace Recyclarr.Tests.Command;

[TestFixture]
[Parallelizable(ParallelScope.All)]
public class CreateConfigCommandTest
{
    [Test]
    public async Task CreateConfig_DefaultPath_FileIsCreated()
    {
        var logger = Substitute.For<ILogger>();
        var filesystem = Substitute.For<IFileSystem>();
        var cmd = new CreateConfigCommand(logger, filesystem);

        await cmd.ExecuteAsync(Substitute.For<IConsole>()).ConfigureAwait(false);

        filesystem.File.Received().Exists(Arg.Is<string>(s => s.EndsWith("recyclarr.yml")));
        filesystem.File.Received().WriteAllText(Arg.Is<string>(s => s.EndsWith("recyclarr.yml")), Arg.Any<string>());
    }

    [Test]
    public async Task CreateConfig_SpecifyPath_FileIsCreated()
    {
        var logger = Substitute.For<ILogger>();
        var filesystem = Substitute.For<IFileSystem>();
        var cmd = new CreateConfigCommand(logger, filesystem)
        {
            Path = "some/other/path.yml"
        };

        await cmd.ExecuteAsync(Substitute.For<IConsole>()).ConfigureAwait(false);

        filesystem.File.Received().Exists(Arg.Is("some/other/path.yml"));
        filesystem.File.Received().WriteAllText(Arg.Is("some/other/path.yml"), Arg.Any<string>());
    }
}
