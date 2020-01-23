using Experimental.System.Messaging;
using System;
using System.Collections.Generic;
using System.Text;

namespace MsmqListener
{
    public delegate void MessageReceivedEventhandler(object sender, MessageEventArgs args);

    class MsmqListener
    {

        private bool _listen;
        private MessageQueue _queue;

        public event MessageReceivedEventhandler MessageReceived;

        public MsmqListener(string queuePath)
        {
            _queue = new MessageQueue(queuePath);
        }


        public void Start()
        {
            _listen = true;
            Console.WriteLine("Msmq Listening....");
            _queue.Formatter = new BinaryMessageFormatter();

            _queue.PeekCompleted += new PeekCompletedEventHandler(OnPeekCompleted);
            _queue.ReceiveCompleted += new ReceiveCompletedEventHandler(OnReceiveCompleted);

            StartListening();
            Console.ReadKey();
        }

        public void Stop()
        {
            _listen = false;
            _queue.PeekCompleted -= new PeekCompletedEventHandler(OnPeekCompleted);
            _queue.ReceiveCompleted -= new ReceiveCompletedEventHandler(OnReceiveCompleted);
        }

        private void OnReceiveCompleted(object sender, ReceiveCompletedEventArgs e)
        {
            Message message = _queue.EndReceive(e.AsyncResult);
            Mail.SendTokenToMail(message.Body.ToString(), message.Label.ToString());
            Console.WriteLine(message.Body.ToString() + " " + message.Label.ToString());

            StartListening();

            FireReceiveEvent(message.Body);
        }

        private void OnPeekCompleted(object sender, PeekCompletedEventArgs e)
        {

            _queue.EndPeek(e.AsyncResult);
            MessageQueueTransaction messageQueueTransaction = new MessageQueueTransaction();
            Message message = null;
            try
            {
                messageQueueTransaction.Begin();
                message = _queue.Receive(messageQueueTransaction);
                messageQueueTransaction.Commit();

                StartListening();

                FireReceiveEvent(message.Body);
            }
            catch
            {
                messageQueueTransaction.Abort();
            }

        }

        private void FireReceiveEvent(object body)
        {
            MessageReceived?.Invoke(this, new MessageEventArgs(body));
        }

        private void StartListening()
        {
            if (!_listen)
                return;

            if (_queue.Transactional)
                _queue.BeginPeek();
            else
                _queue.BeginReceive();
        }

    }


    public class MessageEventArgs : EventArgs
    {

        private object _messageBody;

        public object MessageBody
        {
            get { return _messageBody; }
        }

        public MessageEventArgs(object body)
        {
            _messageBody = body;
        }

    }

}
