using AgentInputCodeExecutor.API.Entities;
using AgentInputCodeExecutor.API.Interfaces;
using AgentInputCodeExecutor.API.Service.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentInputCodeExecutor.API.Tests.UnitTests.Command
{
    public class GetCommandNameCommandTests
    {
        CancellationToken token;

        public GetCommandNameCommandTests()
        {
            var tokenSource = new CancellationTokenSource(TimeSpan.FromMilliseconds(100));
            token = tokenSource.Token;
        }



        [Fact]
        public async void GetCommandNameWithoutCommandMustReturnNull()
        {
            ICommand testCommand = new ExecutableCommand("a = 1 + 5", CommandType.Assigning);
            Assert.Null(await new GetCommandNameCommandHandler().Handle(new GetCommandNameCommand(testCommand), token));
        }


        [Fact]
        public async Task GetCommandNameTest()
        {
            string commandName = "Test";
            ICommand testCommand = new ExecutableCommand($"a = {commandName}()", CommandType.Assigning);
            string name = await new GetCommandNameCommandHandler().Handle(new GetCommandNameCommand(testCommand), token);
            Assert.Equal(commandName, name);
        }
    }
}
