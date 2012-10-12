/* Copyright (c) 2009 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System.Diagnostics;
using System.Windows.Threading;

using Lingua;

namespace LinguaDemo
{
    public class LinguaTraceListener : TraceListener
    {
        #region Fields

        private Dispatcher m_dispatcher;
        private WriteTraceLineDelegate m_writeTraceLineDelegate;

        #endregion

        #region Constructors

        public LinguaTraceListener(Dispatcher dispatcher, WriteTraceLineDelegate writeTraceLineDelegate)
        {
            m_dispatcher = dispatcher;
            m_writeTraceLineDelegate = writeTraceLineDelegate;
        }

        #endregion

        #region Public Methods

        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id)
        {
            WriteLine(id.ToString());
        }

        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string message)
        {
            LinguaTraceId linguaTraceId = (LinguaTraceId)id;
            WriteLine(string.Format("({0}) {1}", linguaTraceId, message));
        }

        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string format, params object[] args)
        {
            LinguaTraceId linguaTraceId = (LinguaTraceId)id;
            WriteLine(string.Format("({0}) {1}", linguaTraceId, string.Format(format, args)));
        }

        public override void Write(string message)
        {
            m_dispatcher.Invoke(m_writeTraceLineDelegate, new object[] { message });
        }

        public override void WriteLine(string message)
        {
            m_dispatcher.Invoke(m_writeTraceLineDelegate, new object[] { message });
        }

        #endregion
    }
}
