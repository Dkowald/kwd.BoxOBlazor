namespace kwd.BoxOBlazor.Demo
{
    /// <summary>
    /// Utility class describing static assets provided by this blazor component library (BCL).
    /// </summary>
    public static class Assets
    {
        private static string Base = "_content/kwd.BoxOBlazor.Demo";

        public static string Util = Base + "/util.js";

        public static class Css
        {
            public static string Site = Base + "/css/site.css";
            public static string Bootstrap = Base + "/css/bootstrap/bootstrap.min.css";
            public static string Iconic = Base + "/css/open-iconic/font/css/open-iconic-bootstrap.min.css";
        }

        public static class Img
        {
            public static string Log = Base + "/img/log.svg";
            public static string Background = Base + "/img/background.png";
            public static string AlarmClock = Base + "/img/alarmClock.svg";
        }
    }
}
