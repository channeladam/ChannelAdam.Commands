//-----------------------------------------------------------------------
// <copyright file="ReversibleCommandManager.cs">
//     Copyright (c) 2016 Adam Craven. All rights reserved.
// </copyright>
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//    http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//-----------------------------------------------------------------------

namespace ChannelAdam.Commands
{
    using System;
    using System.Collections.Concurrent;

    using Abstractions;

    public class ReversibleCommandManager
    {
        #region Private Fields

        private ConcurrentStack<IReversibleCommand> undoCommandStack = new ConcurrentStack<IReversibleCommand>();

        #endregion Private Fields

        #region Public Properties

        public int CountOfCommandsToUndo
        {
            get { return this.undoCommandStack.Count; }
        }

        public bool HasCommandsToUndo
        {
            get { return this.undoCommandStack.Count > 0; }
        }

        #endregion Public Properties

        #region Public Methods

        public void Clear()
        {
            this.undoCommandStack.Clear();
        }

        public void ExecuteCommand(IReversibleCommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            command.Execute();

            this.undoCommandStack.Push(command);
        }

        public TCommandResult ExecuteCommandFunction<TCommandResult>(IReversibleCommandFunction<TCommandResult> command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            var result = command.Execute();

            this.undoCommandStack.Push(command);

            return result;
        }

        public void UndoAllCommands()
        {
            while (this.undoCommandStack.Count > 0)
            {
                this.UndoPreviousCommand();
            }
        }

        public void UndoPreviousCommand()
        {
            IReversibleCommand cmd = null;

            if (this.undoCommandStack.TryPeek(out cmd))
            {
                cmd.Undo();

                this.undoCommandStack.TryPop(out cmd);
            }
        }

        #endregion Public Methods
    }
}