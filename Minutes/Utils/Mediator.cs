#pragma warning disable CS8601 // Possible null reference assignment.

namespace Minutes.Utils
{
    internal class Mediator
    {
        public static Mediator Instance { get; } = new Mediator();

        private readonly Dictionary<string, Action<object?>> _registeredActions = [];

        public void Register(string message, Action<object?> action)
        {
            if (!_registeredActions.TryAdd(message, action))
                _registeredActions[message] += action;
        }

        public void Unregister(string message, Action<object?> action)
        {
            if (_registeredActions.ContainsKey(message))
                _registeredActions[message] -= action;
        }

        public void Send(string message, object? parameter = null)
        {
            if (_registeredActions.TryGetValue(message, out var registeredAction))
                registeredAction?.Invoke(parameter!);
        }
    }
}
