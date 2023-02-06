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
            string testMethod = "ReturnYouself";
            string testCommand2 = "TestLastCommand";
            string testMethod2 = "Sum";
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

            codeResolver = new Mock<ICodeResolveService>();
            mediator = new Mock<IMediator>();
            Mock<IMetaStorageService> metaProvider = new Mock<IMetaStorageService>();
            Mock<ICommandActionsProvider> actionsProvider = new Mock<ICommandActionsProvider>();

            ParseCodeLineCommandHandler codeLineCommandHandler = new ParseCodeLineCommandHandler();
            ExecuteCodeLineCommandHandler handler = new ExecuteCodeLineCommandHandler(codeResolver.Object, mediator.Object);
            GetCommandNameCommandHandler nameResHandler = new GetCommandNameCommandHandler();
            GetCommandTypesMetaQueueHandler metaHandler = new GetCommandTypesMetaQueueHandler(mediator.Object, metaProvider.Object);
            GetCommandArgsValuesQueueHandler getArgsHandler = new GetCommandArgsValuesQueueHandler();
            CodeResolveService service = new CodeResolveService(mediator.Object, actionsProvider.Object);

            metaProvider.Setup(x => x.GetMetaByCommandName(It.IsAny<string>())).Returns((string x) =>
            {
                if (x == testCommand)
                    return new CommandArgsTypesMeta(new List<(Type, string)>(), typeof(int));
                else if (x == testCommand2)
                    return new CommandArgsTypesMeta(new List<(Type, string)>() { (typeof(int), "arg1"), (typeof(int), "arg2"), (typeof(int), "arg3") }, typeof(int));
                else throw new KeyNotFoundException();
            });

            actionsProvider.Setup(x => x.GetDelegateByCommandNameWithoutParams(It.IsAny<string>())).Returns((string x) =>
            {
                if (x == testCommand)
                    return Delegate.CreateDelegate(typeof(Func<int>), typeof(ExecuteCodeLinesCommandTests).GetMethod(testMethod));
                else if (x == testCommand2)
                    return Delegate.CreateDelegate(typeof(Func<int, int, int, int>), typeof(ExecuteCodeLinesCommandTests).GetMethod(testMethod2));
                else throw new KeyNotFoundException();
            });


            mediator.Setup(x => x.Send(It.IsAny<ParseCodeLineCommand>(), It.IsAny<CancellationToken>()))
                .Returns((ParseCodeLineCommand x, CancellationToken y) => codeLineCommandHandler.Handle(x, y));
            mediator.Setup(x => x.Send(It.IsAny<ExecuteCodeLineCommand>(), It.IsAny<CancellationToken>()))
                .Returns((ExecuteCodeLineCommand x, CancellationToken y) => handler.Handle(x,y));
            mediator.Setup(x => x.Send(It.IsAny<GetCommandNameCommand>(), It.IsAny<CancellationToken>()))
                .Returns((GetCommandNameCommand x, CancellationToken y) => nameResHandler.Handle(x, y));
            mediator.Setup(x => x.Send(It.IsAny<GetCommandTypesMetaQueue>(), It.IsAny<CancellationToken>()))
                 .Returns((GetCommandTypesMetaQueue x, CancellationToken y) => metaHandler.Handle(x, y));
            mediator.Setup(x => x.Send(It.IsAny<GetCommandArgsValuesQueue>(), It.IsAny<CancellationToken>()))
                 .Returns((GetCommandArgsValuesQueue x, CancellationToken y) => getArgsHandler.Handle(x, y));

#warning В вызове GetDelegateByCommandName возвращается почему-то не имя метода, а моментально имя класса. 06.02.2023
            codeResolver.Setup(x => x.ResolveCommandAction(It.IsAny<ICommand>(), It.IsAny<CancellationToken>()))
                .Returns((ICommand x, CancellationToken y) => service.ResolveCommandAction(x, y));
  

            await new ExecuteCodeLinesCommandHandler(mediator.Object).Handle(command, token);

            Assert.True(command.Settings.Properties.ContainsKey(testResultName));
            Assert.True(command.Settings.Properties[testResultName].Type == typeof(int));
            Assert.True((int)command.Settings.Properties[testResultName].Value == testVal1 + testVal2 + testVal3);
        }


        public static int ReturnYouself(int i) => i;

        public static int Sum(int a, int b, int c) => a + b + c;

    }
}
