using AgentInputCodeExecutor.API.Entities;
using AgentInputCodeExecutor.API.Interfaces;
using AgentInputCodeExecutor.API.Service.Command;
using Interfaces.DynamicAgent;
using MediatR;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentInputCodeExecutor.API.Tests.UnitTests.Command
{
    public class ExecuteCodeLinesCommandTests
    {
        private Mock<ICodeResolveService> codeResolver;
        private Mock<IMediator> mediator;
        readonly CancellationToken token;
        private readonly string testCommandWithoutArgsName = "Test";

        private readonly Dictionary<string, IProperty> localVars;

        public ExecuteCodeLinesCommandTests()
        {
            localVars = new Dictionary<string, IProperty>();
            var tokenSource = new CancellationTokenSource(TimeSpan.FromMilliseconds(100));
            token = tokenSource.Token;
        }


        [Fact]
        public async void NextCodeLineMustUsePrevLocalValuesTest()
        {
            double testVal1 = 45;
            double testVal2 = 5;
            double testVal3 = 50;
            string testCommand = "Test";
            string testCommand2 = "TestLastCommand";
            string testResultName = "c";

            ExecutableAgentCodeSettings sets = new ExecutableAgentCodeSettings(new()
            {
                $"a = {testVal1}",
                $"b = {testCommand}()",
                $"{testResultName} = {testCommand2}(a,b,{testVal3}"
            }, localVars);

            ExecuteCodeLinesCommand command = new ExecuteCodeLinesCommand()
            {
                Settings = sets
            };

            mediator = new Mock<IMediator>() { CallBase = true };
            
            await new ExecuteCodeLinesCommandHandler(mediator.Object).Handle(command, token);

            Assert.True(command.Settings.Properties.ContainsKey(testResultName));
            Assert.True(command.Settings.Properties[testResultName].Type == typeof(int));
            Assert.True((int)command.Settings.Properties[testResultName].Value == testVal1 + testVal2 + testVal3);
        }

    }
}
