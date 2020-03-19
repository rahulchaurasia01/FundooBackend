using System;
using System.Collections.Generic;
using System.Text;

namespace FundooCommonLayer.Model
{
    
    public class NotificationModel
    {

        public NoteModel Notification { set; get; }

        public string To { set; get; }

    }

    public class NoteModel
    {
        public string Title { set; get; }

        public string Body { set; get; }
    }


}
