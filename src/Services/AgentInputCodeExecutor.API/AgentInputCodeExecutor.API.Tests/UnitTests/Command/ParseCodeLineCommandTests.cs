using AgentInputCodeExecutor.API.Interfaces;
using AgentInputCodeExecutor.API.Service.Command;
using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentInputCodeExecutor.API.Tests.UnitTests.Command
{
    public class ParseCodeLineCommandTests
    {
        CancellationToken token;

        public ParseCodeLineCommandTests()
        {
            var tokenSource = new CancellationTokenSource(TimeSpan.FromMilliseconds(100));
            token = tokenSource.Token;
        }


        [Fact]
        public async Task ParseAssigningCommandMustReturnAssigningType()
        {
            string testLine = "testVar=TestCall()";
            ICommand command = await new ParseCodeLineCommandHandler().Handle(new ParseCodeLineCommand(testLine), token);
            Assert.True(command.CommandType == CommandType.Assigning);
        }


        [Fact]
        public async Task ParseVoidCommandMustReturnVoidType()
        {
            string testLine = "TestCall()";
            ICommand command = await new ParseCodeLineCommandHandler().Handle(new ParseCodeLineCommand(testLine), token);
            Assert.True(command.CommandType == CommandType.VoidCall);
        }


        [Fact]
        public async Task ParseInternalSystemAssigningParamMustSetParametersName()
        {
            string testLine = "Age = TestCall()";
            string testLine1 = "Weight = 45";
            ICommand command = await new ParseCodeLineCommandHandler().Handle(new ParseCodeLineCommand(testLine), token);
            Assert.True(command.AssigningParameter == ParameterNames.Age);

            command = await new ParseCodeLineCommandHandler().Handle(new ParseCodeLineCommand(testLine1), token);
            Assert.True(command.AssigningParameter == ParameterNames.Weight);
        }


        [Fact]
        public async Task ParseExternalSystemAssigningParamMustSetParametersNameToNone()
        {
            string testLine = "externalParameter = TestCall()";
            ICommand command = await new ParseCodeLineCommandHandler().Handle(new ParseCodeLineCommand(testLine), token);
            Assert.True(command.AssigningParameter == ParameterNames.None);
        }
    }
}
