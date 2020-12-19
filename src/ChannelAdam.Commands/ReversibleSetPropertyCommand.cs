//-----------------------------------------------------------------------
// <copyright file="ReversibleSetPropertyCommand.cs">
//     Copyright (c) 2017-2020 Adam Craven. All rights reserved.
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
    using ChannelAdam.Commands.Abstractions;
    using System;
    using System.Reflection;

    public class ReversibleSetPropertyCommand : ReversibleCommandBase
    {
        #region Private Fields

        private readonly SetPropertyCommand setPropertyCommand;
        private SetPropertyCommand? undoSetPropertyCommand;

        #endregion Private Fields

        #region Public Constructors

        public ReversibleSetPropertyCommand(SetPropertyCommand setPropertyCommand)
        {
            this.setPropertyCommand = setPropertyCommand ?? throw new ArgumentNullException(nameof(setPropertyCommand));
        }

        public ReversibleSetPropertyCommand(object target, PropertyInfo property, object? newValue)
        {
            this.setPropertyCommand = new SetPropertyCommand(target, property, newValue);
        }

        #endregion Public Constructors

        #region Protected Methods

        protected override void ExecuteCore()
        {
            this.CreateUndoSetPropertyCommand();

            this.setPropertyCommand.Execute();
        }

        protected override void UndoCore()
        {
            this.undoSetPropertyCommand?.Execute();
        }

        #endregion Protected Methods

        #region Private Methods

        private void CreateUndoSetPropertyCommand()
        {
            var previousValue = this.setPropertyCommand.Property.GetValue(this.setPropertyCommand.Object, null);
            this.undoSetPropertyCommand = new SetPropertyCommand(this.setPropertyCommand.Object, this.setPropertyCommand.Property, previousValue);
        }

        #endregion Private Methods
    }
}