﻿using System;
using System.Collections.Generic;

namespace Communicator.BusinessLayer.Models
{
    public class UserDetails
    {
        public UserDetails()
        {
            TopicList = new List<string>();
        }

        public ICollection<string> TopicList { get; set; }
        public DateTime ActivityTime { get; set; }
    }
}