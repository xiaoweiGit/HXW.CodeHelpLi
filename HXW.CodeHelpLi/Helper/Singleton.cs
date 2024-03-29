﻿// ***************************************************************
//  version:  1.0   date: 11/27/2007
//  -------------------------------------------------------------
//  
//  -------------------------------------------------------------
//  (C)2007  HXW.CodeHelpLi All Rights Reserved
// ***************************************************************
// 
// ***************************************************************
using System;
using System.Threading;

namespace  HXW.CodeHelpLi
{
    /// <summary>
    /// Singleton Mode
    /// </summary>
    /// <typeparam name="T">Class type would be implemented singleton mode</typeparam>
    public abstract class Singleton<T>
    {
        private static object syncObject = new object();
        private static T instance;

        /// <summary>
        /// Entry for access singleton unique value
        /// </summary>
        public static T Instance
        {
            get
            {
                if(instance == null)
                {
                    Monitor.Enter(syncObject);
                    if (instance == null)
                    {
                        try
                        {
                            instance = (T)Activator.CreateInstance(typeof(T), true) ;
                        }
                        finally
                        {
                            Monitor.Exit(syncObject);
                        }
                    }
                }

                return instance;
            }
            set
            {
                Monitor.Enter(syncObject);
                try
                {
                    instance = value;
                }
                finally
                {
                    Monitor.Exit(syncObject);
                }
            }
        }
    }
}
