//-----------------------------------------------------------------------
// <copyright file="ReversibleCommandFunctionBase.cs">
//     Copyright (c) 2017 Adam Craven. All rights reserved.
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

namespace ChannelAdam.Commands.Abstractions
{
    using System;

    public abstract class ReversibleCommandFunctionBase<TCommandResult> : CommandFunctionBase<TCommandResult>, IReversibleCommandFunction<TCommandResult>
    {
        #region Public Methods

        public TCommandResult Undo()
        {
            if (this.CanUndo())
            {
                return this.UndoCore();
            }

            return default(TCommandResult);
        }

        void IReversibleCommand.Undo()
        {
            this.Undo();
        }

        #endregion Public Methods

        #region Protected Methods

        protected virtual bool CanUndo()
        {
            return true;
        }

        protected abstract TCommandResult UndoCore();

        #endregion Protected Methods
    }
}