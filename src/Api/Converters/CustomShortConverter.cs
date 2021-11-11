using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Diagnostics.CodeAnalysis;

namespace Api.Converters
{
    [ExcludeFromCodeCoverage]
    public class CustomShortConverter : JsonConverter<short?>
    {
        public override bool HandleNull => true;

        public override short? Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
                return default;

            var value = reader.GetString();

            if (value == string.Empty || value == "0")
                return default;

            reader.TryGetInt16(out short shortValue);

            if (shortValue == short.MinValue)
                throw new JsonException($"Valor {reader.GetString()} é inválido para o tipo short.");

            return shortValue;
        }

        public override void Write(
            Utf8JsonWriter writer,
            short? objectToWrite,
            JsonSerializerOptions options) =>
            JsonSerializer.Serialize(writer, objectToWrite, objectToWrite.GetType(), options);
    }
}
