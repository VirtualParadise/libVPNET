/* **********************************************************************************
 *
 * Copyright (c) TCPX. All rights reserved.
 *
 * This source code is subject to terms and conditions of the Microsoft Public
 * License (Ms-PL). A copy of the license can be found in the license.txt file
 * included in this distribution.
 *
 * You must not remove this notice, or any other, from this software.
 *
 * **********************************************************************************/

using System;
using System.Collections.Generic;
using System.Reflection;
using VpNetFramework.Common.ActionInterpreter;
using VpNetFramework.Common.ActionInterpreter.Interfaces;

namespace VpNetFramework.Common.Reflection
{
    /// <summary>
    /// Various helpers for reflection, primarily targeted at the action string intepreter.
    /// </summary>
    public sealed class ReflectionHelpers
    {
        public class Info<TAttribute, TMember>
        {
            public TAttribute Attribute;
            public TMember Member;
        }

        public List<Info<TAttribute, MethodInfo>> GetMethods<TAttribute>(Type @class, TAttribute attribute)
            where TAttribute : Attribute
        {
            var ret = new List<Info<TAttribute, MethodInfo>>();

            foreach (var item in @class.GetMethods())
            {
                var attributes = @class.GetCustomAttributes(typeof(TAttribute), false);
                if (attributes != null && attributes.Length == 1)
                {
                    ret.Add(new Info<TAttribute, MethodInfo>() { Attribute = (TAttribute)attributes[0], Member = item });
                }
            }
            return ret;
        }


        /// <summary>
        /// Determines whether the specified type implements a certain interface.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="interface">The @interface.</param>
        /// <returns>
        /// 	<c>true</c> if the specified type has interface; otherwise, <c>false</c>.
        /// </returns>
        public static bool HasInterface(Type type, Type @interface)
        {
            if (!@interface.IsInterface)
                throw new Exception("Interface type expected");
            return type.GetInterface(@interface.Name) != null;
        }



        /// <summary>
        /// Determines whether the specified object implements a certain interface.
        /// </summary>
        /// <param name="o">The o.</param>
        /// <param name="interface">The @interface.</param>
        /// <returns>
        /// 	<c>true</c> if the specified o has interface; otherwise, <c>false</c>.
        /// </returns>
        public static bool HasInterface(object o, Type @interface)
        {
            if (!@interface.IsInterface)
                throw new Exception("Interface type expected");
            return o.GetType().GetInterface(@interface.Name) != null;
        }

        public static List<Type> GetTypes(Assembly assembly, Type implements)
        {
            var ret = new List<Type>();
            try
            {
                foreach (var type in assembly.GetTypes())
                {
                    if (type.BaseType == implements)
                        ret.Add(type);
                }
            }
            // occurs when the assembly has been loaded only for reflection purposes.
            catch (ReflectionTypeLoadException ex)
            {
                foreach (var type in ex.Types)
                {
                    if (type != null && type.BaseType != null && type.BaseType.ToString() == implements.ToString())
                        ret.Add(type);
                }
            }
            return ret;
        }

        /// <summary>
        /// Gets the enumerations for a specific enum binding attributes and returns a reflection cache.
        /// </summary>
        /// <typeparam name="TEnumbindingAttribute">The type of the enumbinding attribute.</typeparam>
        /// <typeparam name="TEnumItemBindingAttribute">The type of the enum item binding attribute.</typeparam>
        /// <returns></returns>
        public static ReflectionEnumCache GetEnums<TEnumbindingAttribute, TEnumItemBindingAttribute>(Assembly asm)
            where TEnumbindingAttribute : Attribute
            where TEnumItemBindingAttribute : Attribute, IACLiteralNames
        {
            var cacheEnumItems = new List<ReflectionEnumCacheItem>();

            //var asm = Assembly.GetAssembly(typeof(TEnumbindingAttribute));
            foreach (var type in asm.GetTypes())
            {
                if (type.IsEnum)
                {
                    var attributes = type.GetCustomAttributes(typeof(TEnumbindingAttribute), false);
                    if (attributes != null && attributes.Length == 1)
                    {
                        var cacheFields = new List<ReflectionEnumCacheItemField>();
                        foreach (var field in type.GetFields())
                        {
                            var fieldAttributes = field.GetCustomAttributes(typeof(TEnumItemBindingAttribute), false);
                            if (fieldAttributes != null && fieldAttributes.Length == 1)
                            {
                                var att = (TEnumItemBindingAttribute)fieldAttributes[0];
                                cacheFields.Add(new ReflectionEnumCacheItemField(field.Name, (int)field.GetRawConstantValue(),
                                                                         new List<string>(att.LiteralNames)));
                            }
                        }
                        cacheEnumItems.Add(new ReflectionEnumCacheItem(type, cacheFields));
                    }
                }
            }
            return new ReflectionEnumCache(cacheEnumItems);
        }

        /// <summary>
        /// Gets object instances that implement a certain interface
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<T> GetInstancesOfInterface<T>(Assembly asm)
        {
            //var asm = Assembly.GetAssembly(typeof(T));
            var interpreters = new List<T>();
            foreach (var type in asm.GetTypes())
            {
                var t = type.GetInterface(typeof(T).FullName);
                if (t != null)
                {
                    try
                    {
                        var o = (T)Activator.CreateInstance(type);
                        interpreters.Add(o);
                    }
                    catch
                    {
                        throw new ActionException(string.Format("can't create instance of type '{0}'. No parameterless constructor exists.", type.FullName));
                    }
                }
            }
            return interpreters;
        }

        /// <summary>
        /// Interprets the specified action string, creating instances of objects that implement a restricted type TInterface of ILiteralAction.
        /// </summary>
        /// <typeparam name="TInterface">The type of the interface.</typeparam>
        /// <param name="instancesCache">The instances cache.</param>
        /// <param name="action">The action.</param>
        /// <param name="delimiter">The delimiter to seperate the actions.</param>
        /// <returns></returns>
        public static List<TInterface> Interpret<TInterface>(IEnumerable<TInterface> instancesCache, string action, char delimiter)
            where TInterface : ILiteralAction
        {
            var newInstances = new List<TInterface>();
            foreach (var actionItem in action.Split(delimiter))
            {
                foreach (var cachedInstance in instancesCache)
                {
                    try
                    {
                        if (cachedInstance.LiteralAction.ToLower() !=
                            actionItem.Trim().ToLower().Substring(0, cachedInstance.LiteralAction.Length)) continue;
                    }
                    catch
                    {
                        continue;
                    }
                    var newInstance = (TInterface)Activator.CreateInstance(cachedInstance.GetType());
                    newInstance.LiteralPart = actionItem.Trim().Substring(cachedInstance.LiteralAction.Length).Trim();
                    newInstances.Add(newInstance);
                }
            }
            return newInstances;
        }
    }

}
