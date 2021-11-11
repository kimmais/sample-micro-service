using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Diagnostics.CodeAnalysis;

namespace Api.Converters
{
    [ExcludeFromCodeCoverage]
    public class CustomDateTimeConverter : JsonConverter<DateTime?>
    {
        public override bool HandleNull => true;

        public override DateTime? Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
                return default;

            var value = reader.GetString();

            if (value == string.Empty)
                return default;

            reader.TryGetDateTime(out DateTime datetime);

            if (datetime == DateTime.MinValue)
                throw new JsonException($"Valor {reader.GetString()} é inválido para o tipo data/hora.");

            return datetime;
        }

        public override void Write(
            Utf8JsonWriter writer,
            DateTime? objectToWrite,
            JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, objectToWrite, objectToWrite == null ? typeof(string) : objectToWrite.GetType(), options);
        }
    }
}
