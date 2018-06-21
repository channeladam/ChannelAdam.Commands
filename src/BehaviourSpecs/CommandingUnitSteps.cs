using ChannelAdam.Commands;
using ChannelAdam.TestFramework.MSTestV2.Abstractions;
using TechTalk.SpecFlow;

namespace BehaviourSpecs
{
    [Binding]
    [Scope(Feature = "Commanding")]
    public class CommandingUnitSteps : MoqTestFixture
    {
        ClassWithProperty testObject;
        ReversibleCommandManager commandManager;

        [Given(@"an initial state to perform commands on")]
        public void GivenAnInitialStateToPerformCommandsOn()
        {
            this.testObject = new ClassWithProperty
            {
                MyProperty = 1
            };
            Logger.Log("Initial state: " + this.testObject.MyProperty);

            this.commandManager = new ReversibleCommandManager();
        }

        [When(@"a series of commands are executed")]
        public void WhenASeriesOfCommandsAreExecuted()
        {
            this.commandManager.ExecuteSetPropertyCommand(this.testObject, p => p.MyProperty, 100);
            LogAssert.AreEqual("MyProperty after set property command", 100, this.testObject.MyProperty);

            this.commandManager.ExecuteSetPropertyCommand(this.testObject, p => p.MyProperty, 200);
            LogAssert.AreEqual("MyProperty after set property command", 200, this.testObject.MyProperty);
        }

        [When(@"all the commands are undone")]
        public void WhenAllTheCommandsAreUndone()
        {
            this.commandManager.UndoPreviousCommand();
            LogAssert.AreEqual("MyProperty after undo #1", 100, this.testObject.MyProperty);

            this.commandManager.UndoPreviousCommand();
        }

        [Then(@"the initial state is restored")]
        public void ThenTheInitialStateIsRestored()
        {
            LogAssert.AreEqual("MyProperty after undo #2", 1, this.testObject.MyProperty);
        }

        private class ClassWithProperty
        {
            public int MyProperty { get; set; }
        }
    }
}
