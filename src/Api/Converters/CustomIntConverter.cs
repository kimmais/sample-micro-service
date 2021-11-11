using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Diagnostics.CodeAnalysis;

namespace Api.Converters
{
    [ExcludeFromCodeCoverage]
    public class CustomIntConverter : JsonConverter<int?>
    {
        public override bool HandleNull => true;

        public override int? Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
                return default;

            try
            {

                reader.TryGetInt32(out int integer);

                return integer;
            }
            catch (InvalidOperationException)
            {
                throw new JsonException($"Valor {reader.GetString()} é inválido para o tipo integer.");
            }

        }

        public override void Write(
            Utf8JsonWriter writer,
            int? objectToWrite,
            JsonSerializerOptions options) =>
            JsonSerializer.Serialize(writer, objectToWrite, objectToWrite.GetType(), options);
    }
}
