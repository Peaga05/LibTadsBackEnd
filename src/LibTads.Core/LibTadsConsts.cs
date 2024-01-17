using LibTads.Debugging;

namespace LibTads
{
    public class LibTadsConsts
    {
        public const string LocalizationSourceName = "LibTads";

        public const string ConnectionStringName = "Default";

        public const bool MultiTenancyEnabled = true;


        /// <summary>
        /// Default pass phrase for SimpleStringCipher decrypt/encrypt operations
        /// </summary>
        public static readonly string DefaultPassPhrase =
            DebugHelper.IsDebug ? "gsKxGZ012HLL3MI5" : "c59d7b51a4ae4cff8025979f46af3354";
    }
}
