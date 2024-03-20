using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Inetlab.SMPP.Logging;

namespace IRIDIUM.GMDSS_LRIT.SmppClient
{

    public class TextBoxLogFactory : ILogFactory
    {
        private readonly TextBox _textBox;
        private readonly LogLevel _minLevel;
        private readonly DataStore<string> _logStore = new DataStore<string>();
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private readonly SemaphoreSlim _addEvent = new SemaphoreSlim(0);
        public TextBoxLogFactory(TextBox textBox, LogLevel minLevel)
        {
            _textBox = textBox;
            _minLevel = minLevel;

            _textBox.HandleCreated += (sender, args) => Task.Run(() => AddToTextBox(_cancellationTokenSource.Token));
            _textBox.HandleDestroyed += (sender, args) => _cancellationTokenSource.Cancel();



        }

        public ILog GetLogger(string loggerName)
        {
            return new TextBoxLogger(loggerName, _minLevel, AddToLog);
        }

        private void AddToLog(string text)
        {
            _logStore.Append(text);

            _addEvent.Release();
        }

        private void AddToTextBox(CancellationToken cancellationToken)
        {
            try
            {
                while (true)
                {
                    if (!_logStore.HasData)
                    {
                        _addEvent.Wait(cancellationToken);
                    }

                    if (cancellationToken.IsCancellationRequested) return;

                    StringBuilder sb = new StringBuilder();
                    foreach (string line in _logStore.TakeWork())
                    {
                        sb.AppendLine(line);
                    }

                    _textBox.BeginInvoke(new Action(() => { _textBox.AppendText(sb.ToString()); }));

                }
            }
            catch (OperationCanceledException)
            {

            }

        }



        private class DataStore<T>
        {
            private readonly List<T> _data = new List<T>();

            public void Append(T data)
            {
                lock (_data)
                {
                    _data.Add(data);
                }
            }

            public bool HasData
            {
                get
                {
                    lock (_data)
                    {
                        return _data.Count > 0;
                    }
                }
            }

            public T[] TakeWork()
            {
                T[] result;
                lock (_data)
                {
                    result = _data.ToArray();
                    _data.Clear();
                }

                return result;
            }
        }


        private class TextBoxLogger : ILog
        {
            private readonly string _loggerName;
            private readonly LogLevel _minLevel;
            private readonly Action<string> _append;


            public TextBoxLogger(string loggerName, LogLevel minLevel, Action<string> append)
            {
                _loggerName = loggerName;
                _minLevel = minLevel;
                _append = append;
            }


            public bool IsEnabled(LogLevel level)
            {
                return level >= _minLevel;
            }

            public void Write(LogLevel level, string message, Exception ex, params object[] args)
            {
                if (!IsEnabled(level)) return;

                AddToLog("{0} ({1}) {2}{3}", level, _loggerName, string.Format(message, args),
                    ex != null ? ", Exception: " + ex.ToString() : "");

            }

            private void AddToLog(string message, params object[] args)
            {
                _append(string.Format("{0:HH:mm:ss.fff}: {1}", DateTime.Now, string.Format(message, args)));

            }

        }

    }
}
