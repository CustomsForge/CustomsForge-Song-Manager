﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CustomsForgeManager.CustomsForgeManagerLib
{
    public interface INotifyTabChanged
    {
        void TabEnter();
        void TabLeave();
    }
}