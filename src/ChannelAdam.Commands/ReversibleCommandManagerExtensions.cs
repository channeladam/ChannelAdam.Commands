//-----------------------------------------------------------------------
// <copyright file="ReversibleCommandManagerExtensions.cs">
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
    using System;
    using System.Linq.Expressions;
    using System.Reflection;

    public static class ReversibleCommandManagerExtensions
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Not in this case.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Not in this case.")]
        public static void ExecuteSetPropertyCommand<TTarget, TValue>(this ReversibleCommandManager commandManager, TTarget target, Expression<Func<TTarget, TValue>> propertyExpression, TValue? valueToSet)
        {
            if (commandManager == null)
            {
                throw new ArgumentNullException(nameof(commandManager));
            }

            if (target == null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            if (propertyExpression == null)
            {
                throw new ArgumentNullException(nameof(propertyExpression));
            }

            var memberExpression = propertyExpression.Body as MemberExpression;
            var property = memberExpression != null ? memberExpression.Member as PropertyInfo : null;

            if (memberExpression == null ||
                memberExpression.Expression == null ||
                memberExpression.Expression.NodeType != ExpressionType.Parameter ||
                memberExpression.Member == null ||
                property == null)
            {
                throw new ArgumentException("propertyExpression must be of the form 'p => p.SomeProperty'");
            }

            commandManager.ExecuteCommand(new ReversibleSetPropertyCommand(target, property, valueToSet));
        }
    }
}
