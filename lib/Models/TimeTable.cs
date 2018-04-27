using System;
using System.Collections.Generic;
using System.Text;

namespace Telegram.Bot.vasya_v2
{
    public class TTimeTable
    {
        public int Id { set; get; }
        public int DialogId { set; get; }
        public string DayWeek { set; get; }
        public string NameSbj { set; get; }
        public int Order { set; get; }
        public string Cabinet { set; get; }
    }
}
