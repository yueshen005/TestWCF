using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace BenchmarkTool
{
    public class DBTraceResultLogger : ILogTraceResult
    {
        private string _connString;

        private static readonly string createTableScriptFormat = @"
create table {0}
(
	TraceName varchar(100),
	ThreadID int,
	Elapsed datetime,
	ElapsedMilliseconds bigint,
	CreationTime datetime
)
";
        private string _tableName;

        public DBTraceResultLogger(string connString, string tableName)
        {
            _connString = connString;
            _tableName = tableName;
        }

        public void Log(IEnumerable<TraceEvent> result)
        {
            string tablename = CreateTmpTable();

            var dt = ConvertToDatatable(result);
            BulkCopyToDB(dt, tablename);
        }

        private string CreateTmpTable()
        {
            string tablename = string.Concat("Tmp_", _tableName, DateTime.Now.ToString("yyyyMMdd_HHmmssfff"));
            string cmdtext = string.Format(createTableScriptFormat, tablename);

            using (SqlConnection conn = new SqlConnection(_connString))
            {
                using (SqlCommand cmd = new SqlCommand(cmdtext, conn))
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }

            return tablename;
        }

        private void BulkCopyToDB(DataTable dt, string tablename)
        {
            using (SqlBulkCopy bk = new SqlBulkCopy(_connString, SqlBulkCopyOptions.Default))
            {
                bk.DestinationTableName = tablename;
                bk.WriteToServer(dt);
            }
        }

        private DataTable ConvertToDatatable(IEnumerable<TraceEvent> result)
        {
            DataTable dt = new DataTable("TestWCFPerformance");
            dt.Columns.Add("TraceName", typeof(string));
            dt.Columns.Add("ThreadID", typeof(int));
            dt.Columns.Add("Elapsed", typeof(string));
            dt.Columns.Add("ElapsedMilliseconds", typeof(long));
            dt.Columns.Add("CreationTime", typeof(DateTime));

            foreach (var traceEvent in result)
            {
                var row = dt.NewRow();
                row[0] = traceEvent.TraceName;
                row[1] = traceEvent.ThreadID;
                row[2] = traceEvent.Elapsed;
                row[3] = traceEvent.ElapsedMilliseconds;
                row[4] = traceEvent.CreationTime;
                dt.Rows.Add(row);
            }

            return dt;
        }
    }
}
