using AgentInputCodeExecutor.API.Entities;
using AgentInputCodeExecutor.API.Interfaces;
using AgentInputCodeExecutor.API.Service.Command;
using AgentInputCodeExecutor.API.Service.Queue;
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


        [Fact]
        public async void ExecuteAssigningVariableMustBeSettedInCommandDictByOriginalName()
        {
            string testVar = "a";
            Type testType = typeof(int);

            MethodInfo testMethod = typeof(ExecuteCodeLineCommandTests).GetMethod("GetTestVal");

            ICommand testCommand = 
                new ExecutableCommand($"{testVar} = {testCommandWithoutArgsName}()", CommandType.Assigning, localVars, testVar);
            ExecuteCodeLineCommand testCodeLineCommand = new ExecuteCodeLineCommand(testCommand);
            ICommandArgsTypesMeta meta = new CommandArgsTypesMeta(new List<(Type, string)> {}, testType);

            GetCommandArgsValuesQueue queue = new GetCommandArgsValuesQueue(testCommand, meta);

            mediator = new Mock<IMediator>();
            mediator.Setup(x => x.Send(It.IsAny<GetCommandArgsValuesQueue>(), token)).ReturnsAsync(() => new List<object>());
           
            codeResolver = new Mock<ICodeResolveService>();
            codeResolver.Setup(x=>x.ResolveCommandAction(It.IsAny<ICommand>())).ReturnsAsync((meta, Delegate.CreateDelegate(typeof(Func<int>), testMethod)));

            await new ExecuteCodeLineCommandHandler(codeResolver.Object, mediator.Object).Handle(testCodeLineCommand, token);

            Assert.True(testCodeLineCommand.Command.LocalVariables.ContainsKey(testVar));
        }


        public static int GetTestVal() => int.MaxValue;
       

        public static double GetTestDouble() =>  45;


        [Fact]
        public async Task ExecuteAssigningInternalVarMustBeSettedInCommandDictAsync()
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
            codeResolver.Setup(x => x.ResolveCommandAction(It.IsAny<ICommand>())).ReturnsAsync((meta, Delegate.CreateDelegate(typeof(Func<double>), testMethod)));

            await new ExecuteCodeLineCommandHandler(codeResolver.Object, mediator.Object).Handle(testCodeLineCommand, token);

            Assert.True(testCodeLineCommand.Command.LocalVariables.ContainsKey(testParamName.ToString()));
        }


        [Fact]
        public void ExecuteVoidCommandMustBeCalled()
        {
            throw new NotImplementedException();
        }


        [Fact]
        public async void ExecuteWithoutCommandTest()
        {
            throw new NotImplementedException();
            //ParameterNames testParamName = ParameterNames.Weight;
            //string testParamStr = testParamName.ToString();

            //int testVal1 = 1;
            //int testVal2 = 45;

            //ICommand testCommand =
            //     new ExecutableCommand($"{testParamStr} = {testVal1} + {testVal2}", CommandType.Assigning, testParamName, testParamStr);
            //ExecuteCodeLineCommand testCodeLineCommand = new ExecuteCodeLineCommand(testCommand, props, localVars);

            //mediator = new Mock<IMediator>();
            //codeResolver = new Mock<ICodeResolveService>();

            //await new ExecuteCodeLineCommandHandler(codeResolver.Object, mediator.Object).Handle(testCodeLineCommand, token);

            //Assert.True(testCodeLineCommand.LocalVariables.ContainsKey(testParamStr));

        }


        [Fact]
        public async void ExecuteWithoutCommandAndWithLocalVariableTest()
        {
            throw new NotImplementedException();
        }


        [Fact]
        public void ExecuteWithUnknownCommandMustThrow()
        {
            throw new NotImplementedException();
        }


        [Fact]
        public void ExecuteWithIncorrectAgrsMustThrow()
        {
            throw new NotImplementedException();
        }


        [Fact]
        public void ExecuteWithLocalVarArgMustUseThisArg()
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
