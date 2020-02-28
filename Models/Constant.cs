namespace SailTally.Classes
{
    public class Constant
    {
        public class BlockName
        {
            public const string WorkingDialog = "";
        }

        public class GetVar
        {
            public const string RaceId = "rid";
        }

        public class Scripts
        {
            public const string IncludeScriptFormat = @"<script type=""text/{0}"" src=""{1}{2}""></script>";
            public const string BlockScriptFormat = @"<script type=""text/{0}"">{1}</script>";

            public const string DisplayWorking = @"displayWorking();";
            public const string HideWorking = @"hideWorking();"; // hide as soon as script can run
            public const string HideWorkingLast = @"hideWorking(true);"; // hide after page has completely loaded
            public const string HideWorkingLoad = @"function pageLoad() { hideWorking(); }"; // pageLoad() is fired after every UpdatePanel refresh
        }

        public class Schedule
        {
            public const int TimeWidth = 7; // HH:MMTT (where TT is AM or PM)
        }

        public class Url
        {
            public const string Assign = "=";
            public const string Separator = "&";
            public const string Quote = "\"";
            public const string Parameters = "?";
            public const string Bookmark = "#"; // Named Anchor
        }
    }
}
