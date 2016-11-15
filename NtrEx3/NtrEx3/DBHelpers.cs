using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NtrEx3
{
    public class DBHelpers
    {
        public static bool IsUniqueKeyViolation( SqlException ex)
        {
            return ex.Errors.Cast<SqlError>().Any(e => e.Class == 14 && (e.Number == 2601 || e.Number == 2627));
        }
    }
}
