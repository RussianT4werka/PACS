using PACS.Models;

namespace PACS.Tools
{
    public class Session
    {
        static Dictionary<string, Admin> _sessions = new Dictionary<string, Admin>();

        public static string CreateSession(Admin admin)
        {
            string guid = Guid.NewGuid().ToString();
            _sessions[guid] = admin;
            return guid;
        }

        public static Admin GetAdmin(string session)
        {
            if (session == null || !_sessions.ContainsKey(session))
                return new Admin();
            return _sessions[session];
        }
    }
}
