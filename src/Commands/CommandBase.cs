using System;
using System.Windows.Input;

namespace DnDManager.Commands
{
    /// <summary>
    /// Base for all commands
    /// </summary>
    internal abstract class CommandBase : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        /// <summary>
        /// Check if the button can execute.
        /// </summary>
        /// <param name="parameter">Passed parameter</param>
        /// <returns></returns>
        public virtual bool CanExecute(object? parameter)
        {
            return true;
        }

        /// <summary>
        /// Execute on button's click
        /// </summary>
        /// <param name="parameter"></param>
        public abstract void Execute(object? parameter);

        /// <summary>
        /// Call this method to update CanExecute state of the UI Element
        /// </summary>
        /// <param name="parameter"></param>
        protected void OnCanExecuteChanged(object? parameter)
        {
            CanExecuteChanged?.Invoke(this, new EventArgs());
        }
    }
}
