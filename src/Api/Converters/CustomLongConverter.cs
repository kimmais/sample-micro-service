using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Diagnostics.CodeAnalysis;

namespace Api.Converters
{
    [ExcludeFromCodeCoverage]
    public class CustomLongConverter : JsonConverter<long?>
    {
        public override bool HandleNull => true;

        public override long? Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
                return default;

            var value = reader.GetString();

            if (value == string.Empty || value == "0")
                return default;

            reader.TryGetInt64(out long longValue);

            if (longValue == long.MinValue)
                throw new JsonException($"Valor {reader.GetString()} é inválido para o tipo long.");

            return longValue;
        }

        public override void Write(
            Utf8JsonWriter writer,
            long? objectToWrite,
            JsonSerializerOptions options) =>
            JsonSerializer.Serialize(writer, objectToWrite, objectToWrite.GetType(), options);
    }
}
