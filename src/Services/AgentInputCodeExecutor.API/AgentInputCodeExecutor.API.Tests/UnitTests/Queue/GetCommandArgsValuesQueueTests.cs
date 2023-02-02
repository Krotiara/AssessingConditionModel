using AgentInputCodeExecutor.API.Entities;
using AgentInputCodeExecutor.API.Interfaces;
using AgentInputCodeExecutor.API.Service.Queue;
using Interfaces;
using Interfaces.DynamicAgent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;


namespace AgentInputCodeExecutor.API.Tests.UnitTests.Queue
{
    public class GetCommandArgsValuesQueueTests
    {

        CancellationToken token;
        private readonly Dictionary<string, IProperty> localVars;

        public GetCommandArgsValuesQueueTests()
        {
            var tokenSource = new CancellationTokenSource(TimeSpan.FromMilliseconds(100));
            token = tokenSource.Token;
            localVars = new Dictionary<string, IProperty>();
        }


        [Fact]
        public async Task GetAgrsWithIncorrectMetaTypesMustThrow()
        {
            ICommand command = new ExecutableCommand("TestCall(1,\"str\")", CommandType.VoidCall, localVars);
            ICommandArgsTypesMeta types = new CommandArgsTypesMeta(new List<(Type, string)> { (typeof(int), "test1"), (typeof(int), "test2")}, null);
            GetCommandArgsValuesQueue request = new GetCommandArgsValuesQueue(command, types);
            await Assert.ThrowsAsync<GetCommandArgsValuesException>(() => new GetCommandArgsValuesQueueHandler().Handle(request, token));
        }


        [Fact]
        public async Task GetAgrsWithMissingArgMustThrow()
        {
            ICommand command = new ExecutableCommand("TestCall(1)", CommandType.VoidCall, localVars);
            ICommandArgsTypesMeta types = new CommandArgsTypesMeta(new List<(Type, string)> { (typeof(int), "test1"), (typeof(int), "test2") }, null);
            GetCommandArgsValuesQueue request = new GetCommandArgsValuesQueue(command, types);
            await Assert.ThrowsAsync<GetCommandArgsValuesException>(() => new GetCommandArgsValuesQueueHandler().Handle(request, token));
        }


        [Fact]
        public async void GetArgsWithoutLocalVariablesMustReturnUnchanged()
        {
            ICommand command = new ExecutableCommand("TestCall(1,\"str\")", CommandType.VoidCall, localVars);
            ICommandArgsTypesMeta types = new CommandArgsTypesMeta(new List<(Type, string)> { (typeof(int), "test1"), (typeof(string), "test2") }, null);
            GetCommandArgsValuesQueue request = new GetCommandArgsValuesQueue(command, types);
            List<object> res = await new GetCommandArgsValuesQueueHandler().Handle(request, token);
            Assert.Equal(1, res[0]);
            Assert.Equal("str", res[1]);
        }


        [Fact]
        public async Task GetArgsWithLocalVariablesMustBeSubstitutedAsync()
        {
            ICommand command = new ExecutableCommand("TestCall(a)", CommandType.VoidCall, localVars);
            ICommandArgsTypesMeta types = new CommandArgsTypesMeta(new List<(Type, string)> { (typeof(int), "test1")}, null);
            localVars["a"] = new AgentProperty(ParameterNames.None, typeof(int), 1, "a");
            GetCommandArgsValuesQueue request = new GetCommandArgsValuesQueue(command, types);
            List<object> res = await new GetCommandArgsValuesQueueHandler().Handle(request, token);
            Assert.Equal(1, res[0]);
        }


        [Fact]
        public async Task GetArgsFromActionMustReturnArgsAsync()
        {
            ICommand command = new ExecutableCommand("TestCall(\"str\")", CommandType.VoidCall, localVars);
            ICommandArgsTypesMeta types = new CommandArgsTypesMeta(new List<(Type, string)> { (typeof(string), "test1") }, null);
            GetCommandArgsValuesQueue request = new GetCommandArgsValuesQueue(command, types);
            List<object> res = await new GetCommandArgsValuesQueueHandler().Handle(request, token);
            Assert.Equal("str", res[0]);
        }


        [Fact]
        public async Task GetArgsWithNullMustReturnNullType()
        {
            ICommand command = new ExecutableCommand("TestCall(null)", CommandType.VoidCall, localVars);
            ICommandArgsTypesMeta types = new CommandArgsTypesMeta(new List<(Type, string)> { (typeof(string), "test1") }, null);
            GetCommandArgsValuesQueue request = new GetCommandArgsValuesQueue(command, types);
            List<object> res = await new GetCommandArgsValuesQueueHandler().Handle(request, token);
            Assert.Null(res[0]);
        }


        [Fact]
        public async Task GetArgsFromFuncMustReturnArgsAsync()
        {
            ICommand command = new ExecutableCommand("testVar = TestCall(\"str\")", CommandType.Assigning, localVars, "testVar");
            ICommandArgsTypesMeta types = new CommandArgsTypesMeta(new List<(Type, string)> { (typeof(string), "test1") }, null);
            GetCommandArgsValuesQueue request = new GetCommandArgsValuesQueue(command, types);
            List<object> res = await new GetCommandArgsValuesQueueHandler().Handle(request, token);
            Assert.Equal("str", res[0]);
        }


        [Fact]
        public async Task GetArgsFromCommandWithoutCommandMustReturnEmpty()
        {
            ICommand command = new ExecutableCommand("a=1+2", CommandType.Assigning, localVars, "a");
            ICommandArgsTypesMeta types = new CommandArgsTypesMeta(new List<(Type, string)> {}, typeof(double));
            GetCommandArgsValuesQueue request = new GetCommandArgsValuesQueue(command, types);
            List<object> res = await new GetCommandArgsValuesQueueHandler().Handle(request, token);
            Assert.Empty(res);
        }
    }
}
