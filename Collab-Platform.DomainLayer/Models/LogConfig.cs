using NpgsqlTypes;
using Serilog.Sinks.PostgreSQL.ColumnWriters;
namespace Collab_Platform.DomainLayer.Models;
public class LogConfig
{
    public Dictionary<string, ColumnWriterBase> GetColoumData()
    {
        var columnWriters = new Dictionary<string, ColumnWriterBase>
        {
            { "Timestamp", new TimestampColumnWriter(NpgsqlDbType.Timestamp) },
            { "Level", new LevelColumnWriter(true, NpgsqlDbType.Varchar) },
            { "Message", new MessageTemplateColumnWriter() },
            { "MessageTemplate", new MessageTemplateColumnWriter() },
            { "Exception", new ExceptionColumnWriter() },
            { "Properties", new PropertiesColumnWriter(NpgsqlDbType.Jsonb) },
            { "LogEvent", new LogEventSerializedColumnWriter(NpgsqlDbType.Jsonb) }
        };
        return columnWriters;
    }
}

