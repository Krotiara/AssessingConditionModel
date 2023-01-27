using AgentInputCodeExecutor.API.Entities;
using AgentInputCodeExecutor.API.Interfaces;
using AgentInputCodeExecutor.API.Service.Command;
using AgentInputCodeExecutor.API.Service.Queue;
using Interfaces;
using MediatR;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AgentInputCodeExecutor.API.Tests.UnitTests.Command
{
    public class ExecuteCodeLineCommandTests
    {
        private Mock<ICodeResolveService> codeResolver;
        private Mock<IMediator> mediator;
        private Mock<IWebRequester> webRequester;
        CancellationToken token;

        private readonly string testCommandWithoutArgsName = "Test";

        private readonly Dictionary<string, object> localVars;

        private readonly Dictionary<ParameterNames, AgentProperty> props;


        public ExecuteCodeLineCommandTests()
        {
            localVars = new Dictionary<string, object>();
            props = new Dictionary<ParameterNames, AgentProperty>();
            var tokenSource = new CancellationTokenSource(TimeSpan.FromMilliseconds(100));
            token = tokenSource.Token;
        }


        [Fact]
        public async void ExecuteAssigningVariableMustBeSettedInCommandDictByOriginalName()
        {
            string testVar = "a";
            int testValue = 45;
            Type testType = typeof(int);

            ICommand testCommand = 
                new ExecutableCommand($"{testVar} = {testCommandWithoutArgsName}()", CommandType.Assigning, ParameterNames.None, testVar);
            ExecuteCodeLineCommand testCodeLineCommand = new ExecuteCodeLineCommand(testCommand, props, localVars);
            CommandArgsTypesMeta meta = new CommandArgsTypesMeta(new List<(Type, string)> { (testType, "arg1") }, testType);

            mediator = new Mock<IMediator>();
            mediator.Setup(x => x.Send(new GetCommandNameCommand(It.IsAny<ICommand>()), token)).ReturnsAsync(testCommandWithoutArgsName);
            mediator.Setup(x => x.Send(new GetCommandTypesMetaQueue(It.IsAny<string>()), token)).ReturnsAsync(new CommandArgsTypesMeta(new List<(Type, string)> {}, testType));
            mediator.Setup(x => x.Send(new GetCommandArgsValuesQueue(It.IsAny<string>(), It.IsAny<ICommandArgsTypesMeta>(), localVars), token)).ReturnsAsync(new List<object> { testValue });

            codeResolver = new Mock<ICodeResolveService>();
            codeResolver.Setup(x=>x.ResolveCommandAction(It.IsAny<ICommand>())).ReturnsAsync((meta, Delegate.CreateDelegate(typeof(Func<int>), typeof(ExecuteCodeLineCommandTests).GetMethod("GetTestVal"))));

            await new ExecuteCodeLineCommandHandler(codeResolver.Object, mediator.Object).Handle(testCodeLineCommand, token);

            Assert.True(localVars.ContainsKey(testVar));

        }


        public static int GetTestVal()
        {
            return int.MaxValue;
        }


        [Fact]
        public void ExecuteAssigningInternalVarMustBeSettedInCommandDict()
        {

        }


        [Fact]
        public void ExecuteVoidCommandMustBeCalled()
        {

        }


        [Fact]
        public void ExecuteCommandWithoutCommandTest()
        {

        }


        [Fact]
        public void ExecuteCommandWithUnknownCommandMustThrow()
        {

        }


        [Fact]
        public void ExecuteCommandWithIncorrectAgrsMustThrow()
        {

        }





    }
}
