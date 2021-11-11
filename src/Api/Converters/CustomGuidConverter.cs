using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Api.Converters
{
    [ExcludeFromCodeCoverage]
    public class CustomGuidConverter : JsonConverter<Guid>
    {
        public override bool HandleNull => true;

        public override Guid Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
                return default;

            var value = reader.GetString();

            if (value == string.Empty)
                return default;

            reader.TryGetGuid(out Guid guid);

            if (guid == Guid.Empty)
                throw new JsonException($"Valor {reader.GetString()} é inválido para o tipo Guid.");

            return guid;
        }

        public override void Write(
            Utf8JsonWriter writer,
            Guid objectToWrite,
            JsonSerializerOptions options)
        {
            writer.WriteStringValue(objectToWrite.ToString());
        }
    }
}