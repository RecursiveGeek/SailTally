using System.Data.Linq;
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Local

namespace SailTally
{
    public partial class SS_Boat
    {
        partial void OnValidate(ChangeAction action)
        {
            CentralLibrary.Post(action, "Boat", BoatID);
        }
    }

    public partial class SS_BoatOwner
    {
        partial void OnValidate(ChangeAction action)
        {
            CentralLibrary.Post(action, "BoatOwner", BoatOwnerID);
        }
    }

    public partial class SS_Fleet
    {
        partial void OnValidate(ChangeAction action)
        {
            // Validate the URL
            if (Website!=null && !Website.ToLower().StartsWith("http://"))
            {
                Website = "http://" + Website;
            }

            CentralLibrary.Post(action, "Fleet", FleetID);
        }
    }

    public partial class SS_FleetSery
    {
        partial void OnValidate(ChangeAction action)
        {
            CentralLibrary.Post(action, "FleetSeries", FleetSeriesID);
        }
    }

    public partial class SS_Penalty
    {
        partial void OnValidate(ChangeAction action)
        {
            CentralLibrary.Post(action, "Penalty", PenaltyID);
        }
    }

    public partial class SS_Prize
    {
        partial void OnValidate(ChangeAction action)
        {
            CentralLibrary.Post(action, "Prize", PrizeID);
        }
    }

    public partial class SS_PrizeDetail
    {
        partial void OnValidate(ChangeAction action)
        {
            CentralLibrary.Post(action, "PrizeDetail", PrizeDetailID);
        }
    }

    public partial class SS_Race
    {
        partial void OnValidate(ChangeAction action)
        {
            CentralLibrary.Post(action, "Race", RaceID);
        }
    }

    public partial class SS_RaceSery
    {
        partial void OnValidate(ChangeAction action)
        {
            if (action == ChangeAction.Insert && ScorePointsFactor == 0)
            {
                // This is to correct an issue encountered since moving to WinHost with the MSSQL Default Value for the column 
                // not being set to the specificed default value.  Initially the thought was that MSSQL default values weren't being 
                // honored.  However, it is likely that a newer version of the Entity Framework (EF) is passing default values, 
                // which would supercede the MSSQL Default Constraint value.
                ScorePointsFactor = 1;
            }
            if (action == ChangeAction.Insert && !IsScoreExcludable)
            {
                // See above note for ScorePointsFactor :-)
                IsScoreExcludable = true;
            }
            CentralLibrary.Post(action, "RaceSeries", RaceID);
        }
    }

    public partial class SS_Registration
    {
        partial void OnValidate(ChangeAction action)
        {
            CentralLibrary.Post(action, "Registration", RegistrationID);
        }
    }

    public partial class SS_Result
    {
        /*
        partial void OnValidate(ChangeAction action)
        {
            // Don't Log results to this table since it is so massive in scale and some detail tracked in the columns of SS_ResultSummary
            //CentralLibrary.Post(action, "Result", this.ResultID);
        }
        */
    }

    public partial class SS_ResultSummary
    {
        /*
        partial void OnValidate(ChangeAction action)
        {
            // Don't Log results to this table since it is so massive in scale and some detail tracked in the columns of SS_ResultSummary
            //CentralLibrary.Post(action, "ResultSummary", this.ResultSummaryID;
        }
        */
    }

    public partial class SS_Score
    {
        partial void OnValidate(ChangeAction action)
        {
            CentralLibrary.Post(action, "Score", ScoreID);
        }
    }

    public partial class SS_ScoreMethod
    {
        partial void OnValidate(ChangeAction action)
        {
            CentralLibrary.Post(action, "ScoreMethod", ScoreMethodID);
        }
    }

    public partial class SS_ScoreMethodDetail
    {
        partial void OnValidate(ChangeAction action)
        {
            CentralLibrary.Post(action, "ScoreMethodID", ScoreMethodDetailID);
        }
    }

    public partial class SS_Season
    {
        partial void OnValidate(ChangeAction action)
        {
            CentralLibrary.Post(action, "Season", SeasonID);
        }
    }

    public partial class SS_Sery
    {
        partial void OnValidate(ChangeAction action)
        {
            CentralLibrary.Post(action, "Series", SeriesID);
        }
    }

    public partial class SS_Throwout
    {
        partial void OnValidate(ChangeAction action)
        {
            CentralLibrary.Post(action, "Throwout", ThrowoutID);
        }
    }

    public partial class SS_ThrowoutDetail
    {
        partial void OnValidate(ChangeAction action)
        {
            CentralLibrary.Post(action, "ThrowoutDetail", ThrowoutDetailID);
        }
    }

    public partial class aspnet_Membership
    {
        partial void OnValidate(ChangeAction action)
        {
            CentralLibrary.Post(action, "Users", UserId.ToString());
        }
    }
}
