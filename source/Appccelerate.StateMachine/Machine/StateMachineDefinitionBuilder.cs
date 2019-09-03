namespace Appccelerate.StateMachine.Machine
{
    using System;
    using System.Collections.Generic;
    using States;
    using SyntaxNew;

    public class StateMachineDefinitionBuilder<TState, TEvent>
        where TState : IComparable
        where TEvent : IComparable
    {
        private readonly List<Func<ISyntaxStart<TState, TEvent>, object>> setupFunctions = new List<Func<ISyntaxStart<TState, TEvent>, object>>();

        public StateMachineDefinitionBuilder<TState, TEvent> WithConfiguration(
            Func<ISyntaxStart<TState, TEvent>, object> setupFunction)
        {
            this.setupFunctions.Add(setupFunction);
            return this;
        }

        public StateMachineDefinition<TState, TEvent> Build()
        {
            var stateDefinitionDictionary = new StateDictionaryNew<TState, TEvent>();
            var initiallyLastActiveStates = new Dictionary<TState, IStateDefinition<TState, TEvent>>();
            var syntaxStart = new SyntaxStart<TState, TEvent>(stateDefinitionDictionary, initiallyLastActiveStates);

            this.setupFunctions.ForEach(f => f(syntaxStart));

            return new StateMachineDefinition<TState, TEvent>(stateDefinitionDictionary, initiallyLastActiveStates);
        }
    }
}