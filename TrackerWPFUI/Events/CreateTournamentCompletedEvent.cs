using System;
using System.Collections.Generic;
using System.Text;
using TrackerLibrary.Models;

namespace TrackerWPFUI.Events
{
    public class CreateTournamentCompletedEvent
    {
        public CreateTournamentCompletedEvent(Type callerType, TournamentModel tournament)
        {
            CallerType = callerType;
            Tournament = tournament;
        }

        public Type CallerType { get; }
        public TournamentModel Tournament { get; }
    }
}
