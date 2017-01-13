using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace YT {
    public static class Extension {

        public static void SafeDispose(this IDisposable o) {
            if (o != null) o.Dispose();
        }
    }
}
