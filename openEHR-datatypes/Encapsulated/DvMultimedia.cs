using System.Security.Cryptography;
using OpenEHR.RM.DataTypes.Text;
using OpenEHR.RM.DataTypes.Uri;

namespace OpenEHR.RM.DataTypes.Encapsulated;

/// <summary>
/// A specialisation of DV_ENCAPSULATED for audiovisual and biosignal types.
/// openEHR RM 1.1.0 data_types.encapsulated.DV_MULTIMEDIA
/// </summary>
public sealed class DvMultimedia : DvEncapsulated
{
    public CodePhrase MediaType { get; }
    public byte[]? Data { get; }
    public CodePhrase? CompressionAlgorithm { get; }
    public CodePhrase? IntegrityCheckAlgorithm { get; }
    public byte[]? IntegrityCheck { get; }
    public DvUri? Uri { get; }
    public string? AlternateText { get; }
    public DvMultimedia? Thumbnail { get; }

    public override int Size => Data?.Length ?? 0;

    public DvMultimedia(
        CodePhrase mediaType,
        byte[]? data = null,
        CodePhrase? compressionAlgorithm = null,
        CodePhrase? integrityCheckAlgorithm = null,
        byte[]? integrityCheck = null,
        DvUri? uri = null,
        string? alternateText = null,
        DvMultimedia? thumbnail = null,
        CodePhrase? language = null,
        CodePhrase? charset = null)
        : base(language, charset)
    {
        if (data is null && uri is null)
            throw new ArgumentException("At least one of Data or Uri must be provided.");

        MediaType = mediaType ?? throw new ArgumentNullException(nameof(mediaType));
        Data = data;
        CompressionAlgorithm = compressionAlgorithm;
        IntegrityCheckAlgorithm = integrityCheckAlgorithm;
        IntegrityCheck = integrityCheck;
        Uri = uri;
        AlternateText = alternateText;
        Thumbnail = thumbnail;
    }

    /// <summary>
    /// Validates the integrity check against the data using the specified algorithm.
    /// Supports SHA-1 ("SHA-1") and SHA-256 ("SHA-256").
    /// Returns null if no data or integrity check is available.
    /// </summary>
    public bool? IntegrityChecksPassed()
    {
        if (Data is null || IntegrityCheck is null || IntegrityCheckAlgorithm is null)
            return null;

        byte[] computed = IntegrityCheckAlgorithm.CodeString.ToUpperInvariant() switch
        {
            "SHA-1" or "SHA1" => SHA1.HashData(Data),
            "SHA-256" or "SHA256" => SHA256.HashData(Data),
            _ => throw new NotSupportedException(
                $"Integrity check algorithm '{IntegrityCheckAlgorithm.CodeString}' is not supported.")
        };

        return computed.SequenceEqual(IntegrityCheck);
    }
}
