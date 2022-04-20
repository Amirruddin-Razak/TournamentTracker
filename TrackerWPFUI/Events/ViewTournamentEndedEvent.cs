using System;
using System.Collections.Generic;
using System.Text;
using TrackerLibrary.Models;
using TrackerWPFUI.ViewModels;

namespace TrackerWPFUI.Events
{
    public class ViewTournamentEndedEvent
    {
        public ViewTournamentEndedEvent(Type callerType, StatusInfoViewModel statusInfo, TournamentModel endedTournament)
        {
            CallerType = callerType;
            StatusInfo = statusInfo;
            EndedTournament = endedTournament;
        }

        public Type CallerType { get; }
        public StatusInfoViewModel StatusInfo { get; }
        public TournamentModel EndedTournament { get; }
    }
}
