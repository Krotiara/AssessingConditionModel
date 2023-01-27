using AgentInputCodeExecutor.API.Entities;
using AgentInputCodeExecutor.API.Interfaces;
using AgentInputCodeExecutor.API.Service.Queue;
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
        public GetCommandArgsValuesQueueTests()
        {
            var tokenSource = new CancellationTokenSource(TimeSpan.FromMilliseconds(100));
            token = tokenSource.Token;
        }


        [Fact]
        public async Task GetAgrsWithIncorrectMetaTypesMustThrow()
        {
            string commandLine = "TestCall(1,\"str\")";
            ICommandArgsTypesMeta types = new CommandArgsTypesMeta(new Type[] { typeof(int), typeof(int) }, new string[] { "test1", "test2" }, null);
            GetCommandArgsValuesQueue request = new GetCommandArgsValuesQueue(commandLine, types);
            await Assert.ThrowsAsync<GetCommandArgsValuesException>(() => new GetCommandArgsValuesQueueHandler().Handle(request, token));
        }


        [Fact]
        public async Task GetAgrsWithMissingArgMustThrow()
        {
            string commandLine = "TestCall(1)";
            ICommandArgsTypesMeta types = new CommandArgsTypesMeta(new Type[] { typeof(int), typeof(int) }, new string[] { "test1", "test2" }, null);
            GetCommandArgsValuesQueue request = new GetCommandArgsValuesQueue(commandLine, types);
            await Assert.ThrowsAsync<GetCommandArgsValuesException>(() => new GetCommandArgsValuesQueueHandler().Handle(request, token));
        }


        [Fact]
        public async void GetArgsWithoutLocalVariablesMustReturnUnchanged()
        {
            string commandLine = "TestCall(1,\"str\")";
            ICommandArgsTypesMeta types = new CommandArgsTypesMeta(new Type[] { typeof(int), typeof(string) }, new string[] { "test1", "test2" }, null);
            GetCommandArgsValuesQueue request = new GetCommandArgsValuesQueue(commandLine, types);
            List<object> res = await new GetCommandArgsValuesQueueHandler().Handle(request, token);
            Assert.Equal(1, res[0]);
            Assert.Equal("str", res[1]);
        }


        [Fact]
        public async Task GetArgsWithLocalVariablesMustBeSubstitutedAsync()
        {
            string commandLine = "TestCall(a)";
            ICommandArgsTypesMeta types = new CommandArgsTypesMeta(new Type[] { typeof(int)}, new string[] { "test1"}, null);
            Dictionary<string, object> localVars = new Dictionary<string, object>();
            localVars["a"] = 1;
            GetCommandArgsValuesQueue request = new GetCommandArgsValuesQueue(commandLine, types, localVars);
            List<object> res = await new GetCommandArgsValuesQueueHandler().Handle(request, token);
            Assert.Equal(1, res[0]);
        }


        [Fact]
        public async Task GetArgsFromActionMustReturnArgsAsync()
        {
            string commandLine = "TestCall(\"str\")";
            ICommandArgsTypesMeta types = new CommandArgsTypesMeta(new Type[] {typeof(string) }, new string[] { "test1"}, null);
            GetCommandArgsValuesQueue request = new GetCommandArgsValuesQueue(commandLine, types);
            List<object> res = await new GetCommandArgsValuesQueueHandler().Handle(request, token);
            Assert.Equal("str", res[0]);
        }


        [Fact]
        public async Task GetArgsWithNullMustReturnNullType()
        {
            string commandLine = "TestCall(null)";
            ICommandArgsTypesMeta types = new CommandArgsTypesMeta(new Type[] { typeof(string)}, new string[] { "test1"}, null);
            GetCommandArgsValuesQueue request = new GetCommandArgsValuesQueue(commandLine, types);
            List<object> res = await new GetCommandArgsValuesQueueHandler().Handle(request, token);
            Assert.Equal(null, res[0]);
        }


        [Fact]
        public async Task GetArgsFromFuncMustReturnArgsAsync()
        {
            string commandLine = "testVar = TestCall(\"str\")";
            ICommandArgsTypesMeta types = new CommandArgsTypesMeta(new Type[] { typeof(string) }, new string[] { "test1" }, null);
            GetCommandArgsValuesQueue request = new GetCommandArgsValuesQueue(commandLine, types);
            List<object> res = await new GetCommandArgsValuesQueueHandler().Handle(request, token);
            Assert.Equal("str", res[0]);
        }
    }
}
