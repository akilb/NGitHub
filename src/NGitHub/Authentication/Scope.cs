using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NGitHub.Authentication {
    /// <summary>
    /// Scopes let you specify exactly what type of access you need. This will
    /// be displayed to the user on the authorize form. Public read-only access
    /// is available by default.
    /// </summary>
    public enum Scope {
        /// <summary>
        /// DB read/write access to profile info only.
        /// </summary>
        User,

        /// <summary>
        /// DB read/write access, and Git read access to public repos.
        /// </summary>
        PublicRepo,

        /// <summary>
        /// DB read/write access, and Git read access to public and private repos.
        /// </summary>
        Repo,

        /// <summary>
        /// Write access to gists.
        /// </summary>
        Gists
    }
}
