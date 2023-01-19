﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackerUI.Library.Models;

namespace TrackerWinFormUI.Interface
{
    public interface ITournamentRequestor
    {
        void NewTournamentComplete(TournamentModel tournament);
    }
}
