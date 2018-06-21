//-----------------------------------------------------------------------
// <copyright file="SetPropertyCommand.cs">
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

namespace ChannelAdam.Commands
{
    using ChannelAdam.Commands.Abstractions;
    using System;
    using System.Reflection;

    public class SetPropertyCommand : CommandBase
    {
        public SetPropertyCommand(SetPropertyCommand original)
        {
            if (original == null)
            {
                throw new ArgumentNullException(nameof(original));
            }

            this.Object = original.Object;
            this.Property = original.Property;
            this.NewValue = original.NewValue;
        }

        public SetPropertyCommand(object target, PropertyInfo property, object newValue)
        {
            if (target == null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            if (property == null)
            {
                throw new ArgumentNullException(nameof(property));
            }

            this.Object = target;
            this.Property = property;
            this.NewValue = newValue;
        }

        public object NewValue { get; set; }

        public object Object { get; set; }

        public PropertyInfo Property { get; set; }

        protected override void ExecuteCore()
        {
#if NET40 || NET45
            this.Property.SetValue(this.Object, this.NewValue, BindingFlags.SetProperty | BindingFlags.Public, null, null, null);
#else
            this.Property.SetValue(this.Object, this.NewValue, null);
#endif
        }
    }
}