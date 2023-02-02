using AgentInputCodeExecutor.API.Entities;
using AgentInputCodeExecutor.API.Interfaces;
using AgentInputCodeExecutor.API.Service.Command;
using AgentInputCodeExecutor.API.Service.Queue;
using AgentInputCodeExecutor.API.Service.Service;
using Interfaces;
using Interfaces.DynamicAgent;
using MediatR;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
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

        private readonly Dictionary<string, IProperty> localVars;


        public ExecuteCodeLineCommandTests()
        {
            localVars = new Dictionary<string, IProperty>();
            var tokenSource = new CancellationTokenSource(TimeSpan.FromMilliseconds(100));
            token = tokenSource.Token;
        }


        public static int GetTestVal() => int.MaxValue;
       

        public static double GetTestDouble() =>  45;


        [Fact]
        public async Task ExecuteAssigningVariableMustBeSettedInCommandDictByOriginalName()
        {
            ParameterNames testParamName = ParameterNames.Weight;
            string testParamStr = testParamName.ToString();
            Type testType = typeof(double);
            MethodInfo testMethod = typeof(ExecuteCodeLineCommandTests).GetMethod("GetTestDouble");

            ICommand testCommand =
                new ExecutableCommand($"{testParamStr} = {testCommandWithoutArgsName}()", CommandType.Assigning, localVars, testParamStr, testParamName);
            ExecuteCodeLineCommand testCodeLineCommand = new ExecuteCodeLineCommand(testCommand);
            ICommandArgsTypesMeta meta = new CommandArgsTypesMeta(new List<(Type, string)> { }, testType);

            mediator = new Mock<IMediator>();
            mediator.Setup(x => x.Send(It.IsAny<GetCommandArgsValuesQueue>(), token)).ReturnsAsync(() => new List<object>());

            codeResolver = new Mock<ICodeResolveService>();
            codeResolver.Setup(x => x.ResolveCommandAction(It.IsAny<ICommand>(), token)).ReturnsAsync((meta, Delegate.CreateDelegate(typeof(Func<double>), testMethod)));

            await new ExecuteCodeLineCommandHandler(codeResolver.Object, mediator.Object).Handle(testCodeLineCommand, token);

            Assert.True(testCodeLineCommand.Command.LocalVariables.ContainsKey(testParamName.ToString()));
        }


        [Fact]
        public async void ExecuteWithoutCommandTest()
        {
            ParameterNames testParamName = ParameterNames.Weight;
            string testParamStr = testParamName.ToString();

            int testVal1 = 1;
            int testVal2 = 45;

            ICommand testCommand =
                 new ExecutableCommand($"{testParamStr} = {testVal1} + {testVal2}", CommandType.Assigning, localVars, testParamStr,testParamName);
            ExecuteCodeLineCommand testCodeLineCommand = new ExecuteCodeLineCommand(testCommand);

            mediator = new Mock<IMediator>();
            codeResolver = new Mock<ICodeResolveService>();

            await new ExecuteCodeLineCommandHandler(codeResolver.Object, mediator.Object).Handle(testCodeLineCommand, token);

            Assert.True(testCodeLineCommand.Command.LocalVariables.ContainsKey(testParamStr));
            Assert.Equal(testCodeLineCommand.Command.LocalVariables[testParamStr].Value, testVal1 + testVal2);

        }


        [Fact]
        public async void ExecuteWithoutCommandAndWithLocalVariableTest()
        {
            ParameterNames testParamName = ParameterNames.Weight;
            string testParamStr = testParamName.ToString();
            int testVal1 = 1;
            int testVal2 = 45;
            string testVal1Param = "testFirst";
            string testVal2Param = "testSecond";
            localVars["testSecond"] = new AgentProperty(typeof(int), testVal2, testVal2Param);
            localVars["testFirst"] = new AgentProperty(typeof(int), testVal1, testVal1Param);
            ICommand testCommand =
                 new ExecutableCommand($"{testParamStr} = testFirst + testSecond", CommandType.Assigning, localVars, testParamStr, testParamName);
            ExecuteCodeLineCommand testCodeLineCommand = new ExecuteCodeLineCommand(testCommand);

            mediator = new Mock<IMediator>();
            codeResolver = new Mock<ICodeResolveService>();

            await new ExecuteCodeLineCommandHandler(codeResolver.Object, mediator.Object).Handle(testCodeLineCommand, token);

            Assert.True(testCodeLineCommand.Command.LocalVariables.ContainsKey(testParamStr));
            Assert.Equal(testCodeLineCommand.Command.LocalVariables[testParamStr].Value, testVal1 + testVal2);
        }


        [Fact]
        public async Task ExecuteWithUnknownCommandMustThrowAsync()
        {
            ParameterNames testParamName = ParameterNames.Weight;
            string testParamStr = testParamName.ToString();
            Type testType = typeof(double);
            MethodInfo testMethod = typeof(ExecuteCodeLineCommandTests).GetMethod("GetTestDouble");

            ICommand testCommand =
                new ExecutableCommand($"{testParamStr} = {testCommandWithoutArgsName}()", CommandType.Assigning, localVars, testParamStr, testParamName);
            
            ExecuteCodeLineCommand testCodeLineCommand = new ExecuteCodeLineCommand(testCommand);
            ICommandArgsTypesMeta meta = new CommandArgsTypesMeta(new List<(Type, string)> { }, testType);

            mediator = new Mock<IMediator>();
            mediator.Setup(x => x.Send(It.IsAny<GetCommandNameCommand>(), token)).ReturnsAsync(() => testCommandWithoutArgsName);

            var resolver = new CodeResolveService(mediator.Object, new Mock<IWebRequester>().Object);

            await Assert.ThrowsAsync<ResolveCommandActionException>
                (async () => await new ExecuteCodeLineCommandHandler(resolver, mediator.Object).Handle(testCodeLineCommand, token));
        }


        [Fact]
        public void ExecuteWithIncorrectAgrsMustThrow()
        {
            throw new NotImplementedException();
        }


        [Fact]
        public void ExecuteAssigningInternalVarOfTypeListMustBeSave()
        {
            throw new NotImplementedException();
        }
    }
}
