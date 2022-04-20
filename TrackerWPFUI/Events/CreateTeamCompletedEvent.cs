using System;
using System.Collections.Generic;
using System.Text;
using TrackerLibrary.Models;

namespace TrackerWPFUI.Events
{
    public class CreateTeamCompletedEvent
    {
        public CreateTeamCompletedEvent(Type callerType, TeamModel team)
        {
            CallerType = callerType;
            Team = team;
        }

        public Type CallerType { get; }
        public TeamModel Team { get; }
    }
}
